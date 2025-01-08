using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public interface ILevelSystem
{
    event Action OnLevelWin;
    int CurrentPlatformIndex { get; }
    Transform LevelObjectsRoot { get; }
    HashSet<GameObject> ActivePlatforms { get; }
    void ProcessPlatformHide(GameObject platform);
    UniTask Init(IResourcesSystem resources,
        IInstantiator instantiator, IGameCycleSystem gameCycle);
    void Generate(int seed = 0);
}

public class LevelSystem : ILevelSystem
{
    // 1000 platforms + start and win platforms
    private const int MAX_COUNT_TO_GENERATE = 1002;
    // Must be < MAX_COUNT_TO_GENERATE - 1!
    private const int MAX_COUNT_ON_SCREEN = 30;

    private readonly Vector3[] _platforms = new Vector3[MAX_COUNT_TO_GENERATE];

    private IGameCycleSystem _gameCycle;
    private Transform _levelObjectsRoot;
    private GameObjectPool _platformsPool;
    private Transform _lobbyPlatform;
    private Transform _winPlatform;

    public event Action OnLevelWin;

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

        _winPlatform.gameObject.SetActive(false);
    }

    public async UniTask Init(IResourcesSystem resources,
        IInstantiator instantiator, IGameCycleSystem gameCycle)
    {
        _gameCycle = gameCycle;

        _lobbyPlatform = instantiator.InstantiatePrefab(
            await resources.Load<GameObject>("LobbyPlatform"),
            _levelObjectsRoot).transform;

        _platformsPool = new(instantiator,
            await resources.Load<GameObject>("Platform"),
            _levelObjectsRoot, MAX_COUNT_ON_SCREEN);

        _winPlatform = instantiator.InstantiatePrefab(
            await resources.Load<GameObject>("WinPlatform"),
            _levelObjectsRoot).transform;
        _winPlatform.gameObject.SetActive(false);
        _winPlatform.GetComponent<WinPlatformController>().
            OnWinPlatformCollided += ProcessWinLevel;
    }

    public void ProcessPlatformHide(GameObject platform)
    {
        if (platform != null)
        {
            _platformsPool.Release(platform);
        }

        PlaceNewPlatform();
    }

    private void ProcessWinLevel()
    {
        _gameCycle.CreateLevel();
        OnLevelWin?.Invoke();
    }

    private void PlaceNewPlatform()
    {
        CurrentPlatformIndex++;

        // no platforms left
        if (MAX_COUNT_TO_GENERATE <= CurrentPlatformIndex)
        {
            return;
        }

        // place final platform with win, when player collides it -
        // level resets (but not the score)
        if (MAX_COUNT_TO_GENERATE - 1 == CurrentPlatformIndex)
        {
            _winPlatform.gameObject.SetActive(true);
            _winPlatform.transform.position = _platforms[CurrentPlatformIndex];
            return;
        }

        _platformsPool.Get().transform.position = _platforms[CurrentPlatformIndex];
    }
}
