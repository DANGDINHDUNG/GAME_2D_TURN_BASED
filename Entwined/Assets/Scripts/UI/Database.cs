using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Hiển thị icon tương ứng với hệ, class, type tương ứng của nhân vật.
/// </summary>
public class Database : MonoBehaviour
{
    #region Defines
    public static Database Instance;
    [SerializeField] private List<Sprite> elementIcons;
    [SerializeField] public List<Sprite> crystalIcons;
    [SerializeField] public List<CrystalEffects> crystalEffects;
    [SerializeField] public List<FragmentSprites> fragmentSprites;
    [SerializeField] public List<MaterialsSO> materials;
    #endregion

    #region Singleton
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Thay đổi Icon nguyên tố dựa trên nguyên tố của nhân vật.
    /// </summary>
    /// <param name="characterElement"></param>
    public Sprite UpdateElementIcons(Element characterElement)
    {
        Sprite elementIcon = null;
        switch (characterElement)
        {
            case Element.Gaia:
                elementIcon = elementIcons[0];
                break;
            case Element.Tempest:
                elementIcon = elementIcons[1];
                break;
            case Element.Flare:
                elementIcon = elementIcons[2];
                break;
            case Element.Void:
                elementIcon = elementIcons[3];
                break;
            case Element.Zephyr:
                elementIcon = elementIcons[4];
                break;
            case Element.Lumine:
                elementIcon = elementIcons[5];
                break;
            case Element.Frost:
                elementIcon = elementIcons[6];
                break;
        }

        return elementIcon;
    }

    /// <summary>
    /// Đổi màu chữ dựa trên hệ của nhân vật.
    /// </summary>
    /// <param name="characterElement"></param>
    //void ElementDisplay(string characterElement)
    //{
    //    element.text = characterElement;

    //    if (characterElement == "Flare") element.color = new Color(0.9f, 0f, 0.05f);
    //    else if (characterElement == "Void") element.color = new Color(0.04f, 0.02f, 0.13f);
    //    else if (characterElement == "Zephyr") element.color = new Color(0.08f, 0.55f, 0.08f);
    //    else if (characterElement == "Tempest") element.color = new Color(0.6f, 0.07f, 0.65f);
    //    else if (characterElement == "Lumine") element.color = Color.yellow;
    //    else if (characterElement == "Frost") element.color = new Color(0.0518868f, 8913329f, 1f);
    //    else if (characterElement == "Gaia") element.color = new Color(0.56f, 0.28f, 0f);

    //}


    #endregion
}

/// <summary>
/// Sprite của fragment tương ứng với set và type.
/// </summary>
[System.Serializable]
public class FragmentSprites
{
    public FragmentSets set;
    public FragmentType type;
    public Sprite sprite;
}
