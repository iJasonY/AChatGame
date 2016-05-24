using UnityEngine;
using System.Collections.Generic;

public class BaseContent : MonoBehaviour {
	public PanelManager pm;
    public ChatManager cm;
    
    /// <summary> 右侧对话 </summary>
    public ChatPerson m = new ChatPerson(false);
    /// <summary> 左侧对话 </summary>
    public ChatPerson l = new ChatPerson(true);
    public Dictionary<string, string> emoji = new Dictionary<string, string>(){{"smile","^_^"},{"cry","T_T"}};

}
