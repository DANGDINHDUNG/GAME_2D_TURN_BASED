using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quản lý ID và kiếm tra nhân vật đã được sở hữu chưa.
/// </summary>
public class Characters : MonoBehaviour
{
    #region Defines
    public Rarity rare;
    [SerializeField] private string id = Guid.NewGuid().ToString();     // Tạo Id cho nhân vật.
    #endregion

    #region Properties
    public string ID => id;
    #endregion

    #region Methods

    //This allows you to re-generate the GUID for this object by clicking the 'Generate New ID' button in the context menu dropdown for this script
    public void GenerateID()
    {
        // Tạo Id khi đối tượng được tạo mới nếu chưa có
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
        }
    }
    #endregion
}

public enum Rarity
{
    Mythic,
    Legend,
    Epic
}
