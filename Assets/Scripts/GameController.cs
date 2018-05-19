using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
	public class GameController : MonoBehaviour, IHitlistener, IFireListener
    {

        public float gameTime;
        private float timeLeft;

        private bool isPlaying = false;

        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        public enum PlayType
        {
            VR, NONVR
        }

        public PlayType playType;
        public GameObject vrPlayer, nonVRPlayer;
        public Text scoreText, nHitsText, nMissText, streakText, bestStreakText;

        [SerializeField] private TimerScript timer;

        private ITargetSpawner[] gameTargetSpawners;

        private Map<IWeapon, int> weaponScores;

        [SerializeField] private MultiplierStreak[] multiplierStreak;
        private int streakCounter, hitCount, missCount, bestStreak;
        private StartStop st;
		public MJPlayer mjPlayer;

        void Awake()
        {
            weaponScores = new Map<IWeapon, int>();
        }

        void Start()
        {
            var spawners = GameObject.FindGameObjectsWithTag(Tags.GAME_TARGET_SPAWNER);
            gameTargetSpawners = new ITargetSpawner[spawners.Length];
			HandleScoring (0, 0, 0, 0, 0);
            for (var i = 0; i < gameTargetSpawners.Length; i++)
            {
                gameTargetSpawners[i] = spawners[i].GetComponent<ITargetSpawner>();
            }
            if (playType == PlayType.VR)
            {
                vrPlayer.SetActive(true);
                Destroy(nonVRPlayer);
            }
            else
            {
                nonVRPlayer.SetActive(true);
                Destroy(vrPlayer);
            }
        }

        void Update()
        {
            if (!isPlaying)
            {
                return;
            }

            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                EndGame();
            }
            timer.UpdateTimeLeft(timeLeft);
        }


        public void StartGame()
        {
			hitCount = missCount = streakCounter = bestStreak = 0;
            timeLeft = gameTime;
            isPlaying = true;
            for (var i = 0; i < gameTargetSpawners.Length; i++)
            {
				gameTargetSpawners [i].StartSpawningProcess ();
            }
            var weapons = weaponScores.Keys;
            for (var i = 0; i < weapons.Count; i++)
            {
                var weapon = weapons[i];
                weaponScores.Put(weapon, 0);
            }
			HandleScoring (GetTotalScore (), hitCount, missCount, streakCounter, bestStreak);
            timer.Begin(timeLeft);
        }

        private string FormatScore(int score) {
			if (score < 1)
				return "-";
            return score.ToString("# ##0");
        }

        public void EndGame()
        {
            timeLeft = 0f;
            isPlaying = false;
            foreach (ITargetSpawner spawner in gameTargetSpawners)
            {
                spawner.EndSpawningProcess();
            }
            st.NotifyGameOver();
            timer.End();
        }

        public void StartButtonClicked()
        {
            if (isPlaying)
            {
                EndGame();
            }
            else
            {
                StartGame();
            }
        }

        public void NotifyHit(Hit hit, IWeapon weapon)
        {
			if (isPlaying) {
				if (hit.IsHit) {
					streakCounter++;
					hitCount++;
					if (streakCounter > bestStreak) {
						bestStreak = streakCounter;
					}
					var oldScore = weaponScores.Get (weapon);
					var hitScore = HandleStreak (hit);
					weaponScores.Put (weapon, oldScore + hitScore);
					ShowScore (hitScore, hit.HitLocation, hitScore > hit.Score, hitScore - hit.Score);
				} else if (hit.IsMiss) {
					missCount++;
					streakCounter = 0;
				}
				HandleScoring (GetTotalScore (), hitCount, missCount, streakCounter, bestStreak);
			}
        }

        private int GetTotalScore()
        {
            int sum = 0;
            var weapons = weaponScores.Keys;
            for (var i = 0; i < weapons.Count; i++)
            {
                var weapon = weapons[i];
                sum += weaponScores.Get(weapon);
            }
            return sum;
        }

        private int HandleStreak(Hit hit)
        {
            if (streakCounter == 0)
            {
                return hit.Score;
            }

            for (var i = multiplierStreak.Length - 1; i >= 0; i--)
            {
                var ms = multiplierStreak[i];
                if (streakCounter >= ms.requiredStreak)
                {
                    return (int)(hit.Score * ms.multiplier);
                }
            }
            return hit.Score;
        }

        private void ShowScore(int score, Vector3 position, bool receivedBonus, int bonus)
        {
            var scorePool = GameObject.FindGameObjectWithTag(Tags.SCORE_CANVAS_POOL).GetComponent<Pool>();
            var scoreViewer = scorePool.Get<TargetScoreCanvas>();
            float distanceForScaleEqualToOneSqr = 330;
            float scale = 1f + 2f * position.sqrMagnitude / distanceForScaleEqualToOneSqr;
            Vector3 vScale = Vector3.one * scale;
            if (receivedBonus)
            {
				scoreViewer.Show(score, false, position + Vector3.up * 0.5f, vScale, mjPlayer.transform.position);
                var bonusScore = scorePool.Get<TargetScoreCanvas>();
				bonusScore.Show(bonus, true, position, vScale, mjPlayer.transform.position);
            }
            else
            {
				scoreViewer.Show(score, false, position, vScale, mjPlayer.transform.position);
            }
            Debug.Log("score: " + score);
        }

        public void AddWeapon(IWeapon weapon)
        {
            weaponScores.Put(weapon, 0);
            weapon.AddHitListener(this);
			weapon.AddFireListener (this);
        }

        public void setStartStopInteractor(StartStop start) {
            this.st = start;
        }

		private void HandleScoring(int totalScore, int nHits, int nMiss, int streak, int bestStreak) {
			scoreText.text = FormatScore (totalScore);
			nHitsText.text = FormatScore (nHits);
			nMissText.text = FormatScore (nMiss);
			streakText.text = FormatScore (streak);
			bestStreakText.text = FormatScore (bestStreak);
		}

		#region IFireListener implementation

		public void NotifyFire (IWeapon weapon) {
			mjPlayer.GetOtherWeapon (weapon).ResetCooldown ();
		}

		#endregion
    }
}