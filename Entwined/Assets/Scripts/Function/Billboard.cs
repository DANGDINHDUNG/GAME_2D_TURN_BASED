using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Khiến Sprite luôn nhìn về phía Camera
/// </summary>
public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] bool freezeXZAxis = true;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (freezeXZAxis)
        {
            transform.rotation = Quaternion.Euler(0f, mainCamera.transform.rotation.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
