using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level01");
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
