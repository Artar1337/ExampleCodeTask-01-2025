public class AudioPlayer
{
    public AudioPlayer(IAudioSystem audioSystem,
        IPlayerMovementSystem movementSystem,
        IGameCycleSystem cycleSystem,
        ILevelSystem levelSystem,
        IUISystem uiSystem)
    {
        movementSystem.OnPlayerJump += () => audioSystem.PlaySoundOneShot(SoundType.Jump);
        movementSystem.OnPlayerLand += () => audioSystem.PlaySoundOneShot(SoundType.Land);
        movementSystem.OnPlayerStartMoveOnGround += () => audioSystem.PlaySoundLoop(SoundType.Roll);
        movementSystem.OnPlayerEndMoveOnGround += () => audioSystem.StopLoopSound();
        movementSystem.OnPlayerDeath += () => audioSystem.PlaySoundOneShot(SoundType.GameOver);

        cycleSystem.OnPauseChange += (paused) =>
        {
            audioSystem.StopLoopSound();

            if (!paused && movementSystem.IsGrounded && movementSystem.Moving)
            {
                audioSystem.PlaySoundLoop(SoundType.Roll);
            }
        };

        cycleSystem.OnGameOver += () => audioSystem.PlayMusic(SoundType.MenuMusic);
        cycleSystem.OnGameInit += () => audioSystem.PlayMusic(SoundType.MenuMusic);
        cycleSystem.OnGameStart += () => audioSystem.PlayMusic(SoundType.GameplayMusic);

        uiSystem.OnViewHideStart += () => audioSystem.PlaySoundOneShot(SoundType.UIHide);
        uiSystem.OnViewShowStart += () => audioSystem.PlaySoundOneShot(SoundType.UIShow);

        levelSystem.OnLevelWin += () => audioSystem.PlaySoundOneShot(SoundType.Teleportation);
    }
}
