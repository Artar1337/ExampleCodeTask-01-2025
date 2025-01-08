using Cysharp.Threading.Tasks;
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
    private const float MAX_DISTANCE_TO_INCREASE_SPEED = 15f;

    [Inject] private ILevelSystem _levelSystem;
    [Inject] private IInstantiator _instantiator;
    [Inject] private IGameCycleSystem _gameCycleSystem;
    [Inject] private IResourcesSystem _resourcesSystem;
    [Inject] private IPlayerMovementSystem _playerMovementSystem;

    private bool _initialized = false;
    private Transform _lavaLevel;
    private Vector3 _initialLavaLevel;

    public async UniTask Init()
    {
        var prefab = await _resourcesSystem.Load<GameObject>("Lava");
        _lavaLevel = _instantiator.
            InstantiatePrefab(prefab, _levelSystem.LevelObjectsRoot).transform;
        _initialLavaLevel = _lavaLevel.position;

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

        float yDelta = _playerMovementSystem.PlayerTransform.position.y -
            _lavaLevel.position.y;
        float speed = yDelta < MAX_DISTANCE_TO_INCREASE_SPEED ?
            LAVA_SPEED_MIN : LAVA_SPEED_MAX;

        _lavaLevel.Translate(speed * Time.deltaTime * Vector3.up);
    }
}
