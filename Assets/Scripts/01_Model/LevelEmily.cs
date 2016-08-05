using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelEmily: LevelBase
{
    
    //==============================================================================================
    // Methods

    public override void StartChat()
    {
        m_cm.m_chatObjectName = "Emily";
        Chat2();     
    }

    void Chat2()
    {
         right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"^_^", message => {
            right.Say(m_view, message);
            Chat3();}},

            {"Hello, Emily", message => {
            right.Say(m_view, message);
            Chat3();
            }
            }
            
         });
    }
     void Chat3()
    {
        left.Say(m_cm, "Hi, 你好");
        Chat4();
    }
    
    void Chat4()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"你家是哪里的？", message => {
            right.Say(m_view, message);
            ChatFail5A();}
            },

            {"我是Bill的好基友", message => {
            right.Say(m_view, message);
            Chat5();
            }
            }
         });
    }
    
    //==============================================================================================
    // 问话模式
    void ChatFail5A()
    { 
        left.Say(m_cm, "重庆");
        ChatFail6A();
    }
    
    void ChatFail6A()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"重庆辣妹子", message => {
            right.Say(m_view, message);
            ChatFail7A();
            }
            },

            {"来深圳几年了？在深圳生活习惯吗？", message => {
            right.Say(m_view, message);
            ChatFail7B();
            }
            }
         });
    }

    void ChatFail7A()
    {
        left.Say(m_cm, "不过我不太能吃辣，容易上火。");
        ChatFail8();
    }

    void ChatFail7B()
    {
        left.Say(m_cm, "3年，还好吧，这儿阳光比重庆多");
        ChatFail8();
    }
    
    void ChatFail8()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"^_^", message => {
            right.Say(m_view, message);
            ChatFail9();
            }
            },

            {"你大学在哪读的？", message => {
            right.Say(m_view, message);
            ChatFail9();
            }
            }
         });
    }
    
    void ChatFail9()
    {
        ChatReCall();
        // left.Say(m_cm, "^_^不早了，有时间再聊，晚安了");
        ChatFail10();
    }
   
    void ChatFail10()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"你读的什么专业？", message => {
            right.Say(m_view, message);
            ChatEndVerify();
            }
            },

            {"今天好热啊，感觉台风要来了", message => {
            right.Say(m_view, message);
            ChatEndVerify();
            }
            }
         });
    }
  
    //==============================================================================================
    // 闲聊模式

    void Chat5()
    { 
        left.Say(m_cm, "^_^，知道，他跟我讲了");
        Chat6();
    }

    void Chat6()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"好热啊，感觉台风要来了", message => {
            right.Say(m_view, message);
            Chat7();
            }
            },

            {"台风要来了，你储备食物没有？", message => {
            right.Say(m_view, message);
            Chat7();
            }
            }
         });
    }

    void Chat7()
    {
        left.Say(m_cm, "下班去超市，东西都被抢光了T_T");
        Chat8();
    }

    void Chat8()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"^_^", message => {
            right.Say(m_view, message);
            Chat9();
            }
            },

            {"我抢到了泡面", message => {
            right.Say(m_view, message);
            Chat9();
            }
            }
         });
    }

    void Chat9()
    {
        left.Say(m_cm, "家里有面，只能吃面了");
        Chat10();
    }

    void Chat10()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"关灯吃面", message => {
            right.Say(m_view, message);
            Chat11();
            }
            },

            {"明天适合吃面", message => {
            right.Say(m_view, message);
            Chat11();
            }
            }
         });
    }

    void Chat11()
    {
        left.Say(m_cm, "^_^不早了，有时间再聊，晚安了");
        Chat12();
    }

    void Chat12()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"嗯，晚安", message => {
            right.Say(m_view, message);
            ChatEnd();
            }
            },

            {"有时间聊，晚安", message => {
            right.Say(m_view, message);
            ChatEnd();
            }
            }
         });
    }
    /// <summary> Emily撤回了一条消息 </summary>
    void ChatReCall()
    {
        left.TextReCall(m_view, "Emily撤回了一条消息");
    }
    /// <summary> Emily将玩家从微信删除，GameOver </summary>
    void ChatEndVerify()
    {
        left.TextEventVerify(m_view, "Emily开启了好友验证，你还不是他（她）的好友,请先发送好友验证请求，对方验证通过后，才能聊天。");
        // left.Say(m_cm, "^_^不早了，有时间再聊，晚安了");
        GameSaver.Instance.gameData.IsGameOver = true;
        GameManager.Instance.ChatEndSetting(7.5f);
    } 
    /// <summary> LevelEmily通关，进入LevelEmilyNext </summary>
    void ChatEnd()
    {
        GameSaver.Instance.gameData.IsEmilyLevelOver = true;
        GameManager.Instance.ChatEndSetting(3.5f);
    }  
   
}
