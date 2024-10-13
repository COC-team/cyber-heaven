using System.Collections.Generic;
using UnityEngine;

public class HeneDialogueManager : MonoBehaviour
{
    public List<CurrentNpcDialogue> npcDialogues;
    private int completedDialogueCount = 0;
    public GameObject enemyPrefab; // Prefab of the new enemy to spawn

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
        foreach (string targetName in new List<string> { "Anna", "Dima", "Margarita", "Mark", "Martin" })
        {
            // Find all objects with the specified name
            GameObject[] targetObjects = GameObject.FindObjectsOfType<GameObject>();
            
            foreach (GameObject targetObject in targetObjects)
            {
                if (targetObject.name == targetName)
                {
                    // Hide the object by disabling it
                    targetObject.SetActive(false);

                    // Get the position of the hidden object
                    Vector3 spawnPosition = targetObject.transform.position;

                    // Spawn a new enemy at the hidden object's position
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
