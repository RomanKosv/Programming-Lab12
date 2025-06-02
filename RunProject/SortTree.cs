using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

public class SortTree<T> where T : IComparable<T>
{
    public class Node
    {
        public required SortTree<T> parent;
        public T value;
        public SortTree<T> left, right;
        public Node(T val)
        {
            value = val;
            left = new SortTree<T> { parent = this };
            right = new SortTree<T> { parent = this };
        }
        public int rH = 0, lH = 0;
        public Node Max()
        {
            if (right.node != null) return right.node.Max();
            else return this;
        }
        public Node Min()
        {
            if (left.node != null) return left.node.Min();
            else return this;
        }
        public T Pop()
        {
            T rez = value;
            if (right.node != null)
            {
                Node mn = right.node.Min();
                value = mn.value;
                mn.Pop();
            }
            else if (left.node != null)
            {
                Node mx = left.node.Max();
                value = mx.value;
                mx.Pop();
            }
            else
            {
                parent.node = null;
                Node current = this;
                while (current.parent.parent != null)
                {
                    current = current.parent.parent;
                    current.Update();
                }
            }
            return rez;
        }
        public void Update()
        {
            lH = left.Height();
            rH = right.Height();
            Stabilize();
        }
        public void Stabilize()
        {
            if (lH - rH > 1) RotateRight();
            else if (rH - lH > 1) RotateLeft();
        }
        public void RotateRight()
        {
            left.node.leftAlighn();
            SortTree<T> n_right = new SortTree<T> { parent = this };
            n_right.node = new Node(value) { left = left.node.right, right = right, parent = n_right };
            left.node.right.parent = n_right.node;
            right.parent = n_right.node;
            n_right.Update();
            (value, left, right) = (left.node.value, left.node.left, n_right);
            left.parent = this;
            Update();
        }
        public void leftAlighn()
        {
            if (rH - lH > 0) RotateLeft();
        }
        public void RotateLeft()
        {
            right.node.RightAlighn();
            SortTree<T> n_left = new SortTree<T> { parent = this };
            n_left.node = new Node(value) { parent = n_left, left = left, right = right.node.left };
            left.parent = n_left.node;
            right.node.left.parent = n_left.node;
            n_left.Update();
            (value, left, right) = (right.node.value, n_left, right.node.right);
            right.parent = this;
            Update();
        }
        public void RightAlighn()
        {
            if (lH - rH > 0) RotateRight();
        }
    }
    Node? node = null;
    public required Node? parent;
    public SortTree()
    {
        node = null;
    }
    public void Update()
    {
        if (node != null) node.Update();
    }

    public void Add(T item)
    {
        if (node != null)
        {
            if (item.CompareTo(node.value) >= 0) node.right.Add(item);
            else node.left.Add(item);
        }
        else
        {
            node = new Node(item) { parent = this };
        }
        Update();
    }

    public bool Empty() { return node == null; }
    public int Height()
    {
        if (node != null) return Math.Max(node.lH, node.rH) + 1;
        else return 0;
    }
    public Node? Find(IComparable<T> item)
    {
        if (node != null)
        {
            if (item.CompareTo(node.value) == 0) return node;
            else if (item.CompareTo(node.value) < 0) return node.left.Find(item);
            else return node.right.Find(item);
        }
        else return null;
    }
    public bool Pop(IComparable<T> item, out T rezult)
    {
        Node? found = Find(item);
        if (found != null)
        {
            rezult = found.Pop();
            return true;
        }
        else
        {
            rezult = default;
            return false;
        }
    }
    public IEnumerable<IEnumerable<T>> Levels() {
        if (node != null) {
            IEnumerable<T> level = [node.value];
            IEnumerator<IEnumerable<T>> a = node.left.Levels().GetEnumerator(), b = node.right.Levels().GetEnumerator();
            bool end;
            do {
                yield return level;
                end = true;
                level = [];
                if (a.MoveNext()) {
                    level = a.Current;
                    end = false;
                }
                if (b.MoveNext()) {
                    level = level.Concat(b.Current);
                    end = false;
                }
            } while (!end);
        }
    }
    public static SortTree<T> NewEmpty() {
        return new SortTree<T>() {parent = null};
    }
    public void Clear()
    {
        if (parent != null) throw new Exception("Clear will destroy invariant of parent");
        node = null;
        SortTree<T> current = this;
    }
    public IEnumerable<T> FindAll(IComparable<T> key)
    {
        if (node != null)
        {
            IEnumerable<T> vals = [];
            int comp = key.CompareTo(node.value);
            if (comp <= 0) vals = node.left.FindAll(key);
            if (comp == 0) vals = vals.Append(node.value);
            if (comp >= 0) vals = vals.Concat(node.right.FindAll(key));
            foreach (T item in vals) yield return item;
        }
    }
}