using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Transform> characterPositionList = new List<Transform>();
    [SerializeField] private Transform finishPoint;

    public Transform FinishPoint { get => finishPoint; }
    private NavMeshSurface navMeshSurface;

    private void Awake() 
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }



    public Transform GetRandomPosition()
    {
        if (characterPositionList.Count == 0)
        {
            Debug.LogWarning("No available positions left." + name);
            return null;
        }

        int randomIndex = Random.Range(0, characterPositionList.Count);
        Transform randomPosition = characterPositionList[randomIndex];

        characterPositionList.RemoveAt(randomIndex);

        return randomPosition;
    }






}
