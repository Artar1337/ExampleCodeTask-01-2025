using Cysharp.Threading.Tasks;
using Logic.Systems;
using UI.Views;
using Zenject;

namespace UI.Models
{
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
                $"{"NewRecord".Localize()}{_gameScoreSystem.HiScore}" :
                $"{"Record".Localize()}{_gameScoreSystem.HiScore}";

            _view.SetScoresText(recordText, $"{"YourScore".Localize()}{_gameScoreSystem.Score}");
        }

        public override void OnEscape()
        {
            OpenMenu();
        }

        public void ReStart()
        {
            _gameCycleSystem.StartGame().Forget();
        }

        public void OpenMenu()
        {
            _uiSystem.Hide(this).Forget();
            _uiSystem.Show(_menuModel).Forget();
        }
    }
}
