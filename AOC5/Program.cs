namespace AOC5
{
    internal class Program
    {
        static Dictionary<string, Converter> converters = new Dictionary<string, Converter>();
        static long[] seeds;
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            var seedParts = lines[0].Split(": ", 2);
            seeds = seedParts[1].Split(' ').Select(long.Parse).ToArray();

            Converter currentConverter = null;

            foreach (var line in lines)
            {
                if(line.StartsWith("seeds:") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (char.IsLetter(line[0]))
                {
                    var converterParts = line.Split(' ')[0].Split("-to-");
                    if(currentConverter != null)
                    {
                        converters[currentConverter.ConvertsFrom] = currentConverter;
                    }

                    currentConverter = new Converter()
                    {
                        ConvertsFrom = converterParts[0],
                        ConvertsTo = converterParts[1]
                    };
                    continue;
                }

                var parts = line.Split(' ', 3).Select(long.Parse).ToArray();
                currentConverter.Convertions.Add(new Entry()
                {
                    DstStart = parts[0],
                    SrcStart = parts[1],
                    Range = parts[2]
                });
            }
            converters[currentConverter.ConvertsFrom] = currentConverter;

            long minLoc = long.MaxValue;
            foreach (var seed  in seeds)
            {
                var currentThing = new Thing() { Id = seed, Type = "seed"};
                while(currentThing.Type != "location")
                {
                    currentThing = converters[currentThing.Type].Convert(currentThing);
                }
                minLoc = Math.Min(minLoc, currentThing.Id);
                Console.WriteLine($"{seed} -> {currentThing.Id}");
            }

            Console.WriteLine($"Min loc: {minLoc}");
        }
    }
}