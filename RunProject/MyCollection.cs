using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RunProject
{
    public class MyCollection<K, V> : IDictionary<K, V> where K: IComparable<K>
    {
        private SortTree<Pair<K, V>> tree = SortTree<Pair<K, V>>.NewEmpty();

        public MyCollection() { }

        public MyCollection(MyCollection<K, V> collection) :  this() {
            foreach (var pair in  collection)
            {
                Add(pair);
            }
        }

        public V this[K key] {
            get => tree.FindAll(new Pair<K, V>.Key(key)).First().val;
            set
            {
                Remove(key);
                Add(key, value);
            }
        }

        public ICollection<K> Keys
        {
            get
            {
                if (Count != 0) return tree.Levels().Aggregate((a, b) => a.Concat(b)).Select(pair => pair.key).ToImmutableList();
                else return [];
            }
        }

        public ICollection<V> Values {
            get
            {
                if (Count != 0) return tree.Levels().Aggregate((a, b) => a.Concat(b)).Select(pair => pair.val).ToImmutableList();
                else return [];
            }
        }


        public int Count { get; private set; } = 0;

        public bool IsReadOnly => false;

        public void Add(K key, V value)
        {
            if (tree.FindAll(new Pair<K, V>.Key(key)).Any()) throw new NotSupportedException();
            else
            {
                tree.Add(new Pair<K, V>(key, value));
                Count++;
            }
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            tree.Clear();
            Count = 0;
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return tree.FindAll(
                new Pair<K, V>.Key(item.Key)).Any(
                    (pair) => item.Equals(new KeyValuePair<K, V>(pair.key, pair.val))
                );
        }

        public bool ContainsKey(K key)
        {
            return tree.FindAll(new Pair<K, V>.Key(key)).Any();
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            foreach(KeyValuePair<K, V> pair in this)
            {
                if (arrayIndex >= array.Length) break;
                else array[arrayIndex++] = pair;
            }
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return tree.Levels().Aggregate((a, b) => a.Concat(b)).Select((pair) => new KeyValuePair<K, V>(pair.key, pair.val)).GetEnumerator();
        }

        public bool Remove(K key)
        {
            if (tree.Pop(new Pair<K, V>.Key(key), out var pair))
            {
                Count--;
                return true;
            }
            else return false;
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            if (tree.Pop(new Pair<K, V>.Key(item.Key), (pair) => item.Equals(new KeyValuePair<K, V>(pair.key, pair.val)), out var rez))
            {
                Count--;
                return true;
            }
            else return false;
        }

        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        {
            if (tree.FindAll(new Pair<K, V>.Key(key)).Any())
            {
                value = tree.FindAll(new Pair<K, V>.Key(key)).First().val;
                return true;
            }
            else
            {
                value = default(V);
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
