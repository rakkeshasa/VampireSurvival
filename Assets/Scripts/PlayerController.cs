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
        // Project Setting - Input Manager�� Horizontal
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // �밢�� �̵��� �� ������ �̵��ϴ� ������ ����
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
