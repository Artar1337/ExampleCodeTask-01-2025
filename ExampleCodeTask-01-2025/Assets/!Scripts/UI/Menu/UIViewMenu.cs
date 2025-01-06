using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIViewMenu : UIViewFadable
{
    [Header("Menu")]
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Button _authorBtn;
    [SerializeField] private Button _exitBtn;
    [SerializeField] private TMP_Text _hiScoreText;

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

    public void SetHiScoreText(string text)
    {
        _hiScoreText.text = text;
    }
}
