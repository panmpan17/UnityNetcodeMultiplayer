using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntry : MonoBehaviour
{
    public FillRule fillRule;
    public uint specificClientId;

    public UsageRule usageRule;
    private bool used;
    public LayerMask overlapLayers;
    public Vector2 overlapSize;
    public Vector3 overlapOffset;

    public bool CheckAvalible(uint clientId)
    {
        switch (fillRule)
        {
            case FillRule.ClientId:
                if (specificClientId != clientId)
                {
                    return false;
                }
                break;
        }

        switch (usageRule)
        {
            case UsageRule.Once:
                if (used)
                {
                    return false;
                }
                used = true;
                break;
            case UsageRule.PhysicOverlap:
                if (Physics2D.OverlapBox(transform.position + overlapOffset, overlapSize, 0, overlapLayers))
                {
                    return false;
                }
                break;
        }

        return true;
    }

    public enum FillRule {
        All,
        ClientId
    }

    public enum UsageRule {
        NoLimit,
        Once,
        PhysicOverlap,
    }
}
