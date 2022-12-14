using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayfieldTracked {
    void OnEnterPlayfield(Playfield f);
    void OnExitPlayfield(Playfield f);
}

public class Playfield : MonoBehaviour
{
    public Transform SpawnPoint;

    public Dictionary<IPlayfieldTracked, int> TrackedObjects = new Dictionary<IPlayfieldTracked, int>();
    
    public void NotifyTrackedEnterSubSection(IPlayfieldTracked tracked) {
        if (TrackedObjects.ContainsKey(tracked)) {
            TrackedObjects[tracked]++;
        }
        else {
            TrackedObjects.Add(tracked, 1);
            tracked.OnEnterPlayfield(this);
        }
    }

    public void NotifyTrackedExitSubSection(IPlayfieldTracked tracked) {
        if (TrackedObjects.ContainsKey(tracked)) {
            TrackedObjects[tracked]--;
            if (TrackedObjects[tracked] == 0) {
                TrackedObjects.Remove(tracked);
                tracked.OnExitPlayfield(this);
            }
        }
    }
}
