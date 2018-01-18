using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
    public class TimerScript : MonoBehaviour
    {

		[SerializeField] private float scaleTime = 0.5f;
		[SerializeField] private Vector3 scale;
		public string gameOverText = "Your score!";
        public Color littleTimeLeftColor = Color.yellow, noTimeLeftColor = Color.red;
		private Color normalColor;
        public float someTimeLeft, littleTimeLeft;    //Completely yellow

        private float timeLeft, normalTime;
        private Text text;

		private Vector3 initScale;
		private float elapsedScaleTime = 0f;
		private bool animate = false;
		private string originalText;

        // Use this for initialization
        void Start()
        {
            this.text = GetComponent<Text>();
			this.normalColor = text.color;
			this.normalTime = someTimeLeft - littleTimeLeft;
			this.initScale = text.rectTransform.localScale;
			originalText = text.text;
			timeLeft = someTimeLeft + 10f;
			ResetVisuals();
        }

        // Update is called once per frame
        void Update()
        {
			if (!animate) {
				return;
			}

			if (timeLeft < littleTimeLeft) {
				elapsedScaleTime += Time.deltaTime;
				text.rectTransform.localScale = Vector3.Lerp(initScale, scale, Mathf.PingPong(elapsedScaleTime, scaleTime));
			}
        }

        public void UpdateTimeLeft(float timeLeft)
        {
			if (!animate) {
				return;
			}
            Color lerpColor;
			if (timeLeft > someTimeLeft) {
				lerpColor = normalColor;
			} else  if (timeLeft > littleTimeLeft)
            {
                float amount = 1f - (timeLeft - littleTimeLeft) / normalTime;
                lerpColor = Color.Lerp(normalColor, littleTimeLeftColor, amount);
            }
            else
            {
                float amount = 1f - (timeLeft / littleTimeLeft);
                lerpColor = Color.Lerp(littleTimeLeftColor, noTimeLeftColor, amount);
            }
            text.color = lerpColor;
			text.text = FormatTimeLeft(timeLeft);
			this.timeLeft = timeLeft;
        }

        public void ResetVisuals()
        {
			text.text = originalText;
            text.color = normalColor;
			text.rectTransform.localScale = initScale;
        }

		public void Begin(float timeLeft) {
			animate = true;
			ResetVisuals();
			this.timeLeft = timeLeft;
			UpdateTimeLeft(timeLeft);
		}

		public void End() {
			animate = false;
			ResetVisuals();
			text.text = gameOverText;
		}

		private string FormatTimeLeft(float timeLeft)
        {
            int timeLeftInt = (int) timeLeft;
            int minutes = timeLeftInt / 60;
            int seconds = timeLeftInt % 60;
            string minuteString = minutes > 9 ? minutes.ToString() : "0" + minutes.ToString();
            string secondsString = seconds > 9 ? seconds.ToString() : "0" + seconds.ToString();
            return string.Format("{0}:{1}", minuteString, secondsString);
        }
    }
}