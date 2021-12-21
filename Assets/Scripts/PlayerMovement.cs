using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPack;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerMovement : NetworkBehaviour
{
    public float walkAcceleration;
    public float maxWalkSpeed;
    public float walkDampingSpeed;

    public float jumpForce;
    public Timer jumpTimer;
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

    void Update()
    {
        int speedChanged = 0;
        if (Input.GetKey(KeyCode.A))
        {
            speedChanged = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            speedChanged += 1;
        }
        if (speedChanged != 0)
        {
            movementVec.x = Mathf.MoveTowards(movementVec.x, speedChanged > 0 ? maxWalkSpeed : -maxWalkSpeed, walkAcceleration * Time.deltaTime);
        }
        else
        {
            movementVec.x = Mathf.MoveTowards(movementVec.x, 0, walkDampingSpeed * Time.deltaTime);
        }

        if (m_smartBoxCollider.LeftTouched && movementVec.x < 0)
        {
            movementVec.x = 0;
        }
        if (m_smartBoxCollider.RightTouched && movementVec.x > 0)
        {
            movementVec.x = 0;
        }

        movementVec.y = m_rigidbody.velocity.y;
        if (jumpTimer.Running)
        {
            if (m_smartBoxCollider.DownTouched)
            {
                if (leaveGround)
                {
                    jumpTimer.Running = false;
                    return;
                }
                else
                {
                    leaveGround = true;
                }
            }

            if (!Input.GetKey(KeyCode.Space))
            {
                jumpTimer.Running = false;
                return;
            }

            if (!jumpTimer.UpdateEnd)
            {
                movementVec.y = jumpForce;
            }
        }
        else
        {
            if (m_smartBoxCollider.DownTouched && Input.GetKey(KeyCode.Space))
            {
                jumpTimer.Reset();
                movementVec.y = jumpForce;
                leaveGround = false;
            }
        }

        m_rigidbody.velocity = movementVec;
    }

    #region Networkkkkking
    public override void OnNetworkSpawn()
    {
        enabled = IsOwner;

        if (NetworkSceneInfo.ins != null)
        {
            SceneEntry entry = NetworkSceneInfo.ins.FindSuitableEntries((uint)NetworkObject.OwnerClientId);
            transform.position = entry.transform.position;
        }
    }
    #endregion
}
