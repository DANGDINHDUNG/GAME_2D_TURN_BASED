using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList_Display : MonoBehaviour
{

    #region Defines
    // Lấy dữ liệu từ UI action bar.
    [SerializeField] private GameObject placeHolder;
    [SerializeField] private GameObject listPrefabs;
    [SerializeField] private GameObject characterHolder;
    #endregion

    #region UnityAction
    private void OnEnable()
    {
        UIManager.OnUpdateCharacterList += UpdateCharacterList;
        UIManager.OnRemoveCharacterList += RemoveCharacterList;
    }

    private void OnDisable()
    {
        UIManager.OnUpdateCharacterList -= UpdateCharacterList;
        UIManager.OnRemoveCharacterList -= RemoveCharacterList;
    }
    #endregion

    /// <summary>
    /// Thực hiện load thông tin nhân vật lên Prefab tương ứng trong danh sách.
    /// </summary>
    /// <param name="characters"></param>
    private void UpdateCharacterList(List<GameObject> characters)
    {
        for (int i = 0; i < characters.Count; i++) 
        {
            var characterList = Instantiate(listPrefabs, placeHolder.transform);
            characterList.GetComponent<CharacterList_UI>().Init(characters[i]);
        }
    }

    /// <summary>
    /// Xóa các Prefab về danh sách nhân vật khi chuyển màn hình về Lobby.
    /// </summary>
    private void RemoveCharacterList()
    {
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
