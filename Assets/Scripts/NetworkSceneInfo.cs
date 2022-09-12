using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSceneInfo : MonoBehaviour
{
    public SceneEntry[] sceneEntries;
    public EntryFallbackRule entryFallback;

    public SceneEntry FindSuitableEntries(uint clientId)
    {
        for (int i = 0; i < sceneEntries.Length; i++)
        {
            if (sceneEntries[i].CheckAvalible(clientId))
            {
                return sceneEntries[i];
            }
        }

        switch (entryFallback)
        {
            case EntryFallbackRule.ChooseRandomly:
                return sceneEntries[Random.Range(0, sceneEntries.Length - 1)];
            case EntryFallbackRule.ChooseFirst:
                return sceneEntries[0];
            case EntryFallbackRule.ChooseLast:
                return sceneEntries[sceneEntries.Length - 1];
            case EntryFallbackRule.ThrowError:
                Debug.LogError("Can't find suitable error");
                break;
        }

        return null;
    }

    public enum EntryFallbackRule {
        ChooseRandomly,
        ChooseFirst,
        ChooseLast,
        ThrowError,
    }
}
