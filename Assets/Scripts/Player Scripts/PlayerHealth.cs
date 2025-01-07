using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    float maxHealth = 100;
    float currentHealth;
    float previousHealth;

    public static PlayerHealth instance { get; private set; } = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning(gameObject.name + " attempted to create a second instance of PlayerHealth, destroyed");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        previousHealth = currentHealth;
    }

    public void applyDamage(float damage)
    {
        if(currentHealth - damage<=0)
        {
            currentHealth = 0;
            playerDefeat();
        }
        else
        {
            currentHealth -= damage;
            Debug.LogWarning(" current health after taking damage: "+ currentHealth);
        }
    }

    void playerDefeat()
    {
        Debug.Log(" handle death");
    }


    public bool didHealthChange()
    {
        bool wasPlayerDamaged = previousHealth != currentHealth;
        previousHealth = currentHealth;
        return wasPlayerDamaged;
    }

    public double getHealth()
    {
        return currentHealth;
    }
}
