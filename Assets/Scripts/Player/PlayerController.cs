using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    enum WallrunSide
    {
        Left,
        Right
    }
    
    [Header("View")]
    public Transform cameraHolder;
    public Camera cam;
    
    [Header("Base movement")]
    [Min(0.0f)]
    public float acceleration = 150.0f;
    [Min(0.0f)]
    public float airAcceleration = 5.0f;
    [Min(0.0f)]
    public float dragFactor = 15f;
    [Min(0.0f)]
    public float airDragFactor = 0.5f;
    [Min(0.0f)]
    public float gravity = 2.0f;
    public float maxFallSpeed = 10.0f;
    
    [Header("Wallrun")]
    [Min(1.0f)]
    public float jumpStrength = 6.0f;
    [Min(1.0f)]
    public float wallrunSpeed = 6.0f;
    public float wallrunCooldown = 0.2f;
    public float wallrunCameraTilt = 15.0f;
    [Min(0f)]
    public float wallrunMinimumSpeedToTrigger = 2.0f;
    [Min(4)]
    public int wallrunNumberOfRays = 10;
    public float wallrunRaySweepAngle = 120.0f;
    [Min(0.1f)]
    public float rayDistance = 2.0f;

    [Header("Input")]
    [Min(1.0f)]
    public float viewSensitivity = 2.0f;

    [Header("Visual")]
    public GameObject handPrefab;
    private CharacterController controller;

    private float currentVerticalSpeed = 0.0f;

    private bool isWallrunning = false;
    private Vector3 currentWallNormal = Vector3.zero;
    private Vector3 currentWallrunDirection = Vector3.zero;
    private WallrunSide currentWallrunSide;

    private Vector3 currentCamSmoothVelocity = Vector3.zero;
    private Vector3 currentCamEuler = Vector3.zero;
    private float currentCamTargetTilt = 0.0f;
    private float currentCamTiltVelocity = 0.0f;

    private Vector3 currentVelocity = Vector3.zero;

    private bool grounded = false;

    private GameObject handInstance;

    private bool canWallrun = true;
    private float currentWallrunCooldown = 0.0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (!controller)
        {
            Debug.LogError("Impossible to get character controller.");
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        handInstance = Instantiate(handPrefab);
        handInstance.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * viewSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * viewSensitivity;
        
        transform.rotation *= Quaternion.Euler(0.0f, mouseX, 0.0f);
        currentCamEuler += new Vector3(-mouseY, 0.0f, 0.0f);
        currentCamEuler.x = Mathf.Clamp(currentCamEuler.x, -90f, 90f);
        cam.transform.localRotation *= Quaternion.Euler(currentCamEuler.x, currentCamEuler.y, currentCamEuler.z);

        HandleWallrun();

        if (!isWallrunning)
        {
            if (!canWallrun)
            {
                currentWallrunCooldown -= Time.deltaTime;
                if (currentWallrunCooldown <= 0)
                    canWallrun = true;
            }
            
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (controller.isGrounded && Input.GetButton("Jump"))
                currentVerticalSpeed = jumpStrength;

            currentVerticalSpeed -= gravity * (currentVerticalSpeed < 0.0f ? 1.3f : 1.0f) * Time.deltaTime;

            Vector3 input = (transform.forward * vertical + transform.right * horizontal).normalized;
            currentVelocity += input * ((controller.isGrounded ? acceleration : airAcceleration) * Time.deltaTime);
            currentVelocity *= 1 - (controller.isGrounded ? dragFactor : airDragFactor) * Time.deltaTime;

            Vector3 move = currentVelocity;

            move.y += currentVerticalSpeed;
            controller.Move(move * Time.deltaTime);
            if (controller.velocity.sqrMagnitude < currentVelocity.sqrMagnitude)
                currentVelocity = controller.velocity;
            currentVelocity.y = 0;
        }

        if (controller.isGrounded)
        {
            currentVerticalSpeed = 0.0f;
            if (isWallrunning)
                StopWallrun();
        }

        grounded = controller.isGrounded;

        currentCamEuler.z = Mathf.SmoothDampAngle(currentCamEuler.z, currentCamTargetTilt, ref currentCamTiltVelocity, 0.20f);

        cam.transform.localRotation = Quaternion.Euler(currentCamEuler.x, currentCamEuler.y, currentCamEuler.z);

        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, cameraHolder.position, ref currentCamSmoothVelocity, 0.15f);
    }

    void HandleWallrun()
    {
        RaycastHit bestHitInfo = new RaycastHit();

        if (!isWallrunning && currentVerticalSpeed > 0f && canWallrun && !controller.isGrounded)
        {
            RaycastHit currentHitInfo;
            float bestDistance = Mathf.Infinity;
            
            for (int i = 0; i < wallrunNumberOfRays; i++)
            {
                float rayAngle = i * wallrunRaySweepAngle / wallrunNumberOfRays - wallrunRaySweepAngle / 2f;
                Physics.Raycast(new Ray(transform.position,
                        Quaternion.Euler(0, rayAngle, 0) * transform.forward),
                    out currentHitInfo, rayDistance);

                if (currentHitInfo.collider != null)
                {
                    WallrunSide wallrunSide = Vector3.Dot(transform.right, -currentHitInfo.normal) >= 0
                        ? WallrunSide.Right
                        : WallrunSide.Left;
                    Vector3 runDirection = wallrunSide == WallrunSide.Left ? 
                        Vector3.Cross(currentHitInfo.normal, Vector3.up) : 
                        Vector3.Cross(Vector3.up, currentHitInfo.normal);
                    float velocityAngle = Vector3.SignedAngle(-currentHitInfo.normal, currentVelocity, Vector3.up);
                    bool validVelocityAngle = false;
                    float distance = (transform.position - currentHitInfo.point).magnitude;


                    if (Vector3.Dot(transform.right, -currentHitInfo.normal) >= 0f)
                    {
                        if (velocityAngle <= -30f && velocityAngle >= -90f)
                            validVelocityAngle = true;
                    }
                    else
                    {
                        if (velocityAngle >= 30f && velocityAngle <= 90f)
                            validVelocityAngle = true;
                    }
                    
                    if (Math.Abs(Vector3.Angle(currentHitInfo.normal, Vector3.up) - 90f) < 0.001f &&
                        distance < bestDistance &&
                        Vector3.Angle(transform.forward, -currentHitInfo.normal) <= 90f && validVelocityAngle &&
                        currentVelocity.magnitude > wallrunMinimumSpeedToTrigger)
                    {
                        bestHitInfo = currentHitInfo;
                        bestDistance = distance;
                    }
                }
            }

            if (bestHitInfo.collider != null)
            {
                currentWallrunSide = Vector3.Dot(transform.right, -bestHitInfo.normal) >= 0
                    ? WallrunSide.Right
                    : WallrunSide.Left;

                currentCamTargetTilt = currentWallrunSide == WallrunSide.Left ? -wallrunCameraTilt : wallrunCameraTilt;
                
                currentWallNormal = bestHitInfo.normal;
                currentWallrunDirection = currentWallrunSide == WallrunSide.Left ? 
                    Vector3.Cross(currentWallNormal, Vector3.up) : 
                    Vector3.Cross(Vector3.up, currentWallNormal);
                isWallrunning = true;
                Vector3 previousCamHolderPos = cameraHolder.position;
                controller.Move((bestHitInfo.point + bestHitInfo.normal * (controller.radius + controller.skinWidth + 0.2f)) -
                                transform.position);
                cam.transform.position = previousCamHolderPos;
                
                handInstance.SetActive(true);
                Vector3 newScale = handInstance.transform.localScale;
                newScale.x = currentWallrunSide == WallrunSide.Right ? 1 : -1;
                handInstance.transform.localScale = newScale;
            }
        }
        else if (isWallrunning)
            Physics.Raycast(new Ray(transform.position, -currentWallNormal), out bestHitInfo, 2.0f);
        
        if (bestHitInfo.collider == null && isWallrunning)
        {
            StopWallrun();
        }

        if (bestHitInfo.collider != null)
        {
            if (isWallrunning)
            {
                if (bestHitInfo.normal != currentWallNormal)
                    StopWallrun();
            }
            else if (Vector3.Angle(transform.forward, -bestHitInfo.normal) < 80f)
            {
            }

            if (isWallrunning)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    currentVelocity = (currentWallNormal + currentWallrunDirection).normalized * 10f;
                    currentVelocity.y = 0f;
                    currentVerticalSpeed = 10f;
                    StopWallrun();
                }
                else
                {
                    currentVelocity = currentWallrunDirection * wallrunSpeed;
                    currentVerticalSpeed -= (gravity / 2.0f) * Time.deltaTime;

                    Vector3 move = currentVelocity;

                    move.y = currentVerticalSpeed;
                    controller.Move(move * Time.deltaTime);

                    if ((controller.collisionFlags & (CollisionFlags.Sides | CollisionFlags.Above)) != 0)
                    {
                        StopWallrun();
                        currentVerticalSpeed = 0f;
                        currentVelocity = controller.velocity;
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (isWallrunning)
        {
            handInstance.transform.position = transform.position - currentWallNormal * (controller.radius + controller.skinWidth + 0.1f);
            handInstance.transform.rotation = Quaternion.LookRotation(currentWallrunDirection, Vector3.up);
        }
    }

    private void StopWallrun()
    {
        isWallrunning = false;
        canWallrun = false;
        currentWallrunCooldown = wallrunCooldown;
        currentCamTargetTilt = 0.0f;
        handInstance.SetActive(false);
    }

    public void SetVerticalSpeed(float newVerticalSpeed)
    {
        currentVerticalSpeed = newVerticalSpeed;
    }
}