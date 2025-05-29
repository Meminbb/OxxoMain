using UnityEngine;

public class Ritmo_NoteObject : MonoBehaviour
{
    public bool wasPressed = false;
    public string keyToPress;
    public bool canBePressed = false;

    public GameObject hitEffect;       // Efecto de normal
    public GameObject goodEffect;      // Efecto de good
    public GameObject perfectEffect;   // Efecto de perfect
    public GameObject missedEffect;    // Efecto de miss

    private float activadorY = 0f; // Se guarda la posición Y del activador

    public void OnHit()
    {
        if (!canBePressed || wasPressed) return;

        wasPressed = true;
        gameObject.SetActive(false);

        float distancia = Mathf.Abs(transform.position.y - activadorY);
        Vector3 spawnPos = transform.position;
        spawnPos.z = 0f;
        spawnPos.y += 1.5f; // ⬆️ Aumentamos altura para que el efecto no quede tapado

        if (distancia > 0.3f)
        {
            Ritmo_GameManager.instance.NormalHit();
            Debug.Log("Instanciando efecto: NormalHit");
            if (hitEffect != null)
                Instantiate(hitEffect, spawnPos, Quaternion.identity);
        }
        else if (distancia > 0.25f)
        {
            Ritmo_GameManager.instance.GoodHit();
            Debug.Log("Instanciando efecto: GoodHit");
            if (goodEffect != null)
                Instantiate(goodEffect, spawnPos, Quaternion.identity);
        }
        else
        {
            Ritmo_GameManager.instance.PerfectHit();
            Debug.Log("Instanciando efecto: PerfectHit");
            if (perfectEffect != null)
                Instantiate(perfectEffect, spawnPos, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            canBePressed = true;
            activadorY = other.transform.position.y;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            canBePressed = false;

            if (!wasPressed && !Ritmo_GameManager.instance.gameOver)
            {
                Ritmo_GameManager.instance.NoteMissed();

                Vector3 spawnPos = transform.position;
                spawnPos.z = 0f;
                spawnPos.y += 1.5f; // ⬆️ También elevamos el efecto de Missed

                Debug.Log("Instanciando efecto: Missed");
                if (missedEffect != null)
                    Instantiate(missedEffect, spawnPos, Quaternion.identity);

                gameObject.SetActive(false);
            }
        }
    }
}
