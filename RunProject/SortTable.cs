using System.Runtime.CompilerServices;

public class SortSet<T> where T : IComparable<T>
{
    public SortSet()
    {
        node = null;
    }
    SortSet(Node node_)
    {
        node = node_;
    }
    struct Node
    {
        public T value;
        public SortSet<T> left, right;
        public int left_h, right_h;
        public void updateH()
        {
            left_h = left.height();
            right_h = right.height();
        }
        public void rotateRight()
        {
            left.alignLeft();
            (value, left, right) = (left.node.Value.value, left.node.Value.left, new SortSet<T>(new Node { value = value, left = left.node.Value.left, right = right }));
            updateH();
        }
        public void rotateLeft()
        {
            right.alignRight();
            (value, left, right) = (right.node.Value.value, new SortSet<T>(new Node { value = value, left = left, right = right.node.Value.left }), right.node.Value.right);
            updateH();
        }
        public void alignRight()
        {
            if (left_h > right_h) rotateRight();
        }
        public void alignLeft()
        {
            if (right_h > left_h) rotateLeft();
        }
        public void stabilize()
        {
            if (left_h - right_h == 2) rotateRight();
            else if (right_h - left_h == 2) rotateLeft();
        }
        public void add(T obj)
        {
            if (obj.CompareTo(value) >= 0) right.add(obj);
            else left.add(obj);
            stabilize();
        }
    }
    Node? node;
    private void alignRight()
    {
        if (node != null) node.Value.alignRight();
    }
    private void alignLeft()
    {
        if (node != null) node.Value.alignLeft();
    }
    public int height()
    {
        if (node != null) return Math.Max(node.Value.left_h, node.Value.right_h) + 1;
        else return 0;
    }
    public void add(T obj)
    {
        if (node != null) node.Value.add(obj);
        else node = new Node { value = obj, left = new SortSet<T>(), right = new SortSet<T>() };
    }
    public bool Empty()
    {
        return node == null;
    }
    public bool popMax(out T? rez)
    {
        if (node != null)
        {
            if (!node.Value.right.Empty())
            {
                node.Value.right.popMax(out rez);
                node.Value.stabilize();
            }
            else
            {
                rez = node.Value.value;
                node = node.Value.left.node;
            }
            return true;
        }
        else
        {
            rez = default;
            return false;
        }
    }
    public bool popMin(out T? rez)
    {
        if (node != null)
        {
            if (!node.Value.left.Empty())
            {
                node.Value.left.popMin(out rez);
                node.Value.stabilize();
            }
            else
            {
                rez = node.Value.value;
                node = node.Value.right.node;
            }
            return true;
        }
        else
        {
            rez = default;
            return false;
        }
    }
}