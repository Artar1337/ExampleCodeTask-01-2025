using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public interface IPlayerMovementSystem
{
    Transform PlayerTransform { get; }
    void ResetBall();
    void ProcessDeath();
    void Init(PlayerMovementMediator mediator);
}

public class PlayerMovementSystem : IPlayerMovementSystem, ITickable
{
    private const float SPEED_MULTIPLIER = 10f;
    private const float ROTATION_SPEED = 10f;
    private const float JUMP_HEIGHT = 6f;
    private const float IS_GROUNDED_MINIMAL_VELOCITY = 0.01f;
    private const float DEATH_ANIMATION_DOWN_HEIGHT = 1.05f;
    private const float DEATH_ANIMATION_TIME = 2f;

    [Inject] private IGameCycleSystem _gameCycleSystem;

    private Rigidbody _rigidbody;
    private Transform _cachedPlayerTransform;
    private Transform _cachedCameraTransform;
    private Transform _playerMesh;

    private CancellationTokenSource _cts = new();

    private bool _canMove = true;
    private bool _canJump = true;
    private bool _isGrounded = false;
    private Vector3 _playerDefaultPosition;

    public Transform PlayerTransform => _cachedPlayerTransform;

    public void ResetBall()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new();

        _canMove = true;
        _canJump = true;
        _isGrounded = false;
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _cachedPlayerTransform.position = _playerDefaultPosition;
        _cachedPlayerTransform.gameObject.SetActive(true);
    }

    public void ProcessDeath()
    {
        DeathTask().Forget();
    }

    public void Init(PlayerMovementMediator mediator)
    {
        _rigidbody = mediator.Rigidbody;
        _playerMesh = mediator.Mesh;
        _cachedPlayerTransform = mediator.transform;
        _playerDefaultPosition = _cachedPlayerTransform.position;

        _cachedCameraTransform = Camera.main.transform;
        _cachedCameraTransform.GetComponent<CinemachineCamera>().Target = new()
        {
            TrackingTarget = _cachedPlayerTransform,
            LookAtTarget = _cachedPlayerTransform,
            CustomLookAtTarget = false,
        };
    }

    public void Tick()
    {
        if (_cachedPlayerTransform == null ||
            !_cachedPlayerTransform.gameObject.activeInHierarchy)
        {
            return;
        }

        _isGrounded = Mathf.Abs(_rigidbody.linearVelocity.y) < IS_GROUNDED_MINIMAL_VELOCITY;

        MoveBall();
        RotateBall();
    }

    private void MoveBall()
    {
        if (!_canMove || _gameCycleSystem.Paused)
        {
            return;
        }

        Vector3 forward = _cachedPlayerTransform.forward;
        Vector3 right = _cachedPlayerTransform.right;

        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        var moving = !Mathf.Approximately(moveHorizontal, 0) ||
            !Mathf.Approximately(moveVertical, 0);

        if (moving)
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
            _playerMesh.Rotate(rotateDirection, Space.World);
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

    private async UniTask DeathTask(bool withAnimation = true)
    {
        _canMove = false;
        _rigidbody.useGravity = false;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.interpolation = RigidbodyInterpolation.None;
        _rigidbody.isKinematic = true;

        float timer = DEATH_ANIMATION_TIME;
        Vector3 initialPos = _cachedPlayerTransform.position;
        Vector3 deadPos = _cachedPlayerTransform.position -
            new Vector3(0, DEATH_ANIMATION_DOWN_HEIGHT);

        if (!withAnimation)
        {
            return;
        }

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
            _cachedPlayerTransform.position = Vector3.Lerp(deadPos, initialPos,
                timer / DEATH_ANIMATION_TIME);
        }

        _cachedPlayerTransform.gameObject.SetActive(false);
        _gameCycleSystem.EndGame();
    }
}
