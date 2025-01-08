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
    [Header("Localization")]
    [SerializeField] private Toggle _ruToggle;
    [SerializeField] private Toggle _enToggle;

    public event Action OnStartClick;
    public event Action OnSettingsClick;
    public event Action OnAuthorClick;
    public event Action OnExitClick;
    public event Action<bool> OnRuToggleChanged;
    public event Action<bool> OnEnToggleChanged;

    private void Awake()
    {
        _startBtn.onClick.AddListener(() => OnStartClick?.Invoke());
        _settingsBtn.onClick.AddListener(() => OnSettingsClick?.Invoke());
        _authorBtn.onClick.AddListener(() => OnAuthorClick?.Invoke());
        _exitBtn.onClick.AddListener(() => OnExitClick?.Invoke());
        _ruToggle.onValueChanged.AddListener((x) => OnRuToggleChanged?.Invoke(x));
        _enToggle.onValueChanged.AddListener((x) => OnEnToggleChanged?.Invoke(x));
    }

    public void SetHiScoreText(string text)
    {
        _hiScoreText.text = text;
    }

    public void InitToggles(int localeIndex)
    {
        if (localeIndex == 0)
        {
            _enToggle.SetIsOnWithoutNotify(true);
            return;
        }

        _ruToggle.SetIsOnWithoutNotify(true);
    }
}
