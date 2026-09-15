using System;
using System.IO;

namespace IgnatovAS_CatApplication
{
    public class CatException : ArgumentException
    {
        public CatException(string message) : base(message) { }
    }

    public abstract class Cat
    {
        public abstract int Fluffiness { get; }
        public abstract string FluffinessCheck();
        public override string ToString()
        {
            return "A cat with fluffiness: {Fluffiness}";
        }
    }

    public class Tiger : Cat
    {
        private readonly int _fluffiness;
        public override int Fluffiness => _fluffiness;
        public double Weight { get; }

        public Tiger(double weight, int fluffiness = 50)
        {
            bool isWeightInvalid = weight < 75.0 || weight > 140.0;
            bool isFluffinessInvalid = fluffiness < 0 || fluffiness > 100;

            if (isWeightInvalid && isFluffinessInvalid)
            {
                throw new CatException("Unable to create a tiger with weight: {weight}. Unable to create a tiger with fluffiness {fluffiness}");
            }
            if (isWeightInvalid)
            {
                throw new CatException("Unable to create a tiger with weight: {weight}");
            }
            if (isFluffinessInvalid)
            {
                throw new CatException("Unable to create a tiger with fluffiness {fluffiness}");
            }

            Weight = weight;
            _fluffiness = fluffiness;
        }

        public override string FluffinessCheck()
        {
            return "Kycb!";
        }

        public override string ToString()
        {
            return "A tiger with weight: {Weight} fluffiness: {Fluffiness}";
        }
    }

    public class CuteCat : Cat
    {
        private readonly int _fluffiness;
        public override int Fluffiness => _fluffiness;

        public CuteCat(int fluffiness = 50)
        {
            if (fluffiness < 0 || fluffiness > 140)
            {
                throw new CatException("Unable to create a cute cat with fluffiness: {fluffiness}");
            }

            _fluffiness = fluffiness;
        }

        public override string FluffinessCheck()
        {
            if (_fluffiness == 0) return "Sphynx";
            if (_fluffiness >= 1 && _fluffiness <= 20) return "Slightly";
            if (_fluffiness >= 21 && _fluffiness <= 50) return "Medium";
            if (_fluffiness >= 51 && _fluffiness <= 75) return "Heavy";
            return "OwO";
        }

        public override string ToString()
        {
            return "A cute cat with fluffiness: {Fluffiness}";
        }
    }

    class Program
    {
        private static readonly Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.Write("Enter the number of cats to generate: ");
            if (!uint.TryParse(Console.ReadLine(), out uint catCount))
            {
                return;
            }

            Cat[] cats = GenerateRandomCats(catCount);

            Console.Write("Enter file path (e.g., cats.txt): ");
            string filePath = Console.ReadLine();

            DisplayCatInfo(cats, filePath);

            Console.WriteLine("Done! Press any key to exit.");
            Console.ReadKey();
        }

        static Cat[] GenerateRandomCats(uint count)
        {
            Cat[] catsArray = new Cat[count];
            uint currentCount = 0;

            while (currentCount < count)
            {
                int catType = rnd.Next(0, 2);
                int randomFluffiness = rnd.Next(-20, 121);
                double randomWeight = rnd.Next(50, 161);

                try
                {
                    if (catType == 0)
                    {
                        catsArray[currentCount] = new Tiger(randomWeight, randomFluffiness);
                    }
                    else
                    {
                        catsArray[currentCount] = new CuteCat(randomFluffiness);
                    }
                    currentCount++;
                }
                catch (CatException ex)
                {
                    Console.WriteLine("[CatException caught]: {ex.Message}");
                }
            }

            return catsArray;
        }

        static void DisplayCatInfo(Cat[] catsArr, string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    Console.WriteLine("Cats Information");
                    foreach (Cat cat in catsArr)
                    {
                        string info = "{cat.ToString()} | Fluffiness Check: {cat.FluffinessCheck()}";
                        Console.WriteLine(info);
                        sw.WriteLine(info);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {ex.Message}");
            }
        }
    }
}
