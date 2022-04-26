using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class someGDRPscripts : MonoBehaviour
{
    public GameObject whatToactive;

    public void DisableEnable()
    {
        whatToactive.SetActive(true);
        transform.gameObject.SetActive(false);

    }

}
