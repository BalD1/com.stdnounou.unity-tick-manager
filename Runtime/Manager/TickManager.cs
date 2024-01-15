using StdNounou.Core;
using UnityEngine;

namespace StdNounou.Tick
{
    public class TickManager : Singleton<TickManager>
    {
        public const float TICK_TIMER_MAX = .25f;

        private int tick;
        private float tickTimer;

        protected override void Awake()
        {
            base.Awake();
            tick = 0;
        }

        protected override void EventsSubscriber()
        {
        }

        protected override void EventsUnSubscriber()
        {
        }

        private void Update()
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= TICK_TIMER_MAX)
            {
                tickTimer -= TICK_TIMER_MAX;
                tick++;
                this.PerformTick(tick: tick);
            }
        }
    } 
}