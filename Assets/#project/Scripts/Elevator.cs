using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool elevatorGo = false;
    public GameObject rouage;
    private Animator animator;

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") && elevatorGo){
            other.transform.parent = transform;
            animator.SetBool("TriggerEnter", true);
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            other.transform.parent = null;
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if(rouage.GetComponent<RototoObject>().rouageOK){
            
            elevatorGo = true;
        }
    }
}
