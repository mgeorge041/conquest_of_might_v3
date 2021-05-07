using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetupUI : MonoBehaviour
{
    private int numPlayers = 0;
    public GameObject playerPanelPrefab;
    public Transform playerPanelsRegion;
    public Slider numPlayersSlider;
    public Text numPlayersText;

    // Set number of player panels
    public void SetPlayerPanels(float sliderNumPlayers) {

        int newNumPlayers = (int)sliderNumPlayers;

        if (newNumPlayers > numPlayers) {
            for (int i = numPlayers; i < newNumPlayers; i++) {
                AddPlayerPanel();
            }
        }
        else if (newNumPlayers < numPlayers) {
            for (int i = newNumPlayers; i < numPlayers; i++) {
                RemovePlayerPanel();
            }
        }
        numPlayers = newNumPlayers;
        numPlayersText.text = numPlayers.ToString();
    }

    // Add new player setup panel
    public void AddPlayerPanel() {
        GameObject playerPanel = Instantiate(playerPanelPrefab, playerPanelsRegion);
    }

    // Remove player setup panel
    public void RemovePlayerPanel() {
        int numPanels = playerPanelsRegion.transform.childCount;
        Transform playerPanel = playerPanelsRegion.transform.GetChild(numPanels - 1);
        Destroy(playerPanel.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerPanels(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
