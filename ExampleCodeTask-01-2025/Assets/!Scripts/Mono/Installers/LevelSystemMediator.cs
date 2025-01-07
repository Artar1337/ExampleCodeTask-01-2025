using UnityEngine;

public class LevelSystemMediator : MonoBehaviour
{
    [SerializeField] private Transform _levelObjectsRoot;

    public Transform LevelObjectsRoot => _levelObjectsRoot;
}
