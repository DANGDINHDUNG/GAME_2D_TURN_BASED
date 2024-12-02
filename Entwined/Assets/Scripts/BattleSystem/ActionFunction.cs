using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
///<summary>
/// Các hành động có thể làm trong trận đấu.
/// Lấy thông tin quái bị đánh bởi những kỹ năng đa mục tiêu của nhân vật.
/// Lấy thông tin của một nhân vật được chọn.
///</summary>
public class ActionFunction
{
    /// <summary>
    ///  Danh sách quái bị ảnh hưởng bởi đòn tấn công đa mục tiêu.
    /// </summary>
    [SerializeField] private List<GameObject> multipleEnemies;

    /// <summary>
    /// Khi kĩ năng của người chơi gây sát thương lên 3 quái.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public List<GameObject> GetNearByEnemy(int index)
    {
        GameObject before, after;
        // index = 0
        if (index == 0)
        {
            if (SpawnHandler.instance.enemyOnField.Count > 2)
            {
                before = SpawnHandler.instance.enemyOnField[index + 2];
                multipleEnemies.Add(before);
                after = SpawnHandler.instance.enemyOnField[index + 1];
                multipleEnemies.Add(after);
            }
            else if (SpawnHandler.instance.enemyOnField.Count == 2)
            {
                after = SpawnHandler.instance.enemyOnField[index + 1];
                multipleEnemies.Add(after);
            }
            else if (SpawnHandler.instance.enemyOnField.Count == 1)
            {
                multipleEnemies.Add(null);
            }
        }

        // index = 1
        if (index == SpawnHandler.instance.enemyOnField.Count - 3)
        {
            if (SpawnHandler.instance.enemyOnField.Count == 4)
            {
                before = SpawnHandler.instance.enemyOnField[index - 1];
                multipleEnemies.Add(before);
                after = SpawnHandler.instance.enemyOnField[index + 2];
                multipleEnemies.Add(after);
            }
            else if (SpawnHandler.instance.enemyOnField.Count == 3)
            {
                before = SpawnHandler.instance.enemyOnField[index + 1];
                multipleEnemies.Add(before);
                after = SpawnHandler.instance.enemyOnField[index + 2];
                multipleEnemies.Add(after);
            }
            else if (SpawnHandler.instance.enemyOnField.Count < 3)
            {
                return null;
            }
            else
            {
                before = SpawnHandler.instance.enemyOnField[index - 2];
                multipleEnemies.Add(before);
                after = SpawnHandler.instance.enemyOnField[index + 2];
                multipleEnemies.Add(after);
            }
        }

        // index = 2
        if (index == SpawnHandler.instance.enemyOnField.Count - 2)
        {
            if (SpawnHandler.instance.enemyOnField.Count <= 2)
            {
                after = SpawnHandler.instance.enemyOnField[index + 1];
                multipleEnemies.Add(after);
            }
            else if (SpawnHandler.instance.enemyOnField.Count == 3)
            {
                before = SpawnHandler.instance.enemyOnField[index - 1];
                multipleEnemies.Add(before);
            }
            else
            {
                before = SpawnHandler.instance.enemyOnField[index - 2];
                multipleEnemies.Add(before);
            }   
        }

        // index = 3
        if (index == SpawnHandler.instance.enemyOnField.Count - 1)
        {
            if (SpawnHandler.instance.enemyOnField.Count == 2)
            {
                before = SpawnHandler.instance.enemyOnField[index - 1];
                multipleEnemies.Add(before);
            }
            else if (SpawnHandler.instance.enemyOnField.Count == 1)
            {
                return null;
            }
            else
            {
                before = SpawnHandler.instance.enemyOnField[index - 2];
                multipleEnemies.Add(before);
            }
        }

        

        return multipleEnemies;
    }

    /// <summary>
    /// Khi kĩ năng của người chơi gây sát thương lên toàn bộ quái.
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetAllEnemy()
    {
        foreach (GameObject enemy in SpawnHandler.instance.enemyOnField)
        {
            multipleEnemies.Add(enemy);
        }

        return multipleEnemies;
    }

    /// <summary>
    /// Làm mới danh sách quái bị ảnh hưởng bởi kĩ năng.
    /// </summary>
    public void ResetMultipleEnemiesList()
    {
        multipleEnemies.Clear();
    }

    /// <summary>
    /// Lấy nhân vật có máu thấp nhất trong team.
    /// </summary>
    /// <returns></returns>
    public GameObject GetCharacterWithLowestHp()
    {
        GameObject lc = SpawnHandler.instance.characterOnField[0];

        foreach (var c in SpawnHandler.instance.characterOnField)
        {
            if (c.GetComponent<PlayerStatus>().currentHp < lc.GetComponent<PlayerStatus>().currentHp)
            {
                lc = c;
            }
        }

        return lc;
    }
}
