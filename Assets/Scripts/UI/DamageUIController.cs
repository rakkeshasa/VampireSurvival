using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DamageUIController : MonoBehaviour
{
    public static DamageUIController instance;

    private void Awake()
    {
        instance = this;
    }

    public DamageUI damageUI;
    public Transform canvas;

    private List<DamageUI> DamagePool = new List<DamageUI>();

    void Start()
    {
        
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.U))
            SetDamageUI(57f, new Vector3(4, 3, 0));*/
    }

    public void SetDamageUI(float damage, Vector3 location)
    {
        int rounded = Mathf.RoundToInt(damage);

        // DamageUI newUI = Instantiate(damageUI, location, Quaternion.identity, canvas);
        DamageUI newUI = SpawnDamageUI();
        newUI.Setup(rounded);
        newUI.gameObject.SetActive(true);
        newUI.transform.position = location;

    }

    public DamageUI SpawnDamageUI()
    {
        DamageUI OutputUI = null;

        if(DamagePool.Count == 0)
        {
            OutputUI = Instantiate(damageUI, canvas);
        }
        else
        {
            OutputUI = DamagePool[0];
            DamagePool.RemoveAt(0);
        }

        return OutputUI;
    }

    public void DespawnDamageUI(DamageUI despawnUI)
    {
        despawnUI.gameObject.SetActive(false);
        DamagePool.Add(despawnUI);
    }
}
