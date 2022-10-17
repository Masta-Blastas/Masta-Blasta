using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Model To Use")]
    [SerializeField]
    private XRGrabInteractable weapon;
    
    [Header("Projectile")]
    [SerializeField]
    private GameObject projectileToInstantiate;
    
    [Header("VFX")]
    [SerializeField]
    private GameObject muzzleFlash;
    [SerializeField]
    private GameObject pointToFireFrom;


    public void Fire()
    {
        Instantiate(muzzleFlash, pointToFireFrom.transform.position, pointToFireFrom.transform.rotation);
        Instantiate(projectileToInstantiate, weapon.transform.position, weapon.transform.rotation);
    }

}
