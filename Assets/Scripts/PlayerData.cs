using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerData : MonoBehaviour
{
    public static PlayerData Ins { get; private set; }

    public string playerName;
    public int topScore;
    public int lastScore;


    void Awake()
    {
        if (Ins != null)
        {
            Destroy(gameObject);
            return;
        }

        Ins = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }

    [System.Serializable]
    public class SaveData
    {
        public string name;
        public int score;
    }

    public void Save()
    {
        var data = new SaveData()
        {
            name = playerName,
            score = topScore
        };

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.name;
            topScore = data.score;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Game Debug/Clear Player Data")]
    static void DeleteData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
            File.Delete(path);
    }
#endif
}