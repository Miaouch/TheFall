using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject prout;
     void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            print("You win!");
            prout.SetActive(true);
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
