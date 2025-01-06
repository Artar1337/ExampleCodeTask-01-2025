using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public abstract class UIViewFadable : UIView
{
    [Header("Fade animation")]
    [SerializeField] private float _showTimeSeconds = 0.3f;
    [SerializeField] private float _hideTimeSeconds = 0.3f;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
    }

    public override async UniTask Hide()
    {
        _canvasGroup.interactable = false;
        await _canvasGroup.DOFade(0f, _hideTimeSeconds).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }

    public override async UniTask Show()
    {
        _canvasGroup.interactable = false;
        gameObject.SetActive(true);
        await _canvasGroup.DOFade(1f, _showTimeSeconds).AsyncWaitForCompletion();
        _canvasGroup.interactable = true;
    }
}
