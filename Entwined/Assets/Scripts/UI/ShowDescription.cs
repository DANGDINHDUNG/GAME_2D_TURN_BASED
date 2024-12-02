using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDescription : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerSkills playerSkills;
    [SerializeField] private GameObject descriptionPref;

    [SerializeField] private GameObject desc;

    private float holdStartTime;
    private bool isButtonPressed;
    private bool isSpawned;
    public bool isSkill;

    private void Start()
    {
        playerSkills = GetComponentInParent<PlayerSkills>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
        isSpawned = true;
        holdStartTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonPressed = false;
    }

    void Update()
    {
        if (isButtonPressed && Time.time - holdStartTime >= 1.0f && isSpawned)
        {
            desc = Instantiate(descriptionPref, GameObject.FindGameObjectWithTag("Description").transform);
            // Reset state after action
            isSpawned = false;

            if (isSkill)
            {
                desc.GetComponent<SkillDescription>().skillName_Text.text = playerSkills.CharacterSkills[1].skillName;
                desc.GetComponent<SkillDescription>().skillShortDescription_Text.text = playerSkills.CharacterSkills[1].skillShortDescription;
            }
            else
            {
                desc.GetComponent<SkillDescription>().skillName_Text.text = playerSkills.CharacterSkills[0].skillName;
                desc.GetComponent<SkillDescription>().skillShortDescription_Text.text = playerSkills.CharacterSkills[0].skillShortDescription;
            }
        }

        if (!isButtonPressed)
        {
            Destroy(desc);
        }
    }
}
