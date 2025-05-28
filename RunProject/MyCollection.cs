using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RunProject
{
    public class MyCollection<T> : IEnumerable<T>, IList<T>
    {
        Node? node;
        class Node
        {
            public T value;
            public MyCollection<T> rest;
            public T get(int ind)
            {
                if (ind < 0) throw new IndexOutOfRangeException();
                else if (ind == 0) return value;
                else return rest[ind - 1];
            }
            public void set(int ind, T val)
            {
                if (ind < 0) throw new IndexOutOfRangeException();
                else if (ind == 0) value = val;
                else rest[ind - 1] = val;
            }
            public int Count()
            {
                return 1 + rest.Count;
            }

            internal void Add(T item)
            {
                rest.Add(item);
            }
        }
        class Enumerator : IEnumerator<T>
        {
            public MyCollection<T> node, start;
            public T Current { get; private set; } = default;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                
            }

            public bool MoveNext()
            {
                if (node.node != null)
                {
                    node = node.node.rest;
                    Current = node.node.value;
                    return true;
                }
                else return false;
            }

            public void Reset()
            {
                node = start;
                Current = default;
            }
        }
        public T this[int index] { 
            get
            {
                if (node != null) return node.get(index);
                else throw new IndexOutOfRangeException();
            }
            set
            {
                if (node != null) node.set(index, value);
                else throw new IndexOutOfRangeException();
            }
        }

        public int Count
        {
            get
            {
                if (node != null) return node.Count();
                else return 0;
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (node != null) node.Add(item);
            else node = new Node { value = item };
        }

        public void Clear()
        {
            node = null;
        }

        public bool Contains(T item)
        {
            return ((IEnumerable<T>)this).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int ind = arrayIndex;
            foreach (var i in this)
            {
                if (arrayIndex >= array.Count()) break;
                array[ind] = i;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MyCollection<T>.Enumerator { start = this, node = this };
        }

        public int IndexOf(T item)
        {
            return ((IEnumerable<T>)this).Index().First(pair=> object.Equals(item, pair.Item2)).Item1;
        }

        public void Insert(int index, T item)
        {
            if (index == 0) node = new Node { value = item, rest = new MyCollection<T> { node = node } };
            else if (index < 0 || node == null) throw new IndexOutOfRangeException();
            else node.rest.Insert(index - 1, item);
        }

        public bool Remove(T item)
        {
            if (node == null) return false;
            else if (object.Equals(node.value, item))
            {
                node = node.rest.node;
                return true;
            }
            else return node.rest.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (node == null || index < 0) throw new IndexOutOfRangeException();
            else if (index == 0) node = node.rest.node;
            else node.rest.RemoveAt(index - 1);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Fill(Func<T> supplier)
        {
            if (node != null)
            {
                node.value = supplier();
                node.rest.Fill(supplier);
            }
        }
    }
}
