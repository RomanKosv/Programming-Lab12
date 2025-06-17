using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunProject
{
    public class Journal
    {
        public class Entry
        {
            public string collection, action, obj;
            public Entry(string collection, string action, string obj)
            {
                this.collection = collection;
                this.action = action;
                this.obj = obj;
            }
            public override string ToString()
            {
                return $"""
                    Collection:
                    {collection}
                    Action:
                    {action}
                    Object
                    {obj}
                    """;
            }
        }
        List<Entry> entries = new List<Entry>();
        public int Count => entries.Count;
        public void Add(Entry entry)
        {
            entries.Add(entry);
        }
        public IEnumerable<int> Numbers ()
        {
            for(int i = 1; ;i++) yield return i;
        }
        public override string ToString()
        {
            return Numbers().Zip(entries).Aggregate("", (s, ne) => s + $"""
            Entry {ne.First}:
            {ne.Second}

            """);
        }
    }
}
