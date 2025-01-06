using Cysharp.Threading.Tasks;
using Zenject;

public class UIModelHud : UIModel<UIViewHud>
{
    [Inject] private UIModelPause _pause;
    [Inject] private IGameScoreSystem _scoreSystem;

    public override string ViewName => "UI/Hud";

    public override void OnInit()
    {
        _view.OnMenuClick += OpenPause;
    }

    public override void OnShow()
    {
        UpdateScore(_scoreSystem.Score);
        _scoreSystem.OnScoreChanged += UpdateScore;
    }

    public override void OnHide()
    {
        _scoreSystem.OnScoreChanged -= UpdateScore;
    }

    private void OpenPause()
    {
        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_pause).Forget();
    }

    private void UpdateScore(int score)
    {
        _view.SetScoreText(score.ToString());
    }
}
