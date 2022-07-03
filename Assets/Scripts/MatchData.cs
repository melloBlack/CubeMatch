using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Match Data", menuName = "Data/Match Data")]
public class MatchData : ScriptableObject
{
    [SerializeField] int maxObjectCount = 7;
    [SerializeField] float matchTime = 0.25f;
    [SerializeField] float collectSpeed = 13f;
    [SerializeField] float comboTime = 5f;

    public int MaxObjectCount => maxObjectCount;

    public float MatchTime => matchTime;
    public float CollectSpeed => collectSpeed;
    public float ComboTime => comboTime;
}
