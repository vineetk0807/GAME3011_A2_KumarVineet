using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public enum Difficulty
{
    EASY,
    MEDIUM,
    HARD
}

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance;

    public static GameManager GetInstance()
    {
        return _instance;
    }

    // Random Number
    [Header("Random Number/Angle")] 
    public float randomAngle = 0f;

    // Lock pick system
    [Header("Lock-Pick elements")] 
    public LockpickSystem lockPickSystem;

    [SerializeField]
    private LockPick lockPick;
    
    public SpriteRenderer lockSprite;
    public Sprite spriteLocked;
    public Sprite spriteClicked;
    public Sprite spriteUnlocked;
    public Sprite spriteBroken;

    // Difficulty
    [Header("Difficulty and Skill")] 
    public List<float> difficultyFactor;

    public Difficulty currentDifficulty = Difficulty.MEDIUM;


    // Executions
    public bool executeOnce = false;
    public bool isClicked = false;

    /// <summary>
    /// Using awake to set instance for lazy singleton
    /// </summary>
    private void Awake()
    {
        _instance = this;
        difficultyFactor = new List<float>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Generate random angle
        GenerateRandomAngle();

        // difficulty factor
        difficultyFactor.Add(0.6f); 
        difficultyFactor.Add(0.3f); 
        difficultyFactor.Add(0.09f);

        lockSprite.sprite = spriteLocked;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for difference
        if (Math.Abs((lockPick.m_fAngle - randomAngle)) < difficultyFactor[(int)currentDifficulty])
        {
            if (!executeOnce)
            {
                executeOnce = true;
                lockPickSystem.PlaySounds((int)Sounds.CLICKED);
                lockSprite.sprite = spriteClicked;
                isClicked = true;
            }
        }
    }

    /// <summary>
    /// Function to generate a random angle
    /// </summary>
    private void GenerateRandomAngle()
    {
        randomAngle = Random.Range(0f, 360f);
    }
}
