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
                Console.WriteLine("5. Show top five contestants (sorted by score)");
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
                    case "5":
                        TopFive(records);
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
                Console.WriteLine($"{Data[count].Name.PadRight(23)} {Data[count].LastName.PadRight(23)} {Data[count].Interest.PadRight(20)} {Convert.ToString(Data[count].Score).PadLeft(10)}");
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
            Console.WriteLine($"{People[PlayerIndex].Name.PadRight(23)} {People[PlayerIndex].LastName.PadRight(23)} {People[PlayerIndex].Interest.PadRight(20)} {Convert.ToString(People[PlayerIndex].Score).PadLeft(10)}");
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

            while (!WordComplete(revealed))
            {
                Console.Clear();
                Console.WriteLine($"Player: {Data[PlayerIndex].Name} {Data[PlayerIndex].LastName}\tScore: ${score}");
                Console.WriteLine($"\n{Show(word, revealed)}");
                Console.WriteLine($"\nLetters used: {guessedLetters}");

                // Spin the wheel
                int spin = wheel[rand.Next(wheel.Length)];
                if (spin < 0)
                {
                    score += spin;
                    Console.WriteLine($"\nThe wheel landed on LOSE ${-spin}! Score is now ${score}.");
                    Pause();
                }
                else
                {
                    Console.WriteLine($"\nThe wheel landed on ${spin}.");
                    Console.Write("Guess a letter (vowels cost $250 once each): ");
                    string input = Console.ReadLine().Trim().ToUpper();

                    if (input.Length != 1 || !char.IsLetter(input[0]))  // Check if the input is a single character and is a letter
                    {
                        Console.WriteLine("Please enter a single letter. Turn lost.");
                        Pause();
                    }
                    else
                    {
                        char letter = input[0];


                        /*
                        - https://www.geeksforgeeks.org/c-sharp/c-sharp-string-contains-method/
                        - Uses the Contains method - checks if a string (guessedLetters) contains a spcific character (letter)
                        - and returns a boolean value (true or false)
                        */
                        if (guessedLetters.Contains(letter))
                        {
                            Console.WriteLine("You already used that letter. Turn lost.");
                            Pause();
                        }
                        else
                        {
                            guessedLetters += letter + " ";
                            int occurrences = RevealLetter(word, revealed, letter);
                            bool IsVowel = "AEIOU".Contains(letter); // Uses the Contains method to check if "AEIOU" contains the letter

                            if (IsVowel)
                            {
                                score -= 250;
                                Console.WriteLine($"{occurrences} x {letter}. Vowel costs $250.");
                            }
                            else if (occurrences != 0 && !IsVowel)
                            {
                                int earned = spin * occurrences;
                                score += earned;
                                Console.WriteLine($"{occurrences} x {letter}! You earn ${earned}.");
                            }
                            else
                            {
                                Console.WriteLine($"No {letter}s in the word.");
                            }
                            Pause();
                        }
                    }
                }
            }

            Console.Clear();
            Console.WriteLine($"The word was {word}!");
            Console.WriteLine($"{Data[PlayerIndex].Name} finishes with ${score}.");

            Data[PlayerIndex].Score = score;
            SaveData(Data);
            Console.WriteLine("Score saved to file.\n");
            Pause();
            TopFive(Data);
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

        static void TopFive(Records[] Data)
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
    }
}