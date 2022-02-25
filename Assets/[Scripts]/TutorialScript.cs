using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public Animator PickAnimator;
    public Animator directionAnimator;


    public readonly int clockwise = Animator.StringToHash("Clockwise");
    public readonly int restart = Animator.StringToHash("Restart");


    public GameObject Trigger;
    public TutorialTrigger tutorialTrigger;
    public Transform startPoint;
    public Transform endPoint;
    public float speedOfTrigger = 1f;

    public bool keyMatch = false;

    public bool executeOnce = false;


    public SpriteRenderer LockSpriteRenderer;
    public Sprite locked;
    public Sprite clicked;
    public Sprite unlocked;

    public GameObject MovingBar;
    public GameObject DirectionCircle;


    // Start is called before the first frame update
    void Start()
    {
        MovingBar.SetActive(false);
        DirectionCircle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (keyMatch && !tutorialTrigger.keyPressTutorialFinished)
        {
            Trigger.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, Mathf.PingPong(Time.time * speedOfTrigger, 1.0f));
        }
        else if(tutorialTrigger.keyPressTutorialFinished && !executeOnce)
        {
            executeOnce = true;
            LockSpriteRenderer.sprite = unlocked;
            StartCoroutine(DelayForRestart());
        }
    }

    /// <summary>
    /// If lock is picked
    /// </summary>
    public void LockClicked()
    {
        MovingBar.SetActive(true);
        DirectionCircle.SetActive(true);
        directionAnimator.SetBool(clockwise,true);
        keyMatch = true;
        LockSpriteRenderer.sprite = clicked;
    }


    /// <summary>
    /// Coroutine for restarting the tutorial
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayForRestart()
    {
        yield return new WaitForSeconds(1f);
        LockSpriteRenderer.sprite = locked;
        RestartTutorial();
        yield return new WaitForSeconds(1f);
        directionAnimator.SetBool(restart, false);
        PickAnimator.SetBool(restart, false);
    }

    /// <summary>
    /// Restart tutorial parameters
    /// </summary>
    public void RestartTutorial()
    {
        directionAnimator.SetBool(restart,true);
        PickAnimator.SetBool(restart, true);
        directionAnimator.SetBool(clockwise, false);

        tutorialTrigger.keyPressTutorialFinished = false;
        keyMatch = false;
        executeOnce = false;

        Trigger.transform.localPosition = new Vector3(-0.4f, 0f, 0f);
        
        tutorialTrigger.ResetTriggers();

        MovingBar.SetActive(false);
        DirectionCircle.SetActive(false);
    }
}
