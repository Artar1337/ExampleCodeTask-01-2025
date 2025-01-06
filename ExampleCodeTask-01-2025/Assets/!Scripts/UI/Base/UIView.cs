using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIView : MonoBehaviour
{
    [Header("Close buttons")]
    [SerializeField] private Button[] _closeButtons;

    public event Action OnCloseClick;

    private void Awake()
    {
        if (_closeButtons == null)
        {
            return;
        }

        foreach (var b in _closeButtons)
        {
            b.onClick.AddListener(() => OnCloseClick?.Invoke());
        }
    }

    public virtual UniTask Show()
    {
        gameObject.SetActive(true);
        return UniTask.CompletedTask;
    }

    public virtual UniTask Hide()
    {
        gameObject.SetActive(false);
        return UniTask.CompletedTask;
    }
}
