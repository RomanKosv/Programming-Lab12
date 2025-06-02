using System;
using System.Reflection.Metadata.Ecma335;

public class BalancedTree<T>
{
	public BalancedTree()
	{
	}
	class Node 
	{
		public T value;
		public BalancedTree<T> left = new BalancedTree<T>(), right = new BalancedTree<T>();
		public int lH = 0, rH = 0;
		public void add(T obj)
		{
			if (left.Size < right.Size)
			{
				left.add(obj);
				lH = left.Height();
			}
			else
			{
				right.add(obj);
				rH = right.Height();
			}
		}
		public int countOfKeys<K>(K key, Func<T, K> keyFun)
		{
			K valK = keyFun(value);
			if ((key == null && valK == null) || key.Equals(valK)) return 1 + left.countOfKeys(key, keyFun) + right.countOfKeys(key, keyFun);
			else return left.countOfKeys(key, keyFun) + right.countOfKeys(key, keyFun);
        }
	}
	Node? node = null;
	public int Size { get; private set; } = 0;
	public void add(T obj)
	{
		if (node != null) node.add(obj);
		else node = new Node { value = obj };
		Size++;
	}
	public int countOfKeys<K>(K key, Func<T, K> keyFun)
	{
		if (node != null) return node.countOfKeys(key, keyFun);
		else return 0;
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
	public void Clear()
	{
		node = null;
		Size = 0;
	}
	public int Height()
	{
		if (node != null) return 1 + Math.Max(node.lH, node.rH);
		else return 0;
	}
}
