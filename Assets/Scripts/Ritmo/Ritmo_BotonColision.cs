using UnityEngine;
using UnityEngine.EventSystems;
public class Ritmo_BotonColision : MonoBehaviour
{
    public string columna;

    //  Este método lo detectará Unity en el OnClick del botón UI
    public void AlPresionar()
    {
        Ritmo_GameManager.instance.PresionarBoton(columna);
    }

    //  Este método sigue funcionando si haces clic directamente sobre el objeto en el mundo (con Collider)
    public void OnPointerClick(PointerEventData eventData)
    {
        AlPresionar(); // Reutiliza el mismo método
    }
}
