using UnityEngine;

[CreateAssetMenu(fileName = "New Application", menuName = "Application", order = 1)]
//////////////////////////////////////////////////////////////////////////////
public class ApplicationSO : ScriptableObject
{
    public string applicationName;
    public Sprite desktopIconImage;
    public Sprite TaskbarIconImage;
}

//////////////////////////////////////////////////////////////////////////////
