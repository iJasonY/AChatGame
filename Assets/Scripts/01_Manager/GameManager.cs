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
    public bool m_isGameOver = false;
    public bool m_isBillOver = false;

    public SoundManager m_soundManager;
    public PanelManager m_pm;
    public Emily m_emily;
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
                    Debug.Log("Could not locate a GameManager");
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
    }
    // 弹出通讯录界面
    public void SlideInContactMenu()
    {
        if (m_isBillOver)
        {
            SideInAndClearData(1.0f);
            m_isBillOver = false;
        }
        if (m_isGameOver && !m_isBillOver)
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
        if (m_isGameOver)
        {
            PopUpFadeOut();
            SlideOutContactMenu();
            m_emily.StartChatWithEmily();
            m_isGameOver = false;
        }
    }

    /// <summary> 开始游戏 </summary>
    public void Play()
    {
        if (!m_isGameOver)
        {
            SlideOutContactMenu();
            m_emily.StartChatWithEmily();
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
