using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float runningSpeed = 10f;

    [Header("카메라 회전")]
    [SerializeField] private Transform camTransform;

    [SerializeField] private bool isFirst = true;
    [SerializeField] private bool isThird = false;
    [SerializeField] private bool isFree = false;

    [SerializeField] private float mouseSensitivity = 1f;

    private Vector3 firstViewOffset = new Vector3(0f, 0.5f, 0.25f);
    private Vector3 thirdViewOffset = new Vector3(0f, 3f, -4.5f);
    private Vector3 lastCamPos;

    private float pitch = 0;
    private float yaw = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        ChangeViewpoint();

        if (isFirst)
        {
            MovePlayer();
            RotateFirstView();
        }
        else if (isThird)
        {
            MovePlayer();
            RotateThridView();
        }
        else if (isFree)
        {
            MoveCamera();
            RotateFreeView();
        }
    }

    private void MovePlayer()
    {
        Vector3 inputAxis = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(inputAxis * runningSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(inputAxis * movementSpeed * Time.deltaTime);
        }
    }
    private void RotatePlayer()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime);
    }
    private void ChangeViewpoint()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isFirst)
        {
            isFirst = true;
            isThird = false;
            isFree = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isThird)
        {
            isFirst = false;
            isThird = true;
            isFree = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !isFree)
        {
            camTransform.position = transform.position + transform.rotation * firstViewOffset;
            camTransform.rotation = transform.rotation;

            isFirst = false;
            isThird = false;
            isFree = true;
        }
    }
    private void RotateFirstView()
    {
        RotatePlayer();
        camTransform.position = transform.position + transform.rotation * firstViewOffset;
        camTransform.rotation = transform.rotation;
    }
    private void RotateThridView()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            pitch = transform.rotation.eulerAngles.x;
            yaw = transform.rotation.eulerAngles.y;

            lastCamPos = camTransform.position;
        }
        else if (Input.GetKey(KeyCode.V))
        {
            Vector2 mouse = new Vector2(Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime,
                                        Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime);

            yaw += mouse.x;
            pitch += mouse.y;
            pitch = Mathf.Clamp(pitch, -30f, 45f);

            camTransform.position = transform.position + Quaternion.Euler(pitch, yaw, 0f) * thirdViewOffset;
        }
        else if (Input.GetKeyUp(KeyCode.V))
        {
            camTransform.position = lastCamPos;
        }
        else
        {
            RotatePlayer();
            camTransform.position = transform.position + transform.rotation * thirdViewOffset;
        }

        camTransform.LookAt(transform);
    }
    private void RotateFreeView()
    {
        Vector2 mouse = new Vector2(Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime,
                                    Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime);

        yaw += mouse.x;
        pitch -= mouse.y;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        camTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
    private void MoveCamera()
    {
        Vector3 inputAxis = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            camTransform.Translate(inputAxis * runningSpeed * Time.deltaTime);
        }
        else
        {
            camTransform.Translate(inputAxis * movementSpeed * Time.deltaTime);
        }
    }
}
