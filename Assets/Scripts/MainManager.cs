using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI ScoreText;
    public GameObject GameOverText;
    public TextMeshProUGUI BestScoreText;
    public string PlayerName;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private int m_BestScore = 0;
    private string m_BestPlayer = "";

    
    // Start is called before the first frame update
    void Start()
    {
        // Get player name from PlayerPrefs
        PlayerName = PlayerPrefs.GetString("CurrentPlayerName", "Player");
        // Always update and display the latest best score at scene start
        UpdateBestScoreUI();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        // Save best score if current is higher
        if (!string.IsNullOrEmpty(PlayerName))
        {
            DataPersistManager.Instance.SaveBestScore(PlayerName, m_Points);
            // Update best score UI if new best
            UpdateBestScoreUI();
        }
    }

    private void UpdateBestScoreUI()
    {
        var bestData = DataPersistManager.Instance.LoadBestScore();
        if (bestData != null && !string.IsNullOrEmpty(bestData.PlayerName))
        {
            m_BestScore = bestData.Score;
            m_BestPlayer = bestData.PlayerName;
            BestScoreText.text = $"Best Score : {m_BestPlayer} : {m_BestScore}";
        }
        else
        {
            BestScoreText.text = "";
        }
    }
}
