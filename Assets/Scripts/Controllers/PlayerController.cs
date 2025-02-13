using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private SpriteRenderer sprite;

    public Animator anim;
    public float pickupRange = 1.5f;
    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
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
}
