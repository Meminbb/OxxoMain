using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip evento;
    public AudioClip colision;

    public void getCount()
    {
        AudioSource.PlayClipAtPoint(evento, Camera.main.transform.position, 0.5f);
    }

    public void Winning()
    {
        AudioSource.PlayClipAtPoint(colision, Camera.main.transform.position, 0.5f);
    }

}