using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finishMatch;
    [SerializeField] private GameObject fieldData;

    // Update is called once per frame

    /// Nếu vào trận, ẩn những nút ấn vào trận ở mỗi màn.

    private void OnEnable()
    {
        SpawnHandler.OnClearWave += ClearWave;
    }

    private void OnDisable()
    {
        SpawnHandler.OnClearWave -= ClearWave;
    }

    // Hiển thị thông báo kết thúc khi hết wave.
    private void ClearWave()
    {
        //StartCoroutine(ExistTime());
    }
}
