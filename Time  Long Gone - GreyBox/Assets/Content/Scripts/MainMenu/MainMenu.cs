using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject SettingsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Continue()
    {

    }

    public void NewGame()
    {

    }

    public void GoToSettings()
    {
        gameObject.SetActive(false);
        SettingsCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
