using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipments : MonoBehaviour
{
    #region Defines
    public MemoryCrystals crystal;

    public Fragments essenceFrag;
    public Fragments vialityFrag;
    public Fragments clarityFrag;
    public Fragments resonanceFrag;

    public List<Fragments> equipedFragments;
    #endregion

    #region Methods
    /// <summary>
    /// Lấy fragment theo type.
    /// </summary>
    public Fragments GetFragmentWithType(FragmentType type)
    {
        if (type == FragmentType.Essence)
        {
            return essenceFrag;
        }
        else if (type == FragmentType.Vitality)
        {
            return vialityFrag;
        }
        else if (type == FragmentType.Clarity)
        {
            return clarityFrag;
        }
        else if (type == FragmentType.Resonance)
        {
            return resonanceFrag;
        }

        return null;
    }

    public void SetFragmentWithType(FragmentType type, Fragments fragment)
    {
        if (type == FragmentType.Essence)
        {
            essenceFrag = fragment;
        }
        else if (type == FragmentType.Vitality)
        {
            vialityFrag = fragment;
        }
        else if (type == FragmentType.Clarity)
        {
            clarityFrag = fragment;
        }
        else if (type == FragmentType.Resonance)
        {
            resonanceFrag = fragment;
        }

        AddFragmentToList(fragment);
    }

    /// <summary>
    /// Thêm Fragment vào danh sách đã trang bị.
    /// </summary>
    /// <param name="frag"></param>
    void AddFragmentToList(Fragments frag)
    {
        var eFr = equipedFragments.FirstOrDefault(fr => fr.fragmentType == frag.fragmentType);

        if (eFr == null)
        {
            equipedFragments.Add(frag);
        }
        else
        {
            for (int i = 0; i < equipedFragments.Count; i++)
            {
                if (equipedFragments[i].fragmentType == frag.fragmentType)
                {
                    equipedFragments[i] = frag;
                }
            }
        }
    }

    /// <summary>
    /// Gỡ Fragment ra khỏi trang bị.
    /// </summary>
    /// <param name="fragment"></param>
    public void RemoveFragment(Fragments fragment)
    {
        equipedFragments.Remove(fragment);
    }
    #endregion
}
