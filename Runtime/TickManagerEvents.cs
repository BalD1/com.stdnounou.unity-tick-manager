using System;

namespace StdNounou.TickManager
{
    public static class TickManagerEvents
    {
        public static event Action<int> OnTick;
        public static void PerformTick(this TickManager manager, int tick)
            => OnTick?.Invoke(tick);
    }
}