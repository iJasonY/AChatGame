public class Chat
{
    //==============================================================================================
    // Fields
    public float m_delayTime;   // 每句话延时
    public string m_message;
    private const float mc_timeOfEveryChar = 0.1f;  // 每个汉字延时0.1s

    //==============================================================================================
    // Methods
    public Chat(string message)
    {
        m_message = message;
        m_delayTime = m_message.Length * mc_timeOfEveryChar;
    }

    public void TextChat(PanelManager pm)
    {
        pm.PopChat(pm.m_leftBubblePrefab, pm.m_leftBubbleposX, m_message);
    }

    public void TextEvent(PanelManager pm)
    {
        // pm.PopEvent(m_message);
    }
	
	public static void CreatChat(ChatManager cm, string message)
	{
		Chat chat = new Chat(message);
        cm.m_leftChats.Enqueue(chat);
	}
}
