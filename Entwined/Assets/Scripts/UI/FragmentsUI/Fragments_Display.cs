using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Fragments_Display : MonoBehaviour
{
    #region Defines
    // Lấy dữ liệu từ UI action bar.
    [SerializeField] private GameObject placeHolder;
    [SerializeField] private GameObject listPrefabs;
    [SerializeField] private CharacterDetail_UI characterDetail;
    private string playerId;
    private List<Fragments> fragments;
    [SerializeField] private List<Button> fragmentButton;
    [SerializeField] private List<Image> fragmentIcon;
    [SerializeField] private Button closeButton;
    public GameObject fragmentListGameObject;
    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        fragmentButton[0].onClick.AddListener(() => UpdateFragmentList(FragmentType.Essence));
        fragmentButton[1].onClick.AddListener(() => UpdateFragmentList(FragmentType.Vitality));
        fragmentButton[2].onClick.AddListener(() => UpdateFragmentList(FragmentType.Clarity));
        fragmentButton[3].onClick.AddListener(() => UpdateFragmentList(FragmentType.Resonance));
        closeButton.onClick.AddListener(() => RemoveFragmentList());
    }

    private void Update()
    {
        fragments = TeamManager.GetInstance().obtainedFragments;
        playerId = characterDetail.Character.GetComponent<Characters>().ID;

        DisplayFragmentsIcon();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Thực hiện load thông tin nhân vật lên Prefab tương ứng trong danh sách.
    /// </summary>
    /// <param name="fragments"></param>
    private void UpdateFragmentList(FragmentType type)
    {
        fragmentListGameObject.SetActive(true);

        // Lọc danh sách crystal theo tên, ở đầu luôn là crystal được trang bị
        List<Fragments> frList = fragments
            .Where(x => x.fragmentType == type)
            .OrderByDescending(x => x.playerUsedID == playerId)
            .ThenByDescending(x => x.playerUsedID != "" &&
                           x.playerUsedID != playerId)
            .ThenByDescending(x => x.fragmentRarity == Rarity.Mythic)
            .ThenBy(x => x.playerUsedID == "")
            .ThenBy(x => x.fragmentsName) // Finally, sort by crystal name
            .ToList();

        for (int i = 0; i < frList.Count; i++)
        {
            var crystalList = Instantiate(listPrefabs, placeHolder.transform);
            crystalList.GetComponent<Fragments_UI>().Init(frList[i]);
        }

        if (frList.Count > 0)
        {
            fragmentListGameObject.GetComponentInChildren<FragmentDetail_UI>().Init(frList[0]);
        }
    }

    /// <summary>
    /// Xóa các Prefab về danh sách nhân vật khi chuyển màn hình về Lobby.
    /// </summary>
    private void RemoveFragmentList()
    {
        playerId = "";
        //this.gameObject.GetComponent<CrystalDetail_UI>().ClearSlot();
        Transform[] list = placeHolder.GetComponentsInChildren<Transform>();

        foreach (Transform child in list)
        {
            if (child.gameObject.name != placeHolder.name)
            {
                Destroy(child.gameObject);
            }
        }
        fragmentListGameObject.SetActive(false);
    }

    void DisplayFragmentsIcon()
    {
        PlayerEquipments playerEquipments = characterDetail.Character.GetComponent<PlayerEquipments>();

        if (playerEquipments != null && playerEquipments.essenceFrag.playerUsedID == playerId)
        {
            fragmentIcon[0].color = new Color(fragmentIcon[0].color.r, fragmentIcon[0].color.g, fragmentIcon[0].color.b, 1f); // Hiển thị
            fragmentIcon[0].sprite = playerEquipments.essenceFrag.fragmentSprite;
        }
        else
        {
            fragmentIcon[0].color = new Color(fragmentIcon[0].color.r, fragmentIcon[0].color.g, fragmentIcon[0].color.b, 0f); // Hiển thị
        }

        if (playerEquipments != null && playerEquipments.vialityFrag.playerUsedID == playerId)
        {
            fragmentIcon[1].color = new Color(fragmentIcon[1].color.r, fragmentIcon[1].color.g, fragmentIcon[1].color.b, 1f); // Hiển thị
            fragmentIcon[1].sprite = playerEquipments.vialityFrag.fragmentSprite;
        }
        else fragmentIcon[1].color = new Color(fragmentIcon[1].color.r, fragmentIcon[1].color.g, fragmentIcon[1].color.b, 0f); // Hiển thị


        if (playerEquipments != null && playerEquipments.clarityFrag.playerUsedID == playerId)
        {
            fragmentIcon[2].color = new Color(fragmentIcon[2].color.r, fragmentIcon[2].color.g, fragmentIcon[2].color.b, 1f);
            fragmentIcon[2].sprite = playerEquipments.clarityFrag.fragmentSprite;
        }
        else fragmentIcon[2].color = new Color(fragmentIcon[2].color.r, fragmentIcon[2].color.g, fragmentIcon[2].color.b, 0f); // Hiển thị


        if (playerEquipments != null && playerEquipments.resonanceFrag.playerUsedID == playerId)
        {
            fragmentIcon[3].color = new Color(fragmentIcon[3].color.r, fragmentIcon[3].color.g, fragmentIcon[3].color.b, 1f);
            fragmentIcon[3].sprite = playerEquipments.resonanceFrag.fragmentSprite;
        }
        else fragmentIcon[3].color = new Color(fragmentIcon[3].color.r, fragmentIcon[3].color.g, fragmentIcon[3].color.b, 0f); // Hiển thị
    }
    #endregion


}
