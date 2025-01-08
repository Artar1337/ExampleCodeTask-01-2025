using Cysharp.Threading.Tasks;
using Zenject;

public class UIModelHud : UIModel<UIViewHud>
{
    private const float TEXT_ANIMATION_TIME = 1f;

    [Inject] private UIModelPause _pause;
    [Inject] private IGameScoreSystem _scoreSystem;
    [Inject] private IPlayerMovementSystem _playerMovementSystem;

    public override string ViewName => "UI/Hud";

    public override void OnInit()
    {
        _view.OnMenuClick += TryOpenPause;
    }

    public override void OnShow()
    {
        _view.SetScreenPrompt(string.Empty);
        UpdateScore(_scoreSystem.Score);
        _scoreSystem.OnScoreChanged += UpdateScore;
    }

    public override void OnHide()
    {
        _scoreSystem.OnScoreChanged -= UpdateScore;
    }

    public async UniTask AnimateStartPrompt(string startPrompt)
    {
        await UniTask.WaitWhile(() =>!_uiSystem.IsShown(this));

        _view.SetScreenPrompt(startPrompt);

        await _view.AnimatePromptColor(TEXT_ANIMATION_TIME);
    }

    private void TryOpenPause()
    {
        if (!CanOpenPause())
        {
            return;
        }

        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_pause).Forget();
    }

    private void UpdateScore(int score)
    {
        _view.SetScoreText(score.ToString());
    }

    private bool CanOpenPause()
    {
        return _playerMovementSystem.CanMove;
    }
}
