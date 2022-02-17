using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void onClickNewGame()
    {
        Debug.Log("Make New Game");
    }

    public void onClickLoadGame()
    {
        Debug.Log("Load Game");
        
    }

    public void onClickOptions()
    {
        Debug.Log("Options");
        SceneManager.LoadScene("Options");
    }

    public void onClickExit()
    {
        Debug.Log("Exit");
    }
}
