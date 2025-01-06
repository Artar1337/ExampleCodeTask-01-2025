using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public interface IUIModel
{
    bool Shown { get; }
    UniTask Show();
    UniTask Hide();
    UIView View { get; }
}

public abstract class UIModel<T> : IUIModel where T : UIView
{
    [Inject] protected IResourcesSystem _resourcesSystem;
    [Inject] protected IUISystem _uiSystem;

    protected T _view;
    public UIView View => _view;

    public bool Shown => _uiSystem.IsShown(this);

    public abstract string ViewName { get; }

    public virtual void OnShow()
    {

    }

    public virtual void OnHide()
    {

    }

    public virtual void OnInit()
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
    }
}
