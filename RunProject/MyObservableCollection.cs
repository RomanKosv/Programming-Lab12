using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RunProject
{
    public class MyObservableCollection<T> : MyCollection<T>
    {
        public delegate void CollectionHandler<El>(ICollection<El> collection, CollectionEventArgs<El> args);
        public event CollectionHandler<T>? CollectionCountChanged;
        public event CollectionHandler<T>? CollectionReferenceChanged;
        public new void Add(T item)
        {
            base.Add(item);
            if (CollectionCountChanged != null) CollectionCountChanged(this, new CollectionEventArgs<T>("add", item));
        }
        public new bool Remove(T item)
        {
            if (base.Remove(item))
            {
                CollectionCountChanged?.Invoke(this, new CollectionEventArgs<T>("remove", item));
                return true;
            }
            else return false;
        }
        public new void Clear() {
            while (Count != 0)
            {
                Remove(this[0]);
            }
        }
        public new T this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
                CollectionReferenceChanged?.Invoke(this, new CollectionEventArgs<T>("change", value));
            }
        }

    }
    public class CollectionEventArgs<Item> : EventArgs
    {
        public string Typename { get; set; }
        public Item Element { get; set; }
        public CollectionEventArgs(string typename, Item item)
        {
            Typename = typename;
            Element = item;
        }
    }
}
