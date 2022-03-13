using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameWinScore : MonoBehaviour
{
    [SerializeField] private GameObject winObject;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text hightscoreText;

    private void Update()
    {
        if (winObject.activeInHierarchy)
        {
            scoreText.text = "Score: " + ScoreManager.instance.GetScore().ToString();
            hightscoreText.text = "Highscore: " + ScoreManager.instance.GetHighScore().ToString();
        }
    }
}
