using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameSaver : MonoBehaviour
{

    private GameData m_gameData;
    public bool[] m_activeLevels;
	public bool m_isTheGameStartedForTheFirstTime;
	public List<Button> m_Levels = new List<Button>();
 	
	void Awake()
	{
		// 加载游戏
		InitializeGame();
	}
	void Start()
	{
		AddListener();
		Debug.Log(m_activeLevels[1]+ " :m_activeLevels[1]");
		for (int i = 0; i < m_activeLevels.Length; i++)
		{
			m_Levels[i].gameObject.SetActive(m_activeLevels[i]);
		}
		
		// Debug.Log(m_isTheGameStartedForTheFirstTime+ " :m_isTheGameStartedForTheFirstTime");
		
	}
	
	void AddListener()
	{
		foreach (Button item in m_Levels)
		{
			item.onClick.RemoveAllListeners();
			item.onClick.AddListener(() => ActiveNextlevel());
		}
	}
	
	void ActiveNextlevel()
	{
		m_activeLevels[1] = true;
		m_Levels[1].gameObject.SetActive(m_activeLevels[1]);
		Save(0);
		
	}
		
	

	void InitializeGame()
	{
		LoadGameData();
		
		if(m_gameData != null)
		{
			m_isTheGameStartedForTheFirstTime = m_gameData.GetIsGameStartForTheFirstTime();

		}
		else
		{
			m_isTheGameStartedForTheFirstTime = true;
		}
		
		// 第一次启动游戏，初始化.
		if(m_isTheGameStartedForTheFirstTime)
		{
			m_isTheGameStartedForTheFirstTime = false;

			m_activeLevels = new bool[3];
			//Level1 is Active.
			m_activeLevels[0] = true;

			//Other levels are disactive.
			for(int i = 1; i < m_activeLevels.Length; i++)
			{
				m_activeLevels[i] = false;
				
			}


			m_gameData = new GameData();

			m_gameData.SetLevels(m_activeLevels);
			

			m_gameData.SetIsGameStartForTheFirstTime(m_isTheGameStartedForTheFirstTime);
			

			SaveGameData();
			LoadGameData();
		}
	}


    public void SaveGameData()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            file = File.Create(Application.persistentDataPath + "/GameData03.dat");
            if (m_gameData != null)
            {
                m_gameData.SetLevels(m_activeLevels);

                bf.Serialize(file, m_gameData);
            }
        }
        catch (Exception e)
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    public void LoadGameData()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            file = File.Open(Application.persistentDataPath + "/GameData03.dat", FileMode.Open);
            m_gameData = (GameData)bf.Deserialize(file);
            if (m_gameData != null)
            {
                m_activeLevels = m_gameData.GetLevels();
            }
        }
        catch (Exception e)
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    public void Save(int level)
    {
        int unlockNextLevel = -1;
        unlockNextLevel = level + 1;
        if (unlockNextLevel < m_activeLevels.Length)
        {
            // 解锁下一关
            m_activeLevels[unlockNextLevel] = true;
        }
        SaveGameData();
    }
}
	


