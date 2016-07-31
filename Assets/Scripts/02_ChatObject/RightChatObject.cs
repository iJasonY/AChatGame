using System;
using System.Collections.Generic;

public class RightChatObject {
	/// <summary> 右侧对话，没有等待时间 </summary>
    public void Say(PanelManager pm, string message)
    {
        // 弹出新对话
        pm.PopChat(pm.m_rightBubblePrefab, -pm.m_leftBubbleposX, message);
        // 清空待发送消息显示框
        pm.m_waitForSendText.text = " ";
        // 禁用发送按钮
        pm.SetSendButtonState(SendButtonImageType.DISABLE, false);
        // 隐藏选择面板
        pm.HideChoicePanel();
    }
    
    public void Choose(PanelManager pm, Dictionary<string, Action<string>> m_choices)
    {
        pm.SetChoice(m_choices);
    }
}
