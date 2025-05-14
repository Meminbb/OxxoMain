using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene : MonoBehaviour
{
    [Header("Scene & UI References")]
    [SerializeField] GameObject EventButton;
    [SerializeField] GameObject Player;

    [Header("Promos UI Movement")]
    [SerializeField] GameObject Promos;
    [SerializeField] GameObject ResetButton;
    [SerializeField] float moveSpeed = 500f;
    [SerializeField] float targetXOffset = -448.65f;

    private Vector3 promosOriginalPos;

    private bool isCajero = false;
    private bool isTrivia = false;
    private bool isRitmo = false;
    private bool isPromos = false;

    void Start()
    {
        promosOriginalPos = Promos.transform.localPosition;
    }

    public void pushButton()
    {
        Debug.Log("Pushed, isCajero: " + isCajero + ", isPromos: " + isPromos);

        if (isCajero)
            SceneManager.LoadScene("Cajero");
        else if (isTrivia)
            SceneManager.LoadScene("TriviaScene");
        else if (isRitmo)
            SceneManager.LoadScene("RitmoScene");
        else if (isPromos)
        {
            MovePromos();
            Debug.Log("Se Activó Promos");
        }
    }

    public void ResetPromosPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(Promos, promosOriginalPos, moveSpeed, () =>
        {
            EventButton.SetActive(true);  // Mostrar botón cuando vuelve a su posición
        }));
        ResetButton.SetActive(false);
    }

    private void MovePromos()
    {
        StopAllCoroutines();

        Vector3 targetPos = new Vector3(
            promosOriginalPos.x + targetXOffset,
            promosOriginalPos.y,
            promosOriginalPos.z
        );

        Debug.Log("Moving Promos");
        EventButton.SetActive(false);     // Oculta el botón mientras se mueven
        StartCoroutine(MoveToPosition(Promos, targetPos, moveSpeed));
        ResetButton.SetActive(true);
    }

    private IEnumerator MoveToPosition(GameObject obj, Vector3 targetPos, float speed, System.Action onComplete = null)
    {
        while (Vector3.Distance(obj.transform.localPosition, targetPos) > 0.1f)
        {
            obj.transform.localPosition = Vector3.MoveTowards(
                obj.transform.localPosition,
                targetPos,
                speed * Time.deltaTime
            );
            yield return null;
        }

        obj.transform.localPosition = targetPos;

        if (onComplete != null)
            onComplete.Invoke();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Solo mostrar EventButton si las promos están en su posición original
        if (Promos.transform.localPosition == promosOriginalPos)
            EventButton.SetActive(true);

        if (collision.CompareTag("Cajero"))
            isCajero = true;
        if (collision.CompareTag("Npc"))
            isTrivia = true;
        if (collision.CompareTag("Charco"))
            isRitmo = true;
        if (collision.CompareTag("Promos"))
        {
            isPromos = true;
            Debug.Log("Player entered Promos trigger");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        EventButton.SetActive(false);
        isCajero = false;
        isTrivia = false;
        isRitmo = false;
        isPromos = false;
    }
}
