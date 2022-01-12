using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100;
    private const float maxHealth = 100;

    public GameObject healthbarUI;
    public Slider slider;

    private Transform mainCamera;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    private void Update()
    {
        slider.value = CalculateHealth();
        checkHealth();
    }

    private void LateUpdate()
        =>
            slider.transform.LookAt(transform.position + mainCamera.forward);

    private float CalculateHealth() => health / maxHealth;

    private void checkHealth()
    {
        if (health < maxHealth)
            healthbarUI.SetActive(true);
        if (health <= 0)
            Death();
        if (health > maxHealth)
            health = maxHealth;
    }

    internal void GetDamage(float damage)
    {
        health -= damage;
        slider.value = CalculateHealth();
    }

    private void Death() 
        =>
            Destroy(gameObject);
}