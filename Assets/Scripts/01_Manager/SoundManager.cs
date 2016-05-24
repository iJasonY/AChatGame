using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip m_leftAudio;
    public AudioClip m_rightAudio;
    private AudioSource m_audio;
	public Button m_setSoundButton;
	public List<Sprite> m_soundButtonImgs = new List<Sprite>();
	
	[SerializeField]
	private bool m_soundIsActive = true;
	private Image m_soundButtonImage;

    // Use this for initialization
    void Start()
    {
        m_audio = this.GetComponent<AudioSource>();
		m_setSoundButton.onClick.AddListener(()=> SetSound());
		m_soundButtonImage = m_setSoundButton.GetComponent<Image>();
		m_soundButtonImage.sprite = m_soundButtonImgs[0];
    }

    public void PlayMusic(AudioClip clip)
    {
        m_audio.PlayOneShot(clip, 1.0f);
    }
	/// <summary> 关闭/打开声音 </summary>
	private void SetSound()
	{
		if (m_soundIsActive)
		{
			m_audio.volume = 0f;
			m_soundIsActive = false;
			m_soundButtonImage.sprite = m_soundButtonImgs[1];
		}
		else
		{
			m_audio.volume = 1f;
			m_soundIsActive = true;
			m_soundButtonImage.sprite = m_soundButtonImgs[0];
		}
	}
}
