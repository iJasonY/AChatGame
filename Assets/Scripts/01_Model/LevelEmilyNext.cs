using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelEmilyNext : LevelBase
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
            {"在吗？", message => {
            right.Say(m_view, message);
            ChatUpdate();}},

            {"早", message => {
            right.Say(m_view, message);
            Chat3();
            }
            }

         });
    }
    void Chat3()
    {
        left.Say(m_cm, "早");
        Chat4();
    }

    void Chat4()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"台风来了", message => {
            right.Say(m_view, message);
            Chat5();}
            },

            {"外面好大的雨", message => {
            right.Say(m_view, message);
            Chat5();
            }
            }
         });
    }

    void Chat5()
    {
        left.Say(m_cm, "是啊，还好今天放假。");
        Chat6();
    }

    void Chat6()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"不然要被淋成落汤鸡。", message => {
            right.Say(m_view, message);
            Chat7A();
            }
            },

            {"今天放假在家干吗？", message => {
            right.Say(m_view, message);
            Chat7B();
            }
            }
         });
    }

    void Chat7A()
    {
        left.Say(m_cm, "哈哈");
        Chat8();
    }

    void Chat7B()
    {
        left.Say(m_cm, "睡觉，我觉得下雨天最适合睡觉了");
        Chat8();
    }

    void Chat8()
    {
        right.Choose(m_view, new Dictionary<string, Action<string>> {
            {"刚接到电话，现在要去公司加班", message => {
            right.Say(m_view, message);
            ChatEnd();
            }
            },

            {"有急事，现在要出门了", message => {
            right.Say(m_view, message);
            ChatEnd();
            }
            }
         });
    }

    /// <summary> Emily更新了一条朋友圈 </summary>
    void ChatUpdate()
    {
        left.TextEventUpdate(m_view, "你的好友Emily刚更新了一条朋友圈，快去看看吧！");
        // 弹出ContactPanel
        GameManager.Instance.SlideInContactPanel(7.5f);
    }
    /// <summary> LevelEmilyNext通关，GameOver </summary>
    void ChatEnd()
    {
        left.TextEventUpdate(m_view, "非常感谢您花费时间来试玩我的游戏，和Emily就聊到这吧，因为我编不下去了。");
        GameSaver.Instance.gameData.IsGameOver = true;
        GameManager.Instance.ChatEndSetting(7.5f);
    }
}
