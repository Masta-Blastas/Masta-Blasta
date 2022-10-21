using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject rollingBrainSnatcher;

    [SerializeField]
    private List<Transform> spawnPoint;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            Instantiate(rollingBrainSnatcher, spawnPoint[0].position, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(20.0f, 25.0f));
        }
    }
}
