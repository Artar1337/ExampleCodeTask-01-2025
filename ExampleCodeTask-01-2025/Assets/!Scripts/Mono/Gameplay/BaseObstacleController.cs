using UnityEngine;
using Zenject;

public abstract class BaseObstacleController : MonoBehaviour
{
    [Inject] protected IPlayerMovementSystem _playerMovementSystem;

    private bool _activated = false;

    public virtual bool OneTimeActivation => true;

    public abstract void OnCollisionWithPlayerEntered();

    private void OnCollisionEnter(Collision collision)
    {
        if (OneTimeActivation && _activated)
        {
            return;
        }

        if (collision.gameObject == _playerMovementSystem.PlayerTransform.gameObject)
        {
            OnCollisionWithPlayerEntered();
            _activated = true;
        }
    }

    private void OnDisable()
    {
        _activated = false;
    }
}
