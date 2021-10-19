using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    public bool activeState;
    public GameObject targetObject;
    void Start()
    {
        targetObject.SetActive(activeState);
    }

    public void Toggle(){
        activeState = !activeState;
        
        targetObject.SetActive(activeState);
    }
}
