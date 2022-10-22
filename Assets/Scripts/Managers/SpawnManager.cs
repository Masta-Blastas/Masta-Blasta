using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] rollingBrainSnatcher;

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
            Instantiate(rollingBrainSnatcher[Random.Range(0,rollingBrainSnatcher.Length + 1)], spawnPoint[0].position, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
        }
    }
}
