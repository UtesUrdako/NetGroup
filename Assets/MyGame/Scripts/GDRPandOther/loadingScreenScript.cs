using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadingScreenScript : MonoBehaviour
{
    [SerializeField]
    GameObject _image;


    private void Update()
    {
        _image.transform.rotation *= Quaternion.Euler(50 * Time.deltaTime, 0, 0);
    }
}
