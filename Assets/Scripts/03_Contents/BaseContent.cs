using UnityEngine;
public class BaseContent : MonoBehaviour {
    //==============================================================================================
    // Fields
	public PanelManager m_pm;
    public ChatManager m_cm;
    
    /// <summary> 右侧对话 </summary>
    public RightChatObject right = new RightChatObject();
    /// <summary> 左侧对话 </summary>
    public LeftChatObject left = new LeftChatObject();

}
