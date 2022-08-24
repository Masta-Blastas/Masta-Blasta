using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    public bool holdingGun = false;
    private XRGrabInteractable gun1Grab;
    public GameObject bullet;


    // Start is called before the first frame update
    void Start()
    {
        gun1Grab = GameObject.Find("TestGun").GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    public void SelectedGun ()
    {
        Debug.Log("Grabbed it");
        holdingGun = true;
    }

    public void DroppedGun()
    {
        holdingGun = false;
    }

    public void Fire()
    {
        Instantiate(bullet, gun1Grab.transform.position, gun1Grab.transform.rotation);
    }
}
