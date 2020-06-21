

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Dota2GSI;
using Q42.HueApi.Interfaces;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Original;
using System.Text.RegularExpressions;
using Group = Q42.HueApi.Models.Groups.Group;
using Dota2GSI.Nodes;

namespace DotaLightApp
{
    //Connection should be established by now and dota 2 functionality starts here.
    public partial class Form2 : Form
    {
        //Dota2GSI GameStateListener
        static GameStateListener gameStateListener;
        private bool proccessStarted = false;
        public ILocalHueClient client;
        private int health = -1;
        private bool isAlive = false;
        private int count = 0;
        private int time = -1;
        private int lastBountTime = -1;
        private Dictionary<string, groupProperties> groups;
        private List<groupProperties> activeGroups;
        private List<string> activeLights;

        /*
        * 
        */
        public Form2()
        {
            InitializeComponent();
        }

        /*
        * Setup variables on foarm load
        */
        private async void Form2_Load(object sender, EventArgs e)
        { 
            groups = new Dictionary<string, groupProperties>();
            activeGroups = new List<groupProperties>();
            activeLights = new List<string>();
        }

        /*
        * Initalise Form2 with stuff
        */
        public async void Form2Setup()
        {
            //Get bridges and groups
            Bridge bridgeAsync = await client.GetBridgeAsync();
            Group[] groupsAsync = bridgeAsync.Groups.ToArray();

            //Clear groupRooms
            GroupRooms.Items.Clear();
            GroupRooms.CheckOnClick = true;

            //Get lights
            IEnumerable<Light> lightsAsync = await client.GetLightsAsync();
            Light[] lightsArray = lightsAsync.ToArray();
            Dictionary<string, Light> allLights = new Dictionary<string, Light>();
            foreach (Light lightAsync in lightsArray)
            {
                allLights.Add(lightAsync.Id, lightAsync);
            }

            //Build groups
            foreach (Group group in groupsAsync)
            {
                groupProperties gProperty = new groupProperties(group.Id);
                foreach(String lightId in group.Lights)
                {
                    lightProperties newLight = new lightProperties(allLights[lightId]);
                    gProperty.lights.Add(newLight);
                }
                groups.Add(group.Name, gProperty);

                //Add checkboxes to form 
                GroupRooms.Items.Add(group.Name);
            }

            //Set startup color for bounty alert
            setStartupBounyColor();
        }

        /**
         * Find last bounty color if exists.
         */
        private void setStartupBounyColor()
        {
            string colorFilePath = AppDomain.CurrentDomain.BaseDirectory + @"" + "colorfile" + ".txt";
            if (File.Exists(colorFilePath))
            {
                string colorFromFile = File.ReadAllText(colorFilePath);
                string[] splitColor = colorFromFile.Split(' ');
                List<string> onlyColorValue = new List<string>();

                string[] numbers = Regex.Split(colorFromFile, @"\D+");
                this.BuntyColorLight.ForeColor = Color.FromArgb(int.Parse(numbers[1]), int.Parse(numbers[2]), int.Parse(numbers[3]), int.Parse(numbers[4]));
            }
        }

        /*
        * Update active groups
        */
        private void updateActiveGroups()
        {
            activeGroups.Clear();
            foreach (var item in GroupRooms.CheckedItems)
            {
                activeGroups.Add(groups[item.ToString()]);
                groupProperties tGroup = groups[item.ToString()];
                foreach (var tLight in tGroup.lights)
                {
                    activeLights.Add(tLight.id);
                }
            }
        }

        /*
        * update active groups when group form checkbox chages
        */
        private void GroupRooms_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            updateActiveGroups();
        }

        /*
        * Start dota 2 gamestate listener
        */
        private void StartButton_Click(object sender, EventArgs e)
        {

            //Update active groups from Form
            updateActiveGroups();

            //Check if dota 2 proccess is on else 
            Process[] dota2Process = Process.GetProcessesByName("Dota2");
            if (proccessStarted)
            {
                reset();
                StartButton.Text = "Start";
                this.infoText.Text = "-";
                return;
            }

            if (dota2Process.Length == 0)
            {
                this.infoText.Text = "Cant find dota 2 process running.";
            }
            else
            { 
                this.infoText.Text = "Dota 2 process is running.";

                gameStateListener = new GameStateListener(4000);
                gameStateListener.NewGameState += new NewGameStateHandler(OnNewGameState);

                //Start game state listner
                if (!gameStateListener.Start())
                {
                    this.infoText.Text = "Could not start Gamestate Listner.";
                }
                else
                {
                    this.infoText.Text = "Lisening for dota 2 gamestate calles.";

                    proccessStarted = true;
                    StartButton.Text = "Stop";
                }
            }
        }

        /*
        * Game state chenge recived from dota 2.
        */
        async void OnNewGameState(GameState gs)
        {
            //unly update when game sis in progress
            if (gs.Map.GameState != DOTA_GameState.DOTA_GAMERULES_STATE_GAME_IN_PROGRESS)
            {
                return;
            }

            //bool to check if we should continue the proccess
            bool stopGamechange = false;

            //dead player update light
            if(this.DeathBox.Checked)
            {
                stopGamechange = await TurnOffOnDeath(gs);
            }

            //Bounty alert update light
            if (!stopGamechange && this.BountyBox.Checked)
            {
                stopGamechange = await BountyRuneAlert(gs);
            }

            //Health cahnge update light
            if (!stopGamechange && this.HealthBox.Checked)
            {
                stopGamechange = await HealthChange(gs);
            }

            if(!stopGamechange && gs.Map.Win_team.ToString() != "None")
            {
                reset();
            }

            //Update info text
            float gameTime = gs.Map.ClockTime;
            this.infoText.Text = "Game state changes: " + count + " | Time:" + gameTime % 300;
        }

        /*
        * AlertBounty rune
        */
        private async Task<bool> BountyRuneAlert(GameState gs)
        {
            int realClockTime = gs.Map.ClockTime;
            if (lastBountTime != realClockTime)
            {
                if (realClockTime % 300 == 290 || realClockTime == 0)
                {

                    var command = new LightCommand();
                    command.Alert = Alert.Multiple;
                    Color color = this.BuntyColorLight.ForeColor;
                    command.SetColor(new RGBColor(color.R, color.G, color.B));

                    if (client != null)
                    {
                        lastBountTime = realClockTime;
                        count++;
                        return await SendCommand(command);
                    }
                    else
                    {
                        this.infoText.Text = "client is null";
                    }
                }
            }
            return false;
        }

        /*
        * Set alert color
        */
        private void BuntyColorLight_Click(object sender, EventArgs e)
        {
            string colorFilePath = AppDomain.CurrentDomain.BaseDirectory + @"" + "colorfile" + ".txt";

            //open color dialog
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = this.BuntyColorLight.ForeColor;

            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                this.BuntyColorLight.ForeColor = MyDialog.Color;
                string writeColor = "[A=" + MyDialog.Color.A + ", R="+ MyDialog.Color.R + ", G="+ MyDialog.Color.G + ", B="+ MyDialog.Color.B + "]";
                File.WriteAllText(colorFilePath, writeColor);
            }
                
        }

        /*
        *   HealthChange
        */
        private async Task<bool> HealthChange(GameState gs)
        {
            int newHealth = gs.Hero.HealthPercent;
            int newTime = gs.Map.ClockTime;

            //Only send update if hetlth has changed and oly one message/secound
            if (health != newHealth && time != newTime)
            {
                health = newHealth;
                time = newTime;

                int r = 0;
                int g = 0;
                int b = 0;

                //calculate rgb value from health
                if (newHealth > 50)
                {
                    float newProcentage = (newHealth - 50) * 2;
                    float decimalProcentage = newProcentage / 100;
                    float newColor = decimalProcentage * 255;
                    float reverceColor = 255 - newColor;

                    //float reverceColor = 255 - ((((newHealth - 50) * 2) / 100) * 255);

                    g = 255;
                    r = (int)reverceColor;
                }
                else
                {
                    float newProcentage = (newHealth * 2);
                    float decimalProcentage = newProcentage / 100;
                    float newColor = decimalProcentage * 255;

                    g = (int)newColor;
                    r = 255;
                }

                //Create commant to sent to lights
                var command = new LightCommand();
                command.TurnOn().SetColor(new RGBColor(r, g, b));

                //Send light command
                if (client != null)
                {
                    count++;
                    return await SendCommand(command);
                }
                else
                {
                    this.infoText.Text = "client is null";
                }

            }
            return false;
        }

        /*
         * Check if player is dead and turn lights if so.
         */
        private async Task<bool> TurnOffOnDeath(GameState gs)
        {
            if(gs.Hero.IsAlive != isAlive)
            {
                isAlive = false;
                //we dont turn on light becose health update does that for us.
                if(!gs.Hero.IsAlive)
                {
                    //Create command to sent to lights
                    var command = new LightCommand();
                    command.TurnOff();

                    count++;
                    return await SendCommand(command);
                } 
                else if(!this.HealthBox.Checked)
                {
                    //Create command to sent to lights
                    var command = new LightCommand();
                    command.TurnOn();

                    count++;
                    return await SendCommand(command);
                }
            }
            else
            {
                isAlive = true;
            }
            return false;
        } 


        /*
        * Send command to active lights
        */
        private async Task<bool> SendCommand(LightCommand command)
        {
            //Send light command
            if (client != null)
            {
                if(activeLights.Count != 0 && proccessStarted)
                {
                    await client.SendCommandAsync(command, activeLights);
                }
                count++;
                return true;
            }
            else
            {
                this.infoText.Text = "client is null";
            }
            return false;
        }

        /*
        * Reset all light to what they were before starting
        */
        private async void reset()
        {
            if(gameStateListener != null)
            {
                gameStateListener.Stop();
            }
            gameStateListener = null;
            proccessStarted = false;

            foreach (KeyValuePair<string, groupProperties> group in groups)
            {
                foreach(lightProperties light in group.Value.lights)
                {
                    var command = new LightCommand();
                    if(light.orgOn) { command.TurnOn(); } else { command.TurnOff(); }
                    command.SetColor(light.colorCoordinates[0], light.colorCoordinates[1]);
                    List<string> listId = new List<string>();
                    listId.Add(light.id);
                    await client.SendCommandAsync(command, listId);
                }
            }
        }

        /*
         * Form closed reset lights and exit applicaion
         */
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            reset();
            Application.Exit();
        }
    }
}
