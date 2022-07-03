using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event Data", menuName = "Data/Event Data")]
public class EventData : ScriptableObject
{
    #region Fields and Properties

    public Action OnStart;
    public Action OnPlay;
    public Action OnPause;
    public Action OnLose;
    public Action OnVictory;
    public Action OnClickUndo;
    public Action OnNullMatchArea;
    public Action<MatchingObject> OnUndoObject;
    public Action<int> OnCollectStar;
    public Action<Vector3, int> OnMoveStar;
    public Action<float, int> ComboTime;
    public Action<MatchingObject> OnCollectObject;

    #endregion

}
