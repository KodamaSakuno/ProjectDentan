using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Moen.KanColle.Dentan
{
    public class Table<T> : IReadOnlyDictionary<int, T>
        where T : IID
    {
        IDictionary<int, T> r_Map;
        public IEnumerable<int> Keys { get { return r_Map.Keys; } }
        public IEnumerable<T> Values { get { return r_Map.Values; } }
        public int Count { get { return r_Map.Count; } }
        public T this[int rpKey] { get { return r_Map[rpKey]; } }

        public Table() : this(new Dictionary<int, T>()) { }
        public Table(IEnumerable<T> rpSource) : this(rpSource.ToDictionary(r => r.ID)) { }
        public Table(Dictionary<int, T> rpSource)
        {
            r_Map = rpSource;
        }

        public bool UpdateRawData<TRawData>(IEnumerable<TRawData> rpRawDataCollection, Func<TRawData, T> rpValueFactory, Action<T, TRawData> rpUpdate)
            where TRawData : IID
        {
            var rResult = false;

            var rRemovedIDs = new HashSet<int>(r_Map.Keys);
            foreach (var rRawData in rpRawDataCollection)
            {
                rRemovedIDs.Remove(rRawData.ID);

                T rData;
                if (rpUpdate != null && r_Map.TryGetValue(rRawData.ID, out rData))
                    rpUpdate(rData, rRawData);
                else
                {
                    Add(rpValueFactory(rRawData));
                    rResult = true;
                }
            }

            foreach (var rID in rRemovedIDs)
            {
                Remove(rID);
                rResult = true;
            }

            return rResult;
        }

        internal void Add(T rpData) => r_Map.Add(rpData.ID, rpData);
        internal void Remove(int rpID) => r_Map.Remove(rpID);
        internal void Remove(T rpData) => r_Map.Remove(rpData.ID);
        internal void Clear() => r_Map.Clear();
        public bool ContainsKey(int rpKey) => r_Map.ContainsKey(rpKey);
        public bool TryGetValue(int rpKey, out T ropValue) => r_Map.TryGetValue(rpKey, out ropValue);

        public IEnumerator<KeyValuePair<int, T>> GetEnumerator()=> r_Map.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => r_Map.GetEnumerator();
    }
}
