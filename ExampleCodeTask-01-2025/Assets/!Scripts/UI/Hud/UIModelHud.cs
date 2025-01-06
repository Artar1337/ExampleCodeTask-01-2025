using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class UIModelHud : UIModel<UIViewHud>
{
    [Inject] private UIModelPause _pause;

    public override string ViewName => "UI/Hud";

    public override void OnInit()
    {
        _view.OnMenuClick += OpenPause;
    }

    public void OpenPause()
    {
        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_pause).Forget();
    }
}
