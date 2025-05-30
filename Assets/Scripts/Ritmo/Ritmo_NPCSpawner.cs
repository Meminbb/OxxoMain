using UnityEngine;

public class Ritmo_NPCSpawner : MonoBehaviour
{
    [Header("Prefab y spawn")]
    public GameObject npcPrefab;
    public Transform puntoSpawn;

    [Header("Ruta de puntos (auto detecta si está vacío)")]
    public Transform[] puntosRuta;

    [Header("Delay entre NPCs")]
    public float tiempoEntreNPCs = 2f; 

    void Start()
    {
        if (puntosRuta == null || puntosRuta.Length == 0)
        {
            Transform puntosPadre = GameObject.Find("Puntos a Seguir")?.transform;
            if (puntosPadre != null)
            {
                puntosRuta = new Transform[puntosPadre.childCount];
                for (int i = 0; i < puntosPadre.childCount; i++)
                {
                    puntosRuta[i] = puntosPadre.GetChild(i);
                }
                Debug.Log("Ruta detectada automáticamente con " + puntosRuta.Length + " puntos.");
            }
            else
            {
                Debug.LogWarning("No se encontró el objeto 'Puntos a Seguir'.");
            }
        }


        SpawnNuevoNPC();
    }


    public void SpawnNuevoNPC()
    {
        Invoke(nameof(GenerarNPC), tiempoEntreNPCs);
    }

    
    private void GenerarNPC()
    {
        GameObject nuevoNPC = Instantiate(npcPrefab, puntoSpawn.position, Quaternion.identity);
        Patrullar patrullar = nuevoNPC.GetComponent<Patrullar>();
        patrullar.puntosMovimiento = puntosRuta;

        Debug.Log("NPC generado tras delay.");
    }
}
