using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // [SerializeField] protected Transform startPoint;
    protected Vector3 finishPoint;
    [SerializeField] protected Transform holder;

    [SerializeField] protected  Animator anim;
    
    protected string currentAnimName;

    [SerializeField] protected ColorData colorData;
    [SerializeField] protected Renderer meshRenderer;
    public ColorType color;

    protected List<GameObject> collectedBricks = new List<GameObject>();
    protected List<GameObject> bricksToRemove = new List<GameObject>();
    [SerializeField] protected LayerMask brickInBridgeLayer;
    [SerializeField] protected GameObject brickBeforeCharacterPrefab;

    public int brickSpawnCount = 0;
    public float distanceThreshold = 2.0f; 
    protected float time = 4f;
    
    public virtual void OnInit()
    {
        ClearBrick();
        transform.position = LevelManager.instance.currentLevelInstance.GetRandomPosition().position;
        transform.gameObject.SetActive(true);
        finishPoint =  LevelManager.instance.currentLevelInstance.FinishPoint.position;
    }

    protected void ChangeColor(ColorType colorType)
    {
        color = colorType;
        meshRenderer.material = colorData.GetMat(colorType);
    }


    protected virtual void AddBrick(GameObject birck)
    {
        collectedBricks.Add(birck);
        BrickSpawnBeforePlayer();
    }

    protected virtual void BrickSpawnBeforePlayer()
    {
        brickSpawnCount = collectedBricks.Count;
        Vector3 spawnPos = transform.position;
        Vector3 spawnPosLocal = transform.InverseTransformPoint(spawnPos)+ new Vector3(-0.1f , 0.3f * brickSpawnCount , -0.5f );
        Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
        GameObject newBrick = Instantiate(brickBeforeCharacterPrefab, spawnPosLocal, rotation);
        newBrick.transform.SetParent(holder);
        newBrick.transform.localPosition = spawnPosLocal;
        newBrick.transform.localRotation = rotation;
        bricksToRemove.Add(newBrick);
    }

    protected virtual void ChangeBrickColor(GameObject brick, ColorType colorType, ColorData colorData)
    {
        Renderer brickRenderer = brick.GetComponent<Renderer>();
        
        if (brickRenderer != null)
        {
            Material brickMaterial = colorData.GetMat(colorType);
            
            if (brickMaterial != null)
            {
                brickRenderer.material = brickMaterial;
            }
            else
            {
                Debug.LogWarning("Material not found for ColorType: " + colorType.ToString());
            }
        }
        else
        {
            Debug.LogWarning("Brick renderer not found.");
        }
    }

    protected virtual void RemoveBrick()
    {
        if (collectedBricks.Count > 0)
        {
            BricksToRemoveInScence();
            collectedBricks.RemoveAt(0);
            brickSpawnCount = collectedBricks.Count;
            //Debug.Log(collectedBricks.Count);
        }
    }

    protected virtual void BricksToRemoveInScence()
    {
        if(bricksToRemove.Count > 0)
        {
            int lastIndex = bricksToRemove.Count - 1;
            GameObject brickToRemove = bricksToRemove[lastIndex];
            bricksToRemove.RemoveAt(lastIndex);
            brickToRemove.SetActive(false);
        }
    }


    protected virtual void CheckDistanceToBrick()
    {
        foreach (GameObject brickObj in GameObject.FindGameObjectsWithTag("brick"))
        {
            Brick brick = brickObj.GetComponent<Brick>();
            if (brick != null)
            {
                float distanceToBrick = Vector3.Distance(transform.position, brickObj.transform.position);
                if (distanceToBrick <= distanceThreshold)
                {
                    if (brick.color == color)
                    {
                        AddBrick(brickObj);
                        SetActiveController(brickObj);
                    }
                }
            }
        }
    }

    protected void ClearBrick()
    {
        if(bricksToRemove.Count > 0)
        {
            for (int i =0; i< bricksToRemove.Count; i++)
            {
                Destroy(bricksToRemove[i]);
            }
            bricksToRemove.Clear();
        }

        if (collectedBricks.Count > 0)
        {
            collectedBricks.Clear();
        }
    }

    protected IEnumerator ReactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(obj != null)
        {
            obj.SetActive(true);
        }
    }

    protected void SetActiveController(GameObject other)
    {
        StartCoroutine(ReactivateAfterDelay(other.gameObject, time));
        other.gameObject.SetActive(false);
        if(time<11)
        {
            time+=0.5f;
        }
    }

    protected virtual void CheckRaycastToBrick()
    {
        RaycastHit hitInfo;
        Vector3 raycastStart = transform.position + transform.up * 1f + transform.forward * 0.4f;
        Debug.DrawRay(raycastStart , Vector3.down * 1.5f, Color.red);
        bool hitBrick = Physics.Raycast(raycastStart, Vector3.down, out hitInfo, 1.5f, brickInBridgeLayer);
         
        if(hitBrick)
        {
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
    }

    protected void ChangeAnim(string animName)
    {
        if(currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }




}
