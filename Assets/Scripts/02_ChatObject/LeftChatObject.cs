
public class LeftChatObject {
    /// <summary> 左侧对话，有等待时间 </summary>
    public void Say(ChatManager cm, string message)
    {
        Chat.CreatChat(cm, message);
    }

}
