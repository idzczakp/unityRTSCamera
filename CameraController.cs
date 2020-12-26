using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /* To turn off the visibility 
     * of the float inputs in the editor 
     * just delete [SerializeField] 
     * from the beginning of each float line */

    public Transform cameraTransform;

    [SerializeField] private float _camSpeed = 1f; //Speed of the camera
    [SerializeField] private float _camSpeedFast = 5f; //Speed of the camera while holding "Fast camera movement button"

    [SerializeField] private float _camMovementSpeed = 1f;
    [SerializeField] private float _camSmoothness = 10f;

    [SerializeField] private float _camRotationAmount = 1f;
    [SerializeField] private float _camBorderMovement = 5f;

    [SerializeField] private float _maxCamZoom = 10f;
    [SerializeField] private float _minCamZoom = 100f;

    [SerializeField] private float _minZCamMovement = 100f;
    [SerializeField] private float _maxZCamMovement = 900f;
    [SerializeField] private float _minXCamMovement = 100f;
    [SerializeField] private float _maxXCamMovement = 900f;

    [SerializeField] private bool cursorVisible = true;

    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    //MouseMovement
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    Vector2 pos1;
    Vector2 pos2;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        //Scroll zooming
        if (Input.mouseScrollDelta.y != 0)
        {

            newZoom += Input.mouseScrollDelta.y * zoomAmount;

            if (newZoom.y <= _maxCamZoom) //Max zoom limit
            {
               newZoom = new Vector3(0, _maxCamZoom, -30);

            } else if (newZoom.y >= _minCamZoom) //Min zoom limit
            {
                newZoom = new Vector3(0, _minCamZoom, -120);
            }

        }

        //Camera rotating on mouse scroll button hold


        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        } 
        else {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButton(2))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = cursorVisible;

            newRotation *= Quaternion.Euler(Vector3.up * Input.GetAxis("Mouse X"));
        }

    }

    void HandleMovementInput()

        //Fast camera movement
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _camMovementSpeed = _camSpeedFast;
        }
        else
        {
            _camMovementSpeed = _camSpeed;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - _camBorderMovement)
        {
            newPosition += (transform.forward * _camMovementSpeed);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= _camBorderMovement)
        {
            newPosition += (transform.forward * -_camMovementSpeed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - _camBorderMovement)
        {
            newPosition += (transform.right * _camMovementSpeed);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= _camBorderMovement)
        {
            newPosition += (transform.right * -_camMovementSpeed);
        }

        //Keyboard setup for camera rotate
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * _camRotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -_camRotationAmount);
        }

        //Keyboard setup for camera zoom
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;

            //Max zoom limit
            if (newZoom.y <= 30)
            {
                newZoom = new Vector3(0, 30, -30);

            }
            
        }

        //Min zoom limit
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
            if (newZoom.y >= 120)
            {
                newZoom = new Vector3(0, 120, -120);
            }
        }

        //Setting Borders
        if (newPosition.x < _minXCamMovement)
        {
            newPosition = new Vector3(_minXCamMovement, transform.position.y, transform.position.z);

        } else if(newPosition.x > _maxXCamMovement)
        {
            newPosition = new Vector3(_maxXCamMovement, transform.position.y, transform.position.z);
        }

        if (newPosition.z < _minZCamMovement)
        {
            newPosition = new Vector3(transform.position.x, transform.position.y, _minZCamMovement);

        }
        else if (newPosition.z > _maxZCamMovement)
        {
            newPosition = new Vector3(transform.position.x, transform.position.y, _maxZCamMovement);
        }


        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _camSmoothness);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * _camSmoothness);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * _camSmoothness);
    }

   
}
