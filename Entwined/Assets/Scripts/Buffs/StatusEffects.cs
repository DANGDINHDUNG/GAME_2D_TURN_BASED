using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Hệ thống buff và debuff
/// </summary>
public abstract class StatusEffects
{
    #region Defines
    public enum StatusType
    {
        Buff,
        DoT,
        Debuff
    }
    public string statusName;
    public int statusDuration;
    public int statusValue;
    public int statusStack;
    public StatusType statusType;
    #endregion

    #region Methods
    public abstract void ApplyEffect(GameObject target);
    public abstract void TriggerEffect(GameObject target);
    public abstract void RemoveEffect(GameObject target);
    public abstract void Description(TextMeshProUGUI textComponent);
    public void DecrementDuration()
    {
        if (statusDuration > 0)
        {
            statusDuration--;
        }
    }
    public bool IsExpired()
    {
        return statusDuration <= 0;
    }
    #endregion
}

[System.Serializable]
/// <summary>
/// Các debuff nguyên tố trong trận
/// </summary>
public class ElementStack
{
    #region Defines
    public Element elementType;
    public int stackCount;
    public int stackDuration;
    #endregion

    #region Methods
    public ElementStack(Element element)
    {
        elementType = element;
        stackCount = 0;
        stackDuration = 3;
    }

    public void AddStack(int stack)
    {
        stackCount += stack;
    }

    public void RemoveStack()
    {
        stackCount = 0;
    }

    public void DecrementDuration()
    {
        if (stackDuration > 0)
        {
            stackDuration--;
        }
    }
    public bool IsExpired()
    {
        return stackDuration <= 0;
    }
    #endregion
}

#region Effects
public class RoseBuff : StatusEffects
{
    public RoseBuff(int duration, int value)
    {
        statusName = "Blooming";
        statusDuration = duration;
        statusValue = value;
        statusStack = 1;
        statusType = StatusType.Buff;
    }

    public override void ApplyEffect(GameObject target)
    {
        target.GetComponent<PlayerBattle>().buffedAttack += statusValue;
    }

    public override void RemoveEffect(GameObject target)
    {
        target.GetComponent<PlayerBattle>().buffedAttack -= statusValue;
    }

    public override void Description(TextMeshProUGUI textComponent)
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerEffect(GameObject target)
    {
        throw new NotImplementedException();
    }
}
#endregion

#region Elements
public class FoxBurn : StatusEffects
{
    public FoxBurn(int duration, int value)
    {
        statusName = "Tailing";
        statusDuration = duration;
        statusValue = value;
        statusStack = 1;
        statusType = StatusType.DoT;
    }

    public override void ApplyEffect(GameObject target)
    {
        
    }

    public override void TriggerEffect(GameObject target)
    {
        CharacterBattle targetCb = target.GetComponent<CharacterBattle>();
        if (targetCb.isStartTurn)
        {
            var status = targetCb.ActiveEffects.Find(s => s.statusName == "Tailing");
            if (status != null)
            {
                var element = targetCb.ActiveElements.Find(e => e.elementType == Element.Flare);

                if (element != null)
                {
                    for (int i = 0; i < element.stackCount; i++)
                    {
                        target.GetComponent<CharacterStatus>().TakeDamage(Tuple.Create(statusValue, false));
                    }
                }
            }
            targetCb.isStartTurn = false;
        }
    }

    public override void RemoveEffect(GameObject target)
    {
        
    }

    public override void Description(TextMeshProUGUI textComponent)
    {
        throw new System.NotImplementedException();
    }
}
#endregion


