
public class LeftChatObject {
    /// <summary> 左侧对话，有等待时间 </summary>
    public void Say(ChatManager cm, string message)
    {
        Chat.CreatChat(cm, message);
    }
    /// <summary> 系统通知 </summary>
    public void TextSystemEvent(PanelManager pm, string message)
    {
        pm.PopEvent(message);
        
    }

}
