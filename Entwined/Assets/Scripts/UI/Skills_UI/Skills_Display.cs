using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills_Display : MonoBehaviour
{
    [SerializeField] private List<Skills_UI> skills;
    [SerializeField] private CharacterDetail_UI characterDetail;

    private void Update()
    {
        PlayerSkills ps = characterDetail.Character.GetComponent<PlayerSkills>();

        for (int i = 0; i < ps.CharacterSkills.Count; i++)
        {
            skills[i].Init(ps.CharacterSkills[i]);
        }
    }
}
