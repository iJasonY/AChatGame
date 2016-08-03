public class Chat
{
    //==============================================================================================
    // Fields
    /// <summary> 每句话延时 </summary>
    public float m_delayOfPerChat; 
    public string m_message;
    /// <summary> 每个字符延时0.1s </summary>
    private const float mc_delayOfPerCharInChat = 0.1f;

    //==============================================================================================
    // Methods
    public Chat(string message)
    {
        m_message = message;
        m_delayOfPerChat = m_message.Length * mc_delayOfPerCharInChat;
    }

    public void TextChat(View view)
    {
        view.PopChat(view.m_leftBubblePrefab, view.m_leftBubbleposX, m_message, view.m_soundManager.m_leftAudio);
    }
	
	public static void CreatLeftChat(ChatManager cm, string message)
	{
		Chat chat = new Chat(message);
        cm.m_leftChats.Enqueue(chat);
	}
}
