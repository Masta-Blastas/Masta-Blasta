using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCutscene : MonoBehaviour
{
    [SerializeField]
    private GameObject cutSceneToEnable;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("IntroBot"))
        {
            cutSceneToEnable.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
