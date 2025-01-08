using System;

public class WinPlatformController : PlatformController
{
    public event Action OnWinPlatformCollided;

    public override void OnCollisionWithPlayerEntered()
    {
        _scoreSystem.AddScore(100);
        OnWinPlatformCollided?.Invoke();
    }
}
