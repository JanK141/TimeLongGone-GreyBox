using System.Runtime.Serialization;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject sword;
    private bool inContact;

    public void Attack()
    {
        if (inContact)
        {
            sword.transform.rotation = new Quaternion(0, 0, -50, 0);
            print("Attack");
            ResetAttack();
        }
    }

    public void ResetAttack()
    {
        sword.transform.rotation = new Quaternion(0, 0, 0, 0);
        print("ResetAttack");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("INCOLISION");
            inContact = true;
            Attack();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("OUTCOLISION");
            inContact = false;
            ResetAttack();
        }
    }
}