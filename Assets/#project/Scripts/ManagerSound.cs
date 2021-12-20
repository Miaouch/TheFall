using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSound : MonoBehaviour
{
    public AudioSource boop;
    public AudioSource boopPitch;

    public void Onhover(){
        boopPitch.Play();
    }
    public void OnClick(){
        boop.Play();
    }
}
