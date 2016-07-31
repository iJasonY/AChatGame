
public class EventObject {
	public void TextEvent(PanelManager pm, string message)
    {
        pm.PopChat(pm.m_leftBubblePrefab, pm.m_leftBubbleposX, message);
		// ToDo: 切换菜单
    }

}
