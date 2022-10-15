using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunChest : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private GameObject spawnEffect;
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private XRSocketInteractor socket;

    public bool canOpen;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canOpen = false;
            CloseChest();
        }    
    }

    public void OpenChest()
    {
        if(canOpen == true)
        {
            canOpen = false;
            anim.SetTrigger("Open");
            spawnEffect.SetActive(true);
            StartCoroutine(SpawnWeapon());
        }
      
    }

    private void CloseChest()
    {
        anim.SetTrigger("Close");
    }

    IEnumerator SpawnWeapon()
    {
        yield return new WaitForSeconds(3.0f);
        Instantiate(weapon, socket.transform.position, Quaternion.identity);
        yield break;

    }
}
