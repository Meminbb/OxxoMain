using System.Collections;
using UnityEngine;

public class SpawnNPC : MonoBehaviour
{
    public float seconds;
    public GameObject[] points;
    public GameObject[] NPCs;

    void Start()
    {
        Spawn();
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);

            int spawnChance = Random.Range(1, 4);
            if (spawnChance == 1)
            {
                StartCoroutine(spawnTwo());
            } else if (spawnChance == 2){
                Spawn();
            }
        }
    }

    IEnumerator spawnTwo(){
        Spawn();
        yield return new WaitForSeconds(2);
        Spawn();
    }

    void Spawn()
    {
        int randomNPC = Random.Range(0, NPCs.Length);
        int randomSpawn = Random.Range(0, points.Length);

        Instantiate(NPCs[randomNPC], points[randomSpawn].transform.position, Quaternion.identity);
    }
}
