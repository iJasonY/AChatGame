using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    //==============================================================================================
    // Fields

    public RectTransform m_contactMenu;
    public Image m_popUP;
    public Button m_acceptButton;
    public Button m_cancleButton;
    public Button m_emilyButton;
    private float m_contactMenuPosX;
    public SoundManager m_soundManager;
    public PanelManager m_pm;
    public LevelOne m_levelOne;
    public IntroductionLevel m_introductionLevel;
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

    void Start()
    {
        // 隐藏弹窗
        m_popUP.gameObject.SetActive(false);
        // 给按钮绑定操作方法
        m_emilyButton.onClick.AddListener(() => Play());
        m_acceptButton.onClick.AddListener(() => RePlay());
        m_cancleButton.onClick.AddListener(() => PopUpFadeOut());
        Debug.Log("m_introductionLevelOver: " + GameSaver.Instance.gameData.IsIntroductionLevelOver);
        Debug.Log("m_isLevelOver: " + GameSaver.Instance.gameData.IsLevelOneOver);

        // Debug.Log("m_FirstTime: " + GameSaver.m_gameData.GetIsGameStartForTheFirstTime());
        if (!GameSaver.Instance.gameData.IsIntroductionLevelOver)
        {
            m_introductionLevel.StartChat();
        }
        
    }
    // 弹出通讯录界面
    public void SlideInContactMenu()
    {
        if (GameSaver.Instance.gameData.IsIntroductionLevelOver && !GameSaver.Instance.gameData.IsLevelOneOver)
        {
            SideInAndClearData(1.0f);
        }
        else
        {
            SideInAndClearData(7.5f);
        }
    }

    private void SideInAndClearData(float time)
    {
        StartCoroutine(WaitForSlideIn(time));
        StartCoroutine(ClearGameData(time + 0.2f));
    }
    // 隐藏通讯录界面
    public void SlideOutContactMenu()
    {
        m_contactMenu.DOAnchorPosX(-900.0f, 1.0f, true);
    }

    IEnumerator WaitForSlideIn(float time)
    {
        yield return new WaitForSeconds(time);
        m_contactMenu.DOAnchorPosX(0.0f, 1.5f, true);
        m_soundManager.PlayMusic(m_soundManager.m_slideInContactMenuAudio);
    }
    // 删除聊天数据
    IEnumerator ClearGameData(float time)
    {
        yield return new WaitForSeconds(time);
        m_pm.ClearPopedBubble();
    }
    /// <summary> 重新开始游戏 </summary>
    public void RePlay()
    {
        if (GameSaver.Instance.gameData.IsLevelOneOver)
        {
            PopUpFadeOut();
            SlideOutContactMenu();
            m_levelOne.StartChat();
        }
    }

    /// <summary> 开始游戏 </summary>
    public void Play()
    {
        if (!GameSaver.Instance.gameData.IsLevelOneOver)
        {
            SlideOutContactMenu();
            m_levelOne.StartChat();
        }
        else
        {
            // 弹出弹窗
            m_popUP.gameObject.SetActive(true);
            // 禁用Emily button交互
            m_emilyButton.interactable = false;
        }

    }
    /// <summary> 隐藏弹窗 <summary>
    public void PopUpFadeOut()
    {
        m_popUP.gameObject.SetActive(false);
        // 开启Emily button交互
        m_emilyButton.interactable = true;
    }
}
