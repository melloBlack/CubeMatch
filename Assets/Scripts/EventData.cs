using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event Data", menuName = "Data/Event Data")]
public class EventData : ScriptableObject
{
    MatchController _matchController;
    CubesGenerator _cubesGenerator;
    Movement _movement;
    UIController _uiController;

    public Action<float, int> ComboTime;

    public Action<int> OnCollectStar;

    public Action<MatchingObject> OnCollectObject;

    public void SetMovement(Movement movement)
    {
        _movement = movement;
    }

    public void SetGenerator(CubesGenerator cubesGenerator)
    {
        _cubesGenerator = cubesGenerator;
    }

    public void SetMatchController(MatchController matchController)
    {
        _matchController = matchController;
    }

    public void SetUIController(UIController uiController)
    {
        _uiController = uiController;
    }

    public void GenerationIsDone()
    {
        _movement.CanMove = true;
    }
}
