using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Logic.Systems
{
    public interface IEscapePressHandler
    {
        event Action OnEscapePressed;
        void Start();
        void Stop();
    }

    public class EscapePressHandler : IEscapePressHandler, IDisposable
    {
        [Inject] private IUISystem _uiSystem;

        private CancellationTokenSource _cts = new();

        public event Action OnEscapePressed;

        public void Start()
        {
            Stop();
            ProcessEscapeTask().Forget();
        }

        public void Stop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new();
        }

        private async UniTask ProcessEscapeTask()
        {
            while (true)
            {
                try
                {
                    await UniTask.Yield(cancellationToken: _cts.Token);
                }
                catch(OperationCanceledException)
                {
                    return;
                }

                if (_uiSystem.OperationsInProcess != 0)
                {
                    continue;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OnEscapePressed?.Invoke();
                }
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
