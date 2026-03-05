using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue")]
////////////////////////////////////////////////////////////////////
public class Dialogue : ScriptableObject
{
    [Header("Parameters")]
    public float textSpeed;
    public float timeBetweenMessages;

    [Header("Content")]
    public List<string> lines;
}

////////////////////////////////////////////////////////////////////
    