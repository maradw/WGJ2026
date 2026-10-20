using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundData", menuName = "Scriptable Objects/BackgroundData")]
public class BackgroundData : ScriptableObject
{
    [Header("Background")]
    public Sprite backgroundImage;
}
