using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressAnyKey : MonoBehaviour
{
    public GameObject MainMenuCanvas;

    private void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            GoToMainMenu();
        }
    }

    public void GoToMainMenu()
    {
        gameObject.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }
}
