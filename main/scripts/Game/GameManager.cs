using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager
{
    private List<Player> players = new List<Player>();
    private int numPlayers = GameSetupData.numPlayers;
    private int currentPlayerTurn = 0;

    //All the start positions, 2 spaces from the corners of the board
    readonly Vector3Int[] startLocations = {
        new Vector3Int(0, GameSetupData.mapRadius - 2, -(GameSetupData.mapRadius - 2)),
        new Vector3Int(0, -(GameSetupData.mapRadius - 2), GameSetupData.mapRadius - 2),
        new Vector3Int(-(GameSetupData.mapRadius - 2), GameSetupData.mapRadius - 2, 0),
        new Vector3Int(GameSetupData.mapRadius - 2, -(GameSetupData.mapRadius - 2), 0),
        new Vector3Int(-(GameSetupData.mapRadius - 2), 0, GameSetupData.mapRadius - 2),
        new Vector3Int(GameSetupData.mapRadius - 2, 0, -(GameSetupData.mapRadius - 2))
    };

    // Constructor
    public GameManager(GameMap gameMap) {

        // Create players
        for (int i = 0; i < numPlayers; i++) {
            Player newPlayer = new Player(i, gameMap, Map.ConvertHexToTileCoords(startLocations[i]), this);

            // Add new player to list
            players.Add(newPlayer);
        }
    }

    // Get all players
    public List<Player> GetPlayers() {
        return players;
    }

    // Get player by ID
    public Player GetPlayer(int playerId) {
        return players[playerId];
    }

    // Get current turn player 
    public Player GetCurrentTurnPlayer() {
        return players[currentPlayerTurn];
    }

    // Get current turn player id
    public int GetCurrentTurnPlayerId() {
        return currentPlayerTurn;
    }

    // Start game
    public void StartGame() {
        players[currentPlayerTurn].StartTurn();
    }

    // Move to next turn
    public void NextTurn() {
        currentPlayerTurn = (currentPlayerTurn + 1) % numPlayers;
        players[currentPlayerTurn].StartTurn();
    }

    // End game
    public void EndGame() {

    }
}
