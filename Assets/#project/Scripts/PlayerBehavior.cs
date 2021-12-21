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
    public Transform rayForward;
    public Transform rayCenter;
    public float durationRotation = 2f;
    public List<Transform> rays = new List<Transform>(4);
    public bool isRotating = false;
    public bool moveFurther;
    public bool interactions;
    public bool rotationBas;
    public bool rotationHaut;
    public bool activePivot;
    public bool activeOverlay;
    public bool cubeCreated;
    public Transform head;
    public GameObject instantiateCube;
    public GameObject cubeOverlay;
    public Transform cubeOverlayPivot;
    public bool rototoUp;
    public bool rototoDown;
    public bool rototoLeft;
    public bool rototoRight;
    public bool autorisedValidation;
    public bool validate;
    public Transform plateformSource;
    public bool destroyOverlay = false;
    public bool movingPlatforme;
    public bool upDown;
    public GameObject pauseMenu;
    public bool escapeMenu;
    public AudioSource boop;
    public AudioSource boopPitch;
    public AudioSource platformSoud;


    // Start is called before the first frame update
    void Start()
    {
        firstTime = true;
        interactions = false;
        activeOverlay = false;
        cubeCreated = false;
    }

    public void PauseMenu(InputAction.CallbackContext context){
        if(context.performed && !escapeMenu){
            print("yooooooo");
            escapeMenu = true;
            pauseMenu.SetActive(true);
        }
        else if(context.performed && escapeMenu){
            escapeMenu = false;
            pauseMenu.SetActive(false);
        }
    }
    public void Movement(InputAction.CallbackContext context)
    { //cette méthode lie à l'inpput player pour le déplacement

        moveVector = context.ReadValue<Vector2>();
        // Debug.Log(moveVector);

    }

    public void Interactions(InputAction.CallbackContext context){
        if(context.performed && interactions && !activePivot ){ //performed indique que l'action est en train de se faire
            activePivot = true;
            Debug.Log(activePivot);
        }
        else if(context.performed && activePivot){
            activePivot = false;
            Debug.Log(activePivot);
        }
    }

    public void Validation(InputAction.CallbackContext context){
        if(context.performed && activePivot && autorisedValidation){
            validate = true;
            Debug.Log("validate");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // print("Rotation ?");
        if (other.CompareTag("RotationTrigger") && !isRotating && (rotationBas || rotationHaut))
        {
            StartCoroutine(RotateAroundTheEdge(other.transform, rayCenter));
        }
    }
    IEnumerator RotateAroundTheEdge(Transform pivotPoint, Transform rayCenter) {

        if(rotationBas){
            Plane plane = new Plane(pivotPoint.up - pivotPoint.right, pivotPoint.position);
            Ray downRay = new Ray(transform.position, -transform.up);

            float dist;
            plane.Raycast(downRay, out dist);

            Vector3 rotationPoint = downRay.GetPoint(dist);
            Vector3 rotationAxis = pivotPoint.forward;
            float startTime = Time.time;
            isRotating = true;
            while (Time.time <= durationRotation + startTime) {
                transform.RotateAround(rotationPoint, rotationAxis, -90 / durationRotation * Time.deltaTime);
                yield return true;
            }

            isRotating = false;
            RaycastHit centerHit;
            if(Physics.Raycast(rayCenter.position, transform.up * -1, out centerHit, 1.5f)){
                transform.position = centerHit.point + (transform.up * 0.2f);
            }
            

        }
        if(rotationHaut){
            Plane plane = new Plane(pivotPoint.up - pivotPoint.right, pivotPoint.position);
            Ray downRay = new Ray(transform.position, -transform.up);

            float dist;
            plane.Raycast(downRay, out dist);

            Vector3 rotationPoint = downRay.GetPoint(dist);
            Vector3 rotationAxis = pivotPoint.forward;
            // Vector3 start = head.position;
            // Vector3 end = start + head.transform.up * -5;
            float startTime = Time.time;
            isRotating = true;
            float time = 0f;
            
            while (Time.time <= durationRotation + startTime) {
                time+= Time.deltaTime;
                transform.RotateAround(head.position, rotationAxis, 90 / durationRotation * Time.deltaTime);
                // head.up = Vector3.Lerp(start, end, time/durationRotation);
                yield return true;
            }

            // transform.position = transform.up * 1;
            isRotating = false;
            RaycastHit centerHit;
            if(Physics.Raycast(rayCenter.position, transform.up * -1, out centerHit, 1.5f)){
                transform.position = centerHit.point + (transform.up * 0.2f);
            }
        }

    }
    void Update()
    {
        //groupe de 4 raycasts
        Vector3 direction = transform.up * -1;
        List<RaycastHit> raycasts = new List<RaycastHit>();
        for (int i = 0; i < rays.Count; i++)
        {
            RaycastHit hit;
            Color g = Color.red;
            if(Physics.Raycast(rays[i].position, direction, out hit, hitRange)){
                g = Color.green;
            }
            Debug.DrawRay(rays[i].position, direction * hitRange, g);
            raycasts.Add(hit);
        }



        //création du raycast vers le bas
        RaycastHit objectHit;
        Color e = Color.red;
        if(Physics.Raycast(transform.position, direction, out objectHit, hitRange)){
            e = Color.green; 
        }
        Debug.DrawRay(transform.position, direction * hitRange, e);
        // cubeOverlayPivot = objectHit.transform.gameObject.GetComponentInParent<PlateformBehavior>().pivots[1].transform;







        //création du raycast center
        RaycastHit centerHit;
        Color h = Color.red;
        if(Physics.Raycast(rayCenter.position, direction, out centerHit, hitRange)){
            h = Color.green;
        }
        Debug.DrawRay(rayCenter.position, direction * hitRange, h);

        //création du raycast vers le forward pour test si plateform plane
        RaycastHit forwardHit;
        Color f = Color.red; 
        if(Physics.Raycast(rayForward.position, direction, out forwardHit, hitRange)){
            f = Color.green; 
        }
        
        
 
        

        Vector3 camP = new Vector3();
        camP = transform.position - cam.position;
        Vector3 cameraPlayer2D = new Vector3(camP.x, 0, camP.z).normalized;

        //récupération de la normal when rayforward hit
        // if(objectHit.collider != null && !isRotating){
        //     Vector3 result = objectHit.normal;
        //     // Vector3[] result = platforme.GetComponent<MeshFilter>().mesh.normals;
        //     if(result == Vector3.up || result == Vector3.down){
        //         cameraPlayer2D = new Vector3(camP.x, 0, camP.z).normalized;
        
        Vector3 movement = Vector3.zero;

        //création du raycast vers front
        Vector3 front = transform.forward;
        RaycastHit rayFrontHit;
        Color c = Color.red;
        if (Physics.Raycast(rayFront.position, front, out rayFrontHit, hitRange * 2)){
            c = Color.green; 
        }
        Debug.DrawRay(rayFront.position, front * (hitRange * 2), c);

        //création du raycast vers front en bas
        Vector3 under = transform.forward;
        RaycastHit rayFrontUnderHit;
        Color d = Color.red;
        if(Physics.Raycast(rayFrontUnder.position, front, out rayFrontUnderHit,1.5f * 2)){ 
            d=Color.green;
        }
        Debug.DrawRay(rayFrontUnder.position, front * (1.5f * 2), d);

        GameObject outline;
        if(forwardHit.collider != null || rayFrontHit.collider != null || rayFrontUnderHit.collider != null){
            // while(rayFrontHit.collider != null && !activePivot){
            //     rayFrontHit.transform.gameObject.GetComponentInParent<PlateformBehavior>().cubeInteraction.SetActive(true);
            // }
            // while(rayFrontUnderHit.collider != null && !activePivot){
            //     rayFrontUnderHit.transform.gameObject.GetComponentInParent<PlateformBehavior>().cubeInteraction.SetActive(true);
            // }
            interactions = true;
        } else {
            interactions = false;
        }

        // if(forwardHit.collider != null && !activePivot){
        //     forwardHit.transform.gameObject.GetComponentInParent<PlateformBehavior>().cubeInteraction.SetActive(true);
        // }else{
        //     forwardHit.transform.gameObject.GetComponentInParent<PlateformBehavior>().cubeInteraction.SetActive(false);
        // }



        if(destroyOverlay && activePivot){
            activePivot = false;
        }

        if(activePivot && !cubeCreated && !validate){
            if(forwardHit.collider != null){
                cubeOverlay = Instantiate(instantiateCube, forwardHit.transform.position, Quaternion.identity);
                cubeOverlay.transform.rotation = forwardHit.transform.rotation;
                boop.Play();
                cubeCreated = true;
                plateformSource = forwardHit.transform.gameObject.transform;
            } else if(rayFrontHit.collider != null){
                cubeOverlay = Instantiate(instantiateCube, rayFrontHit.transform.position, Quaternion.identity);
                cubeOverlay.transform.rotation = rayFrontHit.transform.rotation;
                boop.Play();
                cubeCreated = true;
                plateformSource = rayFrontHit.transform.gameObject.transform;
            } else if(rayFrontUnderHit.collider != null){
                cubeOverlay = Instantiate(instantiateCube, rayFrontUnderHit.transform.position, Quaternion.identity);
                cubeOverlay.transform.rotation = rayFrontUnderHit.transform.rotation;
                boop.Play();
                cubeCreated = true; 
                plateformSource = rayFrontUnderHit.transform.gameObject.transform;
            }
            // plateformBox1.GetComponent<BoxCollider>().enabled = false;
        }else if(!activePivot && cubeCreated){
            Destroy(cubeOverlay);
            cubeCreated = false;
            destroyOverlay = false;
            // plateformBox1.GetComponent<BoxCollider>().enabled = true;
        }

        //creation du raycast de l'overlay
        if(cubeCreated){
            RaycastHit rayOverlay;
            Color k = Color.red;
            if(Physics.Raycast(cubeOverlay.transform.position + (cubeOverlay.transform.up * -0.5f) , (cubeOverlay.transform.up * 1), out rayOverlay, hitRange)){
                k = Color.green;
            }
            Debug.DrawRay(cubeOverlay.transform.position + (cubeOverlay.transform.up * -0.5f) , (cubeOverlay.transform.up * 1) * hitRange, k);
            if(rayOverlay.collider != null){
                autorisedValidation = false;
            } else{
                autorisedValidation = true;
            }
            // if(Physics.Raycast(cubeOverlay.transform.position + (cubeOverlay.transform.forward * -2) + (cubeOverlay.transform.up * 2), (cubeOverlay.transform.forward * -1), out rayOverlay, hitRange)){
            //     k = Color.green;
            //     if(moveVector.y > 0){
            //         moveVector.y = 0;
            //     }
            // }
            // Debug.DrawRay(cubeOverlay.transform.position + (cubeOverlay.transform.forward * -2) + (cubeOverlay.transform.up * 2), (cubeOverlay.transform.forward * -1) * hitRange, k);
        }

        //réglage condition de rotation de l'overlay
        if(moveVector.y > 0 && activePivot && !movingPlatforme){
            rototoUp = true;
            rototoLeft = false;
            rototoRight = false;
        }else if(moveVector.y < 0 && activePivot && !movingPlatforme){
            rototoDown = true;
            rototoLeft = false;
            rototoRight = false;
        }
        else if(moveVector.y == 0 && activePivot && !movingPlatforme){
            rototoUp = false;
            rototoDown =false;
        }
        if(moveVector.x > 0 && activePivot && !movingPlatforme){
            rototoRight = true;
            rototoUp = false;
            rototoDown =false;
        }else if(moveVector.x < 0 && activePivot && !movingPlatforme){
            rototoLeft = true;
            rototoUp = false;
            rototoDown =false;
        }else if(moveVector.x == 0 && activePivot && !movingPlatforme){
            rototoLeft = false;
            rototoRight = false;
        }


        
        //activation des rotation de l'overlay ou pas
        if(activePivot && cubeOverlay.transform.position == plateformSource.transform.position){
            cubeOverlayPivot = objectHit.transform.gameObject.GetComponentInParent<PlateformBehavior>().pivots[1].transform;
            if(rototoUp){
                upDown = true;
                cubeOverlay.transform.RotateAround(cubeOverlayPivot.position, cubeOverlayPivot.right * -1, 90);
                boopPitch.Play();
            }else if(rototoDown){
                upDown = true;
                // cubeOverlayPivot = objectHit.transform.gameObject.GetComponentInParent<PlateformBehavior>().pivots[1].transform;
                cubeOverlay.transform.RotateAround(cubeOverlayPivot.position, cubeOverlayPivot.right, 90);
                boopPitch.Play();
            }
        }
        else if(rototoDown && upDown){
            cubeOverlay.transform.RotateAround(cubeOverlayPivot.position, cubeOverlayPivot.right, 90);
            boopPitch.Play();
        }
        else if(rototoUp && upDown){
            cubeOverlay.transform.RotateAround(cubeOverlayPivot.position, cubeOverlayPivot.right * -1, 90);
            boopPitch.Play();
        }
        if(activePivot && cubeOverlay.transform.position == plateformSource.transform.position){
            if(rototoRight){
                upDown =false;
                //condition de si raycast dedans overlay sur platformSource alors replacer le pivot et le left et le right se font en fonctionde ce pivot
                cubeOverlayPivot = objectHit.transform.gameObject.GetComponentInParent<PlateformBehavior>().pivots[0].transform;
                cubeOverlay.transform.RotateAround(cubeOverlayPivot.position, cubeOverlayPivot.up, 90);
                boopPitch.Play();
            }else if(rototoLeft){
                upDown =false;  
                cubeOverlayPivot = objectHit.transform.gameObject.GetComponentInParent<PlateformBehavior>().pivots[2].transform;
                cubeOverlay.transform.RotateAround(cubeOverlayPivot.position, cubeOverlayPivot.up * -1, 90);
                boopPitch.Play();
            }
        }else if(rototoLeft && !upDown){
            cubeOverlay.transform.RotateAround(cubeOverlayPivot.position, cubeOverlayPivot.up * -1, 90);
            boopPitch.Play();
        }else if(rototoRight && !upDown){
            cubeOverlay.transform.RotateAround(cubeOverlayPivot.position, cubeOverlayPivot.up, 90);
            boopPitch.Play();
        }
        
        if(validate){
            StartCoroutine(MovePlatforme(plateformSource, cubeOverlay.transform));
            platformSoud.Play();
            validate= false;
        }

        Debug.DrawRay(rayForward.position, direction * hitRange, f);
            //activate la possibilité d'intéragir
            // List<GameObject> listPivots = new List<GameObject>();
            // listPivots = objectHit.transform.gameObject.GetComponent<PlateformBehavior>().pivots;
            // for(int i = 0; i < listPivots.Count; i++){
            //     listPivots[i].SetActive(true);
        // }
        

        //condition pour activer la rotationvers le bas
        if (rayFrontHit.collider == null && rayFrontUnderHit.collider != null)
        {
            rotationBas = true;
        }else{
            rotationBas = false;
        }

        //condition pour activer la rotationvers le haut
        if(rayFrontHit.collider != null && rayFrontUnderHit.collider == null ){
            rotationHaut = true;
        }else {
            rotationHaut = false;
        }


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

        if (isRotating || cubeCreated || escapeMenu)
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
        { 

            controller.Move(movement * Time.deltaTime); //le joueur se déplace Move est une méthode du player controller
        }


        // print("isRotating : " +isRotating);
        // if(rayFrontUndertHit.collider != null){
        //     Vector3 result = rayFrontUndertHit.normal;
        //     print(result * -1); 
        // }
    }
        IEnumerator MovePlatforme(Transform plateformeMove, Transform cubeOverlay){
            MeshRenderer renderOverlay;
            renderOverlay = cubeOverlay.GetComponent<MeshRenderer>();
            float startTime = Time.time;
            float time = 0f;
            // print(plateformeMove.parent.name);
            float dissolve = 0.8f;
            while(Time.time <= 2f + startTime){
                movingPlatforme = true;
                time += Time.deltaTime;
                dissolve += Time.deltaTime;
                renderOverlay.material.SetFloat("_Disolve", dissolve/2f);
                plateformeMove.parent.position = Vector3.Lerp(plateformeMove.parent.position, cubeOverlay.position, time/2f);
                plateformeMove.parent.rotation = Quaternion.Lerp(plateformeMove.parent.rotation, cubeOverlay.rotation, time/2f);
                yield return true;
            }
           movingPlatforme =false;
           destroyOverlay =true;
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
