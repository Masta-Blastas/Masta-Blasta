using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    //private void Start()
    //{
    //    Destroy(this.gameObject, 5.0f);
    //}

    void Update()
    {
        transform.Translate(new Vector3(0, 0, 1) * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
