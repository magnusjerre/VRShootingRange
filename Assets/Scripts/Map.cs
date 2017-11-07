using System;
using System.Collections.Generic;

namespace Jerre
{
    public class Map<T, V>
    {

        private List<T> keys;
        public List<T> Keys
        {
            get
            {
                return keys;
            }
        }
        private List<V> values;

        public Map()
        {
            keys = new List<T>();
            values = new List<V>();
        }

        public void Put(T key, V value)
        {
            if (!keys.Contains(key))
            {
                keys.Add(key);
                values.Add(value);
            }
            else
            {
                int index = keys.IndexOf(key);
                values[index] = value;
            }
        }

        public V Get(T key)
        {
            int index = keys.IndexOf(key);
            if (index != -1)
            {
                return values[index];
            }
            throw new NullReferenceException("Null ");
        }

    }
}