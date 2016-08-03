using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameSaver : MonoBehaviour
{
    private GameData m_gameData;
    public GameData gameData
    {
        get { return m_gameData; }
    }
    private static GameSaver s_instance = null;
    public static GameSaver Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType(typeof(GameSaver)) as GameSaver;
                if (s_instance == null)
                {
                    Debug.Log("Could not locate GameSaver");
                }
            }
            return s_instance;
        }
    }

    void Awake()
    {
        // 加载游戏
        IsTheGameStartForTheFirstTime();
    }

    void IsTheGameStartForTheFirstTime ()
    {
        // 调试不保存: if内条件不加“!”，保存(发布): if内条件加“!”
        // PlayerPrefs are stored on Mac: ~/Library/Preferences folder, in a file named unity.[company name].[product name].plist
        if(!PlayerPrefs.HasKey("IsTheGameStartForTheFirstTime"))
		{
			PlayerPrefs.SetInt("IsTheGameStartForTheFirstTime", 0);
            m_gameData = new GameData();
            m_gameData.IsIntroductionLevelOver = false;
            m_gameData.IsEmilyLevelOver = false;
            SaveGameData();
            LoadGameData();
		}
        else
        {
            LoadGameData();
        }
    }

    public void SaveGameData()
    {
        FileStream file = null;
        BinaryFormatter bf = new BinaryFormatter();
        // file stored on Mac: /Users/[UserName]/Library/Application Support/[company name]/[product name].
        file = File.Create(Application.persistentDataPath + "/GameData136.txt");
        // Debug.Log(Application.persistentDataPath);
        bf.Serialize(file, m_gameData);
        file.Close();
    }

    public void LoadGameData()
    {
        FileStream file = null;
        BinaryFormatter bf = new BinaryFormatter();
        file = File.Open(Application.persistentDataPath + "/GameData136.txt", FileMode.Open);
        m_gameData = (GameData)bf.Deserialize(file);
        file.Close();
    }
}



