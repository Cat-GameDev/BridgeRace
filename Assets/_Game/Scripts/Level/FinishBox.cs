using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null)
        {
            LevelManager.instance.OnFinishGame();
            if (character is Player)
            {
                UIManager.Instance.OpenUI<Victory>();
            }
            else
            {
                UIManager.Instance.OpenUI<Fail>();
            }
            character.gameObject.SetActive(false);
            UIManager.Instance.CloseUI<Gameplay>();

            GameManager.Instance.ChangeState(GameState.Pause);
            //character.OnInit();
        }
    }
}
