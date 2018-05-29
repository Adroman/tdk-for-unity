using System;
using System.Collections;
using Scrips.CustomTypes;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.BuffData
{
    public abstract class BaseBuffData
    {
        public bool IsActive { get; private set; }
        public float TimeLeft { get; private set; }
        public bool Ended { get; private set; }

        public Coroutine ActiveCoroutine;

        protected bool FailedToStart;

        public EnemyInstance Target { get; }

        private readonly Duration _duration;

        protected BaseBuffData(EnemyInstance target, Duration duration)
        {
            Target = target;
            _duration = duration;
            TimeLeft = duration.ToFloat();

            OnStartEffect += TryAddDebuff;
            OnStopEffect += () => Target.ActiveDebuffs.Remove(this);
            OnUpdate += UpdateRemainingTime;
        }

        public event Action OnStartEffect;
        public event Action OnStopEffect;
        public event Action<float> OnUpdate;

        private void StartEffect()
        {
            if (IsActive || Ended) return;

            IsActive = true;
            OnStartEffect?.Invoke();
        }

        protected void StopEffect()
        {
            if (!IsActive || Ended) return;

            IsActive = false;
            Ended = true;
            OnStopEffect?.Invoke();
        }

        public void Update(float deltaTime)
        {
            if (!IsActive) return;

            OnUpdate?.Invoke(deltaTime);
        }

        public void UpdateRemainingTime(float deltaTime)
        {
            TimeLeft -= deltaTime;
        }

        public void ActivateEffect()
        {
            if (FailedToStart) return;
            ActiveCoroutine = Target.StartCoroutine(UntilEnd());
        }

        protected virtual void TryAddDebuff()
        {
            Target.ActiveDebuffs.Add(this);
            FailedToStart = false;
        }

        private IEnumerator UntilEnd()
        {
            StartEffect();
            yield return _duration.UntilEnds();
            StopEffect();
        }
    }
}