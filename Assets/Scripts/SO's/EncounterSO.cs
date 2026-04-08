using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Encounter/New Encounter")]
public class EncounterSO : ScriptableObject
{
    [Header("Metadata")]
    public string encounterName;
    public string dob;
    public int age;
    public string sex;
    public float height;
    public float weight;
    public float changeInWeight;

    [Header("Data for Documents")]
    public bool virusTestResultsShowVirus;
    public bool bloodTestResultsShowVirus;
    public string bloodType;
    public bool receivedVaccine1;
    public bool receivedVaccine2;
    public bool receivedVaccine3;
    public string timeWeightChecked;
    public string timeHeightChecked;
    public bool alcoholIntoxication;

    [Header("Data for Character Model")]
    public bool rash;
    public bool backAcne;
    public bool chestDiscolouration;
    public bool patterenedChestDiscolouration;


    [Header("Data for Gameplay")]
    public GameObject render;
    public List<GameObject> documents;
    public Dialogue dialogue;
    public List<bool> correctAnswers;
    public bool entryShouldBeAllowed;

    [Header("For Mandatory Encounters")]
    public bool mandatory;
    public int minEncountersBeforeAppearance;

}
