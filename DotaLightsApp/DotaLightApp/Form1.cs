using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Groups;

namespace DotaLightApp
{
    //Connecting Hue bridge form.
    public partial class Form1 : Form
    {
        //Class variables
        private static String appName;
        private static String deviceName;
        private static ILocalHueClient client;
        private bool working;
        String appKey;
        String regFilePath;

        //initialize the app
        public Form1()
        {
            InitializeComponent();

            //Setup string variables
            appName = "TheDotaApp";
            deviceName = Environment.MachineName;

            doTheThing();
        }

        //call doTheThing again when button pressed
        private void button1_Click(object sender, EventArgs e)
        {
            doTheThing();
        }


        //connect to bridge if possible go to form 2.
        private async void doTheThing()
        {
            this.infoText.Text = "Connecting...";
            working = true;
            await hueConfig();
            if (working == true)
            {
                if (CreateGsifile())
                {
                    await BlinkLights();
                    Form2 dotaForm = new Form2();
                    dotaForm.Show();
                    //set client on form 2
                    dotaForm.client = client;
                    dotaForm.Form2Setup();
                    this.Hide();
                }
            }
        }

        //connect to bridge or register to bridge
        private async Task hueConfig()
        {
            //Locate bridges in home network
            IBridgeLocator locator = new HttpBridgeLocator();
            var bridges = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(10));

            
            if (bridges.Count() == 0)
            {
                this.infoText.Text = "Bridge not found!";
                working = false;
                return;
            }
            var bridge = bridges.ElementAt(0);

            //Setup new client on bridge and check if avilabe
            client = new LocalHueClient(bridge.IpAddress);
            if (client == null)
            {
                Console.WriteLine("client is Null!");
                working = false;
                return;
            }

            //Check if regfile already exists for bridge
            regFilePath = AppDomain.CurrentDomain.BaseDirectory + @"" + bridge.BridgeId + ".txt";
            Console.WriteLine("Check for Regfile : " + regFilePath);
            if (File.Exists(regFilePath))
            {
                getAppkeyFromFile();
            }
            else
            {
                await registeAppKeyAsync();
                if (working == false)
                {
                    return;
                }
            }
        }

        //Get the register key from file
        private void getAppkeyFromFile()
        {
            //get appkey from file
            appKey = File.ReadAllText(regFilePath);
            //initialize app
            client.Initialize(appKey);
        }

        //Register app with philips hue bride, save result in txt file for later use.
        private async Task registeAppKeyAsync()
        {
            //Register app, MUST PRESS BRIDGE BUTTON FIRST
            try
            {
                appKey = await client.RegisterAsync(appName, deviceName);
                File.WriteAllText(regFilePath, appKey);
                working = true;
            }
            catch (Exception e)
            {
                this.infoText.Text = "Press brige button then press start";
                working = false;
            }
        }

        //Blink lightes to test if connection was possible
        private async Task BlinkLights()
        {
            try
            {
                var command = new LightCommand();
                command.Alert = Alert.Once;

                if (client != null)
                {
                    HueResults results = await client.SendCommandAsync(command);
                }

            }
            catch (Exception e)
            {
                return;
            }
        }

        //Create CSI file for dota 2 gamestate updates
        private bool CreateGsifile()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");

            if (regKey != null)
            {
                //hardcoded where dota 2 probably is installed
                string cfgfolder = regKey.GetValue("SteamPath") +
                                   @"\steamapps\common\dota 2 beta\game\dota\cfg";

                if (!Directory.Exists(cfgfolder))
                {
                    this.infoText.Text = cfgfolder + "Steam path not found: " + cfgfolder;
                    return false;
                }

                string gsifolder = cfgfolder + "\\gamestate_integration";

                Directory.CreateDirectory(gsifolder);
                string gsifile = gsifolder + @"\gamestate_integration_hue.cfg";
                if (File.Exists(gsifile))
                {
                    return true;
                }

                string[] contentofgsifile =
                {
                    "\"Dota 2 Integration Configuration\"",
                    "{",
                    "    \"uri\"           \"http://localhost:4000/\"",
                    "    \"timeout\"       \"5.0\"",
                    "    \"buffer\"        \"0.1\"",
                    "    \"throttle\"      \"0.1\"",
                    "    \"heartbeat\"     \"30.0\"",
                    "    \"data\"",
                    "    {",
                    "        \"provider\"      \"1\"",
                    "        \"map\"           \"1\"",
                    "        \"player\"        \"1\"",
                    "        \"hero\"          \"1\"",
                    "        \"abilities\"     \"1\"",
                    "        \"items\"         \"1\"",
                    "    }",
                    "}",

                };

                File.WriteAllLines(gsifile, contentofgsifile);
            }
            else
            {
                this.infoText.Text = "Registry key for steam not found, cannot create Gamestate Integration file :(";
                return false;
            }
            return true;
        }
    }
}
