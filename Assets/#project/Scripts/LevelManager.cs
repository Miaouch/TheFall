using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
     void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            print("You win!");
            StartCoroutine(ChangeScene(5f));
        }
    }

    IEnumerator ChangeScene(float seconds){
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("MenuScene");
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
