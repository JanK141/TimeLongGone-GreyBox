using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject MainMenuCanvas;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Difficulty()
    {

    }
    public void Video()
    {

    }

    public void Audio()
    {

    }

    public void Controls()
    {

    }

    public void JustGoBack()
    {
        gameObject.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }


}
