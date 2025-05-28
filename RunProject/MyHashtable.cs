using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GameLibrary;

namespace RunProject
{
    public class MyHashtable<Key, Val>
    {
        class Chain
        {
            public List<(Key, Val)> pairs = new List<(Key, Val)>();
        }
        List<Chain> array = [new Chain()];
        int count = 0;
        protected void Update()
        {
            if (Count > array.Count * PrefferedStack)
            {
                var oldArr = array;
                array = new List<Chain>((int)((Count * 2) / PrefferedStack));
                CurrentMaxStack = 0;
                for (int i = 0; i < array.Capacity; i++) array.Add(new Chain());
                foreach (var chain in oldArr)
                {
                    foreach (var (key, val) in chain.pairs)
                    {
                        RowSet(key, val);
                    }
                }
            }
            
            //Console.WriteLine("aaaaa");
        }
        public int Count {
            get => count;
            private set
            {
                count = value;
                Update();
            }
        }
        public int CurrentMaxStack { get; private set; } = 0;
        public float PrefferedStack{ get; private set; } = 1;
        private bool RowSet(Key key, Val val)
        {
            Chain chain = array[(key.GetHashCode() % array.Count + array.Count)%array.Count];
            for (int i = 0; i < chain.pairs.Count; i++)
            {
                if (chain.pairs[i].Item1.Equals(key))
                {
                    chain.pairs[i] = (key, val);
                    return false;
                }
            }
            chain.pairs.Add((key, val));
            CurrentMaxStack = Math.Max(chain.pairs.Count, CurrentMaxStack);
            return true;
        }
        public Val this[Key key]
        {
            get
            {
                Chain chain = array[(key.GetHashCode() % array.Count + array.Count) % array.Count];
                foreach (var (k, v) in chain.pairs)
                {
                    if (key.Equals(k)) return v;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                if (RowSet(key, value))
                    Count++;
            }
        }
        public Val Remove(Key key)
        {
            Chain chain = array[(key.GetHashCode() % array.Count + array.Count) % array.Count];
            for(int i = 0; i < chain.pairs.Count; i++)
            {
                if (key.Equals(chain.pairs[i].Item1))
                {
                    Val val = chain.pairs[i].Item2;
                    chain.pairs.RemoveAt(i);
                    Count--;
                    return val;
                }
            }
            throw new KeyNotFoundException();
        }
        public void Show()
        {
            int i = 0;
            foreach (var chain in array)
            {
                Console.Write((i++)+": ");
                foreach (var (key, val) in chain.pairs)
                {
                    Console.Write(key + " : " + val + ";");
                }
                Console.WriteLine();
            }
        }
        public bool ContainsKey(Key key)
        {
            Chain chain = array[(key.GetHashCode() % array.Count + array.Count) % array.Count];
            for (int i = 0; i < chain.pairs.Count; i++)
            {
                if (key.Equals(chain.pairs[i].Item1))
                {
                    return true;
                }
            }
            return false;
        }
        public IEnumerable<(Key, Val)> Pairs()
        {
            foreach (var chain in array)
            {
                foreach (var (key, val) in chain.pairs)
                {
                    yield return (key, val);
                }
            }
        }
    }
}
