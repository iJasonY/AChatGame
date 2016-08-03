
public class ChatObjectLeft {
    /// <summary> 左侧对话，有等待时间 </summary>
    public void Say(ChatManager cm, string message)
    {
        Chat.CreatLeftChat(cm, message);
    }
    /// <summary> 系统通知 </summary>
    public void TextSystemEvent(View view, string message)
    {
        view.PopEvent(message);   
    }

}
