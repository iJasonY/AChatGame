using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class ChatManager : MonoBehaviour
{
    public ChatPerson cp = new ChatPerson(true);
    public PanelManager pm;
    public List<Chat> m_leftChats = new List<Chat>();
    /// <summary> 计时器 </summary>
    public float timer = -3f;


    // Update is called once per frame
    void Update()
    {
        Step();
        // Debug.Log(m_leftChats.Count + " :m_leftChats.Count");
    }

    private void Step()
    {
        if (m_leftChats.Count > 0)
        {
            pm.m_topText.text = "对方正在输入...";
            pm.ShowChoicePanel(false);

            timer += Time.deltaTime;
            // 若List不为空，且计时器时间到，继续下一句.
            while (m_leftChats.Count > 0 && timer >= m_leftChats[0].time)
            {
                // 先进先出，执行List头部元素
                Chat item = m_leftChats[0];

                item.AddChatBubble(pm, cp);

                //计时器复位，删除List头部元素
                timer = -0.5f;
                m_leftChats.RemoveAt(0);
            }
            pm.m_isChoice = false;
        }
        else
        {
            if (!pm.m_isChoice)
            {
                pm.ShowChoicePanel(true);
                pm.m_isChoice = true;
            }
            pm.m_topText.text = "Boss";
        }
    }
}
