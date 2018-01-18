using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
    public class GameController : MonoBehaviour, IHitlistener
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
        public Text scoreText;

        [SerializeField] private TimerScript timer;

        private ITargetSpawner[] gameTargetSpawners;

        private Map<IWeapon, int> weaponScores;

        [SerializeField] private MultiplierStreak[] multiplierStreak;
        private int streakCounter;
        private StartStop st;

        void Awake()
        {
            weaponScores = new Map<IWeapon, int>();
        }

        void Start()
        {
            var spawners = GameObject.FindGameObjectsWithTag(Tags.GAME_TARGET_SPAWNER);
            gameTargetSpawners = new ITargetSpawner[spawners.Length];
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
            timeLeft = gameTime;
            isPlaying = true;
            for (var i = 0; i < gameTargetSpawners.Length; i++)
            {
                gameTargetSpawners[i].StartSpawningProcess();
            }
            var weapons = weaponScores.Keys;
            for (var i = 0; i < weapons.Count; i++)
            {
                var weapon = weapons[i];
                weaponScores.Put(weapon, 0);
            }
            scoreText.text = FormatScore(GetTotalScore());
            timer.Begin(timeLeft);
        }

        private string FormatScore(int score) {
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
            if (hit.IsHit)
            {
                streakCounter++;
                var oldScore = weaponScores.Get(weapon);
                var hitScore = HandleStreak(hit);
                weaponScores.Put(weapon, oldScore + hitScore);
                scoreText.text = FormatScore(GetTotalScore());
                ShowScore(hitScore, hit.HitLocation, hitScore > hit.Score, hitScore - hit.Score);
            }
            else
            {
                streakCounter = 0;
            }
            Debug.Log("streakCounter: " + streakCounter);
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
            var scoreViewer = scorePool.Get<ScoreViewer>();
            if (receivedBonus)
            {
                scoreViewer.Show(score, false, position + Vector3.up * 0.5f);
                var bonusScore = scorePool.Get<ScoreViewer>();
                bonusScore.Show(bonus, true, position);
            }
            else
            {
                scoreViewer.Show(score, false, position);
            }
            Debug.Log("score: " + score);
        }

        public void AddWeapon(IWeapon weapon)
        {
            weaponScores.Put(weapon, 0);
            weapon.AddListener(this);
        }

        public void setStartStopInteractor(StartStop start) {
            this.st = start;
        }
    }
}