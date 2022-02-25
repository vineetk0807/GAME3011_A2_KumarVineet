using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public List<SpriteRenderer> arrows;
    public bool keyPressTutorialFinished = false;
    public int i = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ClickPoint"))
        {
            GetComponent<SpriteRenderer>().color = Color.green;

            if (i <= 3)
            {
                arrows[i++].color = Color.green;

                if (i == 4)
                {
                    keyPressTutorialFinished = true;
                }
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ClickPoint"))
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    /// <summary>
    /// Reset Triggers
    /// </summary>
    public void ResetTriggers()
    {
        keyPressTutorialFinished = false;
        i = 0;
        foreach (var arrow in arrows)
        {
            arrow.color = Color.white;
        }
    }
}
