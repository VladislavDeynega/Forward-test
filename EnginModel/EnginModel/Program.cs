using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnginModel
{
    class Program
    {
        static void Main(string[] args)
        {
            int Tsr = 0;
            float I = 10;     
            int[] M = { 20, 75, 100, 105, 75, 0 };
            int[] V = { 0, 75, 150, 200, 250, 300 };
            float Tper = 110;
            float Hm = 0.01f;
            float Hv = 0.0001f;
            float C = 0.1f;

            Console.WriteLine("Взять параметры из файла? y/n");
            ConsoleKey reply = Console.ReadKey().Key;
            if (reply == ConsoleKey.Y)
            {
                if (Config_from_file("configuration.txt") != null)
                {
                    float[] mas_param = Config_from_file("configuration.txt");
                    I = mas_param[0];
                    Tper = mas_param[1];
                    Hm = mas_param[2];
                    Hv = mas_param[3];
                    C = mas_param[4];
                }
               else
                {
                    Console.WriteLine("\nБудут взяты стандартные параметры");
                    reply = ConsoleKey.N;
                }
            }
            else if (reply != ConsoleKey.N && reply != ConsoleKey.Y)
            {
                Console.WriteLine("\nНекорректный ответ");
                Console.ReadKey();
                Environment.Exit(0);
            }           
          
            Console.WriteLine("\nВведите значение температуры среды");
            
            try 
            {
                Tsr = Int32.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Некорректное значение \nАвтоматически задано значение 0");           
            }
            Engine eng = new Engine(I, M, V, Tper, Hm, Hv, C, Tsr);

            eng.Peregrev += Eng_Peregrev;
            eng.CurrentTdv += Eng_CurrentTdv;
            eng.Start();
            Console.ReadKey();
        }

        public static float[] Config_from_file(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path);
                string line;
                List<float> Parameters = new List<float>();

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    char equals = '=';
                    int indexOfChar = line.IndexOf(equals);
                    line = line.Substring(indexOfChar + 1);
                    Parameters.Add(Convert.ToSingle(line));
                }
                sr.Close();
                float[] mas = Parameters.ToArray();
                return mas;
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("\nФайл не найден");
                return null;
            }            
            catch (FormatException)
            {
                Console.WriteLine("\nНарушена структура данных файла");
                return null;
            }
        }
        private static void Eng_CurrentTdv(string T)
        {
            Console.WriteLine("Температура двигателя: " + T);
        }

        private static void Eng_Peregrev(int obj)
        {
            Console.WriteLine("Со старта двигателя прошло " + obj + " секунд");
        }

    }
}
