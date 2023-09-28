using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fail : UICanvas
{

    public void RetryButton()
    {
        LevelManager.instance.OnRetry();
        Close();
    }

}
