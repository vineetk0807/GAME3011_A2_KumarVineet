using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
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
    public Sprite spriteUnsuccessful;

    [Header("Direction Indicator")] 
    public GameObject directionIndicator;

    [Header("Moving Bar")] 
    public MovingBar movingBar;

    public SpriteRenderer VisualAssist0;
    public SpriteRenderer VisualAssist1;
    public SpriteRenderer VisualAssist2;
    public SpriteRenderer VisualAssist3;

    // Difficulty
    [Header("Difficulty and Skill")] 
    public List<float> difficultyFactor;

    public Difficulty currentDifficulty = Difficulty.MEDIUM;

    [Header("UI")] 
    public TextMeshProUGUI checks_TMP;

    private int checksRemaining = 5;


    // Executions
    public bool executeOnce = false;
    public bool isClicked = false;
    public float m_fStartingTime = 0f;
    public bool clockwise = false;
    public bool isBroken = false;
    public bool isInTrigger = false;
    public List<int> RandomDirectionOrder = new List<int>();
    public int currentDirection = 0;
    public int currentIndex = 0;
    public bool keyPressUP = false;
    public bool keyPressDOWN = false;
    public bool keyPressLEFT = false;
    public bool keyPressRIGHT = false;

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

        checksRemaining = 5;

        isBroken = false;

        RandomDirectionOrder.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for difference
        if (!isClicked && !isBroken)
        {
            if (Math.Abs((lockPick.m_fAngle - randomAngle)) < difficultyFactor[(int)currentDifficulty])
            {
                if (!executeOnce)
                {
                    executeOnce = true;
                    lockPickSystem.PlaySounds((int)Sounds.CLICKED);
                    lockSprite.sprite = spriteClicked;
                    isClicked = true;

                    GenerateRandomDirection();
                    if (RandomDirectionOrder.Count > 0)
                    {
                        currentDirection = RandomDirectionOrder[currentIndex];
                    }
                }
            }
        }
        else
        {
            if (!isBroken)
            {
                // Reset time
                if (m_fStartingTime == 0)
                {
                    Debug.Log("Started time");

                    // clockwise/anticlockwise
                    clockwise = (Random.Range(2, 10) % 2 == 0) ? true : false;
                }

                RotateIndicator(clockwise);

                // Increment time
                m_fStartingTime += Time.deltaTime;

                // Reset time if greater than 5 seconds
                if (m_fStartingTime >= 5f)
                {
                    m_fStartingTime = 0f;
                }
            }


            if((keyPressUP = (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) 
                || (keyPressLEFT = (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
                || (keyPressRIGHT = (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
                || (keyPressDOWN = (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))))
            {
                if (!isInTrigger)
                {
                    Debug.Log("Not in Trigger !!!");
                    LockBroken();
                }
                else
                {
                    bool keyPressed = false;
                    switch (currentDirection)
                    {
                        case (int)ArrowDirection.UP:
                            if (keyPressUP)
                            {
                                keyPressed = true;
                            }
                            break;

                        case (int)ArrowDirection.DOWN:
                            if (keyPressDOWN)
                            {
                                keyPressed = true;
                            }
                            break;

                        case (int)ArrowDirection.LEFT:
                            if (keyPressLEFT)
                            {
                                keyPressed = true;
                            }
                            break;

                        case (int)ArrowDirection.RIGHT:
                            if (keyPressRIGHT)
                            {
                                keyPressed = true;
                            }
                            break;
                    }

                    if (keyPressed)
                    {
                        switch (currentIndex)
                        {
                            case 0:
                                VisualAssist0.color = Color.green;
                                break;

                            case 1:
                                VisualAssist1.color = Color.green;
                                break;

                            case 2:
                                VisualAssist2.color = Color.green;
                                break;

                            case 3:
                                VisualAssist3.color = Color.green;
                                break;
                        }

                        currentIndex++;

                        if (currentIndex < RandomDirectionOrder.Count)
                        {
                            currentDirection = RandomDirectionOrder[currentIndex];
                        }
                    }
                    else
                    {
                        GenerateRandomDirection();
                        currentIndex = 0;
                        VisualAssist0.color = Color.white;
                        VisualAssist1.color = Color.white;
                        VisualAssist2.color = Color.white;
                        VisualAssist3.color = Color.white;
                    }

                    keyPressDOWN = false;
                    keyPressUP = false;
                    keyPressLEFT = false;
                    keyPressRIGHT = false;
                }
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


    /// <summary>
    /// Indicator rotates and automatically changes direction based on clock-wise
    /// </summary>
    /// <param name="clockwise"></param>
    private void RotateIndicator(bool clockwise)
    {
        if (clockwise)
        {
            directionIndicator.transform.rotation *= Quaternion.AngleAxis(-45 * Time.deltaTime, Vector3.forward);
        }
        else
        {
            directionIndicator.transform.rotation *= Quaternion.AngleAxis(45 * Time.deltaTime, Vector3.forward);
        }
    }


    /// <summary>
    /// If clicked lock rotation was not kept in check, reduce width
    /// </summary>
    public void NotKeptInCheck()
    {
        checksRemaining -= 1;
        checks_TMP.text = checksRemaining.ToString();

        if (checksRemaining <= 0)
        {
            LockBroken();
        }
    }


    /// <summary>
    /// Lock Broken
    /// </summary>
    public void LockBroken()
    {
        isBroken = true;
        Debug.Log("Broken");
        lockSprite.sprite = spriteUnsuccessful;
    }


    /// <summary>
    /// Generates the random direction 0 - Up, 1 - Down, 2 - Left, 3 - Right
    /// </summary>
    /// <returns></returns>
    public void GenerateRandomDirection()
    {
        RandomDirectionOrder.Clear();
        int i = 0;
        while (RandomDirectionOrder.Count < 4)
        {
            int tempNumber = Random.Range(0, 4);

            if (!RandomDirectionOrder.Contains(tempNumber))
            {
                RandomDirectionOrder.Add(tempNumber);
                switch (i)
                {
                    case 0:
                        movingBar.SetDirection(VisualAssist0, tempNumber);
                        break;

                    case 1:
                        movingBar.SetDirection(VisualAssist1, tempNumber);
                        break;

                    case 2:
                        movingBar.SetDirection(VisualAssist2, tempNumber);
                        break;

                    case 3:
                        movingBar.SetDirection(VisualAssist3, tempNumber);
                        break;
                }

                i++;
            }
        }
    }
}