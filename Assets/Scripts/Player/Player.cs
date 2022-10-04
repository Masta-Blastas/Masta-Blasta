using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    [SerializeField]
    private XRGrabInteractable plasmaBlaster;
    public GameObject bullet;
    public GameObject muzzleFlash;
    public GameObject fireFrom;

    public void Fire()
    {
        Instantiate(muzzleFlash,fireFrom.transform.position, fireFrom.transform.rotation);
        Instantiate(bullet, plasmaBlaster.transform.position, plasmaBlaster.transform.rotation);
    }
}
