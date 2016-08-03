using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelIntroduction : LevelBase
{
    //==============================================================================================
    // Methods
    public override void StartChat()
    {
        m_cm.m_chatObjectName = "Bill";
        left.Say(m_cm, "Hi, 哥们");
        Chat();     
    }
    // ToDo: 测试
    void Chat()
    {
         right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"什么事啊？", message => {
            right.Say(m_view, message);
            ChatEnd();}},

            {"在", message => {
            right.Say(m_view, message);
            ChatEnd();
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
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"这不好吧，你跟她都不熟", message => {
            right.Say(m_view, message);
            Chat5();}
            },

            {"好啊", message => {
            right.Say(m_view, message);
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
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"好吧", message => {
            right.Say(m_view, message);
            Chat7();
            }
            },

            {"^_^", message => {
            right.Say(m_view, message);
            Chat7();
            }
            }
         });
    }
    
    void Chat7()
    {
        left.Say(m_cm, "有时间再聊吧，我去溜娃了");
        
        Chat8();
    }
    
    void Chat8()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"有空聊", message => {
            right.Say(m_view, message);
            ChatEnd();
            }
            },

            {"OK", message => {
            right.Say(m_view, message);
            ChatEnd();
            }
            }
         });
    }
    
    void ChatEnd()
    {
        GameSaver.Instance.gameData.IsIntroductionLevelOver = true;
        GameManager.Instance.ChatEndSetting(1.0f);
        Debug.Log("IsIntroductionLevelOver: " + GameSaver.Instance.gameData.IsIntroductionLevelOver);
    }  
}
