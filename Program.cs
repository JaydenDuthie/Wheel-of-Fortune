namespace Wheel_of_Fortune
{
    public struct Records
    {
        public string Name;
        public string LastName;
        public string Interest;
        public int Score;
    }

    public struct PlayerSelected
    {
        public string Name;
        public string LastName;
        public string Interest;
        public int Score;
    }


    internal class Program
    {
        const string FilePath = "Assets/wheelOfFortune.txt";

        static void Main()
        {
            Records[] records = ReadFile();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("===== WHEEL OF FORTUNE =====");
                Console.WriteLine("1. List all contestants (sorted by last name)");
                Console.WriteLine("2. Update a contestant's interest");
                Console.WriteLine("3. Generate 10 finalists");
                Console.WriteLine("4. Play the game");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Print(records);
                        break;
                    case "2":
                        UpdateInterest(records);
                        break;
                    case "3":
                        PickPeople(records);
                        break;
                    case "4":
                        PlayGame(records);
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        static void Pause()
        {
            Console.WriteLine("\nPress [Enter] to continue");
            Console.ReadLine();
        }

        static Records[] ReadFile()
        {
            Records[] Data = new Records[22];
            StreamReader SR = new StreamReader(FilePath);
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

        static void SaveData(Records[] Data)
        {
            StreamWriter SW = new StreamWriter(FilePath);
            for (int count = 0; count < Data.Length; count++)
            {
                SW.WriteLine(Data[count].Name);
                SW.WriteLine(Data[count].LastName);
                SW.WriteLine(Data[count].Interest);
                SW.WriteLine(Data[count].Score);
            }
            SW.Close();
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
            PlayerSelected[] Player = new PlayerSelected[1];
            int PlayerTemp = rand.Next(Finalists.Length);
            Player[0].Name = People[Finalists[PlayerTemp]].Name;
            Player[0].LastName = People[Finalists[PlayerTemp]].LastName;
            Player[0].Interest = People[Finalists[PlayerTemp]].Interest;
            Player[0].Score = People[Finalists[PlayerTemp]].Score;
            Console.WriteLine($"{Player[0].Name.PadRight(23)} {Player[0].LastName.PadRight(23)} {Player[0].Interest.PadRight(13)} {Convert.ToString(Player[0].Score).PadLeft(7)}");
            Console.ReadLine ();
        }

        static void UpdateInterest(Records[] Data)
        {
            Console.Write("Enter the last name to find: ");
            string search = Console.ReadLine().Trim();
            bool found = false;
            int count = 0;

            while (count < Data.Length && !found)
            {
                if (Data[count].LastName.ToLower() == search.ToLower())
                {
                    found = true;
                    Console.WriteLine($"Found: {Data[count].Name} {Data[count].LastName} (interest: {Data[count].Interest})");
                    Console.Write("Enter the new interest: ");
                    Data[count].Interest = Console.ReadLine().Trim();
                    SaveData(Data);
                    Console.WriteLine("Saved to file!");
                }
                count++;
            }

            if (!found)
            {
                Console.WriteLine("No player with that last name.");
            }
            Pause();
        }

        static void PlayGame(Records[] Data)
        {
            Random rand = new Random();
            int PlayerIndex = rand.Next(Data.Length);
            Records Player = Data[PlayerIndex];
            Console.WriteLine($"Welcome {Player.Name} {Player.LastName}!");
            Console.WriteLine($"Your interest is: {Player.Interest}");
            Console.WriteLine($"Your current score is: {Player.Score}");
            Console.WriteLine("Press [Enter] to spin the wheel...");
            Console.ReadLine();
            int spinResult = rand.Next(1, 11); // Simulate a spin result between 1 and 10
            Console.WriteLine($"You spun a {spinResult}!");
            Player.Score += spinResult;
            Console.WriteLine($"Your new score is: {Player.Score}");
            // Update the player's score in the original data array
            Data[PlayerIndex].Score = Player.Score;
            SaveData(Data);
            Pause();
        }
    }
}
