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
		BalancedTree<T> left = new BalancedTree<T>(), right = new BalancedTree<T>();
		public void add(T obj)
		{
			if (left.Size < right.Size) left.add(obj);
			else right.add(obj);
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
	}
	public int countOfKeys<K>(K key, Func<T, K> keyFun)
	{
		if (node != null) return node.countOfKeys(key, keyFun);
		else return 0;
	}
}
