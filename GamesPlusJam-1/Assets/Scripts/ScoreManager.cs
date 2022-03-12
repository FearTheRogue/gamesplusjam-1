using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highscoreText;
    
    private int score = 0;
    private int highscore = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = "Score: " + score.ToString();
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = "Score: " + score.ToString();

        if (highscore < score)
            PlayerPrefs.SetInt("highscore", score);
    }

    public void RemovePoint()
    {
        score -= 1;
        scoreText.text = "Score: " + score.ToString();
    }
}
