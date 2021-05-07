using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Load a scene
    public void SetScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadDeckBuilder() {
        SceneManager.LoadScene("Deck Builder");
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
