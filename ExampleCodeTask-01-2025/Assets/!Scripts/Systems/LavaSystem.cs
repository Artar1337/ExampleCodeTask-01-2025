using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ILavaSystem
{
    UniTask Init();
    void Reset();
}

public class LavaSystem : ILavaSystem, ITickable
{
    private const float LAVA_SPEED_MIN = 0.2f;
    private const float LAVA_SPEED_MAX = 3f;
    private const float LAVA_CONT_SPEED_X = 2f;
    private const float LAVA_CONT_SPEED_Z = 2f;
    private const float MAX_DISTANCE_TO_INCREASE_SPEED = 15f;

    [Inject] private ILevelSystem _levelSystem;
    [Inject] private IInstantiator _instantiator;
    [Inject] private IGameCycleSystem _gameCycleSystem;
    [Inject] private IResourcesSystem _resourcesSystem;
    [Inject] private IPlayerMovementSystem _playerMovementSystem;

    private bool _initialized = false;
    private Transform _lavaLevel;
    private Transform _platformKillzoneLevel;
    private Vector3 _initialLavaLevel;
    private List<GameObject> _cachedPlatformsToDelete = new(10);

    public async UniTask Init()
    {
        var prefab = await _resourcesSystem.Load<GameObject>("Lava");
        _lavaLevel = _instantiator.
            InstantiatePrefab(prefab, _levelSystem.LevelObjectsRoot).transform;
        _initialLavaLevel = _lavaLevel.position;
        _platformKillzoneLevel = _lavaLevel.GetComponent<LavaController>().PlatformKillzone;

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
        float speed = yDelta < MAX_DISTANCE_TO_INCREASE_SPEED ?
            LAVA_SPEED_MIN : LAVA_SPEED_MAX;

        _lavaLevel.Translate(Time.deltaTime *
            new Vector3(LAVA_CONT_SPEED_X, speed, LAVA_CONT_SPEED_Z));
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
