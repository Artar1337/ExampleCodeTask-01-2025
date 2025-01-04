using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float SPEED_MULTIPLIER = 10f;
    private const float ROTATION_SPEED = 10f;
    private const float JUMP_HEIGHT = 6f;
    private const float IS_GROUNDED_MINIMAL_VELOCITY = 0.01f;

    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _mesh;

    private Rigidbody _rigidbody;
    private Transform _cachedTransform;
    private Transform _cachedCameraTransform;

    private CancellationTokenSource _cts = new();

    private bool _canMove = true;
    private bool _canJump = true;
    private bool _moving = false;
    private bool _isGrounded = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cachedCameraTransform = _camera.transform;
        _cachedTransform = transform;
    }

    private void Update()
    {
        _isGrounded = Mathf.Abs(_rigidbody.linearVelocity.y) < IS_GROUNDED_MINIMAL_VELOCITY;

        Vector3 forward = _cachedTransform.forward;
        Vector3 right = _cachedTransform.right;

        float moveVertical = _canMove ? Input.GetAxis("Vertical") : 0;
        float moveHorizontal = _canMove ? Input.GetAxis("Horizontal") : 0;

        _moving = !Mathf.Approximately(moveHorizontal, 0) ||
            !Mathf.Approximately(moveVertical, 0);

        if (_moving)
        {
            Vector3 moveDirection = SPEED_MULTIPLIER * Time.deltaTime *
                ((forward * moveVertical) + (right * moveHorizontal));
            _rigidbody.AddForce(moveDirection, ForceMode.VelocityChange);
        }

        if (!Mathf.Approximately(0f, _rigidbody.linearVelocity.magnitude))
        {
            Vector3 rotateDirection = ROTATION_SPEED *
                SPEED_MULTIPLIER *
                Time.deltaTime *
                new Vector3(_rigidbody.linearVelocity.z, _rigidbody.linearVelocity.y, _rigidbody.linearVelocity.x);
            _mesh.Rotate(rotateDirection, Space.World);
        }

        if (_isGrounded && _canJump)
        {
            bool jumped = _canMove && Input.GetAxis("Jump") > 0f;

            if (jumped)
            {
                _rigidbody.AddForce(Vector3.up * JUMP_HEIGHT, ForceMode.Impulse);
                BlockJump().Forget();
            }
        }

        _cachedCameraTransform.LookAt(_cachedTransform, Vector3.up);
    }

    private async UniTask BlockJump()
    {
        _canJump = false;

        try
        {
            await UniTask.WaitForSeconds(0.2f, cancellationToken: _cts.Token);
        }
        catch (OperationCanceledException)
        {

        }

        _canJump = true;
    }

    private void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
