using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notification_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notification;
    public static Notification_UI instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowNotification(string noti, float time)
    {
        StartCoroutine(WaitForDisableNotification(noti, time));
    }

    IEnumerator WaitForDisableNotification(string noti, float time)
    {
        notification.gameObject.SetActive(true);
        notification.text = noti;

        yield return new WaitForSeconds(time);

        notification.gameObject.SetActive(false);
    }
}
