using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    [SerializeField]
    private XRGrabInteractable gun1Grab;
    public GameObject bullet;

    public void Fire()
    {
        Instantiate(bullet, gun1Grab.transform.position, gun1Grab.transform.rotation);
    }
}
