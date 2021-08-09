using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  
    public void NewGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(2);
    }

    public void OptionsButton()
    {
       
    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
