using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 targetPosition;
    private Vector3 topPosition;
    
    public float offset = 10f;
    public float moveSpeed = 5f;
    
    private bool movingUp = true;

    void Start()
    {
        // Guarda la posición inicial local
        origin = transform.localPosition;

        // Calcula la posición superior con el offset
        topPosition = origin + new Vector3(0f, offset, 0f);

        // Comienza yendo hacia arriba
        targetPosition = topPosition;
    }

    void Update()
    {
        // Movimiento suave
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        // Si ya llegó cerca del objetivo, cambiar dirección
        if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
        {
            SwitchDirection();
        }
    }

    void SwitchDirection()
    {
        if (movingUp)
            targetPosition = origin;
        else
            targetPosition = topPosition;

        movingUp = !movingUp;
    }
}
