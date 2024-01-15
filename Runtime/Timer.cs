using System;

namespace StdNounou.TickManager
{
    public class Timer : ITickable, IDisposable
    {
        private float maxDuration;
        private float duration;
        private int remainingTicks;

        private event Action onEnd;

        private bool isSubscribed;
        private bool loop;
        private int loopCount = -1;

        public Timer(float duration, Action onEnd)
            => Setup(duration, onEnd, false, 0);
        public Timer(float duration, Action onEnd, bool loop)
            => Setup(duration, onEnd, loop, -1);
        public Timer(float duration, Action onEnd, bool loop, int loopCount)
            => Setup(duration, onEnd, loop, loopCount);

        private void Setup(float duration, Action onEnd, bool loop, int loopCount)
        {
            this.maxDuration = this.duration = duration;
            this.onEnd = onEnd;
            remainingTicks = (int)(duration / TickManager.TICK_TIMER_MAX);

            this.loop = loop;
            this.loopCount = loopCount;
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
            => Restart(callEndAction, maxDuration);

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
            => Reset(maxDuration);

        /// <summary>
        /// Will set the max time and duration to original <paramref name="newTime"/>. Will start the timer again if it is stopped.
        /// </summary>
        /// <param name="newTime"></param>
        private void Reset(float newTime)
        {
            this.duration = newTime;
            this.remainingTicks = (int)(duration / TickManager.TICK_TIMER_MAX);
        }

        public void OnTick(int tick)
        {
            remainingTicks--;
            duration -= TickManager.TICK_TIMER_MAX;

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
            this.loop = loop;
            this.loopCount = loopCount;
        }

        public int RemainingTicks()
            => remainingTicks;

        public float RemainingTimeInSeconds()
            => duration;

        private void OnEnd()
        {
            onEnd?.Invoke();

            if (loop)
            {
                if (loopCount > 0)
                {
                    loopCount--;
                    Reset();
                    return;
                }
                else if (loopCount == 0) loop = false;
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
