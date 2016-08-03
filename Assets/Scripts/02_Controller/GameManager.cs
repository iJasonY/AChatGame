using UnityEngine;
using System.Collections;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    //==============================================================================================
    // Fields
    private View m_view;
    private LevelEmily m_levelEmily;
    private LevelIntroduction m_levelIntroduction;
    private static GameManager s_instance = null;
    public static GameManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType(typeof(GameManager)) as GameManager;
                if (s_instance == null)
                {
                    Debug.Log("Could not locate GameManager");
                }
            }
            return s_instance;
        }
    }

    //==============================================================================================
    // Methods
    void Awake()
    {
        m_view = FindObjectOfType(typeof(View)) as View;
        // 关卡
        m_levelIntroduction = FindObjectOfType(typeof(LevelIntroduction)) as LevelIntroduction;
        m_levelEmily = FindObjectOfType(typeof(LevelEmily)) as LevelEmily;
    }
    void Start()
    {
        // 隐藏弹窗
        m_view.m_popUP.gameObject.SetActive(false);
        // 给按钮绑定操作方法
        m_view.m_emilyButton.onClick.AddListener(() => Play());
        m_view.m_acceptButton.onClick.AddListener(() => Accept());
        m_view.m_cancleButton.onClick.AddListener(() => Cancle());
        // Debug.Log("IsEmilyLevelOver: " + GameSaver.Instance.gameData.IsEmilyLevelOver);
        // Debug.Log("IsIntroductionLevelOver: " + GameSaver.Instance.gameData.IsIntroductionLevelOver);
        // 初次开始游戏，引导关自动运行
        if (!GameSaver.Instance.gameData.IsIntroductionLevelOver)
        {
            ChatStart(m_levelIntroduction);
        }
    }

    /// <summary> 开始游戏 </summary>
    private void Play()
    {
        if (!GameSaver.Instance.gameData.IsEmilyLevelOver)
        {
            ChatStart(m_levelEmily);
        }
        else
        {
            PopUpFadeIn();
        }

    }

    /// <summary> 确定重新开始游戏 </summary>
    private void Accept()
    {
        if (GameSaver.Instance.gameData.IsEmilyLevelOver)
        {
            PopUpFadeOut();
            ChatStart(m_levelEmily);
        }
    }

    private void Cancle()
    {
        PopUpFadeOut();
    }

    private void ChatStart(LevelBase level)
    {
        SlideOutContactPanel();
        level.StartChat();
    }

    public void ChatEndSetting(float duration)
    {
        GameSaver.Instance.SaveGameData();
        // 弹出ContactPanel
        SlideInContactPanel(duration);
    }
    /// <summary> 弹出弹窗 </summary>
    private void PopUpFadeIn()
    {
        m_view.m_popUP.gameObject.SetActive(true);
        // 禁用Emily button交互
        m_view.m_emilyButton.interactable = false;
    }

    /// <summary> 隐藏弹窗 <summary>
    private void PopUpFadeOut()
    {
        m_view.m_popUP.gameObject.SetActive(false);
        // 开启Emily button交互
        m_view.m_emilyButton.interactable = true;
    }

    // 弹出通讯录界面
    public void SlideInContactPanel(float duration)
    {
        StartCoroutine(WaitForSlideInContactPanel(duration));
        StartCoroutine(WaitForClearChats(duration + 0.2f));
    }
    // 隐藏通讯录界面
    public void SlideOutContactPanel()
    {
        m_view.m_contactPanel.DOAnchorPosX(-900.0f, 1.0f, true);
    }

    IEnumerator WaitForSlideInContactPanel(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_view.m_contactPanel.DOAnchorPosX(0.0f, 1.5f, true);
        m_view.m_soundManager.PlayMusic(m_view.m_soundManager.m_slideInContactMenuAudio);
    }
    // 删除聊天数据
    IEnumerator WaitForClearChats(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_view.ClearPopedBubble();
    }
}
