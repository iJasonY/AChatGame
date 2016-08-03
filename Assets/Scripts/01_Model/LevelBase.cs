using UnityEngine;
public class LevelBase : MonoBehaviour {
    //==============================================================================================
    // Fields
	public View m_view;
    public ChatManager m_cm;

    /// <summary> 右侧对话 </summary>
    public ChatObjectRight right = new ChatObjectRight();
    /// <summary> 左侧对话 </summary>
    public ChatObjectLeft left = new ChatObjectLeft();
    //==============================================================================================
    // Methods
    void Awake ()
    {
        m_view = FindObjectOfType(typeof(View)) as View;
        m_cm = FindObjectOfType(typeof(ChatManager)) as ChatManager;
    }
    public virtual void StartChat(){}
}
