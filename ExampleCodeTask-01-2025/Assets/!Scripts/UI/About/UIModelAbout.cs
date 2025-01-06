using Cysharp.Threading.Tasks;
using Zenject;

public class UIModelAbout : UIModel<UIViewAbout>
{
    [Inject] private UIModelMenu _menu;

    public override string ViewName => "UI/About";

    public override void OnInit()
    {
        _view.OnExitClick += Exit;
    }

    public void Exit()
    {
        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_menu).Forget();
    }
}
