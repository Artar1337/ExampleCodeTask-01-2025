using System;
using UnityEngine;
using UnityEngine.UI;

public class UIViewPause : UIViewFadable
{
    [Header("Pause")]
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Button _authorBtn;
    [SerializeField] private Button _exitBtn;

    public event Action OnStartClick;
    public event Action OnSettingsClick;
    public event Action OnAuthorClick;
    public event Action OnExitClick;

    private void Awake()
    {
        _startBtn.onClick.AddListener(() => OnStartClick?.Invoke());
        _settingsBtn.onClick.AddListener(() => OnSettingsClick?.Invoke());
        _authorBtn.onClick.AddListener(() => OnAuthorClick?.Invoke());
        _exitBtn.onClick.AddListener(() => OnExitClick?.Invoke());
    }
}
