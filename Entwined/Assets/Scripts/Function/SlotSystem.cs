using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotSystem : MonoBehaviour
{
    #region Defines
    /// <summary>
    /// Thứ tự của slot.
    /// </summary>
    public int slotNum;

    /// <summary>
    /// Nhân vật hiện tại ở slot.
    /// </summary>
    public GameObject characterInSlot;

    [SerializeField] private Image avatarCharacter;
    private Button button;
    #endregion

    #region Mono Behavours
    private void Awake()
    {
        button = GetComponent<Button>();
        button?.onClick.AddListener(RemoveCharacterInSlot);

        // Slot lấy thông tin nhân vật hiện có trong đội.
        for (int i = 0; i < TeamManager.GetInstance().CharacterInTeam.Count; i++)
        {
            if (TeamManager.GetInstance().CharacterInTeam[i] != null)
            {
                if (i == slotNum)
                    characterInSlot = TeamManager.GetInstance().CharacterInTeam[i];
            }
            else characterInSlot = null;
        }
    }

    private void Update()
    {
        GetAvatarCharacter();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Hiển thị avatar nhân vật trong slot.
    /// </summary>
    void GetAvatarCharacter()
    {
        if (characterInSlot != null)
        {
            avatarCharacter.color = new Color(avatarCharacter.color.r, avatarCharacter.color.g, avatarCharacter.color.b, 1f);
            avatarCharacter.sprite = characterInSlot.GetComponent<CharacterBase>().CharacterIcon;
        }
        else avatarCharacter.color = new Color(avatarCharacter.color.r, avatarCharacter.color.g, avatarCharacter.color.b, 0f);
    }

    /// <summary>
    /// Nếu slot tồn tại nhân vât, sẽ xóa nhân vật đó khi ấn vào.
    /// </summary>
    void RemoveCharacterInSlot()
    {
        if (characterInSlot != null)
        {
            characterInSlot = null;
        }
    }
    #endregion
}
