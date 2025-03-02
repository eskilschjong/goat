using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public GameObject winScreen;
    public int currentLevel;
    public int currentScore = 0;
    public int requiredScore = 1;
    private AudioSource audioSource;

    public AudioClip WinSound;

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
        StartCoroutine(ShowWinScreenWithDelay());
    }

    private IEnumerator ShowWinScreenWithDelay()
    {
        yield return new WaitForSeconds(2);
        winScreen.SetActive(true);
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(WinSound);
    }

    public void goToLevelSelect ()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void goToNextLevel()
    {
        SceneManager.LoadScene(currentLevel + 1);
    }
}

