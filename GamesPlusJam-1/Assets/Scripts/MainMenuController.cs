using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string playGame;
    [SerializeField] private GameObject objectivePanel;

    private void Awake()
    {
        objectivePanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(playGame);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ObjectivePanel()
    {
        if (!objectivePanel.activeInHierarchy)
        {
            objectivePanel.SetActive(true);
        }
        else
        {
            objectivePanel.SetActive(false);
        }
    }
}
