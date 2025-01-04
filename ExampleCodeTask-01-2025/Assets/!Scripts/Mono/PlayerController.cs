using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private const float SPEED_MULTIPLIER = 10f;
    private const float ROTATION_SPEED = 10f;
    private const float JUMP_HEIGHT = 6f;
    private const float IS_GROUNDED_MINIMAL_VELOCITY = 0.01f;
    private const float DEATH_ANIMATION_DOWN_HEIGHT = 1.05f;
    private const float DEATH_ANIMATION_TIME = 2f;

    [SerializeField] private Transform _mesh;

    private Rigidbody _rigidbody;
    private Transform _cachedTransform;
    private Transform _cachedCameraTransform;

    private CancellationTokenSource _cts = new();

    private bool _canMove = true;
    private bool _canJump = true;
    private bool _moving = false;
    private bool _isGrounded = false;

    public bool Moving => _moving;

    public void ResetBall()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new();

        _canMove = true;
        _canJump = true;
        _moving = false;
        _isGrounded = false;
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void ProcessDeath()
    {
        DeathTask().Forget();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cachedCameraTransform = Camera.main.transform;
        _cachedTransform = transform;
    }

    private void Update()
    {
        _isGrounded = Mathf.Abs(_rigidbody.linearVelocity.y) < IS_GROUNDED_MINIMAL_VELOCITY;

        MoveBall();
        RotateBall();

        _cachedCameraTransform.LookAt(_cachedTransform, Vector3.up);
    }

    private void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    private void MoveBall()
    {
        if (!_canMove)
        {
            return;
        }

        Vector3 forward = _cachedTransform.forward;
        Vector3 right = _cachedTransform.right;

        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        _moving = !Mathf.Approximately(moveHorizontal, 0) ||
            !Mathf.Approximately(moveVertical, 0);

        if (_moving)
        {
            Vector3 moveDirection = SPEED_MULTIPLIER * Time.deltaTime *
                ((forward * moveVertical) + (right * moveHorizontal));
            _rigidbody.AddForce(moveDirection, ForceMode.VelocityChange);
        }

        if (_isGrounded && _canJump)
        {
            bool jumped = Input.GetAxis("Jump") > 0f;

            if (jumped)
            {
                _rigidbody.AddForce(Vector3.up * JUMP_HEIGHT, ForceMode.Impulse);
                JumpTask().Forget();
            }
        }
    }

    private void RotateBall()
    {
        if (!Mathf.Approximately(0f, _rigidbody.linearVelocity.magnitude))
        {
            Vector3 rotateDirection = ROTATION_SPEED *
                SPEED_MULTIPLIER *
                Time.deltaTime *
                new Vector3(_rigidbody.linearVelocity.z, _rigidbody.linearVelocity.y, _rigidbody.linearVelocity.x);
            _mesh.Rotate(rotateDirection, Space.World);
        }
    }

    private async UniTask JumpTask()
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

    private async UniTask DeathTask()
    {
        _canMove = false;
        _rigidbody.useGravity = false;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.interpolation = RigidbodyInterpolation.None;
        _rigidbody.isKinematic = true;

        float timer = DEATH_ANIMATION_TIME;
        Vector3 initialPos = _cachedTransform.position;
        Vector3 deadPos = _cachedTransform.position -
            new Vector3(0, DEATH_ANIMATION_DOWN_HEIGHT);

        while (timer > 0f)
        {
            try
            {
                await UniTask.Yield(cancellationToken: _cts.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            timer -= Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, float.MaxValue);
            _cachedTransform.position = Vector3.Lerp(deadPos, initialPos,
                timer / DEATH_ANIMATION_TIME);
        }
    }
}
