using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeValue = 90;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private bool gameEnd;

    public Animator switchItAnim;

    private void Awake()
    {
        if (switchItAnim == null)
            return;

        switchItAnim = gameObject.GetComponent<Animator>();
    }


    private void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue += 10; // setting time back += 90;

            //switchItAnim.SetTrigger("SwitchUp");

            SwitchItUp.instance.PickRandomSwitchUp();

            if (gameEnd)
            {
                GameManager.instance.GameWin();
            }
        }

        DisplayTime(timeValue);


    }

    private void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
