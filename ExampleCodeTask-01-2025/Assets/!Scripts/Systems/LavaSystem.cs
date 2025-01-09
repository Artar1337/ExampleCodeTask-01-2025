using Cysharp.Threading.Tasks;
using Data;
using Logic.Mono.Collisions;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Logic.Systems
{
    public interface ILavaSystem
    {
        UniTask Init();
        void Reset();
    }

    public class LavaSystem : ILavaSystem, ITickable
    {
        private const float LAVA_SPEED_MIN = 0.2f;
        private const float LAVA_SPEED_MAX = 3f;
        private const float LAVA_SPEED_X = 2f;
        private const float LAVA_SPEED_Z = 2f;
        private const float MIN_DISTANCE_TO_INCREASE_SPEED_Y = 7f;
        private const float MIN_DISTANCE_TO_STOP_X = 100f;
        private const float MIN_DISTANCE_TO_STOP_Z = 100f;

        [Inject] private ILevelSystem _levelSystem;
        [Inject] private IGameCycleSystem _gameCycleSystem;
        [Inject] private IResourcesSystem _resourcesSystem;
        [Inject] private IPlayerMovementSystem _playerMovementSystem;
        [Inject] private IPlatformCollisionSystem _platformCollisionSystem;

        private bool _initialized = false;
        private Transform _lavaLevel;
        private Transform _platformKillzoneLevel;
        private Vector3 _initialLavaLevel;
        private List<GameObject> _cachedPlatformsToDelete = new(10);

        public async UniTask Init()
        {
            var lava = await _resourcesSystem.Instantiate<LavaCollisionChecker>(
                AddressableConstants.Lava, _levelSystem.LevelObjectsRoot);

            _lavaLevel = lava.transform;
            _initialLavaLevel = _lavaLevel.position;
            _platformKillzoneLevel = lava.PlatformKillzone;
            _platformCollisionSystem.RegisterLava(lava);

            _initialized = true;
        }

        public void Reset()
        {
            _lavaLevel.position = _initialLavaLevel;
        }

        public void Tick()
        {
            if (!_initialized || _gameCycleSystem.Paused)
            {
                return;
            }

            MoveLava();
            UpdatePlatforms();
        }

        private void MoveLava()
        {
            float yDelta = _playerMovementSystem.PlayerTransform.position.y -
                _lavaLevel.position.y;
            float ySpeed = yDelta < MIN_DISTANCE_TO_INCREASE_SPEED_Y ?
                LAVA_SPEED_MIN : LAVA_SPEED_MAX;

            float zDelta = _playerMovementSystem.PlayerTransform.position.z -
                _lavaLevel.position.z;
            float zSpeed = zDelta < MIN_DISTANCE_TO_STOP_Z ?
                LAVA_SPEED_Z : 0;

            float xDelta = _playerMovementSystem.PlayerTransform.position.x -
                _lavaLevel.position.x;
            float xSpeed = xDelta < MIN_DISTANCE_TO_STOP_X ?
                LAVA_SPEED_X : 0;

            _lavaLevel.Translate(Time.deltaTime *
                new Vector3(xSpeed, ySpeed, zSpeed));
        }

        private void UpdatePlatforms()
        {
            _cachedPlatformsToDelete.Clear();

            foreach (var platform in _levelSystem.ActivePlatforms)
            {
                if (platform.transform.position.y < _platformKillzoneLevel.position.y)
                {
                    _cachedPlatformsToDelete.Add(platform);
                }
            }

            foreach (var platform in _cachedPlatformsToDelete)
            {
                _levelSystem.ProcessPlatformHide(platform);
            }
        }
    }
}
