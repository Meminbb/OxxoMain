using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Entrada del jugador
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normaliza para que no sea más rápido en diagonal
        movement.Normalize();

        // Actualiza parámetros del Animator
        anim.SetFloat("InputX", movement.x);
        anim.SetFloat("InputY", movement.y);

        if (movement != Vector2.zero)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("LastInputX", movement.x);
            anim.SetFloat("LastInputY", movement.y);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void FixedUpdate()
    {
        // Movimiento del personaje
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
