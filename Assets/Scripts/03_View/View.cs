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
public class View : MonoBehaviour
{
    //==============================================================================================
    // Fields
    private ChatManager m_chatManager;
    public SoundManager m_soundManager;
    //======================================
    // ChatPanel
    public GameObject m_chatTopBar;
    // 待发送消息
    public Text m_waitForSendText;
    public Button m_sendButton;
    public Text m_topBarText;
    public Button m_leftBubblePrefab;
    public Button m_rightBubblePrefab;

    public Button m_eventBubblePrefab;

    public RectTransform m_chatContainer;
    public ScrollRect m_panelScroll;
    public GameObject m_handle;
    //======================================
    // ContactPanel
    public RectTransform m_contactPanel;
    public Image m_popUP;
    public Button m_acceptButton;
    public Button m_cancleButton;
    public Button m_emilyButton;
    // End ContactMenu
    //======================================
    private Text m_bubbleText;
    /// <summary> 对话气泡 </summary>
    private Button m_bubble;

    /// <summary> 已弹出的对话气泡 </summary>
    private List<Button> m_popedChatBubbles = new List<Button>();
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
    /// <summary> 右侧对话是否为空 </summary>
    public bool m_isRightChatIsNull = true;
    private Image m_sendButtonImage;
    /// <summary> 发送按钮背景图片 </summary>
    public List<Sprite> m_sendButtonSprites = new List<Sprite>();



    //==============================================================================================
    // public methods
    void Awake()
    {
        m_chatManager = FindObjectOfType(typeof(ChatManager)) as ChatManager;
        m_soundManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;
    }

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

        m_topBarText = m_chatTopBar.GetComponentInChildren<Text>();
        m_sendButtonImage = m_sendButton.GetComponent<Image>();

        m_topBarText.text = m_chatManager.m_chatObjectName;
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



    /// <summary> 弹出对话 </summary>
    public void PopChat(Button prefabType, float posX, string message, AudioClip audioType)
    {
        // 移动滑动条到底端
        StartCoroutine(MoveTowardsBottom(0.1f, m_panelScroll.verticalNormalizedPosition, 0));
        InstantiateBubble(prefabType, posX, message);
        m_soundManager.PlayMusic(audioType);

        m_bubbleText = m_bubble.GetComponentInChildren<Text>();
        // 系统提示不换行
        if (prefabType.CompareTag("EventBubble"))
        {
            m_bubbleText.text = message;
        }
        else
        {
            // 聊天内容换行
            m_bubbleText.text = InsertWrap(message);
        }
        m_bubble.transform.SetParent(m_chatContainer, false);
        m_popedChatBubbles.Add(m_bubble);

    }

    public void PopEvent(string message)
    {
        StartCoroutine(WaitForPopEvent(message, 2.0f));
    }

    IEnumerator WaitForPopEvent(string message, float duration)
    {
        yield return new WaitForSeconds(duration);
        PopChat(m_eventBubblePrefab, -350.0f, message, null);
    }
    /// <summary> 隐藏消息选择面板 </summary>
    public void HideChoicePanel()
    {
        ShowChoicePanel(false);
    }

    /// <summary> 显示消息选择面板 </summary>
    public void ShowChoicePanel(bool isActive)
    {
        for (int i = 0; i < m_choiceButtons.Count; i++)
        {
            m_choiceButtons[i].gameObject.SetActive(isActive);
        }
    }


    /// <summary> 设置选择面板 </summary>
    public void SetChoice(Dictionary<string, Action<string>> choices)
    {
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
                m_choiceButtons[i].onClick.AddListener(() => ShowSendText(keys[buttonIndex], choices));
            }
        }

    }

    /// <summary> 显示待发送的消息 </summary>
    private void ShowSendText(string message, Dictionary<string, Action<string>> choices)
    {
        // 显示待发送的消息
        m_waitForSendText.text = message;
        // 激活发送按钮
        SetSendButtonState(SendButtonImageType.ENABLE, true);
        // 获得用户选中的选择按钮的名字（ChoiceButtonOne/ChoiceButtonTwo）
        string seletedButtonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        // 给发送按钮绑定对应的Say()方法，点击发送按钮时，发送用户选择的对话内容
        AddSayFuncToSendButton(seletedButtonName, choices);
    }
    /// <summary> 给发送按钮绑定Say()方法 </summary>
    private void AddSayFuncToSendButton(string seletedButtonName, Dictionary<string, Action<string>> choices)
    {
        List<string> keys = new List<string>(choices.Keys);
        m_sendButton.onClick.RemoveAllListeners();
        switch (seletedButtonName)
        {
            // 给ChoiceButtonOne绑定Say()方法
            case mc_choiceButtonOne:
                m_sendButton.onClick.AddListener(() => choices[keys[0]](keys[0]));
                break;
            // // 给ChoiceButtonTwo绑定Say()方法
            case mc_choiceButtonTwo:
                m_sendButton.onClick.AddListener(() => choices[keys[1]](keys[1]));
                break;
            default:
                break;
        }

    }

    /// <summary> 聊天内容换行 </summary>
    private string InsertWrap(string message)
    {
        StringBuilder wrapMessage = new StringBuilder(message);
        for (int i = mc_charCountInPerLine; i < wrapMessage.Length; i += (mc_charCountInPerLine + 1))
        {
            wrapMessage.Insert(i, "\n");
        }
        return wrapMessage.ToString();
    }

    /// <summary> 生成bubble </summary>
    private void InstantiateBubble(Button bubblePrefab, float posX, string message)
    {
        int heightFactor = message.Length / mc_charCountInPerLine;
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
        var chatPanelHeight = m_chatContainer.sizeDelta.y;
        chatPanelHeight += deltaHeight;
        // 更新对话框高度
        m_chatContainer.sizeDelta = new Vector2(m_chatContainer.sizeDelta.x, chatPanelHeight);
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
    /// <summary> 每行最大显示字符个数 </summary>
    private const int mc_charCountInPerLine = 18;
    /// <summary> 选择对话框中两个Button的名字 </summary>
    private const string mc_choiceButtonOne = "ChoiceButtonOne";
    private const string mc_choiceButtonTwo = "ChoiceButtonTwo";

}
