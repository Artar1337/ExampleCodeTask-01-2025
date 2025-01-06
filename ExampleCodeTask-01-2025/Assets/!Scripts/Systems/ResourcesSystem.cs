using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public interface IResourcesSystem
{
    /// <summary>
    /// Loads an object from addressables. If name is not specified, searches by passed type
    /// </summary>
    UniTask<T> Load<T>(string assetName = null);
    /// <summary>
    /// Instantiates an object from addressables. If name is not specified, searches by passed type
    /// </summary>
    UniTask<T> Instantiate<T>(string assetName = null, Transform parent = null) where T : Object;
}

public class ResourcesSystem : IResourcesSystem
{
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
