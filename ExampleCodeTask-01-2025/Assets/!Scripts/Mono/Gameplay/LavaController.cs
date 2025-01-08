using UnityEngine;

public class LavaController : BaseObstacleController
{
    [SerializeField] private Transform _platformKillzone;

    public Transform PlatformKillzone => _platformKillzone;
    public override bool OneTimeActivation => false;

    public override void OnCollisionWithPlayerEntered()
    {
        _playerMovementSystem.ProcessDeath();
    }
}
