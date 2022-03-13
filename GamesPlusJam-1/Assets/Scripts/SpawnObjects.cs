using System.Collections;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private GameObject rogueObject;
    [SerializeField] private float spawnAfterTime;

    [SerializeField] private float maxTimer;
    [SerializeField] private float currentTimer;

    public int randomPoint;

    private void Awake()
    {
        currentTimer = maxTimer;
    }

    private void Update()
    {
        currentTimer -= Time.deltaTime;

        if(currentTimer <= 0)
        {
            StartCoroutine(ObjectSpawner());

            currentTimer = maxTimer;
        }
    }

    public void SpawnRogueObject()
    {
        Instantiate(rogueObject, spawnPoints[randomPoint].position, transform.rotation, spawnPoints[randomPoint]);
    }

    public void RemoveAllObjects()
    {
        StopCoroutine(ObjectSpawner());

        AudioManager.instance.Play("Remove All Objects");

        foreach (Transform obj in spawnPoints)
        {
            if(obj.childCount > 0)
            {
                Destroy(obj.GetChild(0).gameObject);
            }
        }
    }

    private void PickNewLocation()
    {
        randomPoint = Random.Range(0, spawnPoints.Length);
    }

    IEnumerator ObjectSpawner()
    {
        PickNewLocation();

        yield return new WaitForSeconds(spawnAfterTime);

        if (spawnPoints[randomPoint].childCount > 1)
        {
            Debug.Log("Spawn Location " + spawnPoints[randomPoint] + " has already got an object");
            yield return null;
        } 
        else
        {
            GameObject obj = Instantiate(objectToSpawn, spawnPoints[randomPoint].position, transform.rotation, spawnPoints[randomPoint]);
        }
    }
}
