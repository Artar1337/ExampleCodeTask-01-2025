using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class UIViewAbout : UIViewFadable
    {
        [Header("About")]
        [SerializeField] private Button _exitBtn;
        [SerializeField] private ScrollRect _scroll;

        public event Action OnExitClick;

        private void Awake()
        {
            _exitBtn.onClick.AddListener(() => OnExitClick?.Invoke());
        }

        private void OnEnable()
        {
            _scroll.verticalNormalizedPosition = 1f;
        }
    }
}
