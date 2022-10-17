using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab; 

    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void LetGo()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(2.0f);
        Explode();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.name == "Ground")
        {
            LetGo();
        }
    }
}
