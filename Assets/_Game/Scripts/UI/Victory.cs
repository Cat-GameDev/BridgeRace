using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : UICanvas
{
    public void RetryButton()
    {
        LevelManager.instance.OnRetry();
        Close();
    }

    public void NextButton()
    {
        LevelManager.instance.OnNextLevel();
        Close();
    }
}
