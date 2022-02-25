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
    public TextMeshProUGUI TMP_SkillInfo;
    public TextMeshProUGUI TMP_DifficultyInfo;
    private string textSkillSilver = "You are novice who doesn't click locks well";
    private string textSkillGold = "You are an experienced lock clicker but has trouble focusing";
    private string textSkillPlat = "You are a world renowned lock clicker";

    private string textDifficultyEasy = "Anyone can get through this difficulty";
    private string textDifficultyNormal = "Now you are challenging yourself";
    private string textDifficultyHard = "Master inner peace. (Do not break your keyboard)";
    


    // Start is called before the first frame update
    void Start()
    {
        dropDown = difficultyDropdown.GetComponent<TMPro.TMP_Dropdown>();
        TMP_SkillLabel.text = Skill.SILVER.ToString();
        TMP_SkillInfo.text = textSkillSilver;
        TMP_DifficultyInfo.text = textDifficultyEasy;
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
                TMP_DifficultyInfo.text = textDifficultyEasy;
                break;

            case 1:
                GameManager.GetInstance().currentDifficulty = Difficulty.NORMAL;
                TMP_DifficultyInfo.text = textDifficultyNormal;
                break;

            case 2:
                GameManager.GetInstance().currentDifficulty = Difficulty.HARD;
                TMP_DifficultyInfo.text = textDifficultyHard;
                break;

            default:
                GameManager.GetInstance().currentDifficulty = Difficulty.EASY;
                TMP_DifficultyInfo.text = textDifficultyEasy;
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
                TMP_SkillInfo.text = textSkillSilver;
                break;

            case 1:
                GameManager.GetInstance().currentSkill = Skill.GOLD;
                TMP_SkillInfo.text = textSkillGold;
                break;

            case 2:
                GameManager.GetInstance().currentSkill = Skill.PLATINUM;
                TMP_SkillInfo.text = textSkillPlat;
                break;

            default:
                GameManager.GetInstance().currentSkill = Skill.SILVER;
                TMP_SkillInfo.text = textSkillSilver;
                break;
        }

        TMP_SkillLabel.text = GameManager.GetInstance().currentSkill.ToString();

    }
}
