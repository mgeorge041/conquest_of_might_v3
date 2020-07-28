using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManagerObject : MonoBehaviour
{
    private GameMap gameMap;
    private GameManager gameManager;
    public Tilemap gameTilemap;
    private List<PlayerObject> playerObjects = new List<PlayerObject>();
    private int numPlayers = GameSetupData.numPlayers;

    private int currentPlayerTurn = 0;

    // Get game map
    public GameMap GetGameMap() {
        return gameMap;
    }

    // Start game
    public void StartGame() {
        playerObjects[currentPlayerTurn].StartTurn();
    }

    // Move to next turn
    public void NextTurn() {
        currentPlayerTurn = (currentPlayerTurn + 1) % numPlayers;
        for (int i = 0; i < playerObjects.Count; i++) {
            StartCoroutine(playerObjects[i].ShowCurrentPlayerTurn(currentPlayerTurn));
        }
        playerObjects[currentPlayerTurn].StartTurn();
    }

    // Get player object
    public PlayerObject GetPlayerObject(int playerId) {
        return playerObjects[playerId];
    }

    // Start is called before the first frame update
    void Start()
    {
        gameMap = new GameMap(gameTilemap);
        gameManager = new GameManager(gameMap);
        gameMap.SetPlayers(gameManager.GetPlayers());
        

        // Create new player objects
        for (int i = 0; i < numPlayers; i++) {

            // Create player object
            PlayerObject newPlayerObject = Instantiate(Resources.Load<PlayerObject>("Prefabs/Player Object"));
            newPlayerObject.SetGameMap(gameMap);
            newPlayerObject.SetGameManagerObject(this);
            newPlayerObject.SetPlayer(gameManager.GetPlayer(i));
            
            // Instantiate starting pieces
            newPlayerObject.SetupFirstTurn();
            playerObjects.Add(newPlayerObject);
        }
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
