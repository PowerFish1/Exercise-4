using UnityEngine;

public class DataPersistManager : MonoBehaviour
{
    public static DataPersistManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string PlayerName;
        public int Score;
    }

    private string SavePath => Application.persistentDataPath + "/bestscore.json";

    public void SaveBestScore(string playerName, int score)
    {
        PlayerData currentBest = LoadBestScore();
        if (currentBest == null || score > currentBest.Score)
        {
            PlayerData data = new PlayerData
            {
                PlayerName = playerName,
                Score = score
            };
            string json = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(SavePath, json);
        }
    }

    public PlayerData LoadBestScore()
    {
        if (System.IO.File.Exists(SavePath))
        {
            string json = System.IO.File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        return null;
    }
}
