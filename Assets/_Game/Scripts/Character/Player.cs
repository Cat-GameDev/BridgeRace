using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character
{
    private CharacterController controller;
    [SerializeField] private FloatingJoystick joystick;

    [SerializeField] private float moveSpeed = 10f;
    private float moveSpeedNormal = 5.0f;  
    private Vector3 moveDirection;
    public float rotationSpeed = 10f;

    private bool isForward = false;
    private bool isSlowSpeed = false;

    public static Player instance;


    void Awake()
    {
        instance = this;
        LoadJoystick();
    }

    private void Start() 
    {
        controller = GetComponent<CharacterController>();
        transform.position = LevelManager.instance.currentLevelInstance.GetRandomPosition().position;
    }

    private void Update()
    {
        if (GameManager.Instance.IsState(GameState.Gameplay) )
        {
            MovingByJoystick();
            CheckDistanceToBrick();
            CheckRaycastToBrick();
        }

    }

    public  override void OnInit()
    {
        base.OnInit();
        isForward = isSlowSpeed = false;
    }

    private void LoadJoystick()
    {
        joystick = FindObjectOfType<FloatingJoystick>();
        if (joystick == null)
        {
            Debug.LogError("FloatingJoystick not found in the scene.");
        }
    }

    private void MovingByJoystick()
    {
        
        MoveSpeedControl();
        if (controller.isGrounded)
        {
            float horizontalInput = joystick.Horizontal;
            float verticalInput = joystick.Vertical;

            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);

            if (movement.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                movement.Normalize();

                controller.Move(movement * moveSpeedNormal * Time.deltaTime);

                ChangeAnim("run");
            }
            else
            {
                ChangeAnim("idle");
            }
        }
        else
        {
            moveDirection.y += -9.81f * Time.deltaTime; // gravity = -9.81f
            controller.Move(moveDirection * Time.deltaTime);
        }
        CheckDirection();
    }

    private void CheckDirection()
    {
        float verticalInput = joystick.Vertical;
        
        if (verticalInput > 0.1f)
        {
            isForward = true;

        }
        else if (verticalInput < -0.1f)
        {
            isForward = false;
            isSlowSpeed = false;
        }
    }

    private void MoveSpeedControl()
    {
        if(isSlowSpeed)
        {
            moveSpeedNormal = 4f;
        }
        else moveSpeedNormal = moveSpeed;
    }

    protected override void BrickSpawnBeforePlayer()
    {
        ChangeBrickColor(brickBeforeCharacterPrefab, color, colorData);
        base.BrickSpawnBeforePlayer();
    }
    
    protected override void CheckRaycastToBrick()
    {
        if(!isForward)
            return;
        RaycastHit hitInfo;
        Vector3 raycastStart = transform.position + transform.up * 1f + transform.forward * 0.4f;
        Debug.DrawRay(raycastStart , Vector3.down * 1.5f, Color.red);
        bool hitBrick = Physics.Raycast(raycastStart, Vector3.down, out hitInfo, 1.5f, brickInBridgeLayer);
         
        if(hitBrick)
        {
            isSlowSpeed = true;
            if (hitInfo.collider.GetComponent<BrickInBridge>().color == color)
            {
                return;
            } 
            else 
            {
                if(brickSpawnCount > 0)
                {
                    hitInfo.collider.GetComponent<BrickInBridge>().DisableBoxCollider();
                    hitInfo.collider.GetComponent<BrickInBridge>().ChangeColor(color); 
                    RemoveBrick();
                }
                else 
                {
                    hitInfo.collider.GetComponent<BrickInBridge>().EnableBoxCollider();
                }
                
            }
            
        }
        else isSlowSpeed = false;
    }






}



