using UnityEngine;
using System.Collections.Generic;
public class ChatManager : MonoBehaviour
{
    //==============================================================================================
    // Fields
    private View m_view; 
    public string m_chatObjectName;
    public Queue<Chat> m_leftChats = new Queue<Chat>();
    private float m_timer = -3.0f;
    private string m_otherIsTyping = "对方正在输入...";

    //==============================================================================================
    // Methods
    void Awake ()
    {
        m_view = FindObjectOfType(typeof(View)) as View;
    }
    
    void Update()
    {
        TextLeftChat();
    }
    
    /// <summary>
    /// 
    ///
    ///</summary>
    private void TextLeftChat()
    {
        // 左侧有对话
        if (m_leftChats.Count > 0)
        {
            // m_view.m_isLeftChatIsNull = false;
            // 隐藏选择面板
            m_view.HideChoicePanel();
            // TopBar显示: 对方正在输入
            m_view.m_topBarText.text = m_otherIsTyping;
            
            m_timer += Time.deltaTime;
            // 若左侧对话不为空，且计时器时间到，继续下一句.
            while (m_leftChats.Count > 0 && m_timer >= m_leftChats.Peek().m_delayOfPerChat)
            {
                // 左侧发送第一句话
                Chat leftChat = m_leftChats.Peek();
                leftChat.TextChat(m_view);
                // 删除第一句话
                m_leftChats.Dequeue();
                m_timer = 0.0f;
            }
        }
        // 左侧没有对话
        else
        {
            // 右侧没有对话
            if (m_view.m_isRightChatIsNull)
            {
                m_view.HideChoicePanel();
            }
            // 右侧有对话
            else
            {
                m_view.ShowChoicePanel(true);
            }
            // TopBar显示对方名字
            m_view.m_topBarText.text = m_chatObjectName;
        }
    }
}
