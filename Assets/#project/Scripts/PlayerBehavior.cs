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
    private bool footRotateDown;
    public Transform rayFront;
    public SphereCaptor captor;
    public List<Transform> rays = new List<Transform>(4);


    // Start is called before the first frame update
    void Start()
    {
        firstTime = true;
        footRotateDown = false;
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

        //groupe de 4 raycasts
        Vector3 direction = transform.up * -1;
        List<RaycastHit> raycasts = new List<RaycastHit>();
        for(int i = 0; i < rays.Count; i++){
            Debug.DrawRay(rays[i].position, direction * hitRange, Color.red);
            RaycastHit hit ;
            Physics.Raycast(rays[i].position, direction,  out hit, hitRange);
            raycasts.Add(hit);
        }

        //création du raycast vers le bas
        Debug.DrawRay(transform.position, direction * hitRange, Color.green);
        RaycastHit objectHit;
        Physics.Raycast(transform.position, direction,  out objectHit, hitRange);

        //création du raycast vers front
        Vector3 front = transform.forward;
        Debug.DrawRay(rayFront.position, front * (hitRange * 2), Color.blue);
        RaycastHit rayFrontHit ;
        Physics.Raycast(rayFront.position, front,  out rayFrontHit, hitRange);
        
        //condition pour activer la rotation


        //vérification firstTime
        if(firstTime){
            if(moveVector.y != 0 || moveVector.x != 0){
                movement = cameraPlayer2D * moveVector.y + transform.right * moveVector.x;
            }
            if (transform.position != startingPoint.position){ //met le firstime à false
                firstTime =false;
            }
        }

        
        
        // limite le movement avant arrière
        if(raycasts[0].collider == null && !firstTime && moveVector.y > 0 && !footRotateDown){
            moveVector.y = 0;      
        }
        if(raycasts[1].collider == null && !firstTime && moveVector.y < 0 && !footRotateDown){
            moveVector.y = 0; 
        }    
        
        // limite movement gauche droite
        if(raycasts[2].collider == null && !firstTime && moveVector.x > 0 && !footRotateDown){
            moveVector.x = 0;          
        }    
        if(raycasts[3].collider == null && !firstTime && moveVector.x < 0 && !footRotateDown){
            moveVector.x = 0;       
        }    
        // règle le mouvement forward backward en fonction  de la camera et le mouvement de gauche droite en fonction du player       
        movement = cameraPlayer2D * moveVector.y + transform.right * moveVector.x;
        movement *= movementSpeed;
    

        if(movement!= Vector3.zero ){ // movement.magnitude.zero => on peut faire ça aussi , magnitude c'est la longueur du vecteur
            // permet que le perso regarde vers là où il va, le perso se tourne doucement
            transform.forward = cameraPlayer2D;

            
        }

        controller.Move( movement * Time.deltaTime); //le joueur se déplace Move est une méthode du player controller


        
    }

}
