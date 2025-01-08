using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class UIViewHud : UIViewFadable
{
    [Header("Hud")]
    [SerializeField] private Button _exitBtn;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _screenPrompt;

    public event Action OnMenuClick;

    private void Awake()
    {
        _exitBtn.onClick.AddListener(() => OnMenuClick?.Invoke());
    }

    public void SetScoreText(string text)
    {
        _score.text = text;
    }

    public void SetScreenPrompt(string text)
    {
        _screenPrompt.text = text;
    }

    public async UniTask AnimatePromptColor(float duration)
    {
        await _screenPrompt.DOFade(0f, duration).
            From(1f).AsyncWaitForCompletion();
    }
}
