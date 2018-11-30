using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{

    // Use this for initialization
    public Canvas Info;
    void Start()
    {
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
        Application.Quit();
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void CloseInfo()
    {
        Info.enabled = false;
    }

    public void ShowInfo()
    {
        Info.enabled = true;
    }

	public void EnterLevel1(){
		UnityEngine.SceneManagement.SceneManager.LoadScene("Level_1");
	}

	public void EnterLevel2(){
		UnityEngine.SceneManagement.SceneManager.LoadScene("Level_2");
	}
}
