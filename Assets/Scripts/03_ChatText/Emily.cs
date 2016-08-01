
using System;
using System.Collections.Generic;

public class Emily: BaseText
{
    
    //==============================================================================================
    // Methods

    public void StartChatWithEmily()
    {
        m_cm.m_chatObjectName = "Emily";
        Chat2();     
    }

    void Chat2()
    {
         right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"在吗？", message => {
            right.Say(m_pm, message);
            Chat3();}},

            {"Hi, emlily!", message => {
            right.Say(m_pm, message);
            Chat3();
            }
            }
            
         });
    }
    
     void Chat3()
    {
        left.TextSystemEvent(m_pm, "Emily 开启了好友验证，你还不是他（她）的好友,请先发送好友验证请求，对方验证通过后，才能聊天。");
        // left.TextSystemEvent(m_pm, "Emily 更新了一条朋友圈，他（她）可能太忙了，没时间回复你的消息。");
        // left.Say(m_cm, "帮我改下这个设计？");
        GameManager.Instance.m_isGameOver = true;
        GameManager.Instance.SlideInContactMenu();
        // Chat4();
    }
    
    void Chat4()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"怎么改？", message => {
            right.Say(m_pm, message);
            Chat5();}
            },

            {"晚上又要加班了", message => {
            right.Say(m_pm, message);
            Chat5();
            }
            }
         });
    }
    
    void Chat5()
    {
        left.Say(m_cm, "很好改的，字体加大，LOGO加大，^_^");
        
        Chat6();
    }
    
    void Chat6()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"不要不要", message => {
            right.Say(m_pm, message);
            Chat7();
            }
            },

            {"我饿了", message => {
            right.Say(m_pm, message);
            Chat7();
            }
            }
         });
    }
    
    void Chat7()
    {
        left.Say(m_cm, "哈哈哈哈哈哈哈哈");
        
        Chat8();
    }
    
    void Chat8()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"不是吧哈哈", message => {
            right.Say(m_pm, message);
            Chat9();
            }
            },

            {"加班", message => {
            right.Say(m_pm, message);
            Chat9();
            }
            }
         });
    }
    
    void Chat9()
    {
        left.Say(m_cm, "哈哈哈哈哈哈哈哈");
        
        Chat10();
    }
    
    void Chat10()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"不是吧哈哈", message => {
            right.Say(m_pm, message);
            Chat11();
            }
            },

            {"加班", message => {
            right.Say(m_pm, message);
            Chat11();
            }
            }
         });
    }
    void Chat11()
    {
        left.Say(m_cm, "哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈");
        
        Chat12();
    }
    
    void Chat12()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"不是吧哈哈", message => {
            right.Say(m_pm, message);
            Chat11();
            }
            },

            {"加班8", message => {
            right.Say(m_pm, message);
            Chat11();
            }
            }
         });
    }  

    void ChatEnd()
    {
        left.TextSystemEvent(m_pm, "Emily 开启了好友验证，你还不是他（她）的好友,请先发送好友验证请求，对方验证通过后，才能聊天。");
        // left.TextSystemEvent(m_pm, "Emily 更新了一条朋友圈，他（她）可能太忙了，没时间回复你的消息。");
        // left.Say(m_cm, "帮我改下这个设计？");
        GameManager.Instance.m_isGameOver = true;
        GameManager.Instance.SlideInContactMenu();
        // Chat4();
    }  
   
}
