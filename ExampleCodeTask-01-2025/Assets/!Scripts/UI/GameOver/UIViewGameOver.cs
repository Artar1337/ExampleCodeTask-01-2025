using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class UIViewGameOver : UIViewFadable
    {
        [Header("Game over")]
        [SerializeField] private TMP_Text _hiScoreText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _againBtn;
        [SerializeField] private Button _toMenuBtn;

        public event Action OnAgainClick;
        public event Action OnExitClick;

        private void Awake()
        {
            _againBtn.onClick.AddListener(() => OnAgainClick?.Invoke());
            _toMenuBtn.onClick.AddListener(() => OnExitClick?.Invoke());
        }

        public void SetScoresText(string hiscore, string score)
        {
            _hiScoreText.text = hiscore;
            _scoreText.text = score;
        }
    }
}
