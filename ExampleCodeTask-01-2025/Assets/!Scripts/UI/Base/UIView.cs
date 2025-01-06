using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [Header("Close buttons")]
    [SerializeField] private Button[] _closeButtons;

    public event Action OnClickClose;

    private void Awake()
    {
        if (_closeButtons == null)
        {
            return;
        }

        foreach (var b in _closeButtons)
        {
            b.onClick.AddListener(() => OnClickClose?.Invoke());
        }
    }

    public virtual async UniTask Show()
    {
        gameObject.SetActive(true);
    }

    public virtual async UniTask Hide()
    {
        gameObject.SetActive(false);
    }
}
