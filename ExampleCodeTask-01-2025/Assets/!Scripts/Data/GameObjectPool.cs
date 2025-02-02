using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Data
{
    public class GameObjectPool
    {
        private readonly ObjectPool<GameObject> _pool;
        private readonly HashSet<GameObject> _activeObjects;
        public HashSet<GameObject> ActiveObjects => _activeObjects;

        public GameObjectPool(Func<GameObject> instantiateFunc, int capacity)
        {
            _activeObjects = new(capacity);
            _pool = new ObjectPool<GameObject>(
                instantiateFunc,
                (x) =>
                {
                    x.SetActive(true);
                    _activeObjects.Add(x);
                },
                (x) =>
                {
                    x.SetActive(false);
                    _activeObjects.Remove(x);
                },
                defaultCapacity: capacity);
        }

        public void ReleaseAll()
        {
            HashSet<GameObject> copy = new(_activeObjects);

            foreach (var obj in copy)
            {
                _pool.Release(obj);
            }
        }

        public GameObject Get() => _pool.Get();
        public void Release(GameObject obj) => _pool.Release(obj);
    }
}
