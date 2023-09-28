using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UICanvas
{
    public void PlayButton()
    {
        LevelManager.instance.OnStartGame();

        UIManager.Instance.OpenUI<Gameplay>();
        Close();
    }
}
