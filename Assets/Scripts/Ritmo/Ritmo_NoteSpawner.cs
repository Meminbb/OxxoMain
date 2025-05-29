using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ritmo_NoteSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject noteLeftPrefab;
    public GameObject noteDownPrefab;
    public GameObject noteUpPrefab;
    public GameObject noteRightPrefab;

    [Header("Spawn Points")]
    public Transform spawnLeft;
    public Transform spawnDown;
    public Transform spawnUp;
    public Transform spawnRight;

    [Header("Other Settings")]
    public Transform noteHolder;
    public float spawnDelay = 1f;

    private float timer = 0f;

    void Update()
{
    if (Ritmo_GameManager.instance.gameOver) return; // ← Esto detiene el spawner si el juego terminó

    timer += Time.deltaTime;

    if (timer >= spawnDelay)
    {
        SpawnRandomNote();
        timer = 0f;
    }
}


    void SpawnRandomNote()
    {
        int rand = Random.Range(0, 4); // genera 0, 1, 2 o 3

        GameObject prefab = null;
        Transform spawnPoint = null;

        switch (rand)
        {
            case 0:
                prefab = noteLeftPrefab;
                spawnPoint = spawnLeft;
                break;
            case 1:
                prefab = noteDownPrefab;
                spawnPoint = spawnDown;
                break;
            case 2:
                prefab = noteUpPrefab;
                spawnPoint = spawnUp;
                break;
            case 3:
                prefab = noteRightPrefab;
                spawnPoint = spawnRight;
                break;
        }

        if (prefab != null && spawnPoint != null)
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity, noteHolder);
        }
    }
}
