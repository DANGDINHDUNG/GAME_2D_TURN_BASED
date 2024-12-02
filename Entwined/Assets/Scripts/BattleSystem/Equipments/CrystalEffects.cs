using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class CrystalEffects : MonoBehaviour
{
    #region Defines
    [SerializeField] public string effectName;
    //[SerializeField] protected string skillType;
    protected int skillLevel;

    public int hpPlus = 0;
    public int defendPlus = 0;
    public int attackPlus = 0;
    public int speedPlus = 0;
    public int crRatePlus = 0;
    public int crDMGPlus = 0;
    public int masteryPlus = 0;
    public int outGoingHealingPlus = 0;

    [SerializeField] protected ActionFunction mulEnemies;

    [SerializeField] protected ActionCalculation ac;

    #endregion

    public abstract void BuffedStat(MemoryCrystals crystal);
    public abstract void Effect(GameObject caster, GameObject target);
    public abstract void Description(MemoryCrystals crystal, TextMeshProUGUI textComponent);
}
