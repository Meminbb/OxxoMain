using UnityEngine;
using UnityEngine.SceneManagement;

public class MoverMenu : MonoBehaviour
{
    public GameObject menu; 
    public GameObject fondo;
    public GameObject botonOriginal; 
    public GameObject botonReemplazo;

    public GameObject botoninicio;
    public GameObject botonSalir;
    public void MoverAbajo()
    {
        if (menu != null)
        {

            menu.transform.position = new Vector3(menu.transform.position.x, 0f, menu.transform.position.z);
            fondo.transform.position = new Vector3(menu.transform.position.x, 0f, menu.transform.position.z);
            Time.timeScale = 0f;

            Debug.Log(menu.transform.position);
            Debug.Log(fondo.transform.position);

            if (botonOriginal != null && botonReemplazo != null)
            {
                botonOriginal.SetActive(false);
                botonReemplazo.SetActive(true);

                botonReemplazo.transform.position = botonOriginal.transform.position;
                botonReemplazo.transform.rotation = botonOriginal.transform.rotation;
                botonReemplazo.transform.localScale = botonOriginal.transform.localScale;
            }
        }
    }

    public void MoverArriba()
    {
        if (menu != null)
        {
            menu.transform.position = new Vector3(menu.transform.position.x, 3000f, menu.transform.position.z);
            fondo.transform.position = new Vector3(menu.transform.position.x, 3000f, menu.transform.position.z);
            Time.timeScale = 1f;


            if (botonOriginal != null && botonReemplazo != null)
            {
                botonReemplazo.SetActive(false);
                botonOriginal.SetActive(true);
            }
        }
    }

    public void IrAMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}