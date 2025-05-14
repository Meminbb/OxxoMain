using UnityEngine;

public class ShowPromos : MonoBehaviour
{
    public GameObject Button;
    public GameObject Promos;
    public GameObject Reset;
    private Vector3 promosOriginalPos;

    public float moveSpeed = 500f;
    public float targetXOffset = -448.65f;

    void Start()
    {
        promosOriginalPos = Promos.transform.localPosition;
    }

    public void OnPromoClick()
    {
        StopAllCoroutines();    
        MovePromos();
        Reset.SetActive(true);
    } 

    public void ResetPos()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(Promos, promosOriginalPos, moveSpeed));
        Reset.SetActive(false);
    }

    public void MovePromos()
    {
        Vector3 targetPos = new Vector3(promosOriginalPos.x + targetXOffset, promosOriginalPos.y, promosOriginalPos.z);
        StartCoroutine(MoveToPosition(Promos, targetPos, moveSpeed));
    }

    private System.Collections.IEnumerator MoveToPosition(GameObject obj, Vector3 targetPos, float speed)
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
    }
}
