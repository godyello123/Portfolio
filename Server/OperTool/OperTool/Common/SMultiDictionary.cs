using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SCommon
{
    public class SDictionarySub<TKey, TSubKey, Data>
    {
		private Dictionary<TKey, Data> _base = new Dictionary<TKey, Data>();
		private Dictionary<TSubKey, Data> _sub = new Dictionary<TSubKey, Data>();

		public Dictionary<TKey, Data> Base { get { return _base; } }

		~SDictionarySub()
        {
			_base.Clear();
			_sub.Clear();
        }
		public bool Insert(TKey key, TSubKey subKey, Data data)
        {
			if (_sub.ContainsKey(subKey))
				return false;

			if (_base.ContainsKey(key))
				return false;

			_base.Add(key, data);
			_sub.Add(subKey, data);

			return true;
        }

		public void Erase(TKey key, TSubKey subKey)
        {
			_sub.Remove(subKey);
			_base.Remove(key);
        }

		public Data Find(TKey key)
        {
            Data retval = default(Data);
            _base.TryGetValue(key, out retval);
            return retval;
        }

		public Data Find(TSubKey subKey)
        {
			Data retval = default(Data);
			_sub.TryGetValue(subKey, out retval);
			return retval;
        }

	}

    public class SConcurrentDictionarySub<TKey, TSubKey, Data>
    {
        private ConcurrentDictionary<TKey, Data> _base = new ConcurrentDictionary<TKey, Data>();
        private ConcurrentDictionary<TSubKey, Data> _sub = new ConcurrentDictionary<TSubKey, Data>();

        public ConcurrentDictionary<TKey, Data> Base { get { return _base; } }

        ~SConcurrentDictionarySub()
        {
            _base.Clear();
            _sub.Clear();
        }
        public bool Insert(TKey key, TSubKey subKey, Data data)
        {
            if (!_sub.TryAdd(subKey, data))
                return false;

            if(!_base.TryAdd(key, data))
            {
                Erase(key, subKey);
                return false;
            }

            return true;
        }

        public void Erase(TKey key, TSubKey subKey)
        {
            Data removedData;
            _sub.TryRemove(subKey, out removedData);
            _base.TryRemove(key, out removedData);
        }

        public Data Find(TKey key)
        {
            Data retval = default(Data);
            _base.TryGetValue(key, out retval);
            return retval;
        }

        public Data Find(TSubKey subKey)
        {
            Data retval = default(Data);
            _sub.TryGetValue(subKey, out retval);
            return retval;
        }
    }

    public class SMultiSortedDictionary<TKey, TValue> : SortedDictionary<TKey, List<TValue>>
	{
		public void Add(TKey key, TValue value)
		{
			List<TValue> container = null;
			if(!TryGetValue(key, out container))
			{
				container = new List<TValue>();
				Add(key, container);
			}
			container.Add(value);
		}

		public void Remove(TKey key, TValue value)
		{
			List<TValue> container = null;
			if(TryGetValue(key, out container))
			{
				container.Remove(value);
				if(container.Count <= 0) Remove(key);
			}
		}

		public List<TValue> Find(TKey key)
		{
			List<TValue> values = null;
			TryGetValue(key, out values);
			return values;
		}
	}

}
