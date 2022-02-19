using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPointTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ClickPoint"))
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            GameManager.GetInstance().isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ClickPoint"))
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            GameManager.GetInstance().isInTrigger = false;
        }
    }
}
