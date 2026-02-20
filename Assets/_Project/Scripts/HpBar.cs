using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public Image healthBarFill; // הגרר את התמונה האדומה לכאן

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthUI();

        if (currentHealth == 0)
            Die();
    }

    void UpdateHealthUI()
    {
        float fillValue = (float)currentHealth / maxHealth;
        healthBarFill.fillAmount = fillValue;
    }

    void Die()
    {
        Debug.Log(gameObject.name + " Died");
    }
}