using UnityEngine;

namespace StdNounou.TickManager.Samples
{
    public class TimerUIDebugger : MonoBehaviour
    {
        [SerializeField] private TimerHandler timerHandler;
        [SerializeField] private TextFormatter formatter;
        [SerializeField] private E_TimerParamsType timerParamsTypes;

        public enum E_TimerParamsType
        {
            MaxDuration,
            RemainingTime,
            RemainingTicks,
            IsLooping,
            LoopingCount,
            IsRunning,
        }

        private void Update()
        {
            DisplayParam();
        }

        private void DisplayParam()
        {
            string param = "NULL";
            switch (timerParamsTypes)
            {
                case E_TimerParamsType.MaxDuration:
                    param = timerHandler.Timer.MaxDuration.ToString();
                    break;

                case E_TimerParamsType.RemainingTime:
                    param = timerHandler.Timer.RemainingTimeInSeconds().ToString();
                    break;

                case E_TimerParamsType.RemainingTicks:
                    param = timerHandler.Timer.RemainingTicks().ToString();
                    break;

                case E_TimerParamsType.IsLooping:
                    param = timerHandler.Timer.Loop.ToString();
                    break;

                case E_TimerParamsType.LoopingCount:
                    param = timerHandler.Timer.CurrentLoopCount.ToString();
                    break;

                case E_TimerParamsType.IsRunning:
                    param = timerHandler.Timer.IsRunning().ToString();
                    break;
            }
            formatter.SetText(param);
        }
    } 
}
