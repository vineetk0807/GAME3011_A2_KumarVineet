using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LockPick : MonoBehaviour
{
    // Position Vector and angle
    [Header("Position and Angle")]
    public Vector3 currentPosition;
    public float m_fAngle;

    [Header("Mouse/Cursor parameters")] 
    public bool isClicking;


    public LockpickSystem lockpickSystem;
    // Start is called before the first frame update
    void Start()
    {
        lockpickSystem = transform.root.gameObject.GetComponent<LockpickSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move if input is down
        if (isClicking)
        {
            if (Input.GetMouseButton(0))
            {
                MovePick();
            }
        }
    }


    /// <summary>
    /// Rotate pick with Mouse Input
    /// </summary>
    private void MovePick()
    {
        // Mouse position
        currentPosition = Input.mousePosition;

        // Take difference of z-axis of the world position of pick and the Camera object
        currentPosition.z = (transform.position.z - Camera.main.transform.position.z);

        // Convert it to world position
        currentPosition = Camera.main.ScreenToWorldPoint(currentPosition);

        // Take the position difference of the world positions of the pointer w.r.t. camera and pick
        currentPosition = currentPosition - transform.position;

        // Get Angle using Atan2
        m_fAngle = Mathf.Atan2(currentPosition.y, currentPosition.x) * Mathf.Rad2Deg;

        // Reset angle to keep within bounds
        if (m_fAngle < 0.0f)
        {
            m_fAngle += 360.0f;
        }

        // Set Rotation
        transform.rotation = Quaternion.Euler(0f, 0f, m_fAngle);
    }


    /// <summary>
    /// When mouse is clicked on the pick
    /// </summary>
    private void OnMouseDown()
    {
        GameManager.GetInstance().executeOnce = false;
        isClicking = true;
        lockpickSystem.PlaySounds((int)Sounds.LOCKPICKING);
    }


    /// <summary>
    /// Mouse click has been released
    /// </summary>
    private void OnMouseUp()
    {
        isClicking = false;
        lockpickSystem.PlaySounds((int)Sounds.STOP);
    }
}
