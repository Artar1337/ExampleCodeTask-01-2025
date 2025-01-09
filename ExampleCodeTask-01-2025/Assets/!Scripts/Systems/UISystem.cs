using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mono.Installers;
using UI.Models;
using UnityEngine;

namespace Logic.Systems
{
    public interface IUISystem
    {
        event Action OnViewHideStart;
        event Action OnViewShowStart;
        HashSet<IUIModel> ShownModels { get; }
        Transform UIRoot { get; }
        int OperationsInProcess { get; }

        bool IsShown(IUIModel model);
        UniTask Show(IUIModel model);
        UniTask Hide(IUIModel model);
    }

    public class UISystem : IUISystem
    {
        public Transform UIRoot { get; private set; }
        public HashSet<IUIModel> ShownModels { get; private set; } = new();
        public int OperationsInProcess { get; private set; } = 0;

        public event Action OnViewHideStart;
        public event Action OnViewShowStart;

        public UISystem(UISystemMediator uiSystemMediator)
        {
            UIRoot = uiSystemMediator.UIRoot;
        }

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

            OperationsInProcess++;
            await model.Show();

            if (!IsShown(model))
            {
                ShownModels.Add(model);
            }

            OperationsInProcess--;
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

            OperationsInProcess++;
            await model.Hide();

            if (IsShown(model))
            {
                ShownModels.Remove(model);
            }

            OperationsInProcess--;
        }

        public bool IsShown(IUIModel model)
        {
            return ShownModels.Contains(model);
        }
    }
}
