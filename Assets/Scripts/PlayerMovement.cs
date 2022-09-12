using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MPack;
using Unity.Netcode;
using Unity.Netcode.Components;


public class PlayerMovement : NetworkBehaviour
{
    public float walkAcceleration;
    public float maxWalkSpeed;
    // public float walkDampingSpeed;

    public float jumpForce;
    public Timer jumpTimer;
    public Timer keepJumpForcetimer;
    private bool leaveGround;

    private Rigidbody2D m_rigidbody;
    private SmartBoxCollider m_smartBoxCollider;
    private NetworkTransform m_networkTransform;
    private NetworkRigidbody2D networkRigidbody2D;
    private Vector2 movementVec;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_smartBoxCollider = GetComponent<SmartBoxCollider>();
        m_networkTransform = GetComponent<NetworkTransform>();

        jumpTimer.Running = false;
    }

    private void HandleMovement()
    {
        Vector2 delta = m_rigidbody.velocity;

        // Handle horizontal movement
        float xAxis = (Input.GetKey(KeyCode.A) ? -1: 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
        delta.x = Mathf.Clamp(delta.x + (xAxis * walkAcceleration * Time.deltaTime), -maxWalkSpeed, maxWalkSpeed);

        if (m_smartBoxCollider.LeftTouched && delta.x < 0) delta.x = 0;
        else if (m_smartBoxCollider.RightTouched && delta.x > 0) delta.x = 0;

        if (delta.x > 0 && transform.localScale.x < 0) transform.localScale = new Vector3(1, 1, 1);
        else if (delta.x < 0 && transform.localScale.x > 0) transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpTimer.Reset();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTimer.Running = false;
        }

        // Handle jump movement
        if (jumpTimer.Running)
        {
            if (m_smartBoxCollider.DownTouched)
            {
                delta.y = jumpForce;
                keepJumpForcetimer.Reset();
            }
            else if (jumpTimer.UpdateEnd)
            {
                jumpTimer.Running = false;
            }
        }
        if (keepJumpForcetimer.Running)
        {
            if (!jumpTimer.Running)
            {
                keepJumpForcetimer.Running = false;
                return;
            }
            if (keepJumpForcetimer.UpdateEnd)
            {
                keepJumpForcetimer.Running = false;
            }
            delta.y = jumpForce;
        }

        m_rigidbody.velocity = delta;
    }

    void Update()
    {
        HandleMovement();
        // int speedChanged = 0;
        // if (Input.GetKey(KeyCode.A))
        // {
        //     speedChanged = -1;
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     speedChanged += 1;
        // }
        // if (speedChanged != 0)
        // {
        //     movementVec.x = Mathf.MoveTowards(movementVec.x, speedChanged > 0 ? maxWalkSpeed : -maxWalkSpeed, walkAcceleration * Time.deltaTime);
        // }
        // else
        // {
        //     movementVec.x = Mathf.MoveTowards(movementVec.x, 0, walkDampingSpeed * Time.deltaTime);
        // }

        // if (m_smartBoxCollider.LeftTouched && movementVec.x < 0)
        // {
        //     movementVec.x = 0;
        // }
        // if (m_smartBoxCollider.RightTouched && movementVec.x > 0)
        // {
        //     movementVec.x = 0;
        // }

        // movementVec.y = m_rigidbody.velocity.y;
        // if (jumpTimer.Running)
        // {
        //     if (m_smartBoxCollider.DownTouched)
        //     {
        //         if (leaveGround)
        //         {
        //             jumpTimer.Running = false;
        //             return;
        //         }
        //         else
        //         {
        //             leaveGround = true;
        //         }
        //     }

        //     if (!Input.GetKey(KeyCode.Space))
        //     {
        //         jumpTimer.Running = false;
        //         return;
        //     }

        //     if (!jumpTimer.UpdateEnd)
        //     {
        //         movementVec.y = jumpForce;
        //     }
        // }
        // else
        // {
        //     if (m_smartBoxCollider.DownTouched && Input.GetKey(KeyCode.Space))
        //     {
        //         jumpTimer.Reset();
        //         movementVec.y = jumpForce;
        //         leaveGround = false;
        //     }
        // }

        // m_rigidbody.velocity = movementVec;
    }

    #region Networkkkkking
    public override void OnNetworkSpawn()
    {
        enabled = IsOwner;

        NetworkSceneInfo sceneInfo = FindObjectOfType<NetworkSceneInfo>();
        if (sceneInfo != null)
        {
            SceneEntry entry = sceneInfo.FindSuitableEntries((uint)NetworkObject.OwnerClientId);
            transform.position = entry.transform.position;
        }
    }

    public void OnSceneChanged()
    {
        NetworkSceneInfo sceneInfo = FindObjectOfType<NetworkSceneInfo>();
        if (sceneInfo != null)
        {
            SceneEntry entry = sceneInfo.FindSuitableEntries((uint)NetworkObject.OwnerClientId);
            transform.position = entry.transform.position;
        }
    }
    #endregion
}
