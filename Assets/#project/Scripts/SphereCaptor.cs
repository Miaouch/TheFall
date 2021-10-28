using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCaptor : MonoBehaviour
{
    public bool footRotate;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("platform")) {
            footRotate = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("platform")) {
            footRotate = false;
        
        }

    }
    
}
