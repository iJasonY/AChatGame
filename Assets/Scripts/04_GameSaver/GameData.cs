using UnityEngine.UI;
using System.Collections.Generic;
using System;

[System.Serializable]
public class GameData{
	private bool m_isLevelOneOver;
	private bool m_isIntroductionLevelOver;

	public bool IsLevelOneOver{
		get { return m_isLevelOneOver; } 
		set { m_isLevelOneOver = value; } 
	}

	public bool IsIntroductionLevelOver{
		get { return m_isIntroductionLevelOver; }
		set { m_isIntroductionLevelOver = value; }
	}
}
