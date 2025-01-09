using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Views
{
    public abstract class UIView : MonoBehaviour
    {
        public virtual UniTask Show()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public virtual UniTask Hide()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}
