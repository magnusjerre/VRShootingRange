using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class Pool : MonoBehaviour
    {
        public GameObject prefab;
        public int initialSize = 10;

        public List<GameObject> pool;

        void Awake()
        {
            pool = new List<GameObject>();
            for (var i = 0; i < initialSize; i++)
            {
                var instance = Instantiate(prefab);
                instance.SetActive(false);
                pool.Add(instance);
            }
        }

        public T Get<T>()
        {
            for (var i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeSelf)
                {
                    var result = pool[i].GetComponent<T>();
                    pool[i].SetActive(true);
                    return result;
                }
            }

            var existingType = pool[0].GetComponent<T>();
            if (existingType == null)
            {
                throw new NullReferenceException("Trying to add an illegal type to the pool: " + typeof(T));
            }

            var instance = Instantiate(prefab);
            pool.Add(instance);
            return instance.GetComponent<T>();
        }

    }
}