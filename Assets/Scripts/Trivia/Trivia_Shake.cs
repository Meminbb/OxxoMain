using UnityEngine;

public class Trivia_Shake : MonoBehaviour
{
    public float duracion = 0.2f;      // Tiempo total de sacudida
    public float intensidad = 10f;     // Qué tanto se mueve de lado a lado

    private Vector3 posicionOriginal;

    void Start()
    {
        posicionOriginal = transform.localPosition;
    }

    public void Sacudir()
    {
        StopAllCoroutines();
        StartCoroutine(SacudirLateral());
    }

    private System.Collections.IEnumerator SacudirLateral()
    {
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            float offsetX = Mathf.Sin(tiempo * 50f) * intensidad;
            transform.localPosition = posicionOriginal + new Vector3(offsetX, 0f, 0f);
            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = posicionOriginal;
    }
}
