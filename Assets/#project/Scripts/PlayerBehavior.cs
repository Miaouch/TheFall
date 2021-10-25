using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    public CharacterController controller;
    public float movementSpeed;
    Vector2 moveVector;

    public Transform camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Movement(InputAction.CallbackContext context){ //cette méthode lie à l'inpput player pour le déplacement
        moveVector = context.ReadValue<Vector2>();
        Debug.Log(moveVector);
    }
    void Update()
    {
        
        // Vector3 camP = new Vector3();
        // camP = transform.position - camera.position;
        // Vector3 cameraPlayer2D = new Vector3(camP.x, 0, camP.z);
        Vector3 camDir = new Vector3(camera.forward.x, 0f, camera.forward.z);

        Vector3 movement =  new Vector3(moveVector.x, 0, moveVector.y) * movementSpeed;
        // Vector3 finalMovement = new Vector3();
        // finalMovement = (cameraPlayer2D.normalized * movevement.y) * movementSpeed;

        if(movement!= Vector3.zero ){ // movement.magnitude.zero => on peut faire ça aussi , magnitude c'est la longueur du vecteur
            transform.forward = Vector3.Lerp(camDir, movement, 0.1f); // permet que le perso regarde vers là où il va, le perso se tourne doucement
            

            
        }
        controller.Move( movement * Time.deltaTime); //le joueur se déplace Move est une méthode du player controller

        
    }

}
