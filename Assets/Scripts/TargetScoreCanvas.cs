using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
    public class TargetScoreCanvas : MonoBehaviour
    {
		[SerializeField] float lifetime = 2f;
		[SerializeField] Vector3 endScale = new Vector3(1.2f, 1.2f, 1.2f); 
		[SerializeField] Vector3 deltaPos = new Vector3(0, 1f, 0);
		[SerializeField] Color positiveScoreColor = Color.green;
		[SerializeField] Color negativeScoreColor = Color.red;
		[SerializeField] Color bonusScoreColor = Color.yellow;
		[SerializeField] Color zeroScoreColor = Color.gray;

		float elapsedTime;
		Text scoreText;
		Vector3 initPos, endPos;

        private void Awake()
        {
            scoreText = GetComponentInChildren<Text>();
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= lifetime)
            {
                gameObject.SetActive(false);
            }
				
            float amount = elapsedTime / lifetime;
			float colorLerp = MJLerp.Lerp (amount, MJLerpMode.BEZIER_INVERT);
			float slerp = MJLerp.LerpBezier (amount);

			scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, colorLerp);
			transform.localScale = Vector3.Lerp(Vector3.zero, endScale, slerp);
			transform.localPosition = Vector3.Lerp (initPos, endPos, slerp);
        }

		public void Show(int score, bool isBonus, Vector3 worldPosition, Vector3 scale, Vector3 lookTarget)
        {
            transform.position = worldPosition;
            scoreText.rectTransform.localScale = scale;
            scoreText.text = score.ToString();
			transform.localScale = Vector3.zero;
			initPos = transform.localPosition;
			endPos = initPos + deltaPos;
			elapsedTime = 0;
            if (isBonus)
            {
                scoreText.color = bonusScoreColor;
            }
            else if (score > 0)
            {
                scoreText.color = positiveScoreColor;
            }
            else if (score == 0)
            {
                scoreText.color = zeroScoreColor;
            }
            else
            {
                scoreText.color = negativeScoreColor;
            }

			transform.LookAt (transform.position + (transform.position - lookTarget));
            gameObject.SetActive(true);
        }
    }
}