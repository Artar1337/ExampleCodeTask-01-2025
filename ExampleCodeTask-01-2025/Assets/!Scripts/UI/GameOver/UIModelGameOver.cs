using Cysharp.Threading.Tasks;
using Zenject;

public class UIModelGameOver : UIModel<UIViewGameOver>
{
    public override string ViewName => "UI/GameOver";

    [Inject] private UIModelMenu _menuModel;
    [Inject] private IGameCycleSystem _gameCycleSystem;
    [Inject] private IGameScoreSystem _gameScoreSystem;

    public override void OnInit()
    {
        _view.OnAgainClick += ReStart;
        _view.OnExitClick += OpenMenu;
    }

    public override void OnShow()
    {
        string recordText = _gameScoreSystem.TryUpdateHiScore() ?
            $"Новый рекорд: {_gameScoreSystem.HiScore}" :
            $"Рекорд: {_gameScoreSystem.HiScore}";

        _view.SetScoresText(recordText, $"Ваш счёт: {_gameScoreSystem.Score}");
    }

    public void ReStart()
    {
        _gameCycleSystem.StartGame();
    }

    public void OpenMenu()
    {
        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_menuModel).Forget();
    }
}
