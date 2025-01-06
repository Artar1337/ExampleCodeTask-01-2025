using UnityEngine;

public class PlayerMovementMediator : MonoBehaviour
{
    [SerializeField] private Transform _mesh;
    [SerializeField] private Rigidbody _rigidbody;
    public Transform Mesh => _mesh;
    public Rigidbody Rigidbody => _rigidbody;
}
