
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
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
}
