using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth Instance;

    //SERIALIZED FIELDS
    [SerializeField] [Min(1)] private float maxHealth = 100;
    [SerializeField] private Slider slider;

    [SerializeField] [Min(1)] [Tooltip("How many combat stages boss has (divided by % of hp) ")]
    private int stages = 1;

    [SerializeField] private float barShakeStrength = 2f;

    //PRIVATE VARIABLES
    private float currHealth;
    private int currStage;
    private float[] stageChangers;

    //PROPERTIES
    public float CurrHealth
    {
        get => currHealth;
        set
        {
            currHealth = value;
            UpdateHealth();
        }
    }

    void Awake() => Instance = this;

    void Start()
    {
        currHealth = maxHealth;
        slider.value = 1;
        currStage = 1;
        stageChangers = new float[stages];
        for (var i = 0; i < stages; i++)
        {
            if (i + 1 == stages) stageChangers[i] = 0;
            else stageChangers[i] = maxHealth - (maxHealth / stages) * (i + 1);
        }
    }

    void UpdateHealth()
    {
        slider.DOValue(currHealth / maxHealth, 0.5f);
        slider.transform.DOShakePosition(0.5f, barShakeStrength);
        if (currHealth <= 0) Death();
        else if (currHealth <= stageChangers[currStage - 1])
        {
            currStage++;
            //TODO next combat stage sequence
        }
    }

    void Death()
    {
        print("You won the level!");
        //TODO start level end sequence
        Destroy(gameObject);
        TimerController.instance.EndTimer();
    }
}