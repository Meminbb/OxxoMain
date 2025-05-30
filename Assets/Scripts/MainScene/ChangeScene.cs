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
        if (Promos != null)
            promosOriginalPos = Promos.transform.localPosition;
    }

    public void pushButton()
    {
        Debug.Log($"Pushed, isCajero: {isCajero}, isTrivia: {isTrivia}, isRitmo: {isRitmo}, isPromos: {isPromos}");

        StopAllCoroutines();

        if (isCajero)
            SceneManager.LoadScene("Cajero");
        else if (isTrivia)
            SceneManager.LoadScene("TriviaScene");
        else if (isRitmo)
            SceneManager.LoadScene("RitmoScene");
        else if (isPromos)
            MovePromos();
    }

    public void ResetPromosPosition()
    {
        StopAllCoroutines();

        if (Promos != null)
        {
            StartCoroutine(MoveToPosition(Promos, promosOriginalPos, moveSpeed, () =>
            {
                if (EventButton != null)
                    EventButton.SetActive(true);
            }));
        }

        if (ResetButton != null)
            ResetButton.SetActive(false);
    }

    private void MovePromos()
    {
        StopAllCoroutines();

        if (Promos == null) return;

        Vector3 targetPos = new Vector3(
            promosOriginalPos.x + targetXOffset,
            promosOriginalPos.y,
            promosOriginalPos.z
        );

        Debug.Log("Moving Promos");

        if (EventButton != null)
            EventButton.SetActive(false);

        StartCoroutine(MoveToPosition(Promos, targetPos, moveSpeed));

        if (ResetButton != null)
            ResetButton.SetActive(true);
    }

    private IEnumerator MoveToPosition(GameObject obj, Vector3 targetPos, float speed, System.Action onComplete = null)
    {
        while (obj != null && Vector3.Distance(obj.transform.localPosition, targetPos) > 0.1f)
        {
            obj.transform.localPosition = Vector3.MoveTowards(
                obj.transform.localPosition,
                targetPos,
                speed * Time.deltaTime
            );
            yield return null;
        }

        if (obj != null)
            obj.transform.localPosition = targetPos;

        if (onComplete != null)
            onComplete.Invoke();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"[OnTriggerStay2D] Tocando con: {collision.gameObject.name}, Tag: {collision.tag}");

        if (Promos != null && Promos.transform.localPosition == promosOriginalPos && EventButton != null)
            EventButton.SetActive(true);

        if (collision.CompareTag("Cajero"))
        {
            isCajero = true;
            Debug.Log("Entró al cajero (isCajero = true)");
        }

        if (collision.CompareTag("Npc"))
        {
            isTrivia = true;
            Debug.Log("Entró a zona de trivia (isTrivia = true)");
        }

        if (collision.CompareTag("Charco"))
        {
            isRitmo = true;
            Debug.Log("Entró a zona de ritmo (isRitmo = true)");
        }

        if (collision.CompareTag("Promos"))
        {
            isPromos = true;
            Debug.Log("Entró a zona de promos (isPromos = true)");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (EventButton != null)
            EventButton.SetActive(false);

        isCajero = false;
        isTrivia = false;
        isRitmo = false;
        isPromos = false;
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
