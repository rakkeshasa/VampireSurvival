using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public Animator anim;
    void Start()
    {
        
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
