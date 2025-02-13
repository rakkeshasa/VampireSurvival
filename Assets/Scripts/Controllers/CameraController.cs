using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;


    void Start()
    {
        target = FindAnyObjectByType<PlayerController>().transform;

    }

    void LateUpdate()
    {
        // ��� ��ü���� Update�� ��
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
