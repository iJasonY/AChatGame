
using System;
using System.Collections.Generic;

public class Content01 : BaseContent
{
    private EventObject m_eventObject = new EventObject();
    //==============================================================================================
    // Methods
    void Start()
    {
        left.Say(m_cm, "在吗？");
        content2();
        // emoji = {{smile,"^_^"},};
       
    }

    void content2()
    {
         right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"什么事啊？", message => {
            right.Say(m_pm, message);
            content3();}},

            {"在", message => {
            right.Say(m_pm, message);
            content3();
            }
            }
            
         });
    }
    
     void content3()
    {
        m_eventObject.TextEvent(m_pm, "对方更新了一条朋友圈");
        left.Say(m_cm, "帮我改下这个设计？");
        
        content4();
    }
    
    void content4()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"怎么改？", message => {
            right.Say(m_pm, message);
            content5();}
            },

            {"晚上又要加班了", message => {
            right.Say(m_pm, message);
            content5();
            // m_pm.SetFeelingButton(1);
            }
            }
         });
    }
    
    void content5()
    {
        left.Say(m_cm, "很好改的，字体加大，LOGO加大，^_^");
        
        content6();
    }
    
    void content6()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"不要不要", message => {
            right.Say(m_pm, message);
            content7();
            }
            },

            {"我饿了", message => {
            right.Say(m_pm, message);
            content7();
            }
            }
         });
    }
    
    void content7()
    {
        left.Say(m_cm, "哈哈哈哈哈哈哈哈");
        
        content8();
    }
    
    void content8()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"不是吧哈哈", message => {
            right.Say(m_pm, message);
            content9();
            }
            },

            {"加班", message => {
            right.Say(m_pm, message);
            content9();
            }
            }
         });
    }
    
    void content9()
    {
        left.Say(m_cm, "哈哈哈哈哈哈哈哈");
        
        content10();
    }
    
    void content10()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"不是吧哈哈", message => {
            right.Say(m_pm, message);
            content11();
            }
            },

            {"加班", message => {
            right.Say(m_pm, message);
            content11();
            }
            }
         });
    }
    void content11()
    {
        left.Say(m_cm, "哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈");
        
        content12();
    }
    
    void content12()
    {
        right.Choose(m_pm, new Dictionary<string, Action<string>> {
            {"不是吧哈哈", message => {
            right.Say(m_pm, message);
            content11();
            }
            },

            {"加班8", message => {
            right.Say(m_pm, message);
            content11();
            }
            }
         });
    }    
   
}
