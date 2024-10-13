using UnityEngine;

public class HeneNPCDialogue : MonoBehaviour
{
    // Reference to the Dialogue Manager
    public HeneDialogueManager dialogueManager;

    // Call this function when the NPC dialogue ends
    public void EndDialogue(int id)
    {
        dialogueManager.IncrementDialogueCount(id);  // Increment the count in the manager
        Debug.Log("NPC "+ id +" dialogue completed.");
        
    }
}