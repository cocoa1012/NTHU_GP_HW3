using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{

    // Use this for initialization
    public Canvas Info;
	public AudioClip click;
	public GameObject player;
	AudioSource m_audio;
    void Start()
    {
		m_audio = player.GetComponent<AudioSource>();
		Time.timeScale = 1;
		if (Info != null){
			Info.enabled = false;
		}
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitGame()
    {
		m_audio.PlayOneShot(click);
        Application.Quit();
    }

    public void Menu()
    {
		m_audio.PlayOneShot(click);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void CloseInfo()
    {
		m_audio.PlayOneShot(click);
        Info.enabled = false;
    }

    public void ShowInfo()
    {
		m_audio.PlayOneShot(click);
        Info.enabled = true;
    }

	public void EnterLevel1(){
		m_audio.PlayOneShot(click);
		UnityEngine.SceneManagement.SceneManager.LoadScene("Level_1");
	}

	public void EnterLevel2(){
		m_audio.PlayOneShot(click);
		UnityEngine.SceneManagement.SceneManager.LoadScene("Level_2");
	}
}
