﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RunProject
{
    public class MyObservableCollection<K, V> : MyCollection<K, V> where K : IComparable<K>
    {
        public MyObservableCollection() : base() { }
        public MyObservableCollection(MyCollection<K, V> coll) : base(coll) { }
        public delegate void CollectionHandler(object collection, CollectionEventArgs<K, V> args);
        public event CollectionHandler CollectionCountChanged;
        public event CollectionHandler CollectionReferenceChanged;
        private void ThrowRewrite(K key, V value)
        {
            CollectionReferenceChanged?.Invoke(this, new CollectionEventArgs<K, V>("rewrite", new KeyValuePair<K, V>(key, value)));
        }
        public new V this[K key]
        {
            set
            {
                if (ContainsKey(key))
                {
                    base[key] = value;
                    ThrowRewrite(key, value);
                }
                else
                {
                    base[key] = value;
                    ThrowAdd(key, value);
                }
            }
        }

        private void ThrowAdd(K key, V value)
        {
            CollectionCountChanged?.Invoke(this, new CollectionEventArgs<K, V>("add", new KeyValuePair<K, V>(key, value)));
        }

        public new void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }
        public new void Add(K key, V value)
        {
            base.Add(key, value);
            ThrowAdd(key, value);
        }
        public new void Clear()
        {
            foreach(K key in Keys)
            {
                Remove(key);
            }
        }
        public new bool Remove(K key)
        {
            if (TryGetValue(key, out V value))
            {
                base.Remove(key);
                ThrowRemove(key, value);
                return true;
            }
            else return false;
        }

        private void ThrowRemove(K key, V value)
        {
            CollectionCountChanged?.Invoke(this, new CollectionEventArgs<K, V>("remove", new KeyValuePair<K, V>(key, value)));
        }

        public new bool Remove(KeyValuePair<K, V> item)
        {
            if (base.Remove(item))
            {
                ThrowRemove(item.Key, item.Value);
                return true;
            }
            else return false;
        }
    }
    public class CollectionEventArgs<K, V> : EventArgs where K : IComparable<K>
    {
        public string info;
        public KeyValuePair<K, V> pair;
        public CollectionEventArgs(string _info, KeyValuePair<K, V> _pair)
        {
            this.info = _info;
            this.pair = _pair;
        }
    }
}
