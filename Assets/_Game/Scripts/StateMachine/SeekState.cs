using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : IState<Bot>
{
    int targetBrick;
    public void OnEnter(Bot bot)
    {
        targetBrick = Random.Range(10,15);
        bot.SetTarget();
    }

    public void OnExecute(Bot bot)
    {
        if(targetBrick < bot.brickSpawnCount)
        {
            bot.ChangeState(new BuildState());
        }
        else
        {
            bot.CheckDistance();
        }
    }

    public void OnExit(Bot bot)
    {

    }

}
