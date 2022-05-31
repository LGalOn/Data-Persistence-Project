using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    Text textPrefab, header;

    [SerializeField]
    Transform container;

    [SerializeField]
    List<PlayerData.SaveData> leaderboard = new List<PlayerData.SaveData>();

    const int MaxEntries = 3;


    void Start()
    {
        header.text = $"Top {MaxEntries}";
        Load();
        CheckNewEntry();
        OpenBoard();
    }

    void CheckNewEntry()
    {
        var player = PlayerData.Ins;

        if (player.lastScore > 0)
        {
            int startCount = leaderboard.Count;

            for (int i = 0; i < startCount; i++)
            {
                if (player.lastScore > leaderboard[i].score)
                {
                    leaderboard.Insert(i, new PlayerData.SaveData() { name = player.playerName, score = player.lastScore });

                    while(leaderboard.Count > MaxEntries)
                        leaderboard.RemoveAt(leaderboard.Count - 1);

                    break;
                }          
            }

            if (leaderboard.Count == startCount
                && startCount < MaxEntries)
                leaderboard.Add(new PlayerData.SaveData() { name = player.playerName, score = player.lastScore });

            Save();

            player.lastScore = 0;
        }
    }

    void OpenBoard()
    {
        for (int i = 0; i < leaderboard.Count; i++)
        {
            var entry = Instantiate(textPrefab, container);
            entry.text = $"{leaderboard[i].name}   {leaderboard[i].score}";
        }
    }

    [System.Serializable]
    public class ListWrapper
    {
        public List<PlayerData.SaveData> list = new List<PlayerData.SaveData>();
    }

    public void Save()
    {
        var leaderboard = new ListWrapper();
        leaderboard.list.AddRange(this.leaderboard);

        string json = JsonUtility.ToJson(leaderboard);
        File.WriteAllText(Application.persistentDataPath + "/saveScores.json", json);
    }

    void Load()
    {
        string path = Application.persistentDataPath + "/saveScores.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            leaderboard.AddRange(JsonUtility.FromJson<ListWrapper>(json).list);
        }
    }

#if UNITY_EDITOR
    [MenuItem("Game Debug/Clear High Scores")]
    static void DeleteData()
    {
        string path = Application.persistentDataPath + "/saveScores.json";
        if (File.Exists(path))
            File.Delete(path);
    }
#endif
}
