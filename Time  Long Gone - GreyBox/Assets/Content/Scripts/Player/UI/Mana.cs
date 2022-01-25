using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Mana : MonoBehaviour
{
    public static Mana Instance;

    //SERIALIZED FIELDS
    [SerializeField] [Min(1)] private float maxMana = 100;
    [SerializeField] private Slider slider;
    [SerializeField] [Tooltip("How many points of mana are generated per second")] private float genRate = 5f;
    [SerializeField] [Tooltip("Determines how often a second will the Mana be generated")] public float refreshRate = 0.2f;

    //PUBLIC VARIABLES
    public float currMana;

    //PRIVATE VARIABLES
    private bool generating;

    //PROPERTIES
    public float CurrMana
    {
        get => currMana;
        set { currMana = value; UpdateMana(); }
    }

    void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
        currMana = 20;
        generating = true;
        slider.value = 0;
        StartCoroutine(nameof(StartGenerating)); 
    }

    IEnumerator StartGenerating()
    {
        print("Started generating");
        while(true)
        {
            yield return new WaitForSeconds(refreshRate);
            if(generating)
            {
                print("Generated " + genRate*refreshRate);
                currMana += genRate * refreshRate;
                UpdateMana();
            }
        }
    }

    void UpdateMana()
    {
        if(currMana >= maxMana)
        {
            currMana = maxMana;
            generating = false;
        }
        else
        {
            generating = true;
        }

        slider.DOValue(currMana / maxMana, 0.1f);
    }

    //example method
    void UseMana()
    {
        if(currMana >= 30)
        {
            currMana -= 30;
            UpdateMana();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            UseMana();
        }
    }
}
