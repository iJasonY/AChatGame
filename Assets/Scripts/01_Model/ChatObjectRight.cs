using System;
using UnityEngine;
using System.Collections.Generic;

public class ChatObjectRight
{
    /// <summary> 右侧对话，没有等待时间 </summary>
    public void Say(View view, string message)
    {
        // 弹出新对话
        view.PopBubble(view.m_rightBubblePrefab, -view.m_leftBubbleposX, message, view.m_soundManager.m_rightAudio);
        // 清空待发送消息显示框
        view.m_waitForSendText.text = " ";
        // 禁用发送按钮
        view.SetSendButtonState(SendButtonImageType.DISABLE, false);
        // 隐藏选择面板
        view.HideChoicePanel();
        view.m_isRightChatIsNull = true;
    }

    public void Choose(View view, Dictionary<string, Action<string>> m_choices)
    {
        if (m_choices.Keys.Count == 2)
        {
            view.m_isRightChatIsNull = false;
            view.SetChoice(m_choices);
        }
        else
        {
            Debug.LogError("选择项不为2");
        }

    }
}
