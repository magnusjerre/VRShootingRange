using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour, IButton {
    private Text buttonText;
    public Text ButtonText {
        get {
            return buttonText;
        }
    }
    private Collider buttonCollider;

    private GameController gameController;

    void Start() {
        buttonText = GetComponentInChildren<Text>();
        buttonCollider = GetComponent<Collider>();
        gameController = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameController>();
    }

    public void Click() {
        gameController.StartButtonClicked();
    }

}