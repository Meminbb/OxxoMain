using UnityEngine;
using UnityEngine.EventSystems;

public class BotonMovimientoTouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Direccion { Arriba, Abajo, Izquierda, Derecha }
    public Direccion direccion;
    public Movement movimientoJugador;

    public void OnPointerDown(PointerEventData eventData)
    {
        LlamarMovimiento(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        LlamarMovimiento(false);
    }

    private void LlamarMovimiento(bool presionado)
    {
        switch (direccion)
        {
            case Direccion.Arriba:
                movimientoJugador.OnMoveUpDown(presionado);
                break;
            case Direccion.Abajo:
                movimientoJugador.OnMoveDownDown(presionado);
                break;
            case Direccion.Izquierda:
                movimientoJugador.OnMoveLeftDown(presionado);
                break;
            case Direccion.Derecha:
                movimientoJugador.OnMoveRightDown(presionado);
                break;
        }
    }
}
