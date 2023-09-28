using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
        bot.SetFinishPoint();
    }

    public void OnExecute(Bot bot)
    {
        if(bot.brickSpawnCount <= 0)
        {
            bot.ChangeState(new SeekState());
        } 
    }

    public void OnExit(Bot bot)
    {

    }

}