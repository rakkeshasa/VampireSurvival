using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed;
    private SpriteRenderer sprite;

    public Animator anim;
    public float pickupRange = 1.5f;

    public List<Weapon> activeWeapons, inactiveWeapons;
    public int maxWeapon = 3;

    [HideInInspector]
    public List<Weapon> maxLevelWepons = new List<Weapon>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();

        if(activeWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, inactiveWeapons.Count));
        }
        
    }

    void Update()
    {
        Vector3 moveInput = new Vector3(0f, 0f, 0f);
        // Project Setting - Input Manager의 Horizontal
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 대각선 이동시 더 빠르게 이동하는 현상을 수정
        moveInput.Normalize();

        transform.position += moveInput * moveSpeed * Time.deltaTime;

        if (moveInput.x > 0f)
            sprite.flipX = true;
        else if (moveInput.x < 0f)
            sprite.flipX = false;

        if(moveInput != Vector3.zero)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }

    public void AddWeapon(int index)
    {
        // 플레이어 초반에 주어지는 기본 무기용
        if(index < inactiveWeapons.Count)
        {
            activeWeapons.Add(inactiveWeapons[index]);
            inactiveWeapons[index].gameObject.SetActive(true);
            inactiveWeapons.RemoveAt(index);
        }
    }

    public void AddWeapon(Weapon newWeapon)
    {
        // 레벨업 시 장착할 무기용
        newWeapon.gameObject.SetActive(true);
        activeWeapons.Add(newWeapon);
        inactiveWeapons.Remove(newWeapon);
    }
}
