using UnityEngine;

namespace Mono.Installers
{
    public class UISystemMediator : MonoBehaviour
    {
        [SerializeField] private Transform _uiRoot;

        public Transform UIRoot => _uiRoot;
    }
}
