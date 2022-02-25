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
    NORMAL,
    HARD
}

public enum Skill
{
    SILVER,
    GOLD,
    PLATINUM
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
    public SpriteRenderer directionBackground;
    public Color originalBkgColor = Color.gray;

    [Header("Moving Bar")] 
    public MovingBar movingBar;
    public GameObject Trigger;
    public SpriteRenderer VisualAssist0;
    public SpriteRenderer VisualAssist1;
    public SpriteRenderer VisualAssist2;
    public SpriteRenderer VisualAssist3;

    // Difficulty
    [Header("Difficulty and Skill")] 
    public List<float> skillFactor;

    public Difficulty currentDifficulty = Difficulty.EASY;
    public Skill currentSkill = Skill.SILVER;

    [Header("UI")] 
    public TextMeshProUGUI checks_TMP;
    private int checksRemaining = 3;
    public int numberOfChecks = 3;

    public TextMeshProUGUI time_TMP;
    private float timeRemaining = 30f;
    private float timeFactorEasy = 1.5f;
    private float timeFactorDefault = 1f;
    private float currentTime = 0f;

    public TextMeshProUGUI TMP_UI_difficulty;
    public TextMeshProUGUI TMP_UI_skill;

    [Header("-------EXECUTIONS-------")]
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
    private bool isPlaying = false;
    public bool isUnlocked = false;

    /// <summary>
    /// Using awake to set instance for lazy singleton
    /// </summary>
    private void Awake()
    {
        _instance = this;
        //skillFactor = new List<float>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Generate random angle
        GenerateRandomAngle();

        lockSprite.sprite = spriteLocked;

        checksRemaining = numberOfChecks;

        if (currentSkill == Skill.SILVER)
        {
            timeRemaining = 30f * timeFactorEasy;
        }
        else
        {
            timeRemaining = 30f * timeFactorDefault;
        }

        time_TMP.text = ((int)timeRemaining).ToString();

        isBroken = false;

        RandomDirectionOrder.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            if (!isBroken && !isUnlocked)
            {
                if (Time.time - currentTime >= 1)
                {
                    currentTime = Time.time;
                    timeRemaining -= 1;
                    time_TMP.text = ((int)timeRemaining).ToString();

                    if (timeRemaining <= 0)
                    {
                        LockBroken();
                    }
                }
            }

            PickLock();
        }
    }


    //****************** PICK LOCK METHOD ******************//

    /// <summary>
    /// PICK THE LOCK !!!!!!!!!!!!!!!!!!!
    /// </summary>
    private void PickLock()
    {
        // Check for difference
        if (!isClicked && !isBroken)
        {
            if (Math.Abs((lockPick.m_fAngle - randomAngle)) < skillFactor[(int)currentSkill])
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
        else if(!isUnlocked)
        {
            if (!isBroken)
            {
                // Reset time
                if (m_fStartingTime == 0)
                {
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


            if ((keyPressUP = (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
                || (keyPressLEFT = (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
                || (keyPressRIGHT = (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
                || (keyPressDOWN = (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))))
            {
                if (!isInTrigger)
                {
                    //Debug.Log("Not in Trigger !!!");
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
                        else
                        {
                            LockUnlocked();
                        }
                    }
                    else
                    {
                        currentIndex = 0;
                        GenerateRandomDirection();
                        currentDirection = RandomDirectionOrder[currentIndex];
                        VisualAssist0.color = Color.white;
                        VisualAssist1.color = Color.white;
                        VisualAssist2.color = Color.white;
                        VisualAssist3.color = Color.white;
                    }
                }


                // Reset key press
                keyPressDOWN = false;
                keyPressUP = false;
                keyPressLEFT = false;
                keyPressRIGHT = false;
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
            directionIndicator.transform.rotation *= Quaternion.AngleAxis(-45 * Time.deltaTime * 10f, Vector3.forward);
            directionIndicator.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            directionIndicator.transform.rotation *= Quaternion.AngleAxis(45 * Time.deltaTime * 10f, Vector3.forward);
            directionIndicator.GetComponent<SpriteRenderer>().flipX = false;
        }
    }


    /// <summary>
    /// If clicked lock rotation was not kept in check, reduce width
    /// </summary>
    public void NotKeptInCheck()
    {
        lockPickSystem.PlaySounds((int)Sounds.BUZZER);
        checksRemaining -= 1;
        checks_TMP.text = checksRemaining.ToString();

        if (checksRemaining <= 0)
        {
            LockBroken();
        }
        directionBackground.color = Color.red;
        
        StartCoroutine(LerpColor());
    }

    IEnumerator LerpColor()
    {
        float timer = 0.0f;

        while (timer < 2f)
        {
            timer += Time.deltaTime;
            directionBackground.color = Color.Lerp(Color.red, Color.grey, timer / 2f);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2);
    }


    /// <summary>
    /// Lock Broken
    /// </summary>
    public void LockBroken()
    {
        isBroken = true;
        lockSprite.sprite = spriteUnsuccessful;
        Trigger.GetComponent<SpriteRenderer>().color = Color.blue;
        // End Game
        GameOver();
    }


    /// <summary>
    /// Lock Unlocked
    /// </summary>
    public void LockUnlocked()
    {
        isBroken = false;
        isUnlocked = true;
        isClicked = false;
        isPlaying = false;
        lockSprite.sprite = spriteUnlocked;

        // End Game
        GameOver();
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


    //************************* MENU *************************//

    [Header("-------MENU PANEL-------")]
    public GameObject MenuPanel;
    public GameObject LockGamePanel;
    public GameObject GameOverPanel;
    public TextMeshProUGUI TMP_Results;

    public void MainMenu()
    {
        MenuPanel.SetActive(true);
        LockGamePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        isPlaying = false;
    }
    
    /// <summary>
    /// Start game to reset values
    /// </summary>
    public void StartGame()
    {
        MenuPanel.SetActive(false);
        LockGamePanel.SetActive(true);
        GameOverPanel.SetActive(false);
        isPlaying = true;

        // Add additional restart functionality
        // Change the angle, pick rotation and sprite
        GenerateRandomAngle();
        lockPick.gameObject.transform.rotation = Quaternion.Euler(0f,0f,0f);
        lockSprite.sprite = spriteLocked;

        // reset UI values
        checksRemaining = numberOfChecks;
        checks_TMP.text = checksRemaining.ToString();
        timeRemaining = 30f;
        if (currentSkill == Skill.SILVER)
        {
            timeRemaining = 30f * timeFactorEasy;
        }
        else
        {
            timeRemaining = 30f * timeFactorDefault;
        }

        time_TMP.text = ((int)timeRemaining).ToString();

        isClicked = isBroken = isUnlocked = false;

        RandomDirectionOrder.Clear();
        currentIndex = 0;
        movingBar.ResetMovingBar(VisualAssist0, VisualAssist1, VisualAssist2, VisualAssist3);
    }

    /// <summary>
    /// Game Over functionality
    /// </summary>
    public void GameOver()
    {
        MenuPanel.SetActive(false);
        LockGamePanel.SetActive(true);
        GameOverPanel.SetActive(true);

        if (isUnlocked)
        {
            TMP_Results.text = "LOCK PICK SUCCESSFUL";
        }
        else if (isBroken)
        {
            TMP_Results.text = "LOCK PICK FAILED";
        }
    }

    /// <summary>
    /// Quit game functionality
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }
}
