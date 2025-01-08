using Zenject;

public class PlatformController : BaseObstacleController
{
    [Inject] protected IGameScoreSystem _scoreSystem;

    public override void OnCollisionWithPlayerEntered()
    {
        _scoreSystem.AddScore(1);
    }
}
