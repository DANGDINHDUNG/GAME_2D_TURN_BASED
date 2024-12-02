using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ObjectsListManager : MonoBehaviour
{
    #region Defines
    public List<GameObject> allCharacters;

    public static ObjectsListManager instance;
    #endregion

    #region Core MonoBehaviors
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Danh sách toàn bộ nhân vật trong game
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetAllCharacters()
    {
        return allCharacters;
    }
    #endregion
}
