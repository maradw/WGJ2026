using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Character")]
    public string characterName;
    public Sprite characterImage;

    [Header("Dialogue Style")]
    public Color textColor = Color.white;
    public Sprite dialogueBox;
}
