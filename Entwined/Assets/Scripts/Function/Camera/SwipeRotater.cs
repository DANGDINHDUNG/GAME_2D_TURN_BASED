using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRotater : MonoBehaviour
{

    #region Defines
    [SerializeField] private float rotationSpeed = 10.0f; // Adjust this value to control rotation sensitivity
    [SerializeField] private float minYThreshold = -20.0f; // Minimum allowed Y swipe delta (negative for downward swipe)
    [SerializeField] private float maxYThreshold = 20.0f; // Maximum allowed Y swipe delta (positive for upward swipe)
    [SerializeField] private GameObject characterPlaceHolder;
    private Vector3 bottomCorner;
    Vector3 position;
    #endregion

    #region Core MonoBehavior
    // Update is called once per frame
    void Update()
    {
        if (FieldManager.Instance.InBattle)
        {
            SwipeScreen();
            PlaceHolder();
        }
    }
    #endregion

    #region Methods
    // Thực hiện chức năng thay đổi góc Camera khi lướt qua lại màn hình theo chiều ngang.
    public void SwipeScreen()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
        {

            Vector2 delta = Input.touches[0].deltaPosition;
            Quaternion deltaRotation = Quaternion.Euler(0f, -delta.x * rotationSpeed, 0f);
            Quaternion newRotation = transform.rotation * deltaRotation;

            // Convert the quaternion to euler angles
            Vector3 newEulerAngles = newRotation.eulerAngles;

            // Clamp each angle individually
            newEulerAngles.x = 0;
            newEulerAngles.y = ClampAngle(newEulerAngles.y, minYThreshold, maxYThreshold);
            newEulerAngles.z = 0;

            // Assign the clamped euler angles back to transform.rotation
            transform.rotation = Quaternion.Euler(newEulerAngles);
            characterPlaceHolder.transform.rotation = Quaternion.Euler(newEulerAngles);
        }
    }

    // Hàm Clamp không có giá trị âm, sử dụng hàm này để gắn giá trị âm.
    float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360); // keep angle between 0 and 360
        if (angle > 180) angle -= 360; // convert angle to range -180 to 180
        return Mathf.Clamp(angle, min, max);
    }

    // Làm camera follow nhân vật đang hành động.
    void PlaceHolder()
    {
        int index = TeamManager.GetInstance().GetCharacterIndex();
        switch (index)
        {
            case 2:
                position = new Vector3(1.5f, 0.47f, 10f);
                break;
            case 0:
                position = new Vector3(0.86f, 0.47f, 10f);
                break;
            case 1:
                position = new Vector3(0.15f, 0.47f, 10f);
                break;
            case 3:
                position = new Vector3(-0.55f, 0.47f, 10f);
                break;
            default: break;
        }
        // Calculate bottom corner position based on viewport and offset
        bottomCorner = Camera.main.ViewportToWorldPoint(position);
        bottomCorner.x -= 0.01f; // Adjust offset values as needed
        // Update sprite position to stick to the corner
        characterPlaceHolder.transform.position = bottomCorner;
    }
    #endregion

}
