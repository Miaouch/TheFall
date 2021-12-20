using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInGame : MonoBehaviour
{
    public void Menu(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public void Restart(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}
