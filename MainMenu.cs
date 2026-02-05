using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject specialCharacterImage; //Secret character

    void Start()
    {
        int finishStatus = PlayerPrefs.GetInt("GameFinished", 0);
        
        //Check if player has finished the game
        if (PlayerPrefs.GetInt("GameFinished", 0) == 1)
        {
            if (specialCharacterImage != null)
                specialCharacterImage.SetActive(true); //Show secret character after finished game in main menu
        }
        else
        {
            if (specialCharacterImage != null)
                specialCharacterImage.SetActive(false);
        }
    }

    //Start button
    public void StartGame()
    {
        SceneManager.LoadScene("Scenes1"); //Load chapther 1
    }

    //Exit button
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }

    //Reset Progress button
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("GameFinished");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
