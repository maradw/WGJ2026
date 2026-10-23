
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
    public string storyTime;
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
    // Hora por línea de diálogo; vacío = no cambia.
    public List<string> lineTimes = new List<string>();
    // Si tiene valor, las opciones aparecen al pulsar Siguiente en la última línea, con esta hora.
    public string choiceTime;

    [Header("Story Flow")]
    public DialogueNode nextNode;
    public List<DialogueChoice> choices = new List<DialogueChoice>();
}
