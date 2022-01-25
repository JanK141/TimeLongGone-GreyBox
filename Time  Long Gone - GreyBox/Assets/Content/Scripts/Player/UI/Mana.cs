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
    [SerializeField] [Tooltip("Time for lerping slider value")] private float DoValueTime;
    [SerializeField] [Tooltip("Mana cost per second while using slow mo")] private float slowMoCost;
    [SerializeField] [Tooltip("Flat amount of mana used to start rewinding time")] private float flatRewindCost;
    [SerializeField] [Tooltip("Mana cost per second while rewinding")] public float rewindCost;

    //PUBLIC VARIABLES
    private float currMana;

    //PRIVATE VARIABLES
    private bool generating;

    //PROPERTIES
    public float CurrMana
    {
        get => currMana;
        set { currMana = value; UpdateMana();}
    }
    public bool Generating{get=>generating; set=>generating=value;}
    public float SlowMoCost{get=>slowMoCost;}
    public float FlatRewindCost{get=>flatRewindCost;}
    public float RewindCost{get=>rewindCost;}

    void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
        currMana = 20;
        generating = true;
        slider.value = 0;
    }

    void Update()
    {
        if (currMana<=maxMana && generating)
        {
            currMana += genRate * Time.unscaledDeltaTime;
            slider.value = currMana / maxMana;
        }
        Mathf.Clamp(currMana, 0, maxMana);
        //slider.value = currMana / maxMana;
    }


    void UpdateMana()
    {
        slider.DOValue(currMana / maxMana, DoValueTime);
        if (generating)
        {
            generating = false;
            Invoke(nameof(ResetGenerating), DoValueTime);
        }
    }

    void ResetGenerating() => generating = true;
    //example method
    void UseMana()
    {
        if(currMana >= 30)
        {
            currMana -= 30;
            UpdateMana();
        }
    }

}
