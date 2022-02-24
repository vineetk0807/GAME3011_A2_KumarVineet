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
    public float m_fPreviousAngle;
    public bool m_bKeepInCheck = false;

    [Header("Mouse/Cursor parameters")] 
    public bool isClicking;

    [Header("Time")] 
    public float time = 0f;

    public LockpickSystem lockpickSystem;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Move if input is down
        if (isClicking && !GameManager.GetInstance().isBroken && !GameManager.GetInstance().isUnlocked)
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
        m_fPreviousAngle = m_fAngle;

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

        // In clicked mode, perform keep in check VERIFICATION in 3s
        // Keep on angle in check
        if (GameManager.GetInstance().isClicked)
        {
            time += Time.deltaTime;

            if (time > 3f)
            {
                time = 0;

                if (!m_bKeepInCheck)
                {
                    GameManager.GetInstance().NotKeptInCheck();
                }
            }

            // Angle check
            AngleCheck();
        }
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

        // Release the lock during clicked mode, lock breaks !
        if (GameManager.GetInstance().isClicked)
        {
            GameManager.GetInstance().LockBroken();
        }
    }

    /// <summary>
    /// Compares the current direction clock is supposed to rotate and
    /// checks the angle of rotation
    /// </summary>
    private void AngleCheck()
    {
        // If clockwise
        if (GameManager.GetInstance().clockwise)
        {

            // If rotating clockwise
            if (m_fPreviousAngle > m_fAngle)
            {
                m_bKeepInCheck = true;
            }
            else
            {
                m_bKeepInCheck = false;
            }
        }
        else 
        {
            // If rotating anticlockwise
            if (m_fPreviousAngle < m_fAngle)
            {
                m_bKeepInCheck = true;
            }
            else
            {
                m_bKeepInCheck = false;
            }
        }
    }
}
