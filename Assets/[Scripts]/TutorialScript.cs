using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [Header("Animations")]
    public Animator PickAnimator;
    public Animator directionAnimator;
    public readonly int clockwise = Animator.StringToHash("Clockwise");
    public readonly int restart = Animator.StringToHash("Restart");

    [Header("Lock")]
    public SpriteRenderer LockSpriteRenderer;
    public Sprite locked;
    public Sprite clicked;
    public Sprite unlocked;

    [Header("Moving Bar and Circle")]
    public GameObject MovingBar;
    public GameObject DirectionCircle;
    public GameObject Trigger;
    public TutorialTrigger tutorialTrigger;
    public Transform startPoint;
    public Transform endPoint;

    [Header("Text")]
    public TextMeshProUGUI TMP_Step1;
    public TextMeshProUGUI TMP_Step2;
    public TextMeshProUGUI TMP_Step3;

    [Header("Executions")]
    public float speedOfTrigger = 1f;
    public bool keyMatch = false;
    public bool executeOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        MovingBar.SetActive(false);
        DirectionCircle.SetActive(false);
        Step1();
    }

    private void OnEnable()
    {
        RestartTutorial();
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
        Step2();
        StartCoroutine(DelayForStep3());
    }

    IEnumerator DelayForStep3()
    {
        yield return new WaitForSeconds(1.5f);
        Step3();
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

        Step1();
    }



    /// <summary>
    /// Step 1 text
    /// </summary>
    public void Step1()
    {
        TMP_Step1.color = Color.green;
        TMP_Step2.color = Color.black;
        TMP_Step3.color = Color.black;
        
    }

    /// <summary>
    /// Step 1 text
    /// </summary>
    public void Step2()
    {
        TMP_Step1.color = Color.black;
        TMP_Step2.color = Color.green;
        TMP_Step3.color = Color.black;

    }

    /// <summary>
    /// Step 1 text
    /// </summary>
    public void Step3()
    {
        TMP_Step1.color = Color.black;
        TMP_Step2.color = Color.black;
        TMP_Step3.color = Color.green;

    }
}
