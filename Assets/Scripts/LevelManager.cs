using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentLevel;
    public int currentScore = 0;
    public int requiredScore = 1;

    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void IncreaseScore()
    {
        currentScore++;
        if (currentScore >= requiredScore)
        {
            LevelComplete();
        }
    }
    private void LevelComplete()
    {
        // Retrieve the current highest unlocked level (default is 1)
        int highestUnlocked = PlayerPrefs.GetInt("HighestUnlockedLevel", 1);

        // Check if the current level is the highest unlocked level
        if (highestUnlocked < currentLevel + 1)
        {
            PlayerPrefs.SetInt("HighestUnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
            Debug.Log("Level " + (currentLevel + 1) + " unlocked!");
        }
    }

    public void goToLevelSelect ()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}

