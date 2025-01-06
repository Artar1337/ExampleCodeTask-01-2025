using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIViewHud : UIViewFadable
{
    [Header("Hud")]
    [SerializeField] private Button _exitBtn;
    [SerializeField] private TMP_Text _score;

    public event Action OnMenuClick;

    private void Awake()
    {
        _exitBtn.onClick.AddListener(() => OnMenuClick?.Invoke());
    }

    public void SetScoreText(string text)
    {
        _score.text = text;
    }
}
