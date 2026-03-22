using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Encounter/New Encounter")]
public class EncounterSO : ScriptableObject
{
    [Header("Data")]
    public GameObject render;
    public List<GameObject> documents;
    public Dialogue dialogue;
    public List<bool> correctAnswers;
    public bool entryShouldBeAllowed;

    [Header("For Mandatory Encounters")]
    public bool mandatory;
    public int minEncountersBeforeAppearance;
}
