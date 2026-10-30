namespace Wheel_of_Fortune
{
    public struct Records
    {
        public string LastName;
        public string Name;
        public string Interest;
        public int Score;
    }

    internal class Program
    {
        static void Main()
        {
            Records[] Data = new Records[22];
            StreamReader SR = new StreamReader("Assets/wheelOfFortune.txt");
            for (int count = 0; count < Data.Length; count++)
            {
                Data[count].LastName = SR.ReadLine();
                Data[count].Name = SR.ReadLine();
                Data[count].Interest = SR.ReadLine();
                Data[count].Score = Convert.ToInt32(SR.ReadLine());
            }
            SR.Close();

            for (int i = 0; i < Data.Length - 1; i++)
            {
                for (int pos = 0; pos < Data.Length - 1; pos++)
                {
                    if (Data[pos + 1].Name.CompareTo(Data[pos].Name) < 0)
                    {
                        Records temp;
                        temp = Data[pos + 1];
                        Data[pos + 1] = Data[pos];
                        Data[pos] = temp;
                    }
                }
            }

            Print(Data);
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
    }
}
