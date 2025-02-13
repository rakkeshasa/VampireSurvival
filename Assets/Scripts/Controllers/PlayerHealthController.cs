using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float maxHealth;

    public Slider healthSlider;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
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
        }

        healthSlider.value = currentHealth;
    }
}
