using UnityEngine;

public class MoveBroom : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 30f; 

    private float rotationZ = 0f; 

    void Update()
    {

        rotationZ -= rotationSpeed * Time.deltaTime;

        if (rotationZ <= -20f || rotationZ >= 20f)
        {
            rotationSpeed = -rotationSpeed;
        }

        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
