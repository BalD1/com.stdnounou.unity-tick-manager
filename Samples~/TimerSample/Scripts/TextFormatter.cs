using UnityEngine;
using UnityEngine.UI;

namespace StdNounou.TickManager.Samples
{
    [System.Serializable]
    public class TextFormatter
    {
        [SerializeField] private Text uiText;
        [SerializeField] private string format;

        public void SetText(string arg)
        {
            uiText.text = string.Format(format, arg);
        }
    } 
}
