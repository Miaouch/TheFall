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
    public Transform startingPoint;
    private bool firstTime;
    public Transform rayFront;
    public Transform rayFrontUnder;
    // public Transform rayForward;
    public float durationRotation = 2f;
    public List<Transform> rays = new List<Transform>(4);
    public bool isRotating = false;
    public bool moveFurther;
    public bool interactions;


    public Transform pivot;



    // Start is called before the first frame update
    void Start()
    {
        firstTime = true;

    }

    // Update is called once per frame
    public void Movement(InputAction.CallbackContext context)
    { //cette méthode lie à l'inpput player pour le déplacement
        moveVector = context.ReadValue<Vector2>();
        // Debug.Log(moveVector);

    }

    public void Interactions(InputAction.CallbackContext context){
        if(context.performed && interactions){ //performed indique que l'action est en train de se faire
            Debug.Log("Pivot activated");
        }else if(context.canceled && interactions){
            Debug.Log("Pivot desactivated");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        print("Rotation ?");
        if (other.CompareTag("RotationTrigger") && !isRotating)
        {
            StartCoroutine(RotateAroundTheEdge(other.transform));
        }
    }
    IEnumerator RotateAroundTheEdge(Transform pivotPoint) {

        
        Plane plane = new Plane(pivotPoint.up - pivotPoint.right, pivotPoint.position);
        Ray downRay = new Ray(transform.position, -transform.up);

        float dist;
        plane.Raycast(downRay, out dist);

        Vector3 rotationPoint = downRay.GetPoint(dist);

        float startTime = Time.time;
        isRotating = true;
        while (Time.time <= durationRotation + startTime) {
            transform.RotateAround(rotationPoint, pivotPoint.forward, -90 / durationRotation * Time.deltaTime);
            yield return true;
        }

        isRotating = false;
    }
    void Update()
    {
        //groupe de 4 raycasts
        Vector3 direction = transform.up * -1;
        List<RaycastHit> raycasts = new List<RaycastHit>();
        for (int i = 0; i < rays.Count; i++)
        {
            Debug.DrawRay(rays[i].position, direction * hitRange, Color.red);
            RaycastHit hit;
            Physics.Raycast(rays[i].position, direction, out hit, hitRange);
            raycasts.Add(hit);
        }



        //création du raycast vers le bas
        RaycastHit objectHit;
        Color e = Color.red;
        if(Physics.Raycast(transform.position, direction, out objectHit, hitRange)){
            e = Color.green; }
        Debug.DrawRay(transform.position, direction * hitRange, e);

        //création du raycast vers le forward pour test si plateform plane
        // RaycastHit forwardHit;
        // Color f = Color.red;
        // if(Physics.Raycast(rayForward.position, direction, out forwardHit, hitRange)){
        //     f = Color.green; }
        // Debug.DrawRay(rayForward.position, direction * hitRange, f);
        // if(forwardHit.collider != null || firstTime){
        //     //activate la possibilité d'intéragir
        //     interactions = true;
        //     List<GameObject> listPivots = new List<GameObject>();
        //     // listPivots = objectHit.transform.gameObject.GetComponent<PlateformBehavior>().pivots;
        //     // for(int i = 0; i < listPivots.Count; i++){
        //     //     listPivots[i].SetActive(true);
        //     // }
            
        // }else{
        //     interactions = false;
        // }

        Vector3 camP = new Vector3();
        camP = transform.position - cam.position;
        Vector3 cameraPlayer2D = new Vector3(camP.x, 0, camP.z).normalized;

        //récupération de la normal when rayforward hit
        // if(objectHit.collider != null && !isRotating){
        //     Vector3 result = objectHit.normal;
        //     // Vector3[] result = platforme.GetComponent<MeshFilter>().mesh.normals;
        //     if(result == Vector3.up || result == Vector3.down){
        //         cameraPlayer2D = new Vector3(camP.x, 0, camP.z).normalized;
        //         print("up = y");
        //     }else if(result == Vector3.left || result == Vector3.right){
        //         cameraPlayer2D = new Vector3(0, camP.y, camP.z).normalized;
        //         print("up = x");
        //     }else if(result == Vector3.forward || result == Vector3.back){
        //         cameraPlayer2D = new Vector3(camP.x, camP.y, 0).normalized;
        //         print("up = z");
        //     }           
        //     print(result);
        // }

        // cameraPlayer2D = cameraPlayer2D.normalized;
        Vector3 movement = Vector3.zero;

        //création du raycast vers front
        Vector3 front = transform.forward;
        RaycastHit rayFrontHit;
        Color c = Color.red;
        if (Physics.Raycast(rayFront.position, front, out rayFrontHit, hitRange))
        { c = Color.green; }
        Debug.DrawRay(rayFront.position, front * (hitRange * 2), c);

        //création du raycast vers front en bas
        Vector3 under = transform.forward;
        RaycastHit rayFrontUndertHit;
        Color d = Color.red;
        if(Physics.Raycast(rayFrontUnder.position, front, out rayFrontUndertHit, hitRange))
        { d=Color.green;}
        Debug.DrawRay(rayFrontUnder.position, front * (hitRange * 2), d);

        

        //condition pour activer la rotationvers le bas
        // if (rayFrontHit.collider == null && rayFrontUndertHit.collider != null )
        // {
        //     print("rotation");
        //     moveFurther = true;
        //     if (!isRotating && objectHit.collider == null)
        //     {
        //         StartCoroutine(RotationDown(rayFrontUndertHit));

        //     }
        //     // transform.Rotate(90f, 0f, 0f);
        // }else{
        //     moveFurther = false;
        // }

        //vérification firstTime
        if (firstTime)
        {
            if (moveVector.y != 0)
            {
                movement = transform.forward * moveVector.y;
                // movement =  new Vector3(moveVector.x, 0, moveVector.y) * movementSpeed;
            }

            if (transform.position != startingPoint.position)
            { //met le firstime à false
                firstTime = false;
            }
        }



        if (isRotating)
        {
            moveVector.y = 0;
            moveVector.x = 0;
        }
        // limite le movement avant arrière
        if (raycasts[0].collider == null && !firstTime && moveVector.y > 0 && !isRotating && !moveFurther)
        {
            moveVector.y = 0;
            print("stop");
            //faut rajouter une condition pour que ça avance un peu plus pour activer la condition de la rotation    

        }
        if (raycasts[1].collider == null && !firstTime && moveVector.y < 0 && !isRotating)
        {
            moveVector.y = 0;
        }

        // règle le mouvement forward backward en fonction  de la camera et le mouvement de gauche droite en fonction du player       

        if (moveVector.x > 0)
        {
            // transform.rotation = Quaternion.Euler(transform.up * 90);
            transform.Rotate(Vector3.up * 1f);
            // limite movement 
            if (raycasts[2].collider == null && !firstTime && moveVector.x > 0 && !isRotating)
            {
                moveVector.x = 0;
            }
        }
        if (moveVector.x < 0)
        {
            // transform.rotation = Quaternion.Euler(transform.up * 90);
            transform.Rotate(Vector3.up * -1f);
            if (raycasts[3].collider == null && !firstTime && moveVector.x < 0 && !isRotating)
            {
                moveVector.x = 0;
            }
        }

        // movement =  new Vector3(moveVector.x, 0, moveVector.y) * movementSpeed;
        movement = transform.forward * moveVector.y;// + transform.forward * moveVector.x;
        movement *= movementSpeed;

        if (movement != Vector3.zero)
        { // movement.magnitude.zero => on peut faire ça aussi , magnitude c'est la longueur du vecteur
          // permet que le perso regarde vers là où il va, le perso se tourne doucement
          // transform.forward = cameraPlayer2D;

            // transform.forward = Vector3.Lerp(transform.forward, movement, 0.1f);

            controller.Move(movement * Time.deltaTime); //le joueur se déplace Move est une méthode du player controller
        }


        // print("isRotating : " +isRotating);
        // if(rayFrontUndertHit.collider != null){
        //     Vector3 result = rayFrontUndertHit.normal;
        //     print(result * -1); 
        // }
    }

    // IEnumerator RotationDown(RaycastHit rayFrontUndertHit)
    // {
    //     Vector3 normal = rayFrontUndertHit.normal * -1;
    //     transform.forward = normal;
    //     Debug.DrawRay(rayFrontUndertHit.transform.position, normal * 100, Color.cyan, 10f);

    //     isRotating = true;
    //     float time = 0;
    //     // Quaternion start = transform.localRotation;
        
    //     // Quaternion r = Quaternion.Euler(start.eulerAngles + Vector3.right * 90);
    //     Vector3 start = transform.up;
    //     Vector3 end = normal;

    //     float ratio = 0;
    //     Vector3 basePosition = transform.position;
    //     transform.up = normal;

    //     while (ratio < 1f && false)
    //     {
    //         transform.position = basePosition;
    //         time += Time.deltaTime;
    //         ratio = time / durationRotation;
    //         // print(ratio);

    //         // transform.localRotation =  Quaternion.Lerp(start, end, ratio);
    //         Debug.DrawRay(transform.position, Vector3.Lerp(start, end, ratio) * 100, Color.yellow,1f);
    //         transform.up = Vector3.Lerp(start, end, ratio);
    //         //transform.Rotate(Vector3.right * speedRotation * Time.deltaTime, Space.Self);
    //         //Quaternion angulu = Quaternion.Lerp(start, r, ratio);
    //         // transform.Rotate(Vector3.right * 90, Space.Self);

    //         yield return true;
    //         // yield return new WaitForEndOfFrame(); c'est pareil que yield return true
    //     }

    //     isRotating = false;


    // }


}
