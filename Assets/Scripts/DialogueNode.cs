
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

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
    menuName = "Visual Novel/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    [Header("Dialogo")]
    public string characterName;

    [TextArea(3, 6)]
    public string dialogueText;

    public Sprite characterPortrait;

    [Header("Continuacion")]
    public DialogueNode nextNode;

    [Header("Decisiones")]
    public List<DialogueChoice> choices = new List<DialogueChoice>();

    [Header("Multiple Lines")]
    [TextArea(3, 6)]
    public List<string> dialogueLines = new List<string>();
    [Header("Visuales")]
    public Sprite backgroundImage;
    public Sprite dialogueBoxImage;
    public Color dialogueTextColor = Color.white;
}
