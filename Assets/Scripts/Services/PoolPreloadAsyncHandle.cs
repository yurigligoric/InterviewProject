using System;
using UnityEngine;

namespace Tools.Pool
{
    public delegate float PreloadPass();
    
    public class PoolPreloadAsyncHandle : CustomYieldInstruction
    {
        public float Progress { get; private set; }

        private bool _isDone;
        public bool IsDone { 
            get => _isDone;
            private set
            {
                _isDone = value;
                if (_isDone)
                {
                    OnCompleted?.Invoke(this);
                }
            }
        }

        private readonly PreloadPass _preloadPass;

        public event Action<PoolPreloadAsyncHandle> OnCompleted;
        public event Action<PoolPreloadAsyncHandle> OnProgressing;

        public override bool keepWaiting {
            get
            {
                Progress = _preloadPass.Invoke();
                OnProgressing?.Invoke(this);
                
                if (Progress < 1.0f)
                {
                    return true;
                }
                else
                {
                    IsDone = true;
                    return false;
                }
            }
        }
        
        public PoolPreloadAsyncHandle(PreloadPass preloadPass)
        {
            _preloadPass = preloadPass;
        }
    }
}