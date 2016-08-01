using UnityEngine;
using System;
using System.Collections.Generic;

public class Bill : BaseText
{
    //==============================================================================================
    // Methods
    public void Start()
    {
        m_cm.m_chatObjectName = "Bill";
        left.Say(m_cm, "Hi, 哥们");
        Chat();     
    }

    void Chat()
    {
         right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"什么事啊？", message => {
            right.Say(m_pm, message);
            Chat3();}},

            {"在", message => {
            right.Say(m_pm, message);
            Chat3();
            }
            }
            
         });
    }
    
     void Chat3()
    {
        left.Say(m_cm, "公司最近新来了位妹子，人不错，给你介绍一下吧？");
        Chat4();
    }

    void Chat4()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"这不好吧，你跟她都不熟", message => {
            right.Say(m_pm, message);
            Chat5();}
            },

            {"好啊", message => {
            right.Say(m_pm, message);
            Chat5();
            }
            }
         });
    }
    
    void Chat5()
    {
        left.Say(m_cm, "这是她的微信: Emily，你们自己聊吧，只能帮到这了。");
        
        Chat6();
    }
    
    void Chat6()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"好吧", message => {
            right.Say(m_pm, message);
            Chat7();
            }
            },

            {"^_^", message => {
            right.Say(m_pm, message);
            Chat7();
            }
            }
         });
    }
    
    void Chat7()
    {
        left.Say(m_cm, "有空再聊吧，我去溜娃了");
        
        Chat8();
    }
    
    void Chat8()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"有空聊", message => {
            right.Say(m_pm, message);
            ChatEnd();
            }
            },

            {"OK", message => {
            right.Say(m_pm, message);
            ChatEnd();
            }
            }
         });
    }
    
    void ChatEnd()
    {
        GameManager.Instance.m_isBillOver = true;
        GameManager.Instance.SlideInContactMenu();
    }  
}
