using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] levelButtons;
    void Start()
    {
        int highestUnlocked = PlayerPrefs.GetInt("HighestUnlockedLevel", 1);
        Debug.Log("Highest unlocked level: " + highestUnlocked);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > highestUnlocked)
            {
                levelButtons[i].interactable = false;
            }
        }
    }
    // Method to load a scene dynamically
    public void LoadLevel(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is not set.");
        }
    }
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
