
using System;
using System.Collections.Generic;
using UnityEngine;

public enum MiniGameType
{
    None,
    Typing,
    Code,
    Maze
}

[Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
    public MiniGameType miniGameType;
}

[CreateAssetMenu(
    fileName = "NewDialogueNode",
    menuName = "Visual Novel/Dialogue Node"
)]
public class DialogueNode : ScriptableObject
{
    [Header("Character")]
    public CharacterData character;

    [Header("Background")]
    public BackgroundData background;

    public List<string> dialogueLines = new List<string>();

    [Header("Narrative Time")]
    public bool changeTime;
    public string storyTime;

    [Header("Story Flow")]
    public DialogueNode nextNode;
    public List<DialogueChoice> choices = new List<DialogueChoice>();
}
