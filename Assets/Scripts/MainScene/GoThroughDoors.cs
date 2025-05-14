using UnityEngine;

public class GoThroughDoors : MonoBehaviour
{
    public GameObject Destination;
    public GameObject Player;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player.transform.localPosition = Destination.transform.localPosition;
    }
}
