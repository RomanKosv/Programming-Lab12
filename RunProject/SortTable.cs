using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

public class SortSet<T> where T : IComparable<T>
{
    public class Node
    {
        public required SortSet<T> parent;
        public required T value;
        public SortSet<T> left = new SortSet<T>(), right = new SortSet<T>();
        public int rH, lH;
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
                value = right.node.Min().Pop();
                Update();
            }
            else if (left.node != null)
            {
                value = left.node.Max().Pop();
                Update();
            }
            else if (parent != null)
            {
                parent.node = null;
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
            SortSet<T> n_right = new SortSet<T> { parent = this };
            n_right.node = new Node { value = value, left = left.node.right, right = right, parent = n_right };
            n_right.Update();
            (value, left, right) = (left.node.value, left.node.left, n_right);
            Update();
        }
        public void leftAlighn()
        {
            if (rH - lH > 0) RotateLeft();
        }
        public void RotateLeft()
        {
            right.node.RightAlighn();
            SortSet<T> n_left = new SortSet<T> { parent = this };
            n_left.node = new Node { parent = n_left, value = value, left = left, right = right.node.left };
            n_left.Update();
            (value, left, right) = (right.node.value, n_left, right.node.right);
            Update();
        }
        public void RightAlighn()
        {
            if (lH - rH > 0) RotateRight();
        }
    }
    Node? node = null;
    public required Node? parent;
    public SortSet()
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
        else node = new Node { value = item, parent = this };
    }

    public bool Empty() { return node != null; }
    public int Height()
    {
        if (node != null) return Math.Max(node.lH, node.rH) + 1;
        else return 0;
    }
}