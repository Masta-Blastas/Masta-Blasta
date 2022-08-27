using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    [SerializeField]
    private XRGrabInteractable plasmaBlaster;
    public GameObject bullet;

    public void Fire()
    {
        Instantiate(bullet, plasmaBlaster.transform.position, plasmaBlaster.transform.rotation);
    }
}
