public class LavaController : BaseObstacleController
{
    public override bool OneTimeActivation => false;

    public override void OnCollisionWithPlayerEntered()
    {
        _playerMovementSystem.ProcessDeath();
    }
}
