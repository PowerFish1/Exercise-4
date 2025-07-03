using TMPro;
using UnityEngine;

public class UIMenuController : MonoBehaviour
{
    public TMP_InputField PlayerNameInputField;
    public TextMeshProUGUI BestScoreText;

    private void Start()
    {
        // Display best score on menu
        var bestData = DataPersistManager.Instance.LoadBestScore();
        if (bestData != null && !string.IsNullOrEmpty(bestData.PlayerName))
        {
            BestScoreText.text = $"Best Score : {bestData.PlayerName} : {bestData.Score}";
        }
        else
        {
            BestScoreText.text = "Best Score : ";
        }
        // Optionally, set input field to last used name
        if (PlayerPrefs.HasKey("CurrentPlayerName"))
        {
            PlayerNameInputField.text = PlayerPrefs.GetString("CurrentPlayerName");
        }
    }

    public void OnStartButtonClicked()
    {
        string playerName = PlayerNameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
            PlayerNameInputField.text = playerName;
        }
        PlayerPrefs.SetString("CurrentPlayerName", playerName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnPlayerNameInputEndEdit(string value)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnStartButtonClicked();
        }
    }

    public void OnExitButtonClicked()
    {
        // Exit the application
        Application.Quit();
    }
}