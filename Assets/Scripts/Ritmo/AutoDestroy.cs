using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f); // Se destruye después de 0.5 segundos
    }
}
