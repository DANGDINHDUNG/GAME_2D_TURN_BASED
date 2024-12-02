using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crystals_Display : MonoBehaviour
{
    #region Defines
    // Lấy dữ liệu từ UI action bar.
    [SerializeField] private GameObject placeHolder;
    [SerializeField] private GameObject listPrefabs;
    [SerializeField] private CharacterDetail_UI characterDetail;
    private string playerId;
    #endregion

    #region UnityAction
    private void OnEnable()
    {
        UIManager.OnUpdateCrystalList += UpdateCrystalList;
        UIManager.OnRemoveCrystalList += RemoveCrystalList;
    }

    private void OnDisable()
    {
        UIManager.OnUpdateCrystalList -= UpdateCrystalList;
        UIManager.OnRemoveCrystalList -= RemoveCrystalList;
    }
    #endregion

    /// <summary>
    /// Thực hiện load thông tin nhân vật lên Prefab tương ứng trong danh sách.
    /// </summary>
    /// <param name="crystals"></param>
    private void UpdateCrystalList(List<MemoryCrystals> crystals)
    {
        playerId = characterDetail.Character.GetComponent<Characters>().ID;

        // Lọc danh sách crystal theo tên, ở đầu luôn là crystal được trang bị
        List<MemoryCrystals> crList = crystals
            .OrderByDescending(x => x.PlayerUsedID == playerId) 
            .ThenByDescending(x => x.PlayerUsedID != "" &&
                           x.PlayerUsedID != playerId) 
            .ThenByDescending(x => x.crystalPurify == Purity.Eternal) 
            .ThenBy(x => x.PlayerUsedID == "") 
            .ThenBy(x => x.crystalName) // Finally, sort by crystal name
            .ToList();

        for (int i = 0; i < crList.Count; i++)
        {
            var crystalList = Instantiate(listPrefabs, placeHolder.transform);
            crystalList.GetComponent<Crystals_UI>().Init(crList[i]);
        }

        if (crList.Count > 0)
        {
            this.gameObject.GetComponent<CrystalDetail_UI>().Init(crList[0]);
        }
    }

    /// <summary>
    /// Xóa các Prefab về danh sách nhân vật khi chuyển màn hình về Lobby.
    /// </summary>
    private void RemoveCrystalList()
    {
        playerId = "";
        this.gameObject.GetComponent<CrystalDetail_UI>().ClearSlot();
        Transform[] list = placeHolder.GetComponentsInChildren<Transform>();

        foreach (Transform child in list)
        {
            if (child.gameObject.name != placeHolder.name)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
