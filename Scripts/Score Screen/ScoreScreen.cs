using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    // Active stats
    public GameObject activeStatHeaders;

    // Play stat row prefab
    public GameObject playerStatPrefab;

    // Player data
    public int x;

    // Set current stat headers
    public void SetStats(GameObject activeHeader) {
        activeStatHeaders.SetActive(false);
        activeStatHeaders = activeHeader;
        activeStatHeaders.SetActive(true);
    }


    // Show appropriate player stats
    public void ShowPlayerStats() {

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
