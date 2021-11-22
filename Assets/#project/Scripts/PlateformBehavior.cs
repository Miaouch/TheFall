using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlateformBehavior : MonoBehaviour
{
    
    public List<GameObject> pivots = new List<GameObject>();

    public Transform player;
    public Transform pivotsTrigger;
    public int increment = 90;
    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 direction = transform.up * -1;
        float hitRange = 1f;
        RaycastHit objectHit;
        Color e = Color.red;
        if(Physics.Raycast(transform.position, direction, out objectHit, hitRange)){
            e = Color.green; }
        Debug.DrawRay(transform.position, direction * hitRange, e);


        // pivotsTrigger.rotation = player.rotation;
        pivotsTrigger.eulerAngles = new Vector3(player.rotation.eulerAngles.x, (Mathf.Round(player.rotation.eulerAngles.y / increment) * increment), player.rotation.eulerAngles.z);
        if(objectHit.normal = Vector3.up);
    }

    

}
