using UnityEngine;
using Zenject;

public class KillzoneController : MonoBehaviour
{
    [Inject] private ILevelSystem _levelSystem;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<BaseObstacleController>(
            out var controller))
        {
            _levelSystem.ProcessPlatformHide(controller);
        }
    }
}
