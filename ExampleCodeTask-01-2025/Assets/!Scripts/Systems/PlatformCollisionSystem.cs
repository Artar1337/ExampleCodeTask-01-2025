using Logic.Mono.Collisions;
using UnityEngine;
using Zenject;

namespace Logic.Systems
{
    public interface IPlatformCollisionSystem
    {
        void RegisterWinPlatform(BasePlayerCollisionChecker platform);
        void RegisterRegularPlatform(BasePlayerCollisionChecker platform);
        void RegisterLava(BasePlayerCollisionChecker platform);
    }

    public class PlatformCollisionSystem : IPlatformCollisionSystem
    {
        [Inject] private IPlayerMovementSystem _playerMovementSystem;
        [Inject] private IGameScoreSystem _scoreSystem;
        [Inject] private ILevelSystem _levelSystem;

        public void RegisterLava(BasePlayerCollisionChecker platform)
        {
            platform.OnCollisionEntered += ProcessLavaCollision;
        }

        public void RegisterRegularPlatform(BasePlayerCollisionChecker platform)
        {
            platform.OnCollisionEntered += ProcessRegularPlatformCollision;
        }

        public void RegisterWinPlatform(BasePlayerCollisionChecker platform)
        {
            platform.OnCollisionEntered += ProcessWinPlatformCollision;
        }

        private void ProcessLavaCollision(GameObject collidedObject)
        {
            if (!IsColliderObjectPlayer(collidedObject))
            {
                return;
            }

            _playerMovementSystem.ProcessDeath();
        }

        private void ProcessRegularPlatformCollision(GameObject collidedObject)
        {
            if (!IsColliderObjectPlayer(collidedObject))
            {
                return;
            }

            _scoreSystem.AddScore(1);
        }

        private void ProcessWinPlatformCollision(GameObject collidedObject)
        {
            if (!IsColliderObjectPlayer(collidedObject))
            {
                return;
            }

            _scoreSystem.AddScore(100);
            _levelSystem.ProcessWinLevel();
        }

        private bool IsColliderObjectPlayer(GameObject collidedObject)
        {
            return collidedObject == _playerMovementSystem.PlayerTransform.gameObject;
        }
    }
}
