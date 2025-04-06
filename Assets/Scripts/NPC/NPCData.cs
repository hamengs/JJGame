using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "NPC/NPCData")]
public class NPCData : ScriptableObject
{
    public string npcName;
    public DialogueData dialogueData;
    public Shop shopdata;
}
