using UnityEngine;

public class CurrentNpcDialogue : MonoBehaviour
{
    private bool isDialogueComplete = false;

    public delegate void DialogueCompleteEvent();
    public static event DialogueCompleteEvent OnNPCDialogueComplete;

    // Call this function when the NPC dialogue ends
    public void EndDialogue()
    {
        if (!isDialogueComplete)
        {
            isDialogueComplete = true;
            OnNPCDialogueComplete?.Invoke();
            Debug.Log("Dialogue completed with NPC: " + gameObject.gameObject.name);
        }
    }

}
