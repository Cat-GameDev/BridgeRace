using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private List<Brick> bricks = new List<Brick>();
    private List<Vector3> brickPoints = new List<Vector3>();
    [SerializeField] private Brick brickPrefab;
    [SerializeField] private int numRows = 10;
    [SerializeField] private int numColumns = 13;
    public Transform holder;
    [SerializeField] private Vector3 positionSpawnBrick;

    private void Start()
    {
        BrickSpawner();
    }

    private void BrickSpawner()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int column = 0; column < numColumns; column++)
            {
                Vector3 spawnPosition = new Vector3(
                    column,
                    transform.position.y,
                    row * 1.2f
                ) + positionSpawnBrick;

                Brick newBrick = Instantiate(brickPrefab, spawnPosition, Quaternion.identity);
                bricks.Add(newBrick);
                newBrick.transform.SetParent(holder);
            }
        }
    }


    public Vector3 SeekRandomBrick(ColorType color)
    {
        List<Vector3> matchingPositions = new List<Vector3>();

        foreach(Brick brick in bricks)
        {
            if(brick.color == color)
            {
                if(brick != null)
                {
                    matchingPositions.Add(brick.transform.position);
                }
                
            }
        }

        if (matchingPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, matchingPositions.Count);
            return matchingPositions[randomIndex];
        }

        return Vector3.zero;
    }


}
