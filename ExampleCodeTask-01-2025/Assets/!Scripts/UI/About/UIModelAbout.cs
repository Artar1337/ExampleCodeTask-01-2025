using Cysharp.Threading.Tasks;
using UI.Views;
using Zenject;

namespace UI.Models
{
    public class UIModelAbout : UIModel<UIViewAbout>
    {
        [Inject] private UIModelMenu _menu;

        public override string ViewName => "UI/About";

        public override void OnInit()
        {
            _view.OnExitClick += Exit;
        }

        public override void OnEscape()
        {
            Exit();
        }

        public void Exit()
        {
            _uiSystem.Hide(this).Forget();
            _uiSystem.Show(_menu).Forget();
        }
    }
}
