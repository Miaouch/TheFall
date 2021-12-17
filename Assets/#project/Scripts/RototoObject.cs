using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RototoObject : MonoBehaviour
{
    public GameObject rouage;
    public float speed = 20f;
    public bool rouageOK;
    public void Start()
    {
        rouageOK = false;
    }

    void OnTriggerEnter(Collider other){
        rouageOK = true;
        Destroy(rouage);
    }
    // Update is called once per frame
    public void Update()
    {
        transform.Rotate(Vector3.right * speed * Time.deltaTime);
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);

    }
}
