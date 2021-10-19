using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSnapshotTransition : MonoBehaviour
{
    public AudioMixerSnapshot snapshot;
    public float transitionTime;

    void OnMouseDown(){
        snapshot.TransitionTo(transitionTime);
    }
}
