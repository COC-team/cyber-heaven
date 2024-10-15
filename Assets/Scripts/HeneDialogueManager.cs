using System.Collections.Generic;
using UnityEngine;

public class HeneDialogueManager : MonoBehaviour
{
    public BG_music_script bgMusicScript;
    private int dialogueCount = 0;
    public GameObject enemyPrefab; // Prefab of the new enemy to spawn
    private bool[] ids = new bool[9];

    // This function is called by the NPC script to increment the count
    public void IncrementDialogueCount(int id)
    {
        if (!ids[id] && id != 0)
        {
            dialogueCount++;
            Debug.Log("Dialogue count: " + dialogueCount);
            ids[id] = true;
            if (dialogueCount == 8)
            {
                TriggerFinalFight();
            }
        }
    }

    // Trigger the final fight or event
    private void TriggerFinalFight()
    {
        Debug.Log("All dialogues completed! Triggering the final fight!");
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
                    enemyPrefab.SetActive(true);

                    // Spawn a new enemy at the hidden object's position
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    bgMusicScript.StartBossFightMusic();
                }
            }
        }
    }
}