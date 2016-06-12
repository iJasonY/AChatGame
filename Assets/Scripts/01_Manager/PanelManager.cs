using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class PanelManager : MonoBehaviour
{
    public GameObject m_handle;
    public ChatManager cm;
    public GameObject m_topPanel;
    public Text m_sendText;
    public Button m_sendButton;
    public Text m_topText;
    public Button m_leftBubblePrefab;
    public Button m_rightBubblePrefab;

    public RectTransform m_chatPanel;
    public ScrollRect m_panelScroll;

    /// <summary> manage bubbles </summary>
    public List<Button> m_chatBubbles = new List<Button>();
    public List<Button> m_choiceButtons = new List<Button>();
    public SoundManager m_sm;

    private Text m_bubbleText;
    private Button m_bubble;
    /// <summary> 单行对话气泡高度（Button） </summary>
    private float m_bubbleCellHeight = 78.91891f + 20f;
    private float m_newBubblePosY;

    /// <summary> 已弹出对话的总高度 </summary>
    private float m_popedBubblesHeights = 0f;

    /// <summary> 对话框底边的posY </summary>
    private float m_chatPanelBottomposY = -515f;
    /// <summary> 对话框高度 </summary>
    private float m_chatPanelHeight = 1100f;

    private bool m_isStartScroll = false;
    private bool m_isScrolling = false;
    public bool m_isChoice = true;
    private string m_selectedChoiceButton;
    private Dictionary<string, Action<string>> m_choose;
    private Image m_sendButtonImage;
    /// <summary> 发送按钮背景图片 </summary>
    public List<Sprite> m_buttonSprites = new List<Sprite>();
    // public List<Sprite> m_feelingSprites = new List<Sprite>();
    // public Button m_feelingButton;
    private Image m_feelingButtonImg;
    


    //==============================================================================================
    // 公开方法

    void Start()
    {
        // 初始化对话框
        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            m_handle.SetActive(false);
        #endif
        m_topText = m_topPanel.GetComponentInChildren<Text>();
        m_sendButtonImage = m_sendButton.GetComponent<Image>();
        // m_feelingButtonImg = m_feelingButton.GetComponent<Image>();

        m_topText.text = "Boss";
        SetSendButton(1, false);
        // SetFeelingButton(0);
    }
    /// <summary> 设置表情状态 </summary>
    // public void SetFeelingButton(int i)
    // {
    //     m_feelingButtonImg.sprite = m_feelingSprites[i];
    // }
    /// <summary> 设置发送按钮状态 </summary>
    public void SetSendButton(int i, bool m_isInteractable)
    {
        m_sendButtonImage.sprite = m_buttonSprites[i];
        m_sendButton.interactable = m_isInteractable;
    }
    
    /// <summary> 选择面板 </summary>
    public void Choice(Dictionary<string, Action<string>> m_choices)
    {
        // TODO: 显示选择面板
        ShowChoicePanel(true);
        m_choose = m_choices;
        List<string> keys = new List<string>(m_choices.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            int buttonIndex = i;
            if (m_choices.ContainsKey(keys[i]))
            {
                m_choiceButtons[i].GetComponentInChildren<Text>().text = keys[i];
                // 清空已绑定的方法.
                m_choiceButtons[i].onClick.RemoveAllListeners();
                // 不能直接用m_choices[keys[[i]](keys[i])，否则会报错, m_choices[key](param).
                // m_choiceButtons[i].onClick.AddListener(() => m_choices[keys[buttonIndex]](keys[buttonIndex]));
                m_choiceButtons[i].onClick.AddListener(() => ShowSendText(keys[buttonIndex]));
            }
        }

    }

    
    /// <summary> Add one new chat Bubble </summary>
    public void AddNewBubble(bool m_isLeftBubble, string message)
    {
        StartCoroutine(MoveTowardsBottom(0.1f, m_panelScroll.verticalNormalizedPosition, 0));

        if (m_isLeftBubble)
        {
            InstantiateBubble(m_leftBubblePrefab, m_leftBubbleposX, message);
            // Play Sound
            m_sm.PlayMusic(m_sm.m_leftAudio);
        }
        else
        {
            InstantiateBubble(m_rightBubblePrefab, m_rightBubbleposX, message);
            m_sm.PlayMusic(m_sm.m_rightAudio);
            //TODO: 隐藏选择面板
            if (cm.m_leftChats.Count == 0)
            {
                m_isChoice = true;
                ShowChoicePanel(false);
            }
        }

        m_bubbleText = m_bubble.GetComponentInChildren<Text>();
        string text = InsertWrap(message);
        m_bubbleText.text = text;

        m_bubble.transform.SetParent(m_chatPanel, false);
        m_chatBubbles.Add(m_bubble);

    }
    /// <summary> 显示消息选择面板 </summary>
    public void ShowChoicePanel(bool isActive)
    {
        foreach (Button item in m_choiceButtons)
        {
            item.gameObject.SetActive(isActive);
        }
    }


    //==============================================================================================
    // 非公开方法 
    
    /// <summary> 显示待发送的消息 </summary>
    private void ShowSendText(string message)
    {
        m_sendText.text = message;
        SetSendButton(0, true);
        m_selectedChoiceButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
    }
    /// <summary> 发送消息 </summary>
    private void SendMessage(Dictionary<string, Action<string>> m_choices)
    {
        string m_seletedButtonName = m_selectedChoiceButton;
        List<string> keys = new List<string>(m_choices.Keys);
        m_sendButton.onClick.RemoveAllListeners();
        
        if (keys.Count >= 2)
        {
            switch (m_seletedButtonName)
            {
                case m_choice1:
                    m_sendButton.onClick.AddListener(() => m_choices[keys[0]](keys[0]));
                    break;
                case m_choice2:
                    m_sendButton.onClick.AddListener(() => m_choices[keys[1]](keys[1]));
                    break;

                default:
                    break;
            }
        }
    }

    
    /// <summary> 对话内容换行 </summary>
    private string InsertWrap(string message)
    {
        StringBuilder wrapMessage = new StringBuilder(message);
        int interval = m_hanCount;
        for (int i = interval; i < wrapMessage.Length; i += (interval + 1))
        {
            wrapMessage.Insert(i, "\n");
        }
        return wrapMessage.ToString();
    }

    /// <summary> 生成bubble </summary>
    private void InstantiateBubble(Button BubblePrefab, float posX, string message)
    {
        int heightFactor = message.Length / m_hanCount;
        float newBubbleHeight = m_bubbleCellHeight;
        // 消息包含两行文字，其bubble高度。
        if (heightFactor == 1)
        {
            newBubbleHeight += m_secondTextLineHeight;
        }
        // 消息包含三行及以上文字，其bubble高度。
        if (heightFactor > 1)
        {
            newBubbleHeight += m_secondTextLineHeight + (heightFactor - 1) * m_textLineHeight;
        }

        m_bubble = Instantiate(BubblePrefab) as Button;
        m_bubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, GetNewBubblePosY(newBubbleHeight));
    }

    /// <summary> 计算新插入对话的Bubble的posY </summary>
    private float GetNewBubblePosY(float m_newBubbleHeight)
    {
        if ((m_popedBubblesHeights + m_newBubbleHeight) <= m_scrollLimitHeight)
        {
            // 若已弹出的Bubble和即将插入的新Bubble总高度没达到滚屏限制高度， 
            // Bubble的posY从m_firstBubblePosY开始下移.
            m_newBubblePosY = m_firstBubblePosY - m_popedBubblesHeights;
        }
        // ------------------------------------------------------------ //
        // 下落结束，新Bubble插在对话框底端，向上滚屏.
        else
        {
            m_isStartScroll = true;
            CheckAndScrollBubbles(m_newBubbleHeight);
            m_newBubblePosY = m_chatPanelBottomposY + m_newBubbleHeight;
        }
        m_popedBubblesHeights += m_newBubbleHeight;
        return m_newBubblePosY;
    }

    /// <summary> 检查是否滚屏，并更新对话框的底边posY </summary>
    private void CheckAndScrollBubbles(float m_newBubbleHeight)
    {
        /// <summary> 第一次滚屏时，对话框的高度增量。除第一次外，以后的对话框高度增量是新Bubble的高度 </summary>
        float m_deltaHeight = m_newBubbleHeight - (m_originChatPanelHeight - m_popedBubblesHeights);
        if (m_isStartScroll)
        {
            if (!m_isScrolling)
            {
                HeightenChatPanel(m_deltaHeight);
                m_chatPanelBottomposY -= m_deltaHeight / 2;
                ScrollBubbles(m_deltaHeight);
                m_isScrolling = true;
            }
            else
            {
                HeightenChatPanel(m_newBubbleHeight);
                m_chatPanelBottomposY -= m_newBubbleHeight / 2;
                ScrollBubbles(m_newBubbleHeight);
            }
        }
    }

    /// <summary> 增大对话框高度 </summary>
    private void HeightenChatPanel(float m_step)
    {
        m_chatPanelHeight += m_step;
        m_chatPanel.sizeDelta = new Vector2(800, m_chatPanelHeight);
    }
    /// <summary> 向上移动各个Bubble </summary>
    private void ScrollBubbles(float m_step)
    {
        foreach (Button view in m_chatBubbles)
        {
            float posX = view.GetComponent<RectTransform>().anchoredPosition.x;
            float posY = view.GetComponent<RectTransform>().anchoredPosition.y;
            view.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY + m_step / 2);
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

    private void Update()
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

    /// <summary> 对话Bubble的posX坐标 </summary>
    private const float m_leftBubbleposX = -360f;
    private const float m_rightBubbleposX = 360f;

    /// <summary> 对话框高度: 从m_firstBubblePosY到m_chatPanelBottomposY </summary>
    private const float m_originChatPanelHeight = 945f;

    /// <summary> 第一条对话的PosY </summary>
    private const float m_firstBubblePosY = 430f;
    /// <summary> 滚屏限制高度 </summary>
    private const float m_scrollLimitHeight = 950f;
    /// <summary> 单行文字的高度, 需要随字体类型和字体大小手动改变 </summary>
    private const float m_textLineHeight = 38.919f;
    private const float m_secondTextLineHeight = 39.6487f;
    /// <summary> 每行显示最大汉字个数 </summary>
    private const int m_hanCount = 18;
    /// <summary> 选择对话框中两个Button的名字，声明为常量，避免字符串拼写错误引起的bug </summary>
    private const string m_choice1 = "Choice1";
    private const string m_choice2 = "Choice2";
}
