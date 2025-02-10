using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField]
    private Transform sprite;

    public float speed;
    public float minSize, maxSize;
    private float activeSize;
    void Start()
    {
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform transform = gameObject.transform.GetChild(i);
            if (transform.name == "Sprite")
            {
                sprite = gameObject.transform.GetChild(i);
                break;
            }
        }

        activeSize = maxSize;
        speed = speed * Random.Range(.75f, 1.25f);
    }

    void Update()
    {
        sprite.localScale = Vector3.MoveTowards(sprite.localScale, Vector3.one * activeSize, speed * Time.deltaTime);

        if (sprite.localScale.x == activeSize)
        {
            if (activeSize == maxSize)
            {
                activeSize = minSize;
            }
            else
            {
                activeSize = maxSize;
            }
        }
    }
}
