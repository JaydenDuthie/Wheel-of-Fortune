using System;

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


        static string[] words =
        {
            "PROGRAMMING", "COMPUTATION", "ADVENTURES", "CHRISTCHURCH", "DEVELOPMENT",
            "ALGORITHMS", "KEYBOARDING", "SOFTWARE ENGINEER".Replace(" ", ""), "PLAYGROUNDS", "BREAKTHROUGH",
            "WATERMELONS", "ELEPHANTINE", "TELEVISIONS", "GOVERNMENTS", "MOUNTAINEERING"
        };

        static int[] wheel =
        {
            300, 350, 400, 450, 500, 600, 700, 800, 900, 1000, 1200, 1500,
            1800, 2000, 2200, 2500, 2800, 3000, 3500, 4000, 4500, 5000,
            -2000, -3000
        };

        static void Main()
        {
            Records[] records = ReadFile();
            int PlayerIndex = -1;
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
                        PickPeople(records, ref PlayerIndex);
                        break;
                    case "4":
                        PlayGame(records, ref PlayerIndex);
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

        static void PickPeople(Records[] People, ref int PlayerIndex)
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

            int PlayerTemp = rand.Next(Finalists.Length);
            PlayerIndex = Finalists[PlayerTemp];
            Console.WriteLine($"{People[PlayerIndex].Name.PadRight(23)} {People[PlayerIndex].LastName.PadRight(23)} {People[PlayerIndex].Interest.PadRight(13)} {Convert.ToString(People[PlayerIndex].Score).PadLeft(7)}");
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

        static void PlayGame(Records[] Data, ref int PlayerIndex)
        {
            if (PlayerIndex == -1)
            {
                Console.WriteLine("Select a player first (option 3).");
                Pause();
                return;
            }

            Random rand = new Random();
            string word = words[rand.Next(words.Length)];
            bool[] revealed = new bool[word.Length];
            string guessedLetters = "";
            int score = 0;

            Records player = Data[PlayerIndex];
            Console.WriteLine($"Welcome {player.Name} {player.LastName}!");
            Console.WriteLine($"Your interest is: {player.Interest}");
            Console.WriteLine($"Your current score is: {player.Score}");
            Console.WriteLine("Press [Enter] to spin the wheel...");
            Console.ReadLine();










            int spin = rand.Next(1, 24);

            Console.WriteLine($"Your new score is: {player.Score}");
            // Update the player's score in the original data array
            Data[PlayerIndex].Score = player.Score;
            SaveData(Data);
            Pause();
        }


        static bool WordComplete(bool[] revealed)
        {
            for (int i = 0; i < revealed.Length; i++)
                if (!revealed[i])
                {
                    return false;
                }
            return true;
        }


        static string Show(string word, bool[] revealed)
        {
            string display = "";
            for (int i = 0; i < word.Length; i++)
                if (revealed[i])
                {
                    display += word[i] + " ";
                }
                else
                {
                    display += "_ ";
                }
            return display;
        }


        // Reveal every occurrence of letter, return how many there were
        static int RevealLetter(string word, bool[] revealed, char letter)
        {
            int count = 0;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == letter)
                {
                    revealed[i] = true;
                    count++;
                }
            }
            return count;
        }
    }
}
