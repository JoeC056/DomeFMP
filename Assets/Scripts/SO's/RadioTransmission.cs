using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Radio Transmission", menuName = "Radio Transmission/New Radio Transmission")]
////////////////////////////////////////////////////////////////////
public class RadioTransmissionSO : ScriptableObject
{
    [Header("Parameters")]
    public float delayBetweenCharacters;
    public float delayBetweenLines;

    [Header("Content")]
    public List<string> lines;
}

////////////////////////////////////////////////////////////////////
    