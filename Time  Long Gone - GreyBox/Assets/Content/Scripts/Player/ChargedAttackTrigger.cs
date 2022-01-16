using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedAttackTrigger : MonoBehaviour
{
    [HideInInspector] public float damage;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            print("Dash damage! " + damage);
            EnemyHealth.Instance.CurrHealth -= damage;
            gameObject.SetActive(false);
        }
    }
}
