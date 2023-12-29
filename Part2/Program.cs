using System;

namespace Part2
{
    internal class Program
    {
        static Dictionary<string, Converter> converters = new Dictionary<string, Converter>();
        static Dictionary<string, Converter> backwardsConverters = new Dictionary<string, Converter>();
        static long[] seeds;
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            var seedParts = lines[0].Split(": ", 2);
            seeds = seedParts[1].Split(' ').Select(long.Parse).ToArray();

            Converter currentConverter = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("seeds:") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (char.IsLetter(line[0]))
                {
                    var converterParts = line.Split(' ')[0].Split("-to-");
                    if (currentConverter != null)
                    {
                        converters[currentConverter.ConvertsFrom] = currentConverter;
                        backwardsConverters[currentConverter.ConvertsTo] = currentConverter;
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
            backwardsConverters[currentConverter.ConvertsTo] = currentConverter;

            long min;
            for (min = 0; min < 836040385; min++)
            {
                Thing currentThing = new Thing()
                {
                    Type = "location",
                    Id = min,
                };
                while(currentThing.Type != "seed") {
                    currentThing = backwardsConverters[currentThing.Type].ConvertBackwards(currentThing);
                }
                bool done = false;
                for (int i = 0; i < seeds.Length; i += 2)
                {
                    if(currentThing.Id >= seeds[i] && currentThing.Id < seeds[i] + seeds[i+1])
                    {
                        done = true;
                        break;
                    }
                }

                if(done)
                    break;
            }

            Console.WriteLine($"Min loc: {min}");
        }
    }
}