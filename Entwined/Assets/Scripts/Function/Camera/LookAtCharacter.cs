using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCharacter : MonoBehaviour
{
    private static LookAtCharacter instance;

    public static LookAtCharacter GetInstance() { return instance; }

    private void Awake()
    {
        instance = this;
    }


    // Tham số instance lấy Transform hiện tại của Camera.
    public Transform GetTransform() { return this.transform; }

    // Thay đổi Rotation của Camera.
    public void SetRotation(Vector3 eulerAngle) { this.transform.localEulerAngles = eulerAngle; }

    // Thực hiện trỏ Camera vào nhân vật đang tới lượt hành động.
    public void FollowCharacter(Transform tf)
    {

        this.transform.LookAt(tf);
    }
}
