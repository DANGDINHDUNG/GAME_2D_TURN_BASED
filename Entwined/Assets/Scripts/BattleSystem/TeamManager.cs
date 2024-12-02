using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    #region Defines
    // Danh sách nhân vật đang ở trong team hiện tại.
    [SerializeField] private List<GameObject> characterInTeam;

    private static TeamManager instance;
    // Lấy thông tin nhân vật đang tới lượt hành động.
    [SerializeField] public GameObject characterInAction;

    // Khai báo danh sách các nhân vật trong team ra Public để class khác sử dụng.
    public List<GameObject> CharacterInTeam => characterInTeam;
    int index; // Số thứ tự của nhân vật trong team

    /// <summary>
    /// Danh sách nhân vật người chơi sở hữu.
    /// </summary>
    public List<GameObject> obtainedCharacters;

    /// <summary>
    /// Danh sách crystal người chơi sở hữu
    /// </summary>
    public List<MemoryCrystals> obtainedCrystals;

    /// <summary>
    /// Danh sách fragment mà người chơi sở hữu.
    /// </summary>
    public List<Fragments> obtainedFragments;

    public static TeamManager GetInstance() { return instance; }
    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        instance = this;
    }

    #endregion

    #region Methods

    // Hàm lấy số thứ tự của nhân vật hiện đang hành động trong team.
    public int GetCharacterIndex()
    {

        StartCoroutine(WaitForCharacterIndex());
        return index;
    }

    IEnumerator WaitForCharacterIndex()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < characterInTeam.Count; i++)
        {
            if (characterInAction.GetComponent<Characters>().ID == characterInTeam[i].GetComponent<Characters>().ID)
            {
                index = i;
            }
        }
    }
    #endregion
}
