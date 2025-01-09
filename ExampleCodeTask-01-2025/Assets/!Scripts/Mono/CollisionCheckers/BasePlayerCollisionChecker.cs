using System;
using UnityEngine;

namespace Logic.Mono.Collisions
{
    public abstract class BasePlayerCollisionChecker : MonoBehaviour
    {
        private bool _activated = false;

        public virtual bool OneTimeActivation => true;
        public event Action<GameObject> OnCollisionEntered;

        private void OnCollisionEnter(Collision collision)
        {
            if (OneTimeActivation && _activated)
            {
                return;
            }

            _activated = true;
            OnCollisionEntered?.Invoke(collision.gameObject);
        }

        private void OnDisable()
        {
            _activated = false;
        }
    }
}
