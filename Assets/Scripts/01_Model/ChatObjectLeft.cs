
public class ChatObjectLeft {
    /// <summary> 左侧对话，有等待时间 </summary>
    public void Say(ChatManager cm, string message)
    {
        Chat.CreatLeftChat(cm, message);
    }
    /// <summary> 更新朋友圈 </summary>
    public void TextEventUpdate(View view, string eventMessage)
    {
        view.PopEventUpdate(eventMessage);   
    }

    /// <summary> 好友验证 </summary>
    public void TextEventVerify(View view, string eventMessage)
    {
        view.PopEventVerify(eventMessage);   
    }

    public void TextReCall(View view, string reCall)
    {
        view.PopReCall(reCall);
    }

}
