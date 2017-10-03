using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class PlaqueManager : MonoBehaviour
{
    private Canvas canvas;
    private Text scoreText;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        var textChildren = canvas.GetComponentsInChildren<Text>();
        foreach (var textChild in textChildren)
        {
            Debug.Log("textChildname: " + textChild.name);
            if (textChild.name.Equals("ScoreText"))
            {
                Debug.Log("scoretext has been set");
                scoreText = textChild;
            }
        }
    }

    void Start()
    {
        ResetTexts();
    }

    public void SetScore(int totalScore)
    {
        scoreText.text = totalScore.ToString();
    }


    private void ResetTexts()
    {
        scoreText.text = "0";
    }
    
}