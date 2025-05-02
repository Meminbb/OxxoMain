using UnityEngine;
using UnityEngine.UIElements;

public class NpcMove : MonoBehaviour
{

    float moveSpeed = 3;
    int side;
    Vector3 pos;
    Vector3 scale;
    public Rigidbody2D rig;

    void Start()
    {
        pos = gameObject.transform.position;
        if (pos.x < 0){
            side = 1;
        } else {
            side = -1;
        }

        moveSpeed = side * moveSpeed;

        scale = transform.localScale;
        scale.x = side * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void Update()
    {
        rig.linearVelocityX = moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish")){
            Destroy(gameObject);
        }
    }

}
