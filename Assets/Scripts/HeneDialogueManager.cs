using System.Collections.Generic;
using UnityEngine;

public class HeneDialogueManager : MonoBehaviour
{
    public List<CurrentNpcDialogue> npcDialogues;
    private int completedDialogueCount = 0;

    private void OnEnable()
    {
        CurrentNpcDialogue.OnNPCDialogueComplete += CheckDialogueCompletion;
    }

    private void OnDisable()
    {
        CurrentNpcDialogue.OnNPCDialogueComplete -= CheckDialogueCompletion;
    }

    private void CheckDialogueCompletion()
    {
        completedDialogueCount++;
        Debug.Log(completedDialogueCount);
        if (completedDialogueCount == 7)
        {
            TriggerFinalEvent();
        }
    }

    private void TriggerFinalEvent()
    {
        Debug.Log("All NPC dialogues completed. Triggering the final event!");
        // logic here
    }
}
