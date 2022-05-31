using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField]
    Text scoreText;

    [SerializeField]
    InputField nameFiled;

    [SerializeField]
    string defaultName = "player";


    void Awake() => SceneManager.LoadScene(2, LoadSceneMode.Additive);

    void Start()
    {
        string pName = PlayerData.Ins.playerName;

        nameFiled.text = pName;

        scoreText.text = $"Best Score : ";

        if (pName != string.Empty)
            scoreText.text += $"{pName} : ";

        scoreText.text += PlayerData.Ins.topScore;
    }

    public void StartGame()
    {
        if (nameFiled.text != PlayerData.Ins.playerName)
            PlayerData.Ins.topScore = 0;

        if (nameFiled.text != string.Empty)
            PlayerData.Ins.playerName = nameFiled.text;
        else
            PlayerData.Ins.playerName = defaultName;

        PlayerData.Ins.Save();

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
