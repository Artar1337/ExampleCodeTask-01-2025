using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IUISystem
{
    event Action OnViewHideStart;
    event Action OnViewShowStart;
    HashSet<IUIModel> ShownModels { get; }
    Transform UIRoot { get; }

    bool IsShown(IUIModel model);
    UniTask Show(IUIModel model);
    UniTask Hide(IUIModel model);
}

public class UISystem : IUISystem
{
    public Transform UIRoot { get; private set; }
    public HashSet<IUIModel> ShownModels { get; private set; } = new();

    public UISystem(UISystemMediator uiSystemMediator)
    {
        UIRoot = uiSystemMediator.UIRoot;
    }

    public event Action OnViewHideStart;
    public event Action OnViewShowStart;

    public async UniTask Show(IUIModel model)
    {
        if (IsShown(model))
        {
            return;
        }

        if (!model.IsSilent)
        {
            OnViewShowStart?.Invoke();
        }
        
        await model.Show();

        if (!IsShown(model))
        {
            ShownModels.Add(model);
        }
    }

    public async UniTask Hide(IUIModel model)
    {
        if (!IsShown(model))
        {
            return;
        }

        if (!model.IsSilent)
        {
            OnViewHideStart?.Invoke();
        }

        await model.Hide();

        if (IsShown(model))
        {
            ShownModels.Remove(model);
        }
    }

    public bool IsShown(IUIModel model)
    {
        return ShownModels.Contains(model);
    }
}
