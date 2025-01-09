using UnityEngine;

namespace Logic.Mono.Collisions
{
    public class LavaCollisionChecker : BasePlayerCollisionChecker
    {
        [SerializeField] private Transform _platformKillzone;

        public Transform PlatformKillzone => _platformKillzone;
        public override bool OneTimeActivation => false;
    }
}
