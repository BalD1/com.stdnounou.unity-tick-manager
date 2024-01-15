using System;

namespace StdNounou.Tick
{
    public class Timer : ITickable, IDisposable
    {
        public float MaxDuration { get; private set; }
        public float Duration { get; private set; }
        private int remainingTicks;

        private event Action onEnd;

        private bool isSubscribed;
        public bool Loop { get; private set; }
        public int CurrentLoopCount { get; private set; } = -1;

        public Timer(float duration, Action onEnd)
            => Setup(duration, onEnd, false, 0);
        public Timer(float duration, Action onEnd, bool loop)
            => Setup(duration, onEnd, loop, -1);
        public Timer(float duration, Action onEnd, bool loop, int loopCount)
            => Setup(duration, onEnd, loop, loopCount);

        private void Setup(float duration, Action onEnd, bool loop, int loopCount)
        {
            this.MaxDuration = this.Duration = duration;
            this.onEnd = onEnd;
            remainingTicks = (int)(duration / TickManager.TICK_TIMER_MAX);

            this.Loop = loop;
            this.CurrentLoopCount = loopCount;
        }

        private void Subscriber()
        {
            if (isSubscribed) return;
            TickManagerEvents.OnTick += OnTick;
            isSubscribed = true;
        }
        private void Unsubscriber()
        {
            TickManagerEvents.OnTick -= OnTick;
            isSubscribed = false;
        }

        public void Dispose()
        {
            Unsubscriber();
        }

        /// <summary>
        /// Starts the timer. Will check if it is not already running.
        /// </summary>
        public void Start()
            => Subscriber();

        public void Pause()
            => Unsubscriber();

        /// <summary>
        /// Completly stops and resets the timer.
        /// </summary>
        /// <param name="callEndAction"></param>
        public void Stop(bool callEndAction)
        {
            Unsubscriber();
            Reset();

            if (callEndAction) onEnd?.Invoke();
        }

        /// <summary>
        /// Will start the timer again from max duration.
        /// /// </summary>
        /// <param name="callEndAction"></param>
        public void Restart(bool callEndAction)
            => Restart(callEndAction, MaxDuration);

        /// <summary>
        /// Will start the timer again from <paramref name="newDuration"/>.
        /// </summary>
        /// <param name="callEndAction"></param>
        /// <param name="newDuration"></param>
        public void Restart(bool callEndAction, float newDuration)
        {
            Reset(newDuration);
            if (callEndAction) onEnd?.Invoke();
            Subscriber();
        }

        /// <summary>
        /// Will set the max time and duration to original max duration. Will start the timer again if it is stopped.
        /// </summary>
        public void Reset()
            => Reset(MaxDuration);

        /// <summary>
        /// Will set the max time and duration to original <paramref name="newTime"/>. Will start the timer again if it is stopped.
        /// </summary>
        /// <param name="newTime"></param>
        private void Reset(float newTime)
        {
            this.MaxDuration = this.Duration = newTime;
            this.remainingTicks = (int)(Duration / TickManager.TICK_TIMER_MAX);
        }

        public void OnTick(int tick)
        {
            remainingTicks--;
            Duration -= TickManager.TICK_TIMER_MAX;

            if (remainingTicks <= 0) OnEnd();
        }

        /// <summary>
        /// <para>If true, the timer will indefinitly loop on end.</para> 
        /// <para>See <seealso cref="SetLoop(bool, int)"/> to set a loop count.</para>
        /// </summary>
        /// <param name="loop"></param>
        public void SetLoop(bool loop)
            => SetLoop(loop, -1);
        /// <summary>
        /// <para>If true, the timer will loop for <paramref name="loopCount"/> on end.</para> 
        /// </summary>
        /// <param name="loop"></param>
        /// <param name="loopCount"></param>
        public void SetLoop(bool loop, int loopCount)
        {
            this.Loop = loop;
            this.CurrentLoopCount = loopCount;
        }

        public int RemainingTicks()
            => remainingTicks;

        public float RemainingTimeInSeconds()
            => Duration;

        public bool IsRunning()
            => isSubscribed;

        private void OnEnd()
        {
            onEnd?.Invoke();

            if (Loop)
            {
                if (CurrentLoopCount > 0)
                {
                    CurrentLoopCount--;
                    Reset();
                    return;
                }
                else if (CurrentLoopCount == 0) Loop = false;
                else
                {
                    Reset();
                    return;
                }
            }
            Unsubscriber();
        }
    } 
}
