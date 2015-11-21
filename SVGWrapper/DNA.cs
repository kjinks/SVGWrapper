using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics
{
    class DNA
    {
        private int numAmino     = 256;//number of symbols
        private int index        = 0;  //current pointer
        private int chromoLength = 5;  //number of symbols to read to produce a value
        private int[] sequence;        //array of symbols as ints
        private Random generator;      //random number generator for this instance

        public DNA(int length = 100, int seed = 1, int chromoLength = 5, int numAmino = 256)
        {
            this.index        = 0;
            this.numAmino     = numAmino;
            this.sequence     = new int[length];
            this.generator    = new Random(seed);
            this.chromoLength = chromoLength;

            this.PopulateSequence();
        }

        //private void PopulateSequence()
        //fills the sequence array with random non-negative numbers 
        //less than this.numAmino
        private void PopulateSequence()
        {
            for (int i = 0; i < this.sequence.Count(); i++)
            {
                this.sequence[i] = this.generator.Next(this.numAmino);
            }
        }

        public float Read(int index = 0, int length = 5)
        {
            float result = 0.0f;

            int seqLength = this.sequence.Count();

            for (int i = 0; i < length; i++)
            {
                float amino = this.sequence[(index + i) % seqLength];
                result += amino * (float)Math.Pow(this.numAmino, i);
            }

            result /= (float)Math.Pow(this.numAmino, length);

            return result;
        }

        public float Next()
        {
            float result = this.Read(this.index, this.chromoLength);
            int seqLength = this.sequence.Count();

            this.index = (this.index + this.chromoLength) % seqLength;
            
            return result;
        }

        public float Next(float min, float max)
        {
            return ((max - min) * this.Next()) + min;
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }
    }
}
