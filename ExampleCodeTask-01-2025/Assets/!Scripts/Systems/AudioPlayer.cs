using UnityEngine;
using Zenject;
using System;

public class AudioPlayer
{
    private readonly IAudioSystem _audioSystem;
    private readonly IPlayerMovementSystem _movementSystem;
    private readonly IGameCycleSystem _cycleSystem;
    private readonly IUISystem _uiSystem;

    public AudioPlayer(IAudioSystem audioSystem,
        IPlayerMovementSystem movementSystem,
        IGameCycleSystem cycleSystem,
        IUISystem uiSystem)
    {
        _audioSystem = audioSystem;
        _movementSystem = movementSystem;
        _cycleSystem = cycleSystem;
        _uiSystem = uiSystem;

        _movementSystem.OnPlayerJump += () => _audioSystem.PlaySoundOneShot(SoundType.Jump);
        _movementSystem.OnPlayerLand += () => _audioSystem.PlaySoundOneShot(SoundType.Land);
        _movementSystem.OnPlayerStartMoveOnGround += () => _audioSystem.PlaySoundLoop(SoundType.Roll);
        _movementSystem.OnPlayerEndMoveOnGround += () => _audioSystem.StopLoopSound();
        _movementSystem.OnPlayerDeath += () => _audioSystem.PlaySoundOneShot(SoundType.GameOver);

        _cycleSystem.OnPauseChange += (paused) =>
        {
            _audioSystem.StopLoopSound();

            if (!paused && _movementSystem.IsGrounded && _movementSystem.Moving)
            {
                _audioSystem.PlaySoundLoop(SoundType.Roll);
            }
        };

        _cycleSystem.OnGameOver += () => _audioSystem.PlayMusic(SoundType.MenuMusic);
        _cycleSystem.OnGameInit += () => _audioSystem.PlayMusic(SoundType.MenuMusic);
        _cycleSystem.OnGameStart += () => _audioSystem.PlayMusic(SoundType.GameplayMusic);

        _uiSystem.OnViewHideStart += () => _audioSystem.PlaySoundOneShot(SoundType.UIHide);
        _uiSystem.OnViewShowStart += () => _audioSystem.PlaySoundOneShot(SoundType.UIShow);
    }
}
