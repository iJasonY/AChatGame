using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

// 发送按钮背景图片类型
public enum SendButtonImageType
{
    ENABLE = 0,
    DISABLE
}

public class PanelManager : MonoBehaviour
{
    //==============================================================================================
    // Fields
    public ChatManager m_chatManager;
    public SoundManager m_soundManager;

    public GameObject m_topPanel;
    // 待发送消息
    public Text m_waitForSendText;
    public Button m_sendButton;
    public Text m_topText;
    public Button m_leftBubblePrefab;
    public Button m_rightBubblePrefab;

    public Button m_eventBubblePrefab;

    public RectTransform m_chatPanel;
    public ScrollRect m_panelScroll;
    public GameObject m_handle;
    private Text m_bubbleText;
    /// <summary> 对话气泡 </summary>
    private Button m_bubble;

    /// <summary> 已弹出的对话气泡 </summary>
    public List<Button> m_popedChatBubbles = new List<Button>();
    public List<Button> m_choiceButtons = new List<Button>();

    /// <summary> 单行对话气泡高度（Button） </summary>
    private float m_bubbleCellHeight = 78.91891f + 20f;
    /// <summary> 已弹出对话的总高度 </summary>
    private float m_popedBubblesHeights = 0.0f;
    /// <summary> 新弹出对话气泡的posY </summary>
    private float m_newBubblePosY;

    /// <summary> 对话框底边的posY </summary>
    private float m_chatPanelBottomposY = -515f;
    /// <summary> 对话Bubble的posX坐标 </summary>
    public float m_leftBubbleposX = -360f;

    /// <summary> 由下落转换到向上滚屏状态 </summary>
    private bool m_isFromFallToScrollUp = false;
    /// <summary> 处在向上滚屏状态 </summary>
    private bool m_isScrollingUp = false;
    public bool m_isChoice = true;
    // 用户点击的选择按钮名字
    private string m_seletedButtonName;
    private Dictionary<string, Action<string>> m_choose;
    private Image m_sendButtonImage;
    /// <summary> 发送按钮背景图片 </summary>
    public List<Sprite> m_sendButtonSprites = new List<Sprite>();



    //==============================================================================================
    // public methods

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        // 初始化对话框
        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            m_handle.SetActive(false);
        #endif

        m_topText = m_topPanel.GetComponentInChildren<Text>();
        m_sendButtonImage = m_sendButton.GetComponent<Image>();

        m_topText.text = m_chatManager.m_chatObjectName;
        SetSendButtonState(SendButtonImageType.DISABLE, false);
        ClearPopedBubble();
    }
    // 删除聊天数据
    public void ClearPopedBubble()
    {
        foreach (var popedBubble in m_popedChatBubbles)
        {
            Destroy(popedBubble.gameObject);
        }
        m_popedChatBubbles.Clear();
        m_newBubblePosY = mc_firstBubblePosY;
        m_popedBubblesHeights = 0.0f;
    }
    

    /// <summary> 设置发送按钮状态 </summary>
    public void SetSendButtonState(SendButtonImageType spriteType, bool isInteractable)
    {
        int indexOfSprite = (int)spriteType;
        m_sendButtonImage.sprite = m_sendButtonSprites[indexOfSprite];
        m_sendButton.interactable = isInteractable;
    }

    /// <summary> 设置选择面板 </summary>
    public void SetChoice(Dictionary<string, Action<string>> choices)
    {
        // 显示选择面板
        ShowChoicePanel(true);
        m_choose = choices;

        List<string> keys = new List<string>(choices.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            int buttonIndex = i;
            if (choices.ContainsKey(keys[i]))
            {
                m_choiceButtons[i].GetComponentInChildren<Text>().text = keys[i];
                // 清空已绑定的方法.
                m_choiceButtons[i].onClick.RemoveAllListeners();
                // 给选择按钮绑定ShowSendText()方法
                m_choiceButtons[i].onClick.AddListener(() => ShowSendText(keys[buttonIndex]));
            }
        }

    }

    /// <summary> 弹出对话 </summary>
    public void PopChat(Button prefabType, float posX, string message, AudioClip audioType)
    {
        // 移动滑动条到底端
        StartCoroutine(MoveTowardsBottom(0.1f, m_panelScroll.verticalNormalizedPosition, 0));
        InstantiateBubble(prefabType, posX, message);
        m_soundManager.PlayMusic(audioType);

        m_bubbleText = m_bubble.GetComponentInChildren<Text>();
        m_bubbleText.text = InsertWrap(message);
        m_bubble.transform.SetParent(m_chatPanel, false);
        m_popedChatBubbles.Add(m_bubble);
        
    }

    public void PopEvent(string message)
    {
        StartCoroutine(WaitForPopEvent(message, 2.0f));
    }

    IEnumerator WaitForPopEvent(string message, float time)
    {
        yield return new WaitForSeconds(time);
        PopChat(m_eventBubblePrefab, -320.0f, message, null);
    }
    /// <summary> 隐藏消息选择面板 </summary>
    public void HideChoicePanel()
    {
        // 隐藏选择面板
        if (m_chatManager.m_leftChats.Count == 0)
        {
            m_isChoice = true;
            ShowChoicePanel(false);
        }
    }

    /// <summary> 显示消息选择面板 </summary>
    public void ShowChoicePanel(bool isActive)
    {
        for (int i = 0; i < m_choiceButtons.Count; i++)
        {
            m_choiceButtons[i].gameObject.SetActive(isActive);
        }
    }

    //==============================================================================================
    // private methods

    /// <summary> 显示待发送的消息 </summary>
    private void ShowSendText(string message)
    {
        m_waitForSendText.text = message;
        // 发送按钮激活
        SetSendButtonState(SendButtonImageType.ENABLE, true);
        m_seletedButtonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
    }
    /// <summary> 发送消息 </summary>
    private void SendMessage(Dictionary<string, Action<string>> choices)
    {
        List<string> keys = new List<string>(choices.Keys);
        m_sendButton.onClick.RemoveAllListeners();
        // 给发送按钮绑定方法
        if (keys.Count == 2)
        {
            switch (m_seletedButtonName)
            {
                case mc_choiceOne:
                    m_sendButton.onClick.AddListener(() => choices[keys[0]](keys[0]));
                    break;
                case mc_choiceTwo:
                    m_sendButton.onClick.AddListener(() => choices[keys[1]](keys[1]));
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.LogError("选择内容与选择按钮数量不匹配");
        }
    }

    /// <summary> 对话内容换行 </summary>
    private string InsertWrap(string message)
    {
        StringBuilder wrapMessage = new StringBuilder(message);
        for (int i = mc_chineseCharacterCount; i < wrapMessage.Length; i += (mc_chineseCharacterCount + 1))
        {
            wrapMessage.Insert(i, "\n");
        }
        return wrapMessage.ToString();
    }

    /// <summary> 生成bubble </summary>
    private void InstantiateBubble(Button bubblePrefab, float posX, string message)
    {
        int heightFactor = message.Length / mc_chineseCharacterCount;
        float newBubbleHeight = m_bubbleCellHeight;
        // 消息包含两行文字，其bubble高度。
        if (heightFactor == 1)
        {
            newBubbleHeight += mc_secondTextLineHeight;
        }
        // 消息包含三行及以上文字，其bubble高度。
        if (heightFactor > 1)
        {
            newBubbleHeight += mc_secondTextLineHeight + (heightFactor - 1) * mc_textLineHeight;
        }

        m_bubble = Instantiate(bubblePrefab) as Button;
        m_bubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, GetNewBubblePosY(newBubbleHeight));
    }

    /// <summary> 计算新插入对话的Bubble的posY </summary>
    private float GetNewBubblePosY(float newBubbleHeight)
    {
        if ((m_popedBubblesHeights + newBubbleHeight) <= mc_originChatPanelHeight)
        {
            // 若已弹出的Bubble和即将插入的新Bubble总高度没达到滚屏限制高度， 
            // 新Bubble的posY从m_firstBubblePosY开始下移，下移距离是新Bubble高度。
            m_newBubblePosY = mc_firstBubblePosY - m_popedBubblesHeights;
            m_popedBubblesHeights += newBubbleHeight;
        }
        // ------------------------------------------------------------ //
        // 下落结束，新Bubble插在对话框底端，向上滚屏.
        else
        {
            m_isFromFallToScrollUp = true;
            CheckAndScrollBubbles(newBubbleHeight);
            m_newBubblePosY = m_chatPanelBottomposY + newBubbleHeight;
        }

        return m_newBubblePosY;
    }

    /// <summary> 检查是否滚屏，并更新对话框的底边posY </summary>
    private void CheckAndScrollBubbles(float newBubbleHeight)
    {
        /// <summary> 第一次滚屏时，对话框的高度增量。除第一次外，以后的对话框高度增量是新Bubble的高度 </summary>
        float m_deltaHeight = newBubbleHeight - (mc_originChatPanelHeight - m_popedBubblesHeights);
        if (m_isFromFallToScrollUp)
        {
            if (!m_isScrollingUp)
            {
                HeightenChatPanel(m_deltaHeight);
                m_chatPanelBottomposY -= m_deltaHeight / 2;
                ScrollUpBubbles(m_deltaHeight);
                m_isScrollingUp = true;
            }
            else
            {
                HeightenChatPanel(newBubbleHeight);
                m_chatPanelBottomposY -= newBubbleHeight / 2;
                ScrollUpBubbles(newBubbleHeight);
            }
        }
    }

    /// <summary> 增大对话框高度 </summary>
    private void HeightenChatPanel(float deltaHeight)
    {
        // 暂存当前对话框高度
        var chatPanelHeight = m_chatPanel.sizeDelta.y;
        chatPanelHeight += deltaHeight;
        // 更新对话框高度
        m_chatPanel.sizeDelta = new Vector2(m_chatPanel.sizeDelta.x, chatPanelHeight);
    }
    /// <summary> 向上移动各个Bubble，移动距离是新增Bubble（包括间距）的高度 </summary>
    private void ScrollUpBubbles(float step)
    {
        for (int i = 0; i < m_popedChatBubbles.Count; i++)
        {
            float posX = m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition.x;
            float posY = m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition.y;
            m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY + step / 2.0f);
        }
    }
    /// <summary> 移动滑动条到底部 </summary>
    // 参考: http://www.theappguruz.com/blog/dynamic-scroll-view-in-unity-4-6-ui
    private IEnumerator MoveTowardsBottom(float time, float startPosition, float endPosition)
    {
        float step = 0;
        float rate = 1 / time;
        while (step < 1)
        {
            step += rate * Time.deltaTime;
            m_panelScroll.verticalNormalizedPosition = Mathf.Lerp(startPosition, endPosition, step);
            yield return 0;
        }
    }

    void Update()
    {
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            TouchInput();
        #endif
        SendMessage(m_choose);
    }
    /// <summary> 滑动屏幕显示滚动条 </summary>
    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            //Store the first touch detected.
            Touch myTouch = Input.touches[0];

            switch (myTouch.phase)
            {
                case TouchPhase.Began:

                    break;

                case TouchPhase.Moved:
                    m_handle.SetActive(true);
                    break;

                case TouchPhase.Ended:
                    m_handle.SetActive(false);
                    break;
            }
        }
    }


    //==============================================================================================
    // 常量

    
    /// <summary> 对话框高度: 从m_firstBubblePosY到m_chatPanelBottomposY </summary>
    private const float mc_originChatPanelHeight = 945f;

    /// <summary> 第一条对话的PosY </summary>
    private const float mc_firstBubblePosY = 430f;
    private const float mc_textLineHeight = 38.919f;
    private const float mc_secondTextLineHeight = 39.6487f;
    /// <summary> 每行显示最大汉字个数 </summary>
    private const int mc_chineseCharacterCount = 18;
    /// <summary> 选择对话框中两个Button的名字 </summary>
    private const string mc_choiceOne = "ChoiceButtonOne";
    private const string mc_choiceTwo = "ChoiceButtonTwo";

}
