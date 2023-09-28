using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{   
    private IState<Bot> currentState;
    private NavMeshAgent navMeshAgent;
    public Stage currentStage;
    private Vector3 currentPositon;
    private NavMeshSurface navMeshSurface;

    private void Start() 
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshSurface = GetComponent<NavMeshSurface>();
        ChangeState(new IdleState());
        ChangeColor(color);
        finishPoint =  LevelManager.instance.currentLevelInstance.FinishPoint.position;
        transform.position = LevelManager.instance.currentLevelInstance.GetRandomPosition().position;        
    }

    void Update()
    {
        if( GameManager.Instance.IsState(GameState.Gameplay) )
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
            CheckDistanceToBrick();
            CheckRaycastToBrick();
        }

    }

    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        navMeshAgent.enabled = true;
    }

    public void StopMoving()
    {
        ChangeAnim("idle");
        navMeshAgent.enabled = false;
    }

    public void SetTarget()
    {
        navMeshAgent.enabled = true;
        ChangeAnim("run");
        currentPositon = currentStage.SeekRandomBrick(color);
        navMeshAgent.SetDestination(currentPositon);
    }

    public void SetFinishPoint()
    {
        navMeshAgent.SetDestination(finishPoint);
    }

    public void CheckDistance()
    {
        if(currentPositon.x == transform.position.x) 
        {
            SetTarget();
        }
    }


    public void ChangeState(IState<Bot> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    protected override void BrickSpawnBeforePlayer()
    {
        ChangeBrickColor(brickBeforeCharacterPrefab, color, colorData);
        base.BrickSpawnBeforePlayer();
    }

    private void OnTriggerEnter(Collider other) 
    {
        Stage stage = other.GetComponent<Stage>();
        if(stage != null)
        {
            this.currentStage = stage;
        }
    }
    







}
