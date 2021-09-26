using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraHolder;
    public Camera cam;
    [Header("Physics")]
    [Min(0.0f)]
    public float acceleration = 10.0f;
    [Min(0.0f)]
    public float airAcceleration = 4.0f;
    [Range(0.0f, 1.0f)]
    public float dragFactor = 0.8f;
    [Range(0.0f, 1.0f)]
    public float airDragFactor = 0.5f;
    [Min(0.0f)]
    public float gravity = 2.0f;
    public float maxFallSpeed = 10.0f;
    [Min(1.0f)]
    public float jumpStrength = 6.0f;
    [Min(1.0f)]
    public float wallrunSpeed = 6.0f;

    public float wallrunCooldown = 0.2f;

    public float wallrunCameraTilt = 15.0f;
    [Header("Input")]
    [Min(1.0f)]
    public float viewSensitivity = 2.0f;

    private CharacterController controller;

    private float currentVerticalSpeed = 0.0f;

    private bool isWallrunning = false;
    private Vector3 wallNormal = Vector3.zero;

    private Vector3 currentCamSmoothVelocity = Vector3.zero;
    private Vector3 currentCamEuler = Vector3.zero;
    private float currentCamTargetTilt = 0.0f;
    private float currentCamTiltVelocity = 0.0f;

    private Vector3 currentVelocity = Vector3.zero;

    private bool grounded = false;

    private bool canWallrun = true;
    private float currentWallrunCooldown = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (!controller)
        {
            Debug.LogError("Impossible to get character controller.");
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * viewSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * viewSensitivity;

        transform.rotation *= Quaternion.Euler(0.0f, mouseX, 0.0f);
        currentCamEuler += new Vector3(-mouseY, 0.0f, 0.0f);
        cam.transform.localRotation *= Quaternion.Euler(currentCamEuler.x, currentCamEuler.y, currentCamEuler.z);

        RaycastHit hitInfo;
        if (!isWallrunning)
            Physics.Raycast(new Ray(transform.position, -transform.right), out hitInfo, 2.0f);
        else
            Physics.Raycast(new Ray(transform.position, -wallNormal), out hitInfo, 2.0f);


        if (hitInfo.collider == null && isWallrunning)
        {
            isWallrunning = false;
            canWallrun = false;
            currentWallrunCooldown = wallrunCooldown;
        }

        if (hitInfo.collider != null && !controller.isGrounded && canWallrun)
        {
            if (isWallrunning)
            {
                if (hitInfo.normal != wallNormal)
                {
                    isWallrunning = false;
                    canWallrun = false;
                    currentWallrunCooldown = wallrunCooldown;
                }
            }
            else if (Vector3.Angle(transform.forward, -hitInfo.normal) < 95f)
            {
                wallNormal = hitInfo.normal;
                isWallrunning = true;
                Vector3 previousCamHolderPos = cameraHolder.position;
                controller.Move((hitInfo.point + hitInfo.normal * (controller.radius + controller.skinWidth + 0.2f)) -
                                transform.position);
                cam.transform.position = previousCamHolderPos;
            }

            if (isWallrunning)
            {
                Vector3 runDirection = Vector3.Cross(wallNormal, Vector3.up);

                runDirection *= wallrunSpeed;
                currentVelocity = runDirection;
                currentVerticalSpeed -= (gravity / 3.0f) * Time.deltaTime;
                runDirection.y = currentVerticalSpeed;
                controller.Move(runDirection * Time.deltaTime);

                currentCamTargetTilt = -wallrunCameraTilt;
            }
        }

        if (!isWallrunning)
        {
            if (!canWallrun)
            {
                currentWallrunCooldown -= Time.deltaTime;
                if (currentWallrunCooldown <= 0)
                    canWallrun = true;
            }

            currentCamTargetTilt = 0.0f;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (controller.isGrounded && Input.GetButton("Jump"))
                currentVerticalSpeed = jumpStrength;

            currentVerticalSpeed -= gravity * (currentVerticalSpeed < 0.0f ? 1.3f : 1.0f) * Time.deltaTime;

            Vector3 input = (transform.forward * vertical + transform.right * horizontal).normalized;
            currentVelocity = controller.velocity;
            currentVelocity.y = 0;
            currentVelocity += input * ((controller.isGrounded ? acceleration : airAcceleration) * Time.deltaTime);
            currentVelocity *= 1 - (controller.isGrounded ? dragFactor : airDragFactor) * Time.deltaTime;

            Vector3 move = currentVelocity;

            move.y += currentVerticalSpeed;
            controller.Move(move * Time.deltaTime);
        }

        if (controller.isGrounded)
        {
            currentVerticalSpeed = 0.0f;
            if (isWallrunning)
                isWallrunning = false;
        }

        grounded = controller.isGrounded;

        currentCamEuler.z = Mathf.SmoothDampAngle(currentCamEuler.z, currentCamTargetTilt, ref currentCamTiltVelocity, 0.20f);

        cam.transform.localRotation = Quaternion.Euler(currentCamEuler.x, currentCamEuler.y, currentCamEuler.z);

        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, cameraHolder.position, ref currentCamSmoothVelocity, 0.15f);
    }
}
