using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string restartScene;

    [SerializeField] private GameObject deadPanel;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        deadPanel.SetActive(false);
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
        SceneManager.LoadScene(restartScene);
    }
}
