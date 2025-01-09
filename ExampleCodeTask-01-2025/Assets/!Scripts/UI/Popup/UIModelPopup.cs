using Cysharp.Threading.Tasks;
using System;
using UI.Views;

namespace UI.Models
{
    public class UIModelPopup : UIModel<UIViewPopup>
    {
        public override string ViewName => "UI/Popup";
        public override bool IsSilent => true;

        private Action _onOk;
        private Action _onCancel;
        private string _cachedHeader;
        private string _cachedBody;

        public override void OnShow()
        {
            _view.InitializeText(_cachedHeader, _cachedBody);
        }

        public override void OnInit()
        {
            _view.OnCancelClick += Cancel;
            _view.OnOkClick += Ok;
        }

        public override void OnEscape()
        {
            Cancel();
        }

        public void Init(string header, string body, Action onOk, Action onCancel)
        {
            _onOk = onOk;
            _onCancel = onCancel;
            _cachedHeader = header;
            _cachedBody = body;
        }

        private void Ok()
        {
            _onOk?.Invoke();
            _uiSystem.Hide(this).Forget();
        }

        private void Cancel()
        {
            _onCancel?.Invoke();
            _uiSystem.Hide(this).Forget();
        }
    }
}
