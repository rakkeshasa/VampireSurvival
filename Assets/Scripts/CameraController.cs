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
        // 모든 객체들이 Update된 후
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
