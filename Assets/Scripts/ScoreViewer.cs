using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
    public class ScoreViewer : MonoBehaviour
    {
        public float lifetime;
        public float initialSpeed = 5f;
        public Vector3 endScale = new Vector3(1.2f, 1.2f, 1.2f);

        private float elapsedTime;
        private Text scoreText;
        private Vector3 target = Vector3.zero;

        public Color positiveScoreColor = Color.green, negativeScoreColor = Color.red, bonusScoreColor = Color.yellow, zeroColor = Color.gray;

        private void Awake()
        {
            scoreText = GetComponentInChildren<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= lifetime)
            {
                scoreText.text = "";
                elapsedTime = 0;
                gameObject.SetActive(false);
            }

            float amount = elapsedTime / lifetime;
            float currentSpeed = Mathf.Lerp(initialSpeed, 0, amount);
            transform.position = transform.position + Vector3.up * currentSpeed * Time.deltaTime;
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 1f - amount);
            transform.localScale = Vector3.Lerp(Vector3.one, endScale, amount);
            LookAt(target);
        }

        public void Show(int score, bool isBonus, Vector3 worldPosition)
        {
            transform.position = worldPosition;
            scoreText.text = score.ToString();
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
                scoreText.color = zeroColor;
            }
            else
            {
                scoreText.color = negativeScoreColor;
            }
            target = GameObject.FindGameObjectWithTag("Player").transform.position;
            LookAt(target);
            gameObject.SetActive(true);
        }

        private void LookAt(Vector3 target)
        {
            var diff = transform.position - target;
            transform.LookAt(transform.position + diff);
        }
    }
}