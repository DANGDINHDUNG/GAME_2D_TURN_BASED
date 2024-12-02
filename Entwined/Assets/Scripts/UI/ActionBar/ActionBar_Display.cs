using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// Hiển thị Action Bar ra màn hình.
/// </summary>
public class ActionBar_Display : MonoBehaviour
{
    #region Defines
    // Lấy dữ liệu từ UI action bar.
    [SerializeField] private ActionBar_UI[] actionBar_UIs;
    #endregion

    #region UnityAction
    private void OnEnable()
    {
        BattleHandler.OnUpdateActionBar += UpdateActionBar;
    }

    private void OnDisable()
    {
        BattleHandler.OnUpdateActionBar -= UpdateActionBar;
    }
    #endregion

    #region Methods

    /// <summary>
    /// Hiển thị và liên tục thay đổi thứ tự hành động của các nhân vật.
    /// </summary>
    /// <param name="slot"></param>
    private void UpdateActionBar(List<GameObject> slot)
    {
        // Chạy vòng lặp để lấy thông tin participants.
        for(int i = 0; i < actionBar_UIs.Length; i++)
        {
            // Kiểm tra nếu số lượng nhân vật hành động trong khoảng số lượng ô hiện có.
            // Xuất nhân vật đó lên action bar.
            if (i < slot.Count)
            {
                if (actionBar_UIs[i] != null)
                {
                    actionBar_UIs[i].Init(slot[i].GetComponent<CharacterBattle>());
                }
            }
            // Nếu số lượng nhân vật < số lượng ô hiện có.
            // Nhưng ô không có nhân vật sẽ để trống.
            else actionBar_UIs[i].Init(null);
        }
    }
    
    #endregion
}
