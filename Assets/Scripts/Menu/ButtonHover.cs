using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    Vector3 origin;
    Vector3 targetPosition;
    Vector3 topPosition;
    public float offset = 10f;
    public float moveSpeed = 5f;
    bool movingUp = true;

    void Start()
    {
        origin = transform.localPosition;

        topPosition = origin + new Vector3(0f, offset, 0f);

        targetPosition = topPosition;
    }

    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, targetPosition) < 0.1f)
        {
            ChooseTarget();
        }
    }

    void ChooseTarget()
    {
        if (movingUp)
        {
            targetPosition = origin;
        }
        else
        {
            targetPosition = topPosition;
        }
        movingUp = !movingUp;
    }
}
