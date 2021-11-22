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
    public GameObject cube;
    public bool isRotating;
    public float angleRotation;
    public bool activatedTrigger;
    
    void Start()
    {
        activatedTrigger = false;
        Mesh mesh = cube.GetComponent<MeshFilter>().mesh;
        Vector3[] normals = mesh.normals;
    }

    
    void Update()
    {
        isRotating = player.GetComponent<PlayerBehavior>().isRotating;

        Vector3 direction = player.transform.up * -1;
        float hitRange = 1f;
        RaycastHit objectHit;
        Color e = Color.red;
        if(Physics.Raycast(player.transform.position, direction, out objectHit, hitRange)){
            e = Color.green; }
        Debug.DrawRay(player.transform.position, direction * hitRange, e);
        if(objectHit.transform.gameObject == cube && !isRotating){
            print("prout");
            activatedTrigger = true;
        } else{
            activatedTrigger = false;
            
        }
        
        pivotsTrigger.gameObject.SetActive(activatedTrigger);
        

        
        angleRotation = Vector3.SignedAngle(transform.forward, player.forward, transform.up);
        
        pivotsTrigger.localEulerAngles = new Vector3(0, (Mathf.Round(angleRotation / increment) * increment), 0);
        
        print($"localRotation {player.localRotation.eulerAngles}");
        

    }

    

}
