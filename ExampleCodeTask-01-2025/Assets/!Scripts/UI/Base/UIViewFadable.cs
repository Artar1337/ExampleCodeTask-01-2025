using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class UIViewFadable : UIView
{
    [Header("Fade animation")]
    [SerializeField] private float _showTimeSeconds = 0.3f;
    [SerializeField] private float _hideTimeSeconds = 0.3f;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
    }

    public override async UniTask Hide()
    {
        await _canvasGroup.DOFade(0f, _hideTimeSeconds).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }

    public override async UniTask Show()
    {
        gameObject.SetActive(true);
        await _canvasGroup.DOFade(1f, _showTimeSeconds).AsyncWaitForCompletion();
    }
}
