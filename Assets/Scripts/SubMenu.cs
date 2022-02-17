using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SubMenu : MonoBehaviour
{
    public GameObject subMenuSet;
    public GameObject subMenuPanel;
    public GameObject ControlPanel;
    void Start()
    {
        
        
    }

    
    void Update()
    {
        onAndOffSubMenu();

        
        //서브메뉴에서 control 패널이 떠 있을 때 ESC를 누르면 control 패널이 꺼지고 다시 서브메뉴 패널이 나오게 하는 코드
        if (Input.GetButtonDown("Cancel") && ControlPanel.activeSelf)
        {
            ControlPanel.SetActive(false);
            subMenuPanel.SetActive(true);
        }




    }

    void onAndOffSubMenu()
    {
        if (Input.GetButtonDown("Cancel") && !subMenuPanel.activeSelf)
        {
            subMenuSet.SetActive(true);
            subMenuPanel.SetActive(true);
        }
        else if (Input.GetButtonDown("Cancel") && subMenuPanel.activeSelf)
        {
            subMenuSet.SetActive(false);
            subMenuPanel.SetActive(false);
        }
        
        if (subMenuSet.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    
    public void onClickControl()
    {
        subMenuPanel.SetActive(false);
        ControlPanel.SetActive(true);
    }

    public void onClickExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
