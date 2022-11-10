using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    [SerializeField]
    private float secondsToDestroy;

    void Start()
    {
        Destroy(this.gameObject, secondsToDestroy);        
    }
}
