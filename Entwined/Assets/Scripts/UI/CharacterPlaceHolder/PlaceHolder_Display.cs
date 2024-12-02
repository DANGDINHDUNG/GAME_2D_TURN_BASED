using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder_Display : MonoBehaviour
{
    #region Defines
    // Lấy dữ liệu từ UI action bar.
    [SerializeField] private PlaceHolder_UI[] placeHolder_UIs;
    #endregion

    #region UnityAction
    private void OnEnable()
    {
        BattleHandler.OnSetCharacterOnField += UpdatePlaceHolder;
    }

    private void OnDisable()
    {
        BattleHandler.OnSetCharacterOnField -= UpdatePlaceHolder;
    }
    #endregion

    #region Methods

    /// <summary>
    /// Hiển thị nhân vật đang có trong trận lên PlaceHolder.
    /// </summary>
    /// <param name="slot"></param>
    private void UpdatePlaceHolder(List<GameObject> slot)
    {
        // Chạy vòng lặp để lấy thông tin participants.
        for (int i = 0; i < placeHolder_UIs.Length; i++)
        {
            // Kiểm tra nếu số lượng nhân vật hành động trong khoảng số lượng ô hiện có.
            // Xuất nhân vật đó lên placeHolder.
            if (i < slot.Count)
            {
                slot[i].GetComponent<PlayerStatus>().FloatingHealthBar = placeHolder_UIs[i].floatingHealthBar;
                slot[i].GetComponent<PlayerStatus>().FloatingShieldBar = placeHolder_UIs[i].floatingShieldBar;
                placeHolder_UIs[i].Init(slot[i].GetComponent<PlayerStatus>());
            }
            // Nếu số lượng nhân vật < số lượng ô hiện có.
            // Nhưng ô không có nhân vật sẽ để trống.
            else placeHolder_UIs[i].Init(null);
        }
    }

    #endregion
}
