using UnityEngine;
using System.Collections;

public class PlayerCameraController : MonoBehaviour {

    public Transform target; // public reference to target

    [System.Serializable]
    public class PositionSettings {
        public Vector3 targetPosOffset = new Vector3(0, 3.4f, 0); // target offset position
        public float lookSmooth = 100f; // speed of camera look movement
        public float distanceFromTarget = -8; // variable will be modified with player's camera zooming
        public float zoomSmooth = 100; // speed of camera zoom movement
        public float maxZoom = -2; // max zoom distance from player - max zoom threshold
        public float minZoom = -8; // min zoom distance from player - min zoom threshold
        public bool smoothFollow = true; // option for smooth camera movement
        public float smooth = 0.05f; // set number for smooth camera movement

        [HideInInspector]
        public float newDistance = -8; // set by zoom input
        [HideInInspector]
        public float adjustmentDistance = -8; // set number for adjusting distance between camera & player
    }

    [System.Serializable]
    public class OrbitSettings {
        public float xRotation = -5; // modify camera's X rotation
        public float yRotation = -180; // modify camera's Y rotation
        public float maxXRotation = 25; // max X rotation (stop camera from going over player)
        public float minXRotation = -85; // min X rotation (stop camera from going under player)
        public float vOrbitSmooth = 150; // speed of camera's smooth vertical orbit movement - rotation X axis
        public float hOrbitSmooth = 150; // speed of camera's smooth horizontal orbit movement - rotation Y axis
    }

    [System.Serializable]
    public class InputSettings {
        public string MOUSE_ORBIT = "MouseOrbit";
        public string MOUSE_ORBIT_VERTICAL = "MouseOrbitVertical";
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap"; // input - snaps the camera Y rotation back to the target
        public string ORBIT_HORIZONTAL = "OrbitHorizontal"; // input - rotates the camera on the Y axis
        public string ORBIT_VERTICAL = "OrbitVertical"; // input - rotates the camera on the X axis
        public string ZOOM = "Mouse ScrollWheel"; // input - zooms the camera closer or further away from the target 

        public string MOUSE_HORIZONTAL = "Mouse X";
        public string MOUSE_VERTICAL = "Mouse Y";
    }

    [System.Serializable]
    public class DebugSettings {
        public bool drawDesiredCollisionLines = true; // debug variable to draw desired or adjusted
        public bool drawAdjustedCollisionLines = true; // debug variable to draw desired or adjusted
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public DebugSettings debug = new DebugSettings();
    public CollisionHandler collision = new CollisionHandler();

    Vector3 targetPos = Vector3.zero; // equal to "target" & "targetPosOffset
    Vector3 destination = Vector3.zero; // destination of the camera - camera not colliding
    Vector2 adjustedDestination = Vector3.zero; // destination of the camera - camera is colliding
    Vector3 camVel = Vector3.zero; // used for smooth camera movement
    PlayerController playerController; // reference to PlayerController script
    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput, mouseOrbitInput, vMouseOrbitInput, vOrbitMouseInput, hOrbitMouseInput; // orbit input values
    Vector3 previousMousePos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;

    public Vector3 gravityUpwards = Vector3.up;


    void Start() {
        SetCameraTarget(target);

        vOrbitInput = hOrbitInput = zoomInput = hOrbitSnapInput = mouseOrbitInput = vMouseOrbitInput = vOrbitMouseInput = hOrbitMouseInput = 0;

        MoveToTarget();

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        previousMousePos = currentMousePos = Input.mousePosition;

        Cursor.lockState = CursorLockMode.Locked; //Lock cursor
        gravityUpwards = Vector3.up;
    }

    void SetCameraTarget(Transform t) {
        target = t;

        if (target != null) {
            if (target.GetComponent<PlayerController>()) {
                playerController = target.GetComponent<PlayerController>();
            } else {
                Debug.LogError("The camera's target needs a character controller.");
            }
        } else {
            Debug.LogError("Camera needs a target.");
        }
    }

    void GetInput() {

        vOrbitInput = Input.GetAxisRaw(input.ORBIT_VERTICAL);
        hOrbitInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL);
        hOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL_SNAP);
        zoomInput = Input.GetAxisRaw(input.ZOOM);
        mouseOrbitInput = Input.GetAxisRaw(input.MOUSE_ORBIT);
        vMouseOrbitInput = Input.GetAxisRaw(input.MOUSE_ORBIT_VERTICAL);

        vOrbitMouseInput = Input.GetAxisRaw(input.MOUSE_VERTICAL);
        hOrbitMouseInput = Input.GetAxisRaw(input.MOUSE_HORIZONTAL);
    }

    void Update() {

        GetInput();
        OrbitTarget();
        ZoomInOnTarget();
    }

    void FixedUpdate() {

        // moving
        MoveToTarget();
        // rotating
        LookAtTarget();
        // player input orbit
        OrbitTarget();
        MouseOrbitTarget();

        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        // draw debug lines
        for (int i = 0; i < 5; i++) {
            if (debug.drawDesiredCollisionLines) {
                Debug.DrawLine(targetPos, collision.desiredCameraClipPoints[i], Color.white);
            }

            if (debug.drawAdjustedCollisionLines) {
                Debug.DrawLine(targetPos, collision.adjustedCameraClipPoints[i], Color.green);
            }
        }

        collision.CheckColliding(targetPos); // using raycasts here
        position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPos);
    }

    void MoveToTarget() {
        targetPos = target.position + Vector3.up * position.targetPosOffset.y + Vector3.forward * position.targetPosOffset.z + transform.TransformDirection(Vector3.right * position.targetPosOffset.x);
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos;

        if (collision.colliding) {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * position.adjustmentDistance;
            adjustedDestination = targetPos;
            //adjustedDestination += targetPos;

            if (position.smoothFollow) {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref camVel, position.smooth);
            } else
                transform.position = adjustedDestination;
        } else {
            if (position.smoothFollow) {
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVel, position.smooth);
            } else
                transform.position = destination;

        }
    }

    void LookAtTarget() {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position, gravityUpwards);
        //print(gravityUpwards);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
    }

    void OrbitTarget() {
        if (hOrbitSnapInput > 0) {
            orbit.yRotation = -180;
        }

        //orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        //orbit.yRotation += -hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;

        orbit.xRotation += vOrbitMouseInput * orbit.vOrbitSmooth * Time.deltaTime;
        //orbit.yRotation += hOrbitMouseInput * orbit.hOrbitSmooth * Time.deltaTime;

        orbit.xRotation = Mathf.Clamp(orbit.xRotation, orbit.minXRotation, orbit.maxXRotation);
    }

    void MouseOrbitTarget() {
        // getting the camera to orbit around our character
        previousMousePos = currentMousePos;
        currentMousePos = Input.mousePosition;

        if (mouseOrbitInput > 0) {
            orbit.yRotation += (currentMousePos.x - previousMousePos.x) * orbit.vOrbitSmooth;
        }
    }

    void ZoomInOnTarget() {
        position.distanceFromTarget += zoomInput * position.zoomSmooth * Time.deltaTime;

        position.distanceFromTarget = Mathf.Clamp(position.distanceFromTarget, position.minZoom, position.maxZoom);
    }

    [System.Serializable]
    public class CollisionHandler {
        public LayerMask collisionLayer; // player's camera collision on gameobject with this layer

        [HideInInspector]
        public bool colliding = false; // bool to determine player's camera collision
        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints; // clip points - current position
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints; // clip points - expected position if no collision

        Camera camera; // Reference to the player's camera

        public void Initialize(Camera cam) {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray) {
            if (!camera)
                return;

            // clear the contents of intoArray
            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
            float y = x / camera.aspect;

            // top left
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition; // added and rotate the point relative to camera
            // top right
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition; // added and rotate the point relative to camera
            // bottom left
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition; // added and rotate the point relative to camera
            // bottom right
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition; // added and rotate the point relative to camera
            // camera's position
            intoArray[4] = cameraPosition - camera.transform.forward;
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition) {
            for (int i = 0; i < clipPoints.Length; i++) {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);

                if (Physics.Raycast(ray, distance, collisionLayer)) {
                    return true;
                }
            }

            return false;
        }

        public float GetAdjustedDistanceWithRayFrom(Vector3 from) {
            float distance = -1;

            for (int i = 0; i < desiredCameraClipPoints.Length; i++) {
                Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) {
                    if (distance == -1) {
                        distance = hit.distance;
                    } else {
                        if (hit.distance < distance) {
                            distance = hit.distance;
                        }
                    }
                }
            }

            if (distance == -1)
                return 0;
            else
                return distance;
        }

        public void CheckColliding(Vector3 targetPosition) {
            if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition)) {
                colliding = true;
            } else {
                colliding = false;
            }
        }
    }
}
