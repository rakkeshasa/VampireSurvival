using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public float currentHealth;
    public float maxHealth;

    public Slider healthSlider;
    public GameObject deathEffect;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        maxHealth = PlayerStatController.instance.health[0].value;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

    }

    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10f);
        }*/
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            // setactive와 destroy의 차이?
            gameObject.SetActive(false);
            LevelManager.Instance.EndLevel();
            Instantiate(deathEffect, transform.position, transform.rotation);
            SFXManager.instance.PlaySFX(3);
        }

        healthSlider.value = currentHealth;
    }
}
