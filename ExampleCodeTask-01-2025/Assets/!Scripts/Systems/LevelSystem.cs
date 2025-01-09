using Cysharp.Threading.Tasks;
using Data;
using Logic.Mono.Collisions;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Logic.Systems
{
    public interface ILevelSystem
    {
        event Action OnLevelWin;
        int CurrentPlatformIndex { get; }
        Transform LevelObjectsRoot { get; }
        HashSet<GameObject> ActivePlatforms { get; }
        void ProcessPlatformHide(GameObject platform);
        void ProcessWinLevel();
        UniTask Init();
        void Generate(int seed = 0);
    }

    public class LevelSystem : ILevelSystem
    {
        // 1000 platforms + start and win platforms
        private const int MAX_COUNT_TO_GENERATE = 1002;
        // Must be < MAX_COUNT_TO_GENERATE - 1!
        private const int MAX_COUNT_ON_SCREEN = 30;

        private readonly Vector3[] _platforms = new Vector3[MAX_COUNT_TO_GENERATE];
        private readonly Vector3[] _possibleRotations = new Vector3[]
        {
            new Vector3(-90, 0, 0),
            new Vector3(-90, 90, 0),
            new Vector3(-90, 180, 0),
            new Vector3(-90, 270, 0),
        };

        [Inject] private IGameCycleSystem _gameCycle;
        [Inject] private IResourcesSystem _resources;
        [Inject] private IPlatformCollisionSystem _platformCollisionSystem;

        private Transform _levelObjectsRoot;
        private GameObjectPool _platformsPool;
        private Transform _lobbyPlatform;
        private Transform _winPlatform;

        public event Action OnLevelWin;

        public Transform LevelObjectsRoot => _levelObjectsRoot;
        public HashSet<GameObject> ActivePlatforms => _platformsPool.ActiveObjects;
        public int CurrentPlatformIndex { get; private set; }

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

        public async UniTask Init()
        {
            _levelObjectsRoot = new GameObject("Level objects root").transform;

            _lobbyPlatform = await _resources.Instantiate<Transform>
                (AddressableConstants.LobbyPlatform, _levelObjectsRoot);

            var platformPrefab = await _resources.Load<GameObject>(
                AddressableConstants.RegularPlatform);
            var basePlatform = platformPrefab.GetComponent<PlatformCollisionChecker>();
            _platformsPool = new(() => InstantiateNewPlatform(basePlatform,
                _levelObjectsRoot), MAX_COUNT_ON_SCREEN);

            var winPlatform = await _resources.Instantiate<PlatformCollisionChecker>
                (AddressableConstants.WinPlatform, _levelObjectsRoot);
            _winPlatform = winPlatform.transform;
            _winPlatform.gameObject.SetActive(false);
            _platformCollisionSystem.RegisterWinPlatform(winPlatform);
        }

        public void ProcessPlatformHide(GameObject platform)
        {
            if (platform != null)
            {
                _platformsPool.Release(platform);
            }

            PlaceNewPlatform();
        }

        public void ProcessWinLevel()
        {
            _gameCycle.CreateLevel();
            OnLevelWin?.Invoke();
        }

        private GameObject InstantiateNewPlatform(PlatformCollisionChecker prefab, Transform root)
        {
            var platform = Object.Instantiate(prefab, root);
            _platformCollisionSystem.RegisterRegularPlatform(platform);
            return platform.gameObject;
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

            var platform = _platformsPool.Get().transform;
            platform.position = _platforms[CurrentPlatformIndex];
            platform.eulerAngles = _possibleRotations[Random.Range(0, _possibleRotations.Length)];
        }
    }
}
