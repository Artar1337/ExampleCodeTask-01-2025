using Cysharp.Threading.Tasks;
using Logic.Systems;
using UI.Views;
using UnityEngine;
using Zenject;

namespace UI.Models
{
    public class UIModelHud : UIModel<UIViewHud>
    {
        private const float TEXT_STAY_TIME = 1f;
        private const float TEXT_ANIMATION_TIME = 1f;
        private const float SYMBOL_STAY_TIME = 0.02f;

        [Inject] private UIModelPause _pause;
        [Inject] private IGameScoreSystem _scoreSystem;
        [Inject] private IPlayerMovementSystem _playerMovementSystem;

        private bool _promptAnimating = false;

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

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void OnHide()
        {
            _scoreSystem.OnScoreChanged -= UpdateScore;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public override void OnEscape()
        {
            TryOpenPause();
        }

        public async UniTask AnimateStartPrompt(string startPrompt)
        {
            _promptAnimating = true;
            _view.SetScreenPrompt(startPrompt);

            await _view.AnimatePromptColor(TEXT_STAY_TIME +
                SYMBOL_STAY_TIME * startPrompt.Length, TEXT_ANIMATION_TIME);

            _promptAnimating = false;
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
            return _playerMovementSystem.CanMove && !_promptAnimating;
        }
    }
}
