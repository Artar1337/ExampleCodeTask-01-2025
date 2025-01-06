using Cysharp.Threading.Tasks;
using Zenject;

public class UIModelGameOver : UIModel<UIViewGameOver>
{
    public override string ViewName => "UI/GameOver";

    [Inject] private UIModelMenu _menuModel;

    public override void OnInit()
    {
        _view.OnAgainClick += ReStart;
        _view.OnExitClick += OpenMenu;
    }

    public override void OnShow()
    {
        _view.SetScoresText("Рекорд: 1222", "Ваш счет: 228");
    }

    public void ReStart()
    {

    }

    public void OpenMenu()
    {
        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_menuModel).Forget();
    }
}
