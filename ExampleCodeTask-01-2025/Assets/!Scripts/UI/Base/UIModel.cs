using Cysharp.Threading.Tasks;
using Logic.Systems;
using UI.Views;
using UnityEngine;
using Zenject;

namespace UI.Models
{
    public interface IUIModel
    {
        bool IsSilent { get; }
        bool Shown { get; }
        UniTask Show();
        UniTask Hide();
        UIView View { get; }
    }

    public abstract class UIModel<T> : IUIModel where T : UIView
    {
        [Inject] protected IResourcesSystem _resourcesSystem;
        [Inject] protected IUISystem _uiSystem;
        [Inject] protected IEscapePressHandler _escapePressHandler;

        protected T _view;
        public UIView View => _view;

        public bool Shown => _uiSystem.IsShown(this);

        public abstract string ViewName { get; }
        public virtual bool IsSilent => false;

        public virtual void OnShow()
        {

        }

        public virtual void OnHide()
        {

        }

        public virtual void OnInit()
        {

        }

        public virtual void OnEscape()
        {

        }

        public async UniTask Show()
        {
            if (_view == null)
            {
                await Init();
            }

            OnShow();

            _view.transform.SetAsLastSibling();
            await _view.Show();
        }

        public async UniTask Hide()
        {
            if (_view == null)
            {
                return;
            }

            await View.Hide();

            OnHide();
        }

        private void Escape()
        {
            if (_view == null ||
                !_view.gameObject.activeInHierarchy ||
                _uiSystem.OperationsInProcess != 0)
            {
                return;
            }

            OnEscape();
        }

        private async UniTask Init()
        {
            if (_view != null)
            {
                return;
            }

            _view = await _resourcesSystem.Instantiate<T>(ViewName, _uiSystem.UIRoot);

            if (_view == null)
            {
                Debug.LogError($"{typeof(T)}: init failed: view (asset name: '{ViewName}') is null");
                return;
            }

            OnInit();

            _view.gameObject.SetActive(false);
            _escapePressHandler.OnEscapePressed += Escape;
        }
    }
}
