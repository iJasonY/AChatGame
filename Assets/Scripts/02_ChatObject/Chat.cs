public class Chat
{
    public float time;
    public string message;
    public Chat(string m_message)
    {
        message = m_message;
        // 弹出对话等待时间，每个汉字0.1s.
        time = m_message.Length * 0.1f;
    }

    public void AddChatBubble(PanelManager pm, ChatPerson cp)
    {
        pm.AddNewBubble(cp.m_isLeftBubble, message);
    }
	
	public static void CreatChat(ChatManager cm, string message)
	{
		Chat chat = new Chat(message);
        cm.m_leftChats.Add(chat);
	}
}
