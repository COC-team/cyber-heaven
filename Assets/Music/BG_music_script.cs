using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_music_script : MonoBehaviour
{
    
void Awake() {
    gameObject.tag = "BG_music"; // Assign a tag or name
    DontDestroyOnLoad(gameObject);
}
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
