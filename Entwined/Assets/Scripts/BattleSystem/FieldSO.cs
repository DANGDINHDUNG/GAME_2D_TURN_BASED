using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle Field/Field Data")]
public class FieldSO : ScriptableObject
{
    // Danh sách số lượng wave và quái trong mỗi wave.
    public List<WaveManager> waveManagers;
    public GameObject fieldBackground; // GameObject chứa thông tin sân đấu.

}
