using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TimeManipulation : MonoBehaviour
{
    public static TimeManipulation Instance;

    [SerializeField] [Range(0,1)] [Tooltip("How low time scale can get during slow mo")] private float MinSlowMo;
    [SerializeField] [Tooltip("How long it gets to bring time scale to it's minimum value")] private float SlowMoTime;
    [SerializeField] [Tooltip("How much faster time should slow down when player is killed")] private float DeathSlowMoMulti=3;
    [SerializeField] private float MaxRewindTime;
    [SerializeField] private CinemachineVirtualCamera cam;

    private bool isRewinding = false;
    private bool isPlayerDead = false;
    private bool isSlowMo = false;

    public bool IsPlayerDead{get=>isPlayerDead; set{isPlayerDead=value; DeadOrAlive(value);}}
    public float RewindTime{get=>MaxRewindTime;}
    public bool IsRewinding{get=>isRewinding;}

    void Awake() => Instance = this;
    void Start()
    {

    }

    void Update()
    {
        if (!isPlayerDead)
        {
            if (!isSlowMo && Input.GetButtonDown("Power")){isSlowMo=true; StartCoroutine(StartSlowMo());}
            if (isSlowMo && Input.GetButtonUp("Power")){isSlowMo=false; StartCoroutine(StopSlowMo());}
            if (isSlowMo)
            {
                Mana.Instance.CurrMana -= Mana.Instance.SlowMoCost * Time.unscaledDeltaTime;
                if (Mana.Instance.CurrMana < 1)
                {
                    isSlowMo = false;
                    StartCoroutine(StopSlowMo());
                }
            }
        }
        else
        {
            if (isRewinding) Mana.Instance.CurrMana -= Mana.Instance.rewindCost * Time.unscaledDeltaTime;
            if (Input.GetButtonDown("Power") && Mana.Instance.CurrMana>Mana.Instance.FlatRewindCost)
            {
                Mana.Instance.CurrMana -= Mana.Instance.FlatRewindCost;
                Mana.Instance.Generating = false;
                PlayerMovement.Instance.GetComponent<CharacterController>().enabled = false;
                EnemyStatus.Instance.GetComponent<Animator>().enabled = false;
                isRewinding =true; 
                print("Start Rewind");
            }
            if ((Input.GetButtonUp("Power") && isRewinding) || (Mana.Instance.CurrMana<1 && isRewinding))
            {
                Mana.Instance.Generating = true;
                PlayerMovement.Instance.GetComponent<CharacterController>().enabled = true;
                EnemyStatus.Instance.GetComponent<Animator>().enabled = true;
                print("Stop Rewind");
                isRewinding = false;
                IsPlayerDead = false;
            }
        }
    }

    void DeadOrAlive(bool dead)
    {
        if (dead)
        {
            Mana.Instance.Generating = false;
            StartCoroutine(PlayerDead());
        }
        else
        {
            Mana.Instance.Generating = true;
            Time.timeScale = MinSlowMo;
            StartCoroutine(StopSlowMo());
        }
    }

    IEnumerator StartSlowMo()
    {
        var composer = cam.GetCinemachineComponent<CinemachineGroupComposer>();
        float time = ((1 - Time.timeScale) / (1 - MinSlowMo)) * SlowMoTime;
        while (isSlowMo && Time.timeScale>MinSlowMo)
        {
            time += Time.unscaledDeltaTime;
            composer.m_MinimumFOV = Mathf.Lerp(88f, 60f, time / SlowMoTime);
            Time.timeScale = Mathf.Lerp(1f, MinSlowMo, time / SlowMoTime);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
    }
    //TODO change loop conditions in corutines for safety
    IEnumerator StopSlowMo()
    {
        var composer = cam.GetCinemachineComponent<CinemachineGroupComposer>();
        float time = ((Time.timeScale-MinSlowMo) / (1 - MinSlowMo)) * SlowMoTime;
        while (!isSlowMo && Time.timeScale < 1f)
        {
            time += Time.unscaledDeltaTime;
            composer.m_MinimumFOV = Mathf.Lerp(60f, 88f, time / SlowMoTime);
            Time.timeScale = Mathf.Lerp(MinSlowMo, 1f, time / SlowMoTime);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
    }

    IEnumerator PlayerDead()
    {
        var composer = cam.GetCinemachineComponent<CinemachineGroupComposer>();
        float time = (1 - Time.timeScale) * (SlowMoTime/DeathSlowMoMulti);
        while (Time.timeScale>0.01f)
        {
            time += Time.unscaledDeltaTime;
            composer.m_MinimumFOV = Mathf.Lerp(88f, 40f, time / (SlowMoTime / DeathSlowMoMulti));
            Time.timeScale = Mathf.Lerp(1f, 0.01f, time / (SlowMoTime/DeathSlowMoMulti));
            //Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
    }
}
