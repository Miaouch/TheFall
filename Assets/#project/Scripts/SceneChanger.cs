using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Canvas canvasAnim1;
    public Canvas canvasAnim2;
    public Animator animatorCredits;
    public Animator animatorControls;
    public void changeScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public void Exit(){
        print("Exit Game");
        Application.Quit();
    }

    public void AnimControls(){
        animatorControls.SetBool("controls", true);
    }

    public void AnimCredits(){
        animatorCredits.SetBool("controls", true);
    }

    public void AnimBack(){
        animatorControls.SetBool("controls", false);
        animatorCredits.SetBool("controls", false);
    }
    
    public void Start(){
        animatorControls = canvasAnim1.GetComponent<Animator>();
        animatorCredits = canvasAnim2.GetComponent<Animator>();
    }
}
