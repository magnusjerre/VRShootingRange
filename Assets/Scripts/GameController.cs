using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IHitlistener {
    
    public float gameTime;
    private float timeLeft;
    
    private bool isPlaying = false;

    [SerializeField] private Text timeText;

    private ITargetSpawner[] gameTargetSpawners;
    private StartButton startButton;

    private Map<IWeapon, int> weaponScores;

    private PlaqueManager plaqueManager;

    void Awake() {
        weaponScores = new Map<IWeapon, int>();
        plaqueManager = FindObjectOfType<PlaqueManager>();
    }

    void Start() {
        var spawners = GameObject.FindGameObjectsWithTag(Tags.GAME_TARGET_SPAWNER);
        gameTargetSpawners = new ITargetSpawner[spawners.Length];
        for (var i = 0; i < gameTargetSpawners.Length; i++) {
            gameTargetSpawners[i] = spawners[i].GetComponent<ITargetSpawner>();
        }
        startButton = GameObject.FindObjectOfType<StartButton>();
        plaqueManager.SetScore(0);
        var allWeapons = GameObject.FindGameObjectsWithTag(Tags.WEAPON);
        for (var i = 0; i < allWeapons.Length; i++) {
            var weapon = allWeapons[i].GetComponent<IWeapon>();
            if (weapon != null) {
                weapon.AddListener(this);
                weaponScores.Put(weapon, 0);
            }
        }
    }

    void Update() {
        if (!isPlaying) {
            return;
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) {
            EndGame();
        }
        timeText.text = FormatTimeLeft(timeLeft);

    }

    private string FormatTimeLeft(float timeleft) {
        int timeLeftInt = (int) timeLeft;
        int minutes = timeLeftInt / 60;
        int seconds = timeLeftInt % 60;
        string minuteString = minutes > 9 ? minutes.ToString() : "0" + minutes.ToString();
        string secondsString = seconds > 9 ? seconds.ToString() : "0" + seconds.ToString();
        return string.Format("{0}:{1}", minuteString, secondsString);
    }


    public void StartGame() {
        timeLeft = gameTime;
        isPlaying = true;
        startButton.ButtonText.text = "Stop";
        for (var i = 0; i < gameTargetSpawners.Length; i++) {
            gameTargetSpawners[i].StartSpawningProcess();
        }
        var weapons = weaponScores.Keys;
        for (var i = 0; i < weapons.Count; i++) {
            var weapon = weapons[i];
            weaponScores.Put(weapon, 0);
        }
        plaqueManager.SetScore(GetTotalScore());
        timeText.text = FormatTimeLeft(timeLeft);
    }

    public void EndGame() {
        timeLeft = 0f;
        isPlaying = false;
        startButton.ButtonText.text = "Start";
        foreach (ITargetSpawner spawner in gameTargetSpawners) {
            spawner.EndSpawningProcess();
        }
    }

    public void StartButtonClicked() {
        if (isPlaying) {
            EndGame();
        } else {
            StartGame();
        }
    }

    public void NotifyHit(Hit hit, IWeapon weapon)
    {
        if (hit.IsHit) {
            var oldScore = weaponScores.Get(weapon);
            weaponScores.Put(weapon, oldScore + hit.Score);
            plaqueManager.SetScore(GetTotalScore());
        }
    }

    private int GetTotalScore() {
        int sum = 0;
        var weapons = weaponScores.Keys;
        for (var i = 0; i < weapons.Count; i++) {
            var weapon = weapons[i];
            sum += weaponScores.Get(weapon);
        }
        return sum;
    }
}