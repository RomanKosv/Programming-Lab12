using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class LinkList<T> : IEnumerable<T>, IList<T>
{
    Node? start;

    public int Count {
        get
        {
            if (start != null) return start.Count;
            else return 0;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return false;
        }
    }

    public T this[int index] { 
        get
        {
            if (start != null) return start[index];
            else throw new IndexOutOfRangeException();
        }
        set
        {
            if (start != null) start[index] = value;
            else throw new IndexOutOfRangeException();
        }
    }

    public class Node
    {
        public T this[int index]
        {
            get
            {
                if (index <0)
                {
                    if (prev != null) return prev[index + 1];
                    else throw new IndexOutOfRangeException();
                }
                else if (index == 0) return value;
                else if (next != null) return next[index - 1];
                else throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < 0)
                {
                    if (prev != null) prev[index + 1] = value;
                    else throw new IndexOutOfRangeException();
                }
                else if (index == 0) this.value = value;
                else if (next != null) next[index - 1] = value;
                else throw new IndexOutOfRangeException();
            }
        }
        public T value;
        public Node? prev;
        public Node? next;
        public Node(T val, Node? prev_ = null, Node? next_ = null)
        {
            value = val;
            prev = prev_;
            next = next_;
        }
        public int Count
        {
            get
            {
                if (next != null) return next.Count + 1;
                else return 1;
            }
        }
        public  bool Remove(T item)
        {
            throw new NotImplementedException();
        }
        //public Node PushNext(T val)
        //{

        //    Node newNode = new Node(val, this, next);
        //    this.next = newNode;
        //    return newNode;
        //}
    }
    public LinkList()
    {
        start = null;
    }
    private LinkList(Node? start_)
    {
        start = start_;
    }
    public void PushLast(IEnumerable<T> vals)
    {
        
        if (start == null) {
            if (vals.Any())
            {
                start = new Node(vals.First());
                PushLast(vals.Skip(1));
            }
        }
        else
        {
            LinkList<T> list = new LinkList<T>(start.next);
            list.PushLast(vals);
            start.next = list.start;
            if(list.start!=null)
            {
                list.start.prev = start;
            }
        }
    }

    public bool PopLast(Predicate<T> pred, ref T? obj)
    {
        if (start == null) return false;
        else
        {
            if (pred(start.value))
            {
                obj = start.value;
                if (!new LinkList<T>(start.next).PopLast(pred, ref obj))
                {
                    if (start.prev != null)
                        start.prev.next = start.next;
                    if (start.next != null) 
                        start.next.prev = start.prev;
                    start = start.next;
                }
                return true;
            }
            else return new LinkList<T>(start.next).PopLast(pred, ref obj);
        }
    }
    class Enumerator : IEnumerator<T>
    {
        Node? node;
        LinkList<T> list;
        public Enumerator(LinkList<T> l)
        {
            list = l;
            node = l.start;
        }

        public T Current { get; private set; } = default;

        object? IEnumerator.Current
        {
            get => Current;
        }

        public void Dispose()
        {
            //Reset();
        }

        public bool MoveNext()
        {
            if (node == null) return false;
            else
            {
                Current = node.value;
                node = node.next;
                return true;
            }
        }

        public void Reset()
        {
            node = list.start;
            Current = default;
        }
    }
    public IEnumerator<T> GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new Enumerator(this);
    }
    public void Clear()
    {
        start = null;
    }

    public void Add(T item)
    {
        PushLast([item]);
    }

    public bool Contains(T item)
    {
        return ((IEnumerable<T>) this).Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        ((IEnumerable<T>)this).ToList().CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        if (start != null)
        {
            return start.Remove(item);
        }
        else return false;
    }

    public int IndexOf(T item)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }
}
