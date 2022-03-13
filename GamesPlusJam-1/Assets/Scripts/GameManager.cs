using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private string restartScene;
    [SerializeField] private GameObject deadPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player");

        deadPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    private void Update()
    {
        if(player != null)
        {
            return;
        }

        deadPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SwitchItUp.instance.ResetOtherEnums();
        SceneManager.LoadScene(restartScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameWin()
    {
        winPanel.SetActive(true);
        AudioManager.instance.Play("Game Win");
        Time.timeScale = 0f;
    }
}
