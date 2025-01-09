using UnityEngine;

namespace Mono.Installers
{
    public class AudioSystemMediator : MonoBehaviour
    {
        [SerializeField] private AudioSource _music;
        [SerializeField] private AudioSource _sound;
        [SerializeField] private AudioSource _loopSound;

        public AudioSource Music => _music;
        public AudioSource Sound => _sound;
        public AudioSource LoopSound => _loopSound;
    }
}
