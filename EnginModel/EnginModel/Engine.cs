using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EnginModel
{
    class Engine
    {
        public event Action<int> Peregrev;
        public event Action<string> CurrentTdv;
        public float I { get; set; }
        public int[] M { get; set; }
        public int[] V { get; set; }

        public float Tper { get; set; }
        public float Tsr { get; set; }
        public float Tdv { get; set; }

        public float Hm { get; set; }
        public float Hv { get; set; }
        public float C { get; set; }

        public float Speed = 0;

        public float Vc { get => C * (Tsr - Tdv); }


        public Engine(float I, int[] M, int[] V, float Tper, float Hm, float Hv, float C, int Tsr)
        {
            this.I = I;
            this.M = M;
            this.V = V;
            this.Tper = Tper;
            this.Hm = Hm;
            this.Hv = Hv;
            this.C = C;
            this.Tsr = Tsr;

            Tdv = Tsr;
        }

        public float Vh(int i)
        {
            return M[i] * Hm + V[i] * V[i] * Hv;
        }

        public float Acceleration(int i)
        {
            return M[i] / I;
        }

        private void Simulate()
        {
            int sec = 0;
            int indexM = 0;
            List<string> curr_Tdv = new List<string>();
            while(Tdv <= Tper)
            {                
                float curr_vh = Vh(indexM);

                Tdv = Tdv + curr_vh - Vc;
                curr_Tdv.Add(Tdv.ToString());
                Speed += Acceleration(indexM);

                if (Speed >= V[indexM]) indexM++;

                sec++;               
                //CurrentTdv?.Invoke(Tdv);
            }
            string[] mas_curr_tdv = curr_Tdv.ToArray();

            for (int i = 0; i < mas_curr_tdv.Length; i++)
            {
                string sec_preregrev = "°С\tcекунд до перегрева: " + Convert.ToSingle(sec-i-1);
                mas_curr_tdv[i] += sec_preregrev;
                Thread.Sleep(200);
                CurrentTdv?.Invoke(mas_curr_tdv[i]);
            }
            Peregrev?.Invoke(sec);
        }

        public void Start()
        {
            Simulate();            
        }
    }
}
