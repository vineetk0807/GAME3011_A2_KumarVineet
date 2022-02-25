using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ArrowDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class MovingBar : MonoBehaviour
{
    [Header("Arrow Key Sprites - UP DOWN LEFT RIGHT")]
    public List<Sprite> ArrowKeyList;

    public SpriteRenderer VisualAssist;

    public GameObject ClickPoint;
    public GameObject Trigger;

    [Header("Movement")]
    public Transform startPoint;
    public Transform endPoint;

    private List<int> directionList;

    public float speedOfTrigger = 1f;
    public float resetSpeed = 1f;
    public float difficultyFactor = 1f;
    public Vector3 scale = Vector3.one;
    public float resetScale = 0.05f;

    // Start is called before the first frame update
    void Awake()
    {
        Trigger.transform.localScale = scale;
        directionList = new List<int>();
        LoadList();

        switch (GameManager.GetInstance().currentDifficulty)
        {
            case Difficulty.EASY:
                scale.x = 0.05f;
                Trigger.transform.localScale = scale;
                break;

            case Difficulty.NORMAL:
                scale.x = 0.05f * 0.5f;
                Trigger.transform.localScale = scale;
                speedOfTrigger *= 1.5f;
                break;

            case Difficulty.HARD:
                scale.x = 0.05f * 0.5f;
                Trigger.transform.localScale = scale;
                speedOfTrigger *= 2f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetInstance().isBroken && GameManager.GetInstance().isClicked)
        {
            Trigger.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, Mathf.PingPong(Time.time * speedOfTrigger , 1.0f));
        }
    }


    /// <summary>
    /// Loads the list with 4 directions in order
    /// </summary>
    public void LoadList()
    {
        directionList.Add((int)ArrowDirection.UP);
        directionList.Add((int)ArrowDirection.DOWN);
        directionList.Add((int)ArrowDirection.LEFT);
        directionList.Add((int)ArrowDirection.RIGHT);
    }


    /// <summary>
    /// Loads direction
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(SpriteRenderer visualAssist, int direction)
    {
        visualAssist.sprite = ArrowKeyList[direction];
    }


    /// <summary>
    /// Reset entire 
    /// </summary>
    public void ResetMovingBar(SpriteRenderer VisualAssist0, SpriteRenderer VisualAssist1, SpriteRenderer VisualAssist2, SpriteRenderer VisualAssist3)
    {
        // Set color
        VisualAssist0.color = VisualAssist1.color = VisualAssist2.color = VisualAssist3.color = Color.white;

        // Set direction
        VisualAssist0.sprite = VisualAssist1.sprite = VisualAssist2.sprite = VisualAssist3.sprite = ArrowKeyList[(int)ArrowDirection.UP];

        Trigger.transform.position = startPoint.position;

        speedOfTrigger = resetSpeed;

        switch (GameManager.GetInstance().currentDifficulty)
        {
            case Difficulty.EASY:
                scale.x = resetScale;
                Trigger.transform.localScale = scale;
                break;

            case Difficulty.NORMAL:
                scale.x = resetScale * 0.5f;
                Trigger.transform.localScale = scale;
                speedOfTrigger *= 1.5f;
                break;

            case Difficulty.HARD:
                scale.x = resetScale * 0.5f;
                Trigger.transform.localScale = scale;
                speedOfTrigger *= 2f;
                break;
        }
    }
}
