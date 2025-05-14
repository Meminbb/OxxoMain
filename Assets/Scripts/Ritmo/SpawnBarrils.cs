using System.Collections;
using UnityEngine;

public class SpawnBarrils : MonoBehaviour
{
    [System.Serializable]
    public class BarrilData
    {
        public GameObject prefab;
        public Transform spawnPoint;
        public Transform endPoint;
    }

    public BarrilData[] barrils = new BarrilData[4];
    public float minSpawnDelay = 0.5f;
    public float maxSpawnDelay = 4f;
    public float moveSpeed = 2f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);

            int index = Random.Range(0, barrils.Length);
            SpawnBarril(index);
        }
    }

    void SpawnBarril(int index)
    {
        BarrilData data = barrils[index];
        GameObject obj = Instantiate(data.prefab, data.spawnPoint.position, Quaternion.identity);
        StartCoroutine(MoveToPoint(obj, data.endPoint.position));
    }

    IEnumerator MoveToPoint(GameObject obj, Vector3 targetPos)
    {
        while (obj != null && Vector3.Distance(obj.transform.position, targetPos) > 0.05f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
