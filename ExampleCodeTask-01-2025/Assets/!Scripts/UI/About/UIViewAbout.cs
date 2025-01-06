using System;
using UnityEngine;
using UnityEngine.UI;

public class UIViewAbout : UIViewFadable
{
    [Header("About")]
    [SerializeField] private Button _exitBtn;

    public event Action OnExitClick;

    private void Awake()
    {
        _exitBtn.onClick.AddListener(() => OnExitClick?.Invoke());
    }
}
