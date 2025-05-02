using UnityEngine;
using UnityEngine.SceneManagement;

public class MoverMenu : MonoBehaviour
{
    public GameObject menu; // Asigna el menú en el Inspector
    public GameObject fondo;
    public GameObject botonOriginal; // El botón que activa la acción
    public GameObject botonReemplazo; // El nuevo botón que aparecerá (debe estar desactivado inicialmente)

    public GameObject botoninicio;
    public GameObject botonSalir;
    public void MoverAbajo()
    {
        if (menu != null)
        {
            // Mover el menú
            menu.transform.position = new Vector3(menu.transform.position.x, 750f, menu.transform.position.z);
            fondo.transform.position = new Vector3(menu.transform.position.x, 750f, menu.transform.position.z);
            
            // Reemplazar el botón
            if (botonOriginal != null && botonReemplazo != null)
            {
                botonOriginal.SetActive(false);
                botonReemplazo.SetActive(true);
                
                // Posicionar el nuevo botón en el mismo lugar
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
            // Mover el menú
            menu.transform.position = new Vector3(menu.transform.position.x, 3000f, menu.transform.position.z);
            fondo.transform.position = new Vector3(menu.transform.position.x, 3000f, menu.transform.position.z);

            // Restaurar el botón original
            if (botonOriginal != null && botonReemplazo != null)
            {
                botonReemplazo.SetActive(false);
                botonOriginal.SetActive(true);
            }
        }
    }

    public void IrAMenu()
    {
        SceneManager.LoadScene("Menu");  // Asegúrate de que el nombre coincida con tu escena
    }

    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
}