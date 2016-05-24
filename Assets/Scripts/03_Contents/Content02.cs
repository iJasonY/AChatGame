using UnityEngine;
using System;
using System.Collections.Generic;

public class Content02 :BaseContent
{
    void Start()
    {
        l.s(cm, "在吗？");
        
        content2();
        // emoji = {{smile,"^_^"},};
       
    }

    void content2()
    {
         m.Choose(pm, new Dictionary<string, Action<string>> {
            {"什么事啊？", (string message) => {
            m.c(pm, message);
            content3();}},

            {"在", (string message) => {
            m.c(pm, message);
            content3();
            }
            }
            
         });
    }
    
     void content3()
    {
        l.s(cm, "帮我改下这个设计？");
        
        content4();
    }
    
    void content4()
    {
        m.Choose(pm, new Dictionary<string, Action<string>> {
            {"怎么改？", (string message) => {
            m.c(pm, message);
            content5();}
            },

            {"晚上又要加班了" + emoji["cry"], (string message) => {
            m.c(pm, message);
            content5();
            // pm.SetFeelingButton(1);
            }
            }
         });
    }
    
    void content5()
    {
        l.s(cm, "很好改的，字体加大，LOGO加大，^_^");
        
        content6();
    }
    
    void content6()
    {
        m.Choose(pm, new Dictionary<string, Action<string>> {
            {"不要不要", (string message) => {
            m.c(pm, message);
            content7();
            }
            },

            {"我饿了", (string message) => {
            m.c(pm, message);
            content7();
            }
            }
         });
    }
    
    void content7()
    {
        l.s(cm, "哈哈哈哈哈哈哈哈");
        
        content8();
    }
    
    void content8()
    {
        m.Choose(pm, new Dictionary<string, Action<string>> {
            {"不是吧哈哈", (string message) => {
            m.c(pm, message);
            content9();
            }
            },

            {"加班", (string message) => {
            m.c(pm, message);
            content9();
            }
            }
         });
    }
    
    void content9()
    {
        l.s(cm, "哈哈哈哈哈哈哈哈");
        
        content10();
    }
    
    void content10()
    {
        m.Choose(pm, new Dictionary<string, Action<string>> {
            {"不是吧哈哈", (string message) => {
            m.c(pm, message);
            content11();
            }
            },

            {"加班", (string message) => {
            m.c(pm, message);
            content11();
            }
            }
         });
    }
    void content11()
    {
        l.s(cm, "哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈");
        
        content12();
    }
    
    void content12()
    {
        m.Choose(pm, new Dictionary<string, Action<string>> {
            {"不是吧哈哈", (string message) => {
            m.c(pm, message);
            content11();
            }
            },

            {"加班8", (string message) => {
            m.c(pm, message);
            content11();
            }
            }
         });
    }
    
    
    
    
   
}
