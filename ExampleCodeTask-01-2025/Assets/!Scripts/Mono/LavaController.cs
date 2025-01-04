using UnityEngine;

public class LavaController : MonoBehaviour
{
    // tmp
    [SerializeField] private PlayerController _player;

    public void Initialize(PlayerController player)
    {
        _player = player;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _player.gameObject)
        {
            _player.ProcessDeath();
        }
    }
}
