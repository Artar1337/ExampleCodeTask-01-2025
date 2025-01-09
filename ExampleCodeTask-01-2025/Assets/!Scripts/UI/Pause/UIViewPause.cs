using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class UIViewPause : UIViewFadable
    {
        [Header("Pause")]
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _settingsBtn;
        [SerializeField] private Button _exitBtn;

        public event Action OnStartClick;
        public event Action OnSettingsClick;
        public event Action OnExitClick;

        private void Awake()
        {
            _startBtn.onClick.AddListener(() => OnStartClick?.Invoke());
            _settingsBtn.onClick.AddListener(() => OnSettingsClick?.Invoke());
            _exitBtn.onClick.AddListener(() => OnExitClick?.Invoke());
        }
    }
}
