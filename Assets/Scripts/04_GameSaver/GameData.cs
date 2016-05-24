using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class GameData{
	private bool[] m_activeLevels;
	private bool isTheGameStartedForTheFirstTime;
	
	public void SetLevels(bool[] levels)
	{
		m_activeLevels = levels;
	}
	
	public bool[] GetLevels()
	{
		return m_activeLevels;
	
	}
	
	//Set and Get IsGameStartForTheFirstTime
	public void SetIsGameStartForTheFirstTime(bool isTheGameStartedForTheFirstTime)
	{
		this.isTheGameStartedForTheFirstTime = isTheGameStartedForTheFirstTime;
	}

	public bool GetIsGameStartForTheFirstTime()
	{
		return this.isTheGameStartedForTheFirstTime;
	}
}
