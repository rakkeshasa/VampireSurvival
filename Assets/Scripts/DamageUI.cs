using UnityEngine;
using TMPro;
using NUnit.Framework;

public class DamageUI : MonoBehaviour
{
    public TMP_Text DamageText;

    [SerializeField]
    private float LifeTime;
    private float LifeCounter;

    public float floatSpeed = 1f;

    void Update()
    {
        if (LifeCounter > 0)
        {
            LifeCounter -= Time.deltaTime;

            if(LifeCounter <= 0)
            {
                DamageUIController.instance.DespawnDamageUI(this);
            }
        }

        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    public void Setup(int damage)
    {
        LifeCounter = LifeTime;
        DamageText.text = damage.ToString();
    }
}
