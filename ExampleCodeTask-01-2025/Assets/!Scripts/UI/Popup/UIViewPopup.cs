using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class UIViewPopup : UIViewFadable
    {
        [Header("Popup")]
        [SerializeField] private Button _okButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_Text _body;

        public event Action OnOkClick;
        public event Action OnCancelClick;

        private void Awake()
        {
            _okButton.onClick.AddListener(() => OnOkClick?.Invoke());
            _cancelButton.onClick.AddListener(() => OnCancelClick?.Invoke());
        }

        public void InitializeText(string header, string body)
        {
            _header.text = header;
            _body.text = body;
        }
    }
}
