using System;
using System.Collections.Generic;

namespace SCommon
{
    public class STableRecord<TKey>
    {
        public TKey ID = default(TKey);
    }


	public class STable<T, Key, Data> : SSingleton<T>
		where T : class, new()
		where Data : class
	{
		protected Dictionary<Key, Data> m_Table = new Dictionary<Key, Data>();
		public Dictionary<Key, Data> Table { get { return m_Table; } }

        public virtual bool Load(string path)
        {
            return true;
        }

        public virtual void Prepare()
        {

        }

		public virtual void Clear()
		{
			m_Table.Clear();
		}

		public virtual Data Find(Key key)
		{
			return m_Table.ContainsKey(key) ? m_Table[key] : null;
		}

		public virtual bool Add(Key key, Data data)
		{
			if(m_Table.ContainsKey(key)) return false;
			m_Table.Add(key, data);
			return true;
		}

		
		public virtual void Remove(Key key)
		{
			if(m_Table.ContainsKey(key))
				m_Table.Remove(key);
		}
	}

    public class SPairKeyTable<T, Key1, Key2, Data> : SSingleton<T>
    where T : class, new()
    where Data : class
    {
        protected Dictionary<Key1, Dictionary<Key2, Data>> m_Table = new Dictionary<Key1, Dictionary<Key2, Data>>();
        public Dictionary<Key1, Dictionary<Key2, Data>> Table { get { return m_Table; } }

        public virtual void Clear()
        {
            m_Table.Clear();
        }

        public virtual void Prepare()
        {

        }

        public virtual bool Load(string path)
        {
            return true;
        }

        public virtual Data Find(Key1 key1, Key2 key2)
        {
            if (!m_Table.ContainsKey(key1)) return null;
            if (!m_Table[key1].ContainsKey(key2)) return null;
            return m_Table[key1][key2];
        }

		public virtual Dictionary<Key2, Data> Find(Key1 key1)
		{
			if (!m_Table.ContainsKey(key1)) return null;
			
			return m_Table[key1];
		}

		public virtual bool Add(Key1 key1, Key2 key2, Data data)
        {
            if (m_Table.ContainsKey(key1))
            {
                if (m_Table[key1].ContainsKey(key2)) return false;
                m_Table[key1].Add(key2, data);
            }
            else
            {
                Dictionary<Key2, Data> sub = new Dictionary<Key2, Data>();
                sub.Add(key2, data);
                m_Table.Add(key1, sub);
            }

            return true;
        }

        public virtual void Remove(Key1 key1, Key2 key2)
        {
            Data data = Find(key1, key2);
            if (data == null) return;

            m_Table[key1].Remove(key2);
        }
    }

    public class SDoubleKeyTable<T, Key1, Key2, Data> : SSingleton<T>
		where T : class, new()
		where Data : class
	{
		protected Dictionary<Key1, Data> m_Table = new Dictionary<Key1, Data>();
		public Dictionary<Key1, Data> Table { get { return m_Table; } }
		protected Dictionary<Key2, Data> m_Table2 = new Dictionary<Key2, Data>();
		public Dictionary<Key2, Data> Table2 { get { return m_Table2; } }

        public virtual void Prepare()
        { 
        }

        public virtual bool Load(string path)
        {
            return true;
        }

        public virtual void Clear()
		{
			m_Table.Clear();
			m_Table2.Clear();
		}

		public virtual Data Find(Key1 key)
		{
			return m_Table.ContainsKey(key) ? m_Table[key] : null;
		}

		public virtual Data Find(Key2 key)
		{
			return m_Table2.ContainsKey(key) ? m_Table2[key] : null;
		}

		public virtual bool Add(Key1 key1, Key2 key2, Data data)
		{
			if(m_Table.ContainsKey(key1)) return false;
			if(m_Table2.ContainsKey(key2)) return false;
			m_Table.Add(key1, data);
			m_Table2.Add(key2, data);
			return true;
		}

		public virtual void Remove(Key1 key)
		{
			Data data = Find(key);
			if(data == null) return;
			foreach(var pair in m_Table2)
			{
				if(pair.Value == data)
				{
					m_Table2.Remove(pair.Key);
					break;
				}
			}
			m_Table.Remove(key);
		}

		public virtual void Remove(Key2 key)
		{
			Data data = Find(key);
			if(data == null) return;
			foreach(var pair in m_Table)
			{
				if(pair.Value == data)
				{
					m_Table.Remove(pair.Key);
					break;
				}
			}
			m_Table2.Remove(key);
		}
	}
}
