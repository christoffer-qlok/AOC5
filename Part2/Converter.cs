using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2
{
    internal class Converter
    {
        public string ConvertsFrom { get; set; }
        public string ConvertsTo { get; set; }

        public List<Entry> Convertions { get; set; } = new List<Entry> ();

        public Thing Convert(Thing original)
        {
            if (original.Type != ConvertsFrom)
            {
                throw new ArgumentException("Wrong type of thing", nameof(original));
            }
            foreach (Entry e in Convertions)
            {
                if (original.Id >= e.SrcStart && original.Id < e.SrcStart + e.Range)
                {
                    long diff = original.Id - e.SrcStart;
                    return new Thing()
                    {
                        Id = e.DstStart + diff,
                        Type = ConvertsTo
                    };
                }
            }
            return new Thing()
            {
                Id = original.Id,
                Type = ConvertsTo
            };
        }

        public Thing ConvertBackwards(Thing original)
        {
            if (original.Type != ConvertsTo)
            {
                throw new ArgumentException("Wrong type of thing", nameof(original));
            }
            foreach (Entry e in Convertions)
            {
                if (original.Id >= e.DstStart && original.Id < e.DstStart + e.Range)
                {
                    long diff = original.Id - e.DstStart;
                    return new Thing()
                    {
                        Id = e.SrcStart + diff,
                        Type = ConvertsFrom
                    };
                }
            }
            return new Thing()
            {
                Id = original.Id,
                Type = ConvertsFrom
            };
        }


    }
}
