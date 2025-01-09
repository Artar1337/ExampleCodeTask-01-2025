using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Logic.Systems
{
    public interface IResourcesSystem
    {
        /// <summary>
        /// Initializes system
        /// </summary>
        /// <returns></returns>
        UniTask Init();
        /// <summary>
        /// Loads an object from addressables. If name is not specified, searches by passed type
        /// </summary>
        UniTask<T> Load<T>(string assetName = null);
        /// <summary>
        /// Instantiates an object from addressables. Name must be specified
        /// </summary>
        UniTask<T> Instantiate<T>(string assetName, Transform parent = null) where T : Object;
    }

    public class ResourcesSystem : IResourcesSystem
    {
        private readonly Dictionary<Type, List<string>> _assetLocations = new(128);

        public async UniTask Init()
        {
            _assetLocations.Clear();
            IResourceLocator resourceLocator = await Addressables.InitializeAsync().Task;

            foreach (var key in resourceLocator.Keys)
            {
                if (!resourceLocator.Locate(key, typeof(Object),
                    out IList<IResourceLocation> locations))
                {
                    continue;
                }

                foreach (var location in locations)
                {
                    if (_assetLocations.ContainsKey(location.ResourceType))
                    {
                        var addressableNames = _assetLocations[location.ResourceType];

                        if (!addressableNames.Contains(location.PrimaryKey))
                        {
                            addressableNames.Add(location.PrimaryKey);
                        }

                        _assetLocations[location.ResourceType] = addressableNames;
                    }
                    else
                    {
                        List<string> addressableKeys = new()
                        {
                            location.PrimaryKey
                        };

                        _assetLocations.Add(location.ResourceType, addressableKeys);
                    }
                }
            }
        }

        public async UniTask<T> Instantiate<T>(string assetName = null, Transform parent = null) where T : Object
        {
            try
            {
                var obj = await Addressables.InstantiateAsync(assetName, parent);
                return obj.GetComponent<T>();
            }
            catch (Exception e)
            {
                Debug.LogError($"Cant instantiate '{assetName}' with type '{typeof(T)}'. Exception: {e.Message}");
                return null;
            }
        }

        public async UniTask<T> Load<T>(string assetName = null)
        {
            if (assetName == null)
            {
                if (!_assetLocations.ContainsKey(typeof(T)))
                {
                    Debug.LogError($"Can't load asset of type {typeof(T)} - no entry in addressables found!");
                    return default;
                }

                assetName = _assetLocations[typeof(T)][0];
            }

            try
            {
                return await Addressables.LoadAssetAsync<T>(assetName);
            }
            catch (Exception e)
            {
                Debug.LogError($"Cant load '{assetName}' with type '{typeof(T)}'. Exception: {e.Message}");
                return default;
            }
        }
    }
}
