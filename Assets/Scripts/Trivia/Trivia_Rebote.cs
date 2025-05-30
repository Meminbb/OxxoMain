using UnityEngine;

public class Trivia_Rebote : MonoBehaviour
{
    public float speed = 2f;          // velocidad del movimiento
    public float height = 5f;         // altura del rebote en pixeles

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed) * height;
        transform.localPosition = startPos + new Vector3(0f, yOffset, 0f);
    }
    
}
