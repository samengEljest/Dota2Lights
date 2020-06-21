using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaLightApp
{
    struct lightProperties
    {
        public string id;

        public int? hue;
        public int brightness;
        public double[] colorCoordinates;
        public RGBColor rgbColor;
        public bool on;

        public int? orgHue;
        public int orgBrightness;
        public double[] orgColorCoordinates;
        public RGBColor orgRgbColor;
        public bool orgOn;

        public lightProperties(Light light)
        {
            id = light.Id;

            orgHue = light.State.Hue;
            hue = orgHue;

            orgBrightness = light.State.Brightness;
            brightness = orgBrightness;

            orgColorCoordinates = light.State.ColorCoordinates;
            colorCoordinates = orgColorCoordinates;

            rgbColor = new RGBColor(0,0,0);
            orgRgbColor = rgbColor;

            on = light.State.On;
            orgOn = on;
        }

    }


    struct groupProperties
    {
        public List<lightProperties> lights;
        public String id;

        public groupProperties(String inputId)
        {
            id = inputId;
            lights = new List<lightProperties>();
        }
     }
}
