using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadingVerdure_Effect : CrystalEffects
{
    public override void BuffedStat(MemoryCrystals crystal)
    {
        crRatePlus = 10 + (2 * crystal.refinementRank);
    }

    public override void Description(MemoryCrystals crystal, TextMeshProUGUI textComponent)
    {
        // Gán giá trị văn bản cho đối tượng TextMeshPro
        textComponent.SetText("Increases the wearer's crit rate by <color=#00FF00>{0}</color>%. When the wearer casts a " +
            "combat skill or ultimate skill, it will ignore <color=#00FF00>{1}</color>% of the target's defend.", crRatePlus, 27);
    }

    public override void Effect(GameObject caster, GameObject target)
    {
        //target.GetComponent<CharacterStatus>().TakeDamage(ac.EnhanceDamageCalculation(caster, target, 0));
    }
}
