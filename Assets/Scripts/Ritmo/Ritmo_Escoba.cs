using UnityEngine;

public class Ritmo_Escoba : MonoBehaviour
{
    public float distancia = 0.2f;      // cuánto se mueve a los lados
    public float duracion = 0.1f;       // tiempo por cada movimiento

    private Vector3 posicionOriginal;
    private bool enMovimiento = false;

    void Start()
    {
        posicionOriginal = transform.position;
    }

    public void Limpiar()
    {
        if (!enMovimiento)
            StartCoroutine(MoverEscoba());
    }

    System.Collections.IEnumerator MoverEscoba()
    {
        enMovimiento = true;

        Vector3 destinoIzquierda = posicionOriginal + Vector3.left * distancia;
        Vector3 destinoDerecha = posicionOriginal + Vector3.right * distancia;

        // Mover a la izquierda
        yield return MoverHacia(destinoIzquierda);
        // Mover a la derecha
        yield return MoverHacia(destinoDerecha);
        // Regresar al centro
        yield return MoverHacia(posicionOriginal);

        enMovimiento = false;
    }

    System.Collections.IEnumerator MoverHacia(Vector3 destino)
    {
        Vector3 inicio = transform.position;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            transform.position = Vector3.Lerp(inicio, destino, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.position = destino;
    }
}
