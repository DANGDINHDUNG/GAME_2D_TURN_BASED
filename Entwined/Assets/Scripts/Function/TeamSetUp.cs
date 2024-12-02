using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSetUp : MonoBehaviour
{
    public List<SlotSystem> slots;

    [SerializeField] private Button saveButton;

    public static TeamSetUp instance;

    private void Awake()
    {
        instance = this;
        saveButton?.onClick.AddListener(SaveTeam);
    }

    /// <summary>
    /// Thêm nhân vật từ slot vào team.
    /// </summary>
    void SaveTeam()
    {
        TeamManager.GetInstance().CharacterInTeam.Clear();

        foreach (var slot in slots)
        {
            TeamManager.GetInstance().CharacterInTeam.Add(slot.characterInSlot);
        }
    }
}
