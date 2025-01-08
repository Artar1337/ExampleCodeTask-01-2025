using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ILevelSystem
{
    int CurrentPlatformIndex { get; }
    Transform LevelObjectsRoot { get; }
    HashSet<GameObject> ActivePlatforms { get; }
    void ProcessPlatformHide(GameObject platform);
    UniTask Init(IResourcesSystem resources, IInstantiator instantiator);
    void Generate(int seed = 0);
}

public class LevelSystem : ILevelSystem
{
    private const int MAX_COUNT_TO_GENERATE = 1000;
    private const int MAX_COUNT_ON_SCREEN = 30;

    private readonly Vector3[] _platforms = new Vector3[MAX_COUNT_TO_GENERATE];

    private Transform _levelObjectsRoot;
    private GameObjectPool _platformsPool;
    private Transform _lobbyPlatform;

    public Transform LevelObjectsRoot => _levelObjectsRoot;
    public HashSet<GameObject> ActivePlatforms => _platformsPool.ActiveObjects;
    public int CurrentPlatformIndex { get; private set; }

    public LevelSystem(LevelSystemMediator mediator)
    {
        _levelObjectsRoot = mediator.LevelObjectsRoot;
    }

    public void Generate(int seed = 0)
    {
        if (seed == 0)
        {
            seed = Random.Range(int.MinValue, int.MaxValue - MAX_COUNT_TO_GENERATE);
        }

        Random.InitState(seed);
        _platformsPool.ReleaseAll();
        _platforms[0] = _lobbyPlatform.position;

        for (int i = 1; i < MAX_COUNT_TO_GENERATE; i++)
        {
            _platforms[i] = _platforms[i - 1] +
                new Vector3(6,
                Random.value > 0.5f ? 1 : 0,
                Random.value > 0.5f ? 6 : 0);
        }

        CurrentPlatformIndex = 0;

        for (int i = 0; i < MAX_COUNT_ON_SCREEN; i++)
        {
            PlaceNewPlatform();
        }
    }

    public async UniTask Init(IResourcesSystem resources, IInstantiator instantiator)
    {
        _lobbyPlatform = instantiator.InstantiatePrefab(
            await resources.Load<GameObject>("LobbyPlatform"),
            _levelObjectsRoot).transform;

        _platformsPool = new(instantiator,
            await resources.Load<GameObject>("Platform"),
            _levelObjectsRoot, MAX_COUNT_ON_SCREEN);
    }

    public void ProcessPlatformHide(GameObject platform)
    {
        if (platform != null)
        {
            _platformsPool.Release(platform);
        }

        PlaceNewPlatform();
    }

    private void PlaceNewPlatform()
    {
        CurrentPlatformIndex = (CurrentPlatformIndex + 1) % MAX_COUNT_TO_GENERATE;

        _platformsPool.Get().transform.position = _platforms[CurrentPlatformIndex];
    }
}
