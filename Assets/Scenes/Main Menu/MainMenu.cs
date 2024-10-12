using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Next SCENE");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    
    public void Options()
    {
        SceneManager.LoadScene("Options Scene");
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
