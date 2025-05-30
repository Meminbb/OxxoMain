using UnityEngine;

public class Ritmo_HitEffect : MonoBehaviour
{
    public static Ritmo_HitEffect instance;

    [Header("Contenedor en Canvas para efectos UI")]
    public Transform efectosUI;

    [Header("Prefabs de efectos")]
    public GameObject hitEffect;
    public GameObject goodEffect;
    public GameObject perfectEffect;
    public GameObject missedEffect;

    void Awake()
    {
        instance = this;
    }

    public void SpawnHitEffect(string tipo, Vector3 worldPosition)
{
    if (efectosUI == null || Camera.main == null) return;

    GameObject effectPrefab = null;

    switch (tipo.ToLower())
    {
        case "hit": effectPrefab = hitEffect; break;
        case "good": effectPrefab = goodEffect; break;
        case "perfect": effectPrefab = perfectEffect; break;
        case "missed": effectPrefab = missedEffect; break;
    }

    if (effectPrefab == null) return;

    Vector2 localPoint;
    RectTransform canvasRect = efectosUI as RectTransform;

    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Camera.main.WorldToScreenPoint(worldPosition), Camera.main, out localPoint))
    {
        GameObject fx = Instantiate(effectPrefab, efectosUI);
        fx.GetComponent<RectTransform>().anchoredPosition = localPoint;
        fx.transform.localScale = Vector3.one;
    }
}
}
