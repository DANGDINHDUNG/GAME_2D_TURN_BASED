using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

public class GachaSystem : MonoBehaviour
{
    #region Defines
    public List<GameObject> allCharactersLegend; // Danh sách nhân vật SR có thể roll
    public List<GameObject> allCharactersMythic; // Danh sách nhân vật SSR có thể roll

    public int unityPoints; // Điểm Unity khi roll ra nhân vật đã sở hữu
    [SerializeField] private GameObject resultPrefab;
    [SerializeField] private Transform resultHolder;
    [SerializeField] private Transform gachaCharactersHolder;

    private const float SR_RATE = 0.96f; // Tỷ lệ SR 96%
    private const float SSR_RATE = 0.04f; // Tỷ lệ SSR 4%
    #endregion

    #region Core MonoBehaviours
    private void Start()
    {
        foreach (GameObject character in ObjectsListManager.instance.allCharacters)
        {
            if (character.GetComponent<Characters>().rare == Rarity.Mythic)
            {
                allCharactersMythic.Add(character);
            }
            else if (character.GetComponent<Characters>().rare == Rarity.Legend)
            {
                allCharactersLegend.Add(character);
            }
        }    
    }
    #endregion

    #region Methods
    // Hàm roll x1
    public void Rollx1()
    {
        GameObject rolledCharacter = RollCharacter();
        
        AddCharacterToOwned(rolledCharacter);
    }

    // Hàm roll x10
    public void Rollx10()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject rolledCharacter = RollCharacter();
            AddCharacterToOwned(rolledCharacter);
        }
    }

    // Hàm roll x1
    public void RollCrystalx1()
    {
        MemoryCrystals rolledCrystal = RollCrystal();
        
        AddCrystalToOwned(rolledCrystal);
    }

    // Hàm roll x10
    public void RollCrystalx10()
    {
        for (int i = 0; i < 10; i++)
        {
            MemoryCrystals rolledCrystal = RollCrystal();
            AddCrystalToOwned(rolledCrystal);
        }
    }

    /// <summary>
    /// Hàm roll nhân vật dựa trên tỷ lệ SR/SSR
    /// </summary>
    /// <returns></returns>
    private GameObject RollCharacter()
    {
        float roll = Random.Range(0f, 1f);
        GameObject rolledCharacter;

        // Roll SR nếu tỷ lệ roll nhỏ hơn SR_RATE, ngược lại roll SSR
        if (roll < SR_RATE)
        {
            rolledCharacter = Instantiate(GetRandomObjectFromList(allCharactersLegend), gachaCharactersHolder);
        }
        else
        {
            rolledCharacter = Instantiate(GetRandomObjectFromList(allCharactersMythic), gachaCharactersHolder);
        }

        rolledCharacter.SetActive(false);

        var resultUI = Instantiate(resultPrefab, resultHolder);
        resultUI.transform.GetChild(2).GetComponent<Image>().sprite = rolledCharacter.GetComponent<PlayerBase>().CharacterListAvatar;
        ShowSSREffect(resultUI, rolledCharacter, null, 1);

        return rolledCharacter;
    }

    /// <summary>
    /// Hàm roll crystal dựa trên tỷ lệ SR/SSR
    /// </summary>
    /// <returns></returns>
    private MemoryCrystals RollCrystal()
    {
        float roll = Random.Range(0f, 1f);
        MemoryCrystals rolledCrystal;

        // Roll SR nếu tỷ lệ roll nhỏ hơn SR_RATE, ngược lại roll SSR
        if (roll < SR_RATE)
        {
            rolledCrystal = MemoryCrystals.GenerateNewMemoryCrystal(Purity.Pristine);
        }
        else
        {
            rolledCrystal = MemoryCrystals.GenerateNewMemoryCrystal(Purity.Eternal);
        }

        var resultUI = Instantiate(resultPrefab, resultHolder);
        resultUI.transform.GetChild(2).GetComponent<Image>().sprite = rolledCrystal.crystalSprite;
        resultUI.transform.GetChild(2).GetComponent<Image>().preserveAspect = true;
        ShowSSREffect(resultUI, null, rolledCrystal, 2);

        return rolledCrystal;
    }

    /// <summary>
    /// Lấy nhân vật ngẫu nhiên từ danh sách
    /// </summary>
    /// <param name="objectList"></param>
    /// <returns></returns>
    private GameObject GetRandomObjectFromList(List<GameObject> objectList)
    {
        int randomIndex = Random.Range(0, objectList.Count);
        return objectList[randomIndex];
    }

    /// <summary>
    /// Thêm nhân vật vào danh sách sở hữu, nếu đã sở hữu sẽ quy đổi thành điểm Unity
    /// </summary>
    /// <param name="character"></param>
    private void AddCharacterToOwned(GameObject character)
    {
        if (TeamManager.GetInstance().obtainedCharacters.Exists(c => c.GetComponent<CharacterBase>().Name == character.GetComponent<CharacterBase>().Name))
        {
            // Nếu đã sở hữu, quy đổi thành điểm Unity
            unityPoints += 10; // Tăng số điểm Unity (có thể tùy chỉnh)
            Destroy(character);
        }
        else
        {
            character.GetComponent<Characters>().GenerateID();
            // Nếu chưa sở hữu, thêm vào danh sách sở hữu
            TeamManager.GetInstance().obtainedCharacters.Add(character);
        }
    }

    /// <summary>
    /// Thêm crystal vào danh sách sở hữu
    /// </summary>
    /// <param name="crystal"></param>
    private void AddCrystalToOwned(MemoryCrystals crystal)
    {
        // Nếu chưa sở hữu, thêm vào danh sách sở hữu
        TeamManager.GetInstance().obtainedCrystals.Add(crystal);
    }

    /// <summary>
    /// Hiển thị hiệu ứng khi roll ra SSR (1: char, 2:crystal)
    /// </summary>
    /// <param name="resultUI"></param>
    /// <param name="rolledObject"></param>
    /// <param name="type"></param>
    void ShowSSREffect(GameObject resultUI, GameObject rolledCharacter, MemoryCrystals rolledCrystal, int type)
    {
        if (type == 1 && rolledCharacter.GetComponent<Characters>().rare == Rarity.Mythic || 
            type == 2 && rolledCrystal.crystalPurify == Purity.Eternal)
        {
            resultUI.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    #endregion

}
