using System.Collections.Generic;

[System.Serializable]
public class Producto
{
    public int IdProducto;
    public string Nombre;
    public string Img;
    public float PrecioOriginal;
    public float PrecioFinal;
    public bool PromoAplicada;
}

[System.Serializable]
public class Promocion
{
    public int IdPromocion;
    public string Promo;
    public float Multiplicador;
    public int IdProductoPromo;
    public string ImgPromo;
    public string ImgProductoPromo;
}

[System.Serializable]
public class PedidoResponse
{
    public Promocion Promocion;
    public List<Promocion> TodasLasPromociones;
    public List<Producto> Productos;
    public float PrecioTotalFinal;
}
