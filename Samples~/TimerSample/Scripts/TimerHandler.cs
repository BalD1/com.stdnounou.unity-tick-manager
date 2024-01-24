using StdNounou.Tick;
using UnityEngine;
using UnityEngine.UI;

namespace StdNounou.TickManager.Samples
{
    public class TimerHandler : MonoBehaviour
    {
        public Timer Timer { get; private set; }

        private float duration = 5;
        private bool loop = true;
        private int loopCount = -1;

        [SerializeField] private InputField maxDurationInputField;
        [SerializeField] private InputField loopCountInputField;

        [SerializeField] private Toggle loopToggle;
        [SerializeField] private Toggle stop_callEnd;
        [SerializeField] private Toggle restart_callEnd;

        private void Awake()
        {
            Timer = new Timer(duration, () => Debug.Log("Timer Ended!"), loop, loopCount);
        }

        public void SetDuration()
        {
            float time = duration;
            if (float.TryParse(maxDurationInputField.text, out float newTime)) time = newTime;
            Timer.Restart(false, time);
        }

        public void SetLoop()
        {
            int count = loopCount;
            if (int.TryParse(loopCountInputField.text, out int newCount)) count = newCount;
            Timer.SetLoop(loopToggle.isOn, count);
        }

        public void Pause()
            => Timer.Pause();

        public void Stop()
            => Timer.Stop(stop_callEnd.isOn);

        public void Restart()
            => Timer.Restart(restart_callEnd.isOn);

        public void TimerStart()
            => Timer.Start();
    } 
}
