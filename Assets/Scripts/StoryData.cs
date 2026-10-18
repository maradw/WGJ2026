
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewStory",
    menuName = "Visual Novel/Story")]
public class StoryData : ScriptableObject
{
    public DialogueNode startNode;
}
