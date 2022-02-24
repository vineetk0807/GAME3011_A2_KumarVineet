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
    public float difficultyFactor = 1f;

    // Start is called before the first frame update
    void Start()
    {
        directionList = new List<int>();
        LoadList();

        switch (GameManager.GetInstance().currentDifficulty)
        {
            case Difficulty.EASY:
                break;

            case Difficulty.NORMAL:
                break;

            case Difficulty.HARD:
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
}
