namespace Day1
{
    internal class Day1
    {
        internal class Elf
        {
            public List<int> Rations = new List<int>();
            public int Total = 0;
            public static List<Elf> ParseAll(string path)
            {
                var lines = File.ReadAllLines("Input.txt");
                List<Elf> elves = new List<Elf>();
                Elf elf = new Elf();
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        elves.Add(elf);
                        elf = new Elf();
                        continue;
                    }
                    var ration = int.Parse(line);
                    elf.Rations.Add(ration);
                    elf.Total += ration;
                }
                return elves;
            }
        }

        static void Main(string[] args)
        {
            var elves = Elf.ParseAll("Input.txt");
            Console.WriteLine($"Most total calories: {Part1(elves)}");
            Console.WriteLine($"Top 3 most total calories: {Part2(elves)}");
        }

        static int Part1(IEnumerable<Elf> elves)
        {
            return elves.Max(e => e.Total);
        }

        static int Part2(IEnumerable<Elf> elves)
        {
            return elves.OrderByDescending(e => e.Total).Take(3).Sum(e => e.Total);
        }
    }
}