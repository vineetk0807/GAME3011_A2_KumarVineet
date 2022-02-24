using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class MenuPanel : MonoBehaviour
{
    public Slider slider;

    [SerializeField] 
    private GameObject difficultyDropdown;

    public TMPro.TMP_Dropdown dropDown;

    public TextMeshProUGUI TMP_SkillLabel;


    // Start is called before the first frame update
    void Start()
    {
        dropDown = difficultyDropdown.GetComponent<TMPro.TMP_Dropdown>();
        TMP_SkillLabel.text = Skill.SILVER.ToString();
    }

    /// <summary>
    /// Set difficulty OnValueChanged
    /// </summary>
    public void SetDifficulty()
    {
        int value = dropDown.value;

        switch (value)
        {
            case 0:
                GameManager.GetInstance().currentDifficulty = Difficulty.EASY;
                break;

            case 1:
                GameManager.GetInstance().currentDifficulty = Difficulty.NORMAL;
                break;

            case 2:
                GameManager.GetInstance().currentDifficulty = Difficulty.HARD;
                break;

            default:
                GameManager.GetInstance().currentDifficulty = Difficulty.EASY;
                break;
        }
    }


    /// <summary>
    /// Set Skill level
    /// </summary>
    public void SetSkill()
    {
        int value = (int)slider.value;

        switch (value)
        {
            case 0:
                GameManager.GetInstance().currentSkill = Skill.SILVER;
                break;

            case 1:
                GameManager.GetInstance().currentSkill = Skill.GOLD;
                break;

            case 2:
                GameManager.GetInstance().currentSkill = Skill.PLATINUM;
                break;

            default:
                GameManager.GetInstance().currentSkill = Skill.SILVER;
                break;
        }

        TMP_SkillLabel.text = GameManager.GetInstance().currentSkill.ToString();
    }
}
