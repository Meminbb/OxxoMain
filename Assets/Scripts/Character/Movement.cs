using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public Animator anim;

    private float inputX = 0f;
    private float inputY = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = new Vector2(inputX, inputY).normalized;

        // Animaciones
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
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Estas funciones se llamarán desde los botones (con eventos PointerDown / PointerUp)
    public void OnMoveUpDown(bool isPressed) => inputY = isPressed ? 1 : (inputY == 1 ? 0 : inputY);
    public void OnMoveDownDown(bool isPressed) => inputY = isPressed ? -1 : (inputY == -1 ? 0 : inputY);
    public void OnMoveLeftDown(bool isPressed) => inputX = isPressed ? -1 : (inputX == -1 ? 0 : inputX);
    public void OnMoveRightDown(bool isPressed) => inputX = isPressed ? 1 : (inputX == 1 ? 0 : inputX);
}
