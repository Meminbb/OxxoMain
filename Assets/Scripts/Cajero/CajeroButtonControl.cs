using UnityEngine;
using UnityEngine.SceneManagement;

public class CajeroButtonControl : MonoBehaviour
{
    public GameObject MoneyDrawer;
    public GameObject Promos;
    public GameObject Afirmar;
    public GameObject Cambio;
    public GameObject Exit;
    private Vector3 moneyDrawerOriginalPos;
    private Vector3 cambioOriginalPos;
    private Vector3 promosOriginalPos;
    private Vector3 afirmarOriginalPos; 
    

    private void Start()
    {
        moneyDrawerOriginalPos = MoneyDrawer.transform.localPosition;
        cambioOriginalPos = Cambio.transform.localPosition;
        promosOriginalPos = Promos.transform.localPosition;
        afirmarOriginalPos = Afirmar.transform.localPosition;
    }

    public void MoveDrawerAndCambio()
    {
        Exit.SetActive(true);

        StartCoroutine(MoveToPosition(MoneyDrawer, new Vector3(-872f, moneyDrawerOriginalPos.y, moneyDrawerOriginalPos.z),2000f));
        StartCoroutine(MoveToPosition(Cambio, new Vector3(-2215.2f, cambioOriginalPos.y, cambioOriginalPos.z),1000f));
    }

    public void ResetPos()
    {
        Exit.SetActive(false);
        StopAllCoroutines();

        StartCoroutine(MoveToPosition(Afirmar, afirmarOriginalPos,1000f));
        StartCoroutine(MoveToPosition(MoneyDrawer, moneyDrawerOriginalPos,2000f));
        StartCoroutine(MoveToPosition(Cambio, cambioOriginalPos,1000f));
        StartCoroutine(MoveToPosition(Promos, promosOriginalPos,2000f));
    }

    public void MovePromos()
    {
        Exit.SetActive(true);

        StartCoroutine(MoveToPosition(Afirmar, new Vector3(afirmarOriginalPos.x, -681f, promosOriginalPos.z),1000f));
        StartCoroutine(MoveToPosition(Promos, new Vector3(0, promosOriginalPos.y, promosOriginalPos.z),2000f));
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
    public void Home()
    {
        SceneManager.LoadScene("MainScene");
    }

}

