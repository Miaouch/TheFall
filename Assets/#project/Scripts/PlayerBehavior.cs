using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    public CharacterController controller;
    public float movementSpeed;
    Vector2 moveVector;

    public Transform cam;

    public float hitRange = 0.1f;
    private bool firstTime;
    public Transform startingPoint;

    // Start is called before the first frame update
    void Start()
    {
        firstTime = true;
    }

    // Update is called once per frame
    public void Movement(InputAction.CallbackContext context){ //cette méthode lie à l'inpput player pour le déplacement
        moveVector = context.ReadValue<Vector2>();
        // Debug.Log(moveVector);
    }
    void Update()
    {
        
        Vector3 camP = new Vector3();
        camP = transform.position - cam.position;
        Vector3 cameraPlayer2D = new Vector3(camP.x, 0, camP.z);
        cameraPlayer2D = cameraPlayer2D.normalized;
        Vector3 movement =  Vector3.zero;

        //création du raycast vers le bas
        Vector3 direction = transform.up * -1;
        Debug.DrawRay(transform.position, direction * hitRange, Color.red);
        RaycastHit hit ;
        Physics.Raycast(transform.position, direction,  out hit, hitRange);
        
        //if 
        if (hit.collider != null || firstTime)
        {
            print("Move");
            // règle  le mouvement de gauche droite en fonction du player et le mouvement forward backward en fonction  de la camera
            if(moveVector.x != 0){
                movement = transform.right * moveVector.x;
            }

            if(moveVector.y != 0){
                movement = cameraPlayer2D * moveVector.y;
            }
            movement *= movementSpeed;
            if (transform.position != startingPoint.position){ //met le firstime à false
                firstTime =false;
            }
        }else{
            print("Stop");
            if(moveVector.x != 0){
                movement = transform.right * moveVector.x;
            }

            if(moveVector.y != 0){
                movement = cameraPlayer2D * moveVector.y;
            }
            movement *= movementSpeed;
            
        }
    

        

        if(movement!= Vector3.zero ){ // movement.magnitude.zero => on peut faire ça aussi , magnitude c'est la longueur du vecteur
            // permet que le perso regarde vers là où il va, le perso se tourne doucement
            transform.forward = cameraPlayer2D;

            
        }

        controller.Move( movement * Time.deltaTime); //le joueur se déplace Move est une méthode du player controller


        
    }

}
