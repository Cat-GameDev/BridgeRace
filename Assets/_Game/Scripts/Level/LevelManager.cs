using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public List<Level> levelPrefabs = new List<Level>(); 
    public Level currentLevelInstance; 
    public int currentLevelIndex = 0; 

    public static LevelManager instance;

    [SerializeField] private List<Character> charactersList = new List<Character>();
    [SerializeField] private List<Bot> bots = new List<Bot>();

    private void Awake() 
    {
        LoadBots();
        LoadCharacters();
        instance = this;
        LoadCurrentLevel(); 
        UIManager.Instance.OpenUI<MainMenu>();
    }

    private void LoadCharacters()
    {
        Character[] foundCharacters = FindObjectsOfType<Character>();
        charactersList.AddRange(foundCharacters);
    }

    private void LoadBots()
    {
        Bot[] foundBots = FindObjectsOfType<Bot>();
        bots.AddRange(foundBots);
    }

    public void OnInit()
    {
        for(int i = 0; i < charactersList.Count; i++)
        {
            charactersList[i].OnInit();
        }
    }
    

    public void NextLevel()
    {
        OnNextLevel();
        OnInit();
    }


    public void DestroyCurrentLevel()
    {
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance.gameObject); 
        }
    }

    public void OnNextLevel()
    {
        DestroyCurrentLevel(); 
        currentLevelIndex = (currentLevelIndex + 1) % levelPrefabs.Count; 
        LoadCurrentLevel(); 
        OnInit();
        UIManager.Instance.OpenUI<MainMenu>();
    }

    private void LoadCurrentLevel()
    {
        if (currentLevelIndex < levelPrefabs.Count)
        {
            currentLevelInstance = Instantiate(levelPrefabs[currentLevelIndex]);
        }
        else
        {
            Debug.LogWarning("No more levels to load.");
        }
    }

    public void OnRetry()
    {
        DestroyCurrentLevel(); 
        LoadCurrentLevel();
        OnInit();
        UIManager.Instance.OpenUI<MainMenu>();
        //UIManager.Instance.CloseUI<Fail>();
    }

    public void OnStartGame()
    {
        GameManager.Instance.ChangeState(GameState.Gameplay);
    }

    public void OnFinishGame()
    {
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].ChangeState(null);
            bots[i].StopMoving();
        }
    }




}
