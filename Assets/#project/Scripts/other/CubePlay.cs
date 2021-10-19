using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlay : MonoBehaviour
{
    public AudioSource source;

    private void OnMouseDown(){
        source.Play(0);
        
    }
}
