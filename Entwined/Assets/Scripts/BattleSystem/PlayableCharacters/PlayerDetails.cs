using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerDetails : MonoBehaviour
{
    #region Defines
    public int characterLevel;
    public int characterHP;
    public int characterDefend;
    public int characterAttack;
    public double characterCrRate;
    public double characterCrDMG;
    public int characterSpeed;
    public int characterMastery;
    public int characterPercentageHealing;
    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        Initialized();
    }
    #endregion

    #region Methods
    public void Initialized()
    {
        PlayerBase playerBase = GetComponent<PlayerBase>();
        PlayerEquipments playerEquipments = GetComponent<PlayerEquipments>();
        Characters character = GetComponent<Characters>();

        characterLevel = playerBase.Level;
        characterHP = playerBase.BaseHp;
        characterDefend = playerBase.BaseDefend;
        characterAttack = playerBase.BaseAttack;
        characterCrRate = playerBase.BaseCritRate;
        characterCrDMG = playerBase.BaseCritDamage;
        characterSpeed = playerBase.Speed;
        characterMastery = playerBase.BaseMastery;
        characterPercentageHealing = playerBase.BasePercentageHealing;

        if (playerEquipments != null)
        {

            if (playerEquipments.crystal.crystalID != "" && playerEquipments.crystal.PlayerUsedID == character.ID)
            {
                CrystalEffects effect = playerEquipments.crystal.crystalEffect;
                effect.BuffedStat(playerEquipments.crystal);

                characterHP += playerEquipments.crystal.buffedHp;
                characterAttack += playerEquipments.crystal.buffedAttack;
                characterDefend += playerEquipments.crystal.buffedDefend;

                ApplyCrystalBuffed(effect);
            }            
        }

        foreach (var frag in playerEquipments.equipedFragments)
        {
            if (frag.playerUsedID == character.ID)
            {
                ApplyFragmentBuffed(frag.mainStat);
                foreach (var stat in frag.subStats)
                {
                    ApplyFragmentBuffed(stat);
                }
            }
        }
    }

    void ApplyCrystalBuffed(CrystalEffects eff)
    {
        characterHP += Mathf.RoundToInt(characterHP * (float)eff.hpPlus/100);
        characterDefend += Mathf.RoundToInt(characterDefend * (float)eff.defendPlus / 100);
        characterAttack += Mathf.RoundToInt(characterAttack * (float)eff.attackPlus / 100);
        characterCrRate += eff.crRatePlus;
        characterCrDMG += eff.crDMGPlus;
        characterSpeed += Mathf.RoundToInt(characterSpeed * (float)eff.speedPlus / 100);
        characterMastery += Mathf.RoundToInt(characterMastery * (float)eff.masteryPlus / 100);
        characterPercentageHealing += Mathf.RoundToInt(characterPercentageHealing * (float)eff.outGoingHealingPlus / 100);
    }

    void ApplyFragmentBuffed(FragmentStats fs)
    {
        switch (fs.stats)
        {
            case Stats.atk:
                characterAttack += (int)fs.statValue;
                break;
            case Stats.pAtk:
                characterAttack += Mathf.RoundToInt(characterAttack * (float)fs.statValue);
                break;
            case Stats.pDef:
                characterDefend += Mathf.RoundToInt(characterDefend * (float)fs.statValue);
                break;
            case Stats.def:
                characterDefend += (int)fs.statValue;
                break;
            case Stats.pHp:
                characterHP += Mathf.RoundToInt(characterHP * (float)fs.statValue);
                break;
            case Stats.hp:
                characterHP += (int)fs.statValue;
                break;
            case Stats.crRate:
                characterCrRate += fs.statValue * 100;
                break;
            case Stats.crDmg:
                characterCrDMG += fs.statValue * 100;
                break;
            case Stats.mastery:
                characterMastery += (int)fs.statValue;
                break;
            case Stats.healingPlus:
                characterPercentageHealing += (int)fs.statValue * 100;
                break;
        }
    }


    #endregion
}
