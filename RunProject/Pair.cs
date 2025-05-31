namespace RunProject;

public class Pair<K, V> : IComparable<Pair<K, V>> where K : IComparable<K>
{
    public K key;
    public V val;
    public Pair(K k, V v) {
        key = k;
        val = v;
    }

    public int CompareTo(Pair<K, V>? other)
    {
        return key.CompareTo(other.key);
    }
    public class Key : IComparable<Pair<K, V>> {
        public K key;
        public Key(K k) {
            key = k;
        }
        public int CompareTo(Pair<K, V>? other)
        {
            return key.CompareTo(other.key);
        }
    }
}
