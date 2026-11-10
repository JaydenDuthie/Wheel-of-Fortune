/*
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
} */








namespace Wheel_of_Fortune
{
    // One record per person in the data file
    public struct Records
    {
        public string LastName;
        public string Name;
        public string Interest;
        public int Score;
    }

    internal class Program
    {
        const string FILE_PATH = "Assets/wheelOfFortune.txt";

        static Random rng = new Random();
        static int[] finalists = new int[0];   // indexes (into Data) of the 10 finalists
        static int playerIndex = -1;           // index (into Data) of the chosen player

        // Words must be at least 10 characters long
        static string[] words =
        {
            "PROGRAMMING", "COMPUTATION", "ADVENTURES", "CHRISTCHURCH", "DEVELOPMENT",
            "ALGORITHMS", "KEYBOARDING", "SOFTWARE ENGINEER".Replace(" ", ""), "PLAYGROUNDS", "BREAKTHROUGH",
            "WATERMELONS", "ELEPHANTINE", "TELEVISIONS", "GOVERNMENTS", "MOUNTAINEERING"
        };

        // 24 wheel segments: 22 cash values ($300 to $5000), Lose $2000, Lose $3000
        static int[] wheel =
        {
            300, 350, 400, 450, 500, 600, 700, 800, 900, 1000, 1200, 1500,
            1800, 2000, 2200, 2500, 2800, 3000, 3500, 4000, 4500, 5000,
            -2000, -3000
        };

        // ---------------------------------------------------------------- MAIN / MENU
        static void Main()
        {
            Records[] Data = LoadData();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("===== WHEEL OF FORTUNE =====");
                Console.WriteLine("1. List all contestants (sorted by last name)");
                Console.WriteLine("2. Update a contestant's interest");
                Console.WriteLine("3. Generate 10 finalists");
                Console.WriteLine("4. Select the player");
                Console.WriteLine("5. Play the game");
                Console.WriteLine("6. Show top five scores");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SortByLastName(Data);
                        Print(Data);
                        break;
                    case "2":
                        UpdateInterest(Data);
                        break;
                    case "3":
                        GenerateFinalists(Data);
                        break;
                    case "4":
                        SelectPlayer(Data);
                        break;
                    case "5":
                        PlayGame(Data);
                        break;
                    case "6":
                        ShowTopFive(Data);
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

        // ---------------------------------------------------------------- FILE HANDLING
        // File order per person: first name, last name, interest, score
        static Records[] LoadData()
        {
            string[] lines = File.ReadAllLines(FILE_PATH);
            int people = lines.Length / 4;           // 4 lines per person
            Records[] Data = new Records[people];

            for (int count = 0; count < people; count++)
            {
                Data[count].Name = lines[count * 4];
                Data[count].LastName = lines[count * 4 + 1];
                Data[count].Interest = lines[count * 4 + 2];
                int.TryParse(lines[count * 4 + 3], out Data[count].Score);
            }
            SortByLastName(Data);
            return Data;
        }

        static void SaveData(Records[] Data)
        {
            StreamWriter SW = new StreamWriter(FILE_PATH);
            for (int count = 0; count < Data.Length; count++)
            {
                SW.WriteLine(Data[count].Name);
                SW.WriteLine(Data[count].LastName);
                SW.WriteLine(Data[count].Interest);
                SW.WriteLine(Data[count].Score);
            }
            SW.Close();
        }

        // ---------------------------------------------------------------- 1. LIST / SORT
        // Bubble sort by last name (then first name if last names match)
        static void SortByLastName(Records[] Data)
        {
            for (int i = 0; i < Data.Length - 1; i++)
            {
                for (int pos = 0; pos < Data.Length - 1 - i; pos++)
                {
                    int cmp = string.Compare(Data[pos + 1].LastName, Data[pos].LastName, StringComparison.OrdinalIgnoreCase);
                    if (cmp == 0)
                        cmp = string.Compare(Data[pos + 1].Name, Data[pos].Name, StringComparison.OrdinalIgnoreCase);
                    if (cmp < 0)
                    {
                        Records temp = Data[pos + 1];
                        Data[pos + 1] = Data[pos];
                        Data[pos] = temp;
                    }
                }
            }
        }

        static void Print(Records[] Data)
        {
            Console.Clear();
            Console.WriteLine($"{"#",-4}{"First name",-23} {"Last name",-23} {"Interest",-20} {"Score",7}");
            for (int count = 0; count < Data.Length; count++)
            {
                Console.WriteLine($"{(count + 1),-4}{Data[count].Name,-23} {Data[count].LastName,-23} {Data[count].Interest,-20} {Data[count].Score,7}");
            }
            Pause();
        }

        // ---------------------------------------------------------------- 2. UPDATE INTEREST
        static void UpdateInterest(Records[] Data)
        {
            Console.Write("Enter the last name to find: ");
            string search = Console.ReadLine().Trim();
            bool found = false;

            for (int count = 0; count < Data.Length; count++)
            {
                if (Data[count].LastName.Equals(search, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    Console.WriteLine($"Found: {Data[count].Name} {Data[count].LastName} (interest: {Data[count].Interest})");
                    Console.Write("Enter the new interest: ");
                    Data[count].Interest = Console.ReadLine().Trim();
                    SaveData(Data);
                    Console.WriteLine("Saved to file.");
                    break;
                }
            }
            if (!found)
                Console.WriteLine("No contestant with that last name.");
            Pause();
        }

        // ---------------------------------------------------------------- 3. FINALISTS
        // 10 unique random numbers from 1 to number of people
        static void GenerateFinalists(Records[] Data)
        {
            finalists = new int[10];
            playerIndex = -1;

            for (int i = 0; i < finalists.Length; i++)
            {
                int number;
                do
                {
                    number = rng.Next(1, Data.Length + 1);
                } while (Contains(finalists, i, number - 1));   // reject duplicates
                finalists[i] = number - 1;
            }

            Console.WriteLine("The finalists are:");
            for (int i = 0; i < finalists.Length; i++)
                Console.WriteLine($"{i + 1,2}. {Data[finalists[i]].Name} {Data[finalists[i]].LastName}");
            Pause();
        }

        // Is value already in the first 'count' items of the array?
        static bool Contains(int[] array, int count, int value)
        {
            for (int i = 0; i < count; i++)
                if (array[i] == value) return true;
            return false;
        }

        // ---------------------------------------------------------------- 4. PLAYER
        static void SelectPlayer(Records[] Data)
        {
            if (finalists.Length == 0)
            {
                Console.WriteLine("Generate the finalists first (option 3).");
            }
            else
            {
                playerIndex = finalists[rng.Next(finalists.Length)];
                Console.WriteLine($"The player is: {Data[playerIndex].Name} {Data[playerIndex].LastName}!");
            }
            Pause();
        }

        // ---------------------------------------------------------------- 5. THE GAME
        static void PlayGame(Records[] Data)
        {
            if (playerIndex < 0)
            {
                Console.WriteLine("Select a player first (option 4).");
                Pause();
                return;
            }

            string word = words[rng.Next(words.Length)];
            bool[] revealed = new bool[word.Length];
            string guessedLetters = "";
            int score = 0;

            while (!WordComplete(revealed))
            {
                Console.Clear();
                Console.WriteLine($"Player: {Data[playerIndex].Name} {Data[playerIndex].LastName}    Score: ${score}");
                Console.WriteLine($"\n{ShowWord(word, revealed)}");
                Console.WriteLine($"\nLetters used: {guessedLetters}");

                // Spin the wheel
                int spin = wheel[rng.Next(wheel.Length)];
                if (spin < 0)
                {
                    score += spin;   // spin is negative
                    Console.WriteLine($"\nThe wheel landed on LOSE ${-spin}! Score is now ${score}.");
                    Pause();
                    continue;
                }
                Console.WriteLine($"\nThe wheel landed on ${spin}.");
                Console.Write("Guess a letter (vowels cost $250 once each): ");
                string input = Console.ReadLine().Trim().ToUpper();

                if (input.Length != 1 || !char.IsLetter(input[0]))
                {
                    Console.WriteLine("Please enter a single letter. Turn lost.");
                    Pause();
                    continue;
                }
                char letter = input[0];

                if (guessedLetters.Contains(letter))
                {
                    Console.WriteLine("You already used that letter. Turn lost.");
                    Pause();
                    continue;
                }
                guessedLetters += letter + " ";

                int occurrences = RevealLetter(word, revealed, letter);

                if (occurrences == 0)
                {
                    Console.WriteLine($"No {letter}s in the word.");
                }
                else if (IsVowel(letter))
                {
                    score -= 250;   // flat charge, once per vowel
                    Console.WriteLine($"{occurrences} x {letter}. Vowel costs $250.");
                }
                else
                {
                    int earned = spin * occurrences;
                    score += earned;
                    Console.WriteLine($"{occurrences} x {letter}! You earn ${earned}.");
                }
                Pause();
            }

            Console.Clear();
            Console.WriteLine($"The word was {word}!");
            Console.WriteLine($"{Data[playerIndex].Name} finishes with ${score}.");

            Data[playerIndex].Score = score;
            SaveData(Data);
            Console.WriteLine("Score saved to file.\n");
            ShowTopFive(Data);
        }

        static bool IsVowel(char c)
        {
            return "AEIOU".Contains(c);
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

        static bool WordComplete(bool[] revealed)
        {
            for (int i = 0; i < revealed.Length; i++)
                if (!revealed[i]) return false;
            return true;
        }

        static string ShowWord(string word, bool[] revealed)
        {
            string display = "";
            for (int i = 0; i < word.Length; i++)
                display += revealed[i] ? word[i] + " " : "_ ";
            return display;
        }

        // ---------------------------------------------------------------- TOP FIVE
        static void ShowTopFive(Records[] Data)
        {
            // Sort a copy by score (highest first) so the main array stays sorted by name
            Records[] copy = (Records[])Data.Clone();
            for (int i = 0; i < copy.Length - 1; i++)
            {
                for (int pos = 0; pos < copy.Length - 1 - i; pos++)
                {
                    if (copy[pos + 1].Score > copy[pos].Score)
                    {
                        Records temp = copy[pos + 1];
                        copy[pos + 1] = copy[pos];
                        copy[pos] = temp;
                    }
                }
            }

            Console.WriteLine("===== TOP FIVE =====");
            for (int i = 0; i < 5 && i < copy.Length; i++)
                Console.WriteLine($"{i + 1}. {copy[i].Name} {copy[i].LastName}".PadRight(35) + $"${copy[i].Score}");
            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nPress [Enter] to continue");
            Console.ReadLine();
        }
    }
}