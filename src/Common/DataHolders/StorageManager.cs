using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders
{
    [Serializable]
    public class StorageManager<T> : IEnumerable<T>, ICollection<T>, ICollection, IList<T>, IEquatable<StorageManager<T>>, IEquatable<IEnumerable<T>> where T : CivilianBase
    {
        #region Fields and Properties
        protected List<T> m_list = new List<T>();
        public T this[int index] { get { return m_list[index]; } set { m_list[index] = value; } }
        public int Count => this.Count();
        bool ICollection<T>.IsReadOnly => ((ICollection<T>)m_list).IsReadOnly;
        bool ICollection.IsSynchronized => ((ICollection)m_list).IsSynchronized;
        object ICollection.SyncRoot => ((ICollection)m_list).SyncRoot;
        #endregion

        #region Equatable
        public bool Equals(IEnumerable<T> other)
        {
            if (other.Count() == this.Count())
                for (int i = 0; i < this.Count(); i++)
                {
                    CivilianBase _item = this[i];

                    if (!_item.Equals(other.ToList()[i]))
                        return false;
                }
            else
                return false;

            return true;
        }
        public bool Equals(StorageManager<T> other) => Equals((IEnumerable<T>)other);
        #endregion

        #region List
        public void Add(T item) => m_list.Add(item);
        public bool Remove(T item) => m_list.Remove(item);
        public void RemoveAt(int index) => m_list.RemoveAt(index);
        public int IndexOf(T item) => m_list.IndexOf(item);
        public void Insert(int index, T item) => m_list.Insert(index, item);
        #endregion

        #region Collection
        public void Clear() => m_list.Clear();
        public bool Contains(T item) => m_list.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => m_list.CopyTo(array, arrayIndex);
        public void CopyTo(Array array, int arrayIndex) => ((ICollection)m_list).CopyTo(array, arrayIndex);
        #endregion

        #region Enumerator
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in m_list)
                yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}
