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
    public Transform rayFrontUnder;
    public float durationRotation = 2f;
    public List<Transform> rays = new List<Transform>(4);
    public bool isRotating = false;

    public Transform pivot;


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
        //groupe de 4 raycasts
        Vector3 direction = transform.up * -1;
        List<RaycastHit> raycasts = new List<RaycastHit>();
        for(int i = 0; i < rays.Count; i++){
            Debug.DrawRay(rays[i].position, direction * hitRange, Color.red);
            RaycastHit hit ;
            Physics.Raycast(rays[i].position, direction,  out hit, hitRange);
            raycasts.Add(hit);
        }

        Vector3 camP = new Vector3();
        camP = transform.position - cam.position;
        Vector3 cameraPlayer2D = new Vector3(camP.x, 0, camP.z).normalized;

        //récupération de la normal when rayforward hit
        if(raycasts[0].collider != null && !isRotating){
            Vector3 result = raycasts[0].normal;
            // Vector3[] result = platforme.GetComponent<MeshFilter>().mesh.normals;
            if(result == Vector3.up || result == Vector3.down){
                cameraPlayer2D = new Vector3(camP.x, 0, camP.z).normalized;
            }else if(result == Vector3.left || result == Vector3.right){
                cameraPlayer2D = new Vector3(0, camP.y, camP.z).normalized;
            }else if(result == Vector3.forward || result == Vector3.back){
                cameraPlayer2D = new Vector3(camP.x, camP.y, 0).normalized;
            }           
            print(result);
        }

        // cameraPlayer2D = cameraPlayer2D.normalized;
        Vector3 movement =  Vector3.zero;

        //création du raycast vers le bas
        Debug.DrawRay(transform.position, direction * hitRange, Color.green);
        RaycastHit objectHit;
        Physics.Raycast(transform.position, direction,  out objectHit, hitRange);

        //création du raycast vers front
        Vector3 front = transform.forward;
        Debug.DrawRay(rayFront.position, front * (hitRange * 2), Color.blue);
        RaycastHit rayFrontHit ;
        Physics.Raycast(rayFront.position, front,  out rayFrontHit, hitRange);

        //création du raycast vers front en bas
        Vector3 under = transform.forward;
        Debug.DrawRay(rayFrontUnder.position, front * (hitRange * 2), Color.blue);
        RaycastHit rayFrontUndertHit ;
        Physics.Raycast(rayFrontUnder.position, front,  out rayFrontUndertHit, hitRange);
        
        //condition pour activer la rotationvers le bas
        if(rayFrontHit.collider == null && rayFrontUndertHit.collider != null && raycasts[0].collider == null && objectHit.collider == null){
            print("rotatationdown devient true");
            if(!isRotating){
                StartCoroutine(RotationDown());
                

            }
            // transform.Rotate(90f, 0f, 0f);
            
        }

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
        if(raycasts[0].collider == null && !firstTime && moveVector.y > 0 ){
            moveVector.y = 0;
            //faut rajouter une condition pour que ça avance un peu plus pour activer la condition de la rotation    
            
        }
        if(raycasts[1].collider == null && !firstTime && moveVector.y < 0 && !isRotating){
            moveVector.y = 0; 
        }    
        
        // limite movement gauche droite
        if(raycasts[2].collider == null && !firstTime && moveVector.x > 0 && !isRotating){
            moveVector.x = 0;          
        }    
        if(raycasts[3].collider == null && !firstTime && moveVector.x < 0 && !isRotating){
            moveVector.x = 0;       
        }    
        // règle le mouvement forward backward en fonction  de la camera et le mouvement de gauche droite en fonction du player       
        movement = cameraPlayer2D * moveVector.y + transform.right * moveVector.x;
        movement *= movementSpeed;
    

        if(movement!= Vector3.zero ){ // movement.magnitude.zero => on peut faire ça aussi , magnitude c'est la longueur du vecteur
            // permet que le perso regarde vers là où il va, le perso se tourne doucement
            transform.forward = cameraPlayer2D;
            controller.Move( movement * Time.deltaTime); //le joueur se déplace Move est une méthode du player controller

            
        }
        
        
        
    }

    IEnumerator RotationDown(){
        isRotating = true;
        float time = 0;
        Quaternion start = transform.rotation;
        Quaternion r = Quaternion.Euler(start.eulerAngles + Vector3.right * 90);
        float ratio = 0;
        Vector3 basePosition = transform.position;

        while (ratio <1f){
            transform.position = basePosition;
            time += Time.deltaTime;
            ratio = time / durationRotation;
            // print(ratio);
            transform.rotation =  Quaternion.Lerp(start, r, ratio);
            //transform.Rotate(Vector3.right * speedRotation * Time.deltaTime, Space.Self);
            //Quaternion angulu = Quaternion.Lerp(start, r, ratio);
            // transform.Rotate(Vector3.right * 90, Space.Self);
            
            yield return true;
            // yield return new WaitForEndOfFrame(); c'est pareil que yield return true
        }
        
        isRotating = false;

    }


}
