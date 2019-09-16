using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);

    }

    public void Select(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
