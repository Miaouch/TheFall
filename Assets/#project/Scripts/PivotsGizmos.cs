using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotsGizmos : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnDrawGizmos(){
        Gizmos.color = new Color(0f, 0.5f, 0.9f, 0.4f);
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
