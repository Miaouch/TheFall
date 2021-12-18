using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour
{
    public bool elevatorGo = false;
    public bool validationOK;
    public GameObject rouage;
    private Animator animator;


    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") && elevatorGo){
            other.transform.parent = transform;
            validationOK = true;
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
        if(validationOK){
            StartCoroutine(ElevatorGo(2f));
        }
    }

    IEnumerator ElevatorGo(float seconds){
        yield return new WaitForSeconds(seconds);
        animator.SetBool("TriggerEnter", true);
    }
}
