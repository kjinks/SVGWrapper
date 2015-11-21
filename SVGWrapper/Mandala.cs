using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Genetics;
using Geometry;
using SVGWrapper;
using Colour;

namespace ProceduralArt
{
    class Mandala
    {
        public const float DEFAULT_CANVAS_SIZE  = 1000.0f;
        public const float MIDX                 = DEFAULT_CANVAS_SIZE / 2.0f;
        public const float MIDY                 = DEFAULT_CANVAS_SIZE / 2.0f;

        private const float MAX_STROKE_WIDTH    = 5.0f;

        private const int  MAX_HARMONIC         = 12;

        private const int  DEFAULT_CIRCLE_INDEX = 3;
        private const int  CIRCLE_RING_RANGE    = 9;
        private const int  CIRCLE_RING_MIN      = 3;
        private const int  CIRCLE_CIRC_RANGE    = 9;
        private const int  CIRCLE_CIRC_MIN      = 3;
        private const float CIRC_RADII_MAX      = DEFAULT_CANVAS_SIZE / 4.0f;

        private DNA genes;
        private int harmonic;

        public Mandala(int seed = 1)
        {
            this.genes = new DNA(seed : seed);
            this.Reset();
        }

        public void Reset()
        {
            this.genes.Index = 0;
            this.harmonic = (int)(this.genes.Next() * MAX_HARMONIC);
        }

        public void Circles(SVG svgDoc, XmlNode parent, Attr attr)
        {
            int oldIndex = this.genes.Index;

            this.genes.Index = DEFAULT_CIRCLE_INDEX;

            XmlNode mainGroup = svgDoc.Group(parent, attr);

            int numRings = (int)(this.genes.Next() * (float)CIRCLE_RING_RANGE) + CIRCLE_RING_MIN;
            int harmonic = this.harmonic;

            for (int r = 0; r < numRings; r++)
            {
                int numCircs = (int)((this.genes.Next() * (float)CIRCLE_CIRC_RANGE) + CIRCLE_CIRC_MIN) * harmonic;
                float circRadii = this.genes.Next() * CIRC_RADII_MAX;
                float ringRadii = this.genes.Next() * CIRC_RADII_MAX;

                float angCirc = (float)(Math.PI * 2.0) / (float)numCircs;

                float ringPhase = this.genes.Next() < 0.5f ? 0.0f : (angCirc * 0.5f);

                float strokeWidth = this.genes.Next() * MAX_STROKE_WIDTH;

                for (int c = 0; c < numCircs; c++)
                {
                    float cx = (float)Math.Sin(angCirc * c) * ringRadii + MIDX;
                    float cy = (float)Math.Cos(angCirc * c) * ringRadii + MIDY;

                    svgDoc.Circle(mainGroup, cx, cy, circRadii, new Attr() {
                                                                                {"fill",         "none"},
                                                                                {"stroke",       "black"},
                                                                                {"stroke-width", strokeWidth.ToString("0.###")}
                                                                            });
                }
            }

            this.genes.Index = oldIndex;
        }
    }
}
