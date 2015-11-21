using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genetics;

namespace Colour
{
    class Colour
    {
        private float r = 0.0f;
        private float g = 0.0f;
        private float b = 0.0f;

        private float h = 0.0f;
        private float s = 0.0f;
        private float v = 0.0f;
        private bool HSVIsSet = false;

        public float R
        {
            get
            {
                return r;
            }
            set
            {
                this.HSVIsSet = false;
                this.r = value;
            }
        }
        public float G
        {
            get
            {
                return g;
            }
            set
            {
                this.HSVIsSet = false;
                this.g = value;
            }
        }
        public float B
        {
            get
            {
                return b;
            }
            set
            {
                this.HSVIsSet = false;
                this.b = value;
            }
        }
        public float H
        {
            get
            {
                if (!HSVIsSet)
                {
                    throw new Exception("HSV not set");
                }

                return h;
            }
        }
        public float S
        {
            get
            {
                if (!HSVIsSet)
                {
                    throw new Exception("HSV not set");
                }

                return s;
            }
        }
        public float V
        {
            get
            {
                if (!HSVIsSet)
                {
                    throw new Exception("HSV not set");
                }

                return v;
            }
        }

        public Colour(float r = 0.0f, float g = 0.0f, float b = 0.0f)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        private int ChannelAsInt(float channel)
        {
            int ch = (int)(channel * 255.0f);

            int result =  (ch < 0 ? 0 : (ch > 255 ? 255 : ch));

            return result;
        }

        public string ToSVGString()
        {
            int red   = this.ChannelAsInt(this.r);
            int green = this.ChannelAsInt(this.g);
            int blue  = this.ChannelAsInt(this.b);

            return string.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
        }

        //public void SetHSV(float h = 0.0f, float s = 0.0f, float v = 0.0f)
        //My simplification of the HSV colour space. The three channels are
        //sinusoidal with a 120deg phase between each, the hue defines the 
        //theta. After the channels are calculated for the hue the saturation is how close 
        //to 0.5 the new channels will be. The final stage V will simply 
        //multiply each channel with that value after H and S are calculated.
        public void SetHSV(float h = 0.0f, float s = 0.0f, float v = 0.0f)
        {
            float ph = (float)(Math.PI * (2.0 / 3.0));

            //hue
            float r = (float)(Math.Cos(h) + 1.0) / 2.0f;
            float g = (float)(Math.Cos(h + ph) + 1.0) / 2.0f;
            float b = (float)(Math.Cos(h + ph + ph) + 1.0) / 2.0f;

            //saturation
            r = (r * s) + (0.5f * (1.0f - s));
            g = (g * s) + (0.5f * (1.0f - s));
            b = (b * s) + (0.5f * (1.0f - s));

            //value
            r *= v;
            g *= v;
            b *= v;

            this.r = r;
            this.g = g;
            this.b = b;

            this.h = h;
            this.s = s;
            this.v = v;

            this.HSVIsSet = true;
        }
    }

    class Palette
    {
        public class Variety
        {
            public float hue = 0.0f;
            public float saturation = 0.0f;
            public float value = 0.0f;
        }

        private DNA genes;  //gives the palette repeatability and capability of breeding
        private float hue;          //the main hue in radians (red = 0)
        private float hueLeft;
        private float hueRight;
        private float degree;       //the angle in radians from the main hue for two other hues
        private Variety variation;  //how much the three colours are modified by HSV
        private Colour colour;
        private Colour colourLeft;
        private Colour colourRight;

        public Palette(DNA dna, float hue, float degree, Variety variation)
        {
            this.genes     = dna;
            this.hue       = hue;
            this.hueLeft   = hue - degree;
            this.hueRight  = hue + degree;

            this.degree    = degree;
            this.variation = variation;

            this.DefineColourPrimes();
        }

        public Palette(float hue, float degree, Variety variation)
        {
            this.genes = new DNA();
            this.hue = hue;
            this.hueLeft = hue - degree;
            this.hueRight = hue + degree;

            this.degree = degree;
            this.variation = variation;

            this.DefineColourPrimes();
        }

        private void DefineColourPrimes()
        {
            this.colour = new Colour();
            this.colour.SetHSV(this.hue, 1.0f, 0.5f);

            this.colourLeft = new Colour();
            this.colourLeft.SetHSV(this.hueLeft, 1.0f, 0.5f);

            this.colourRight = new Colour();
            this.colourRight.SetHSV(this.hueRight, 1.0f, 0.5f);
        }

        public Colour GetColour(int index)
        {
            int oldIndex = this.genes.Index;

            this.genes.Index = index;

            Colour newCol = this.GetColour();

            //restore the gene index
            this.genes.Index = oldIndex;

            return newCol;
        }

        public Colour GetColour()
        {
            Colour newCol;

            float choice = this.genes.Next() * 3000.0f;

            //choose which colour to make a variant from
            if (choice < 1000)
            {
                newCol = new Colour();
                newCol.SetHSV(this.colour.H, this.colour.S, this.colour.V);
            }
            else if (choice > 2000)
            {
                newCol = new Colour();
                newCol.SetHSV(this.colourLeft.H, this.colourLeft.S, this.colourLeft.V);
            }
            else
            {
                newCol = new Colour();
                newCol.SetHSV(this.colourRight.H, this.colourRight.S, this.colourRight.V);
            }

            float newHue = newCol.H + (this.genes.Next() * this.variation.hue) - (this.variation.hue / 2.0f);
            float newSat = newCol.S + (this.genes.Next() * this.variation.saturation) - (this.variation.saturation / 2.0f);
            float newVal = newCol.V + (this.genes.Next() * this.variation.value) - (this.variation.value / 2.0f);

            //constrain sat and val to range(0-1)
            newSat = (float)Math.Abs(newSat - Math.Floor(newSat));
            newVal = (float)Math.Abs(newSat - Math.Floor(newSat));

            newCol.SetHSV(newHue, newSat, newVal);

            return newCol;
        }
    }

}
