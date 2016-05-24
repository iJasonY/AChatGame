
using System;
using System.Collections.Generic;

public class ChatPerson
{
    public bool m_isLeftBubble;
    public ChatPerson(bool isLeft)
    {
        m_isLeftBubble = isLeft;
    }
    /// <summary> 左侧对话，有等待时间 </summary>
    public void s(ChatManager cm, string message)
    {
        Chat.CreatChat(cm, message);
    }
    
    /// <summary> 右侧对话,无等待时间 </summary>
    public void c(PanelManager pm, string message)
    {
        pm.AddNewBubble(m_isLeftBubble, message);
        pm.m_sendText.text = " ";
        pm.SetSendButton(1, false);
    }
    
    public void Choose(PanelManager pm, Dictionary<string, Action<string>> m_choices)
    {
        pm.Choice(m_choices);
    }
}
