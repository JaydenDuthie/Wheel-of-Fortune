namespace Wheel_of_Fortune
{
    public struct Records
    {
        public string Name;
        public string LastName;
        public string Interest;
        public int Score;
    }

    internal class Program
    {
        static void Main()
        {
            Records[] records = ReadFile();
            PickPeople(records);
        }

        static Records[] ReadFile()
        {
            Records[] Data = new Records[22];
            StreamReader SR = new StreamReader("Assets/wheelOfFortune.txt");
            for (int count = 0; count < Data.Length; count++)
            {
                Data[count].Name = SR.ReadLine();
                Data[count].LastName = SR.ReadLine();
                Data[count].Interest = SR.ReadLine();
                Data[count].Score = Convert.ToInt32(SR.ReadLine());
            }
            SR.Close();

            for (int i = 0; i < Data.Length - 1; i++)
            {
                for (int pos = 0; pos < Data.Length - 1; pos++)
                {
                    if (Data[pos + 1].LastName.CompareTo(Data[pos].LastName) < 0)
                    {
                        Records temp;
                        temp = Data[pos + 1];
                        Data[pos + 1] = Data[pos];
                        Data[pos] = temp;
                    }
                }
            }

            Print(Data);
            return Data;
        }

        static void Print(Records[] Data)
        {
            for (int count = 0; count < Data.Length; count++)
            {
                Console.WriteLine($"{Data[count].Name.PadRight(23)} {Data[count].LastName.PadRight(23)} {Data[count].Interest.PadRight(13)} {Convert.ToString(Data[count].Score).PadLeft(7)}");
            }
            Console.WriteLine("\nPress [Enter] to continue");
            Console.ReadLine();
        }

        static void PickPeople(Records[] People)
        {
            Random rand = new Random();
            int[] Finalists = new int[10];
            int temp;
            bool InArray;

            Console.WriteLine("Generating the finalists...");
            Console.ReadLine();

            for (int i = 0; i < Finalists.Length; i++)
            {
                temp = rand.Next(People.Length);
                InArray = true;
                while (InArray)
                {
                    InArray = false;
                    for (int slot = 0; slot < i; slot++)
                    {
                        if (temp == Finalists[slot])
                        {
                            temp = rand.Next(People.Length);
                            InArray = true;
                        }
                    }
                    Finalists[i] = temp;
                }
            }

            for (int i = 0; i < Finalists.Length; i++)
            {
                Console.WriteLine($"{People[Finalists[i]].Name.PadRight(15)} {People[Finalists[i]].LastName}");
            }

            Console.WriteLine("\n\n");
            int plyr = rand.Next(Finalists.Length);
            string Player = People[Finalists[plyr]].Name + " " + People[Finalists[plyr]].LastName;
            Console.WriteLine($"{Player}");
            Console.ReadLine ();
        }
    }
}
