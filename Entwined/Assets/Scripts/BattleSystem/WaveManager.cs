using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveManager
{
    [SerializeField] private List<GameObject> enemy;

    public List<GameObject> Enemy => enemy;
}
