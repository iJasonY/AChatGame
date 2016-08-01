using UnityEngine;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour
{
    //==============================================================================================
    // Fields
    public PanelManager m_pm;
    public string m_chatObjectName;
    public Queue<Chat> m_leftChats = new Queue<Chat>();
    private float m_timer = -3.0f;

    //==============================================================================================
    // Methods
    
    void Update()
    {
        Step();
        // Debug.Log(m_leftChats.Count + " :m_leftChats.Count");
    }

    private void Step()
    {
        if (m_leftChats.Count > 0)
        {
            m_pm.m_topText.text = "对方正在输入...";
            m_pm.ShowChoicePanel(false);

            m_timer += Time.deltaTime;
            // 若队列不为空，且计时器时间到，继续下一句.
            while (m_leftChats.Count > 0 && m_timer >= m_leftChats.Peek().m_delayTime)
            {
                // 先进先出，执行头部元素
                Chat item = m_leftChats.Peek();

                item.TextChat(m_pm);

                //计时器复位，删除头部元素
                m_timer = -0.5f;
                m_leftChats.Dequeue();
            }
            m_pm.m_isChoice = false;
        }
        else
        {
            if (!m_pm.m_isChoice)
            {
                m_pm.ShowChoicePanel(true);
                m_pm.m_isChoice = true;
            }
            m_pm.m_topText.text = m_chatObjectName;
        }
    }
}
