using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FinalSceneChanger : MonoBehaviour
{
    public float changeTime;
    public string sceneName;

    void Start() {
//        GameObject persistentObject = GameObject.FindWithTag("BG_music");
//        if (persistentObject != null) {
//            Destroy(persistentObject); // Destroy it in the new scene
//        }
    }
    
    // Update is called once per frame
    void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
