using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager
{
    private List<Player> players = new List<Player>();
    private int numPlayers;
    private int currentPlayerTurn = 0;
        
    // Constructor
    public GameManager(int numPlayers, GameMap gameMap) {
        this.numPlayers = numPlayers;

        // All the start positions, 2 spaces from the corners of the board
        Vector3Int[] startLocations = {
            new Vector3Int(0, gameMap.GetMapRadius() - 2, -(gameMap.GetMapRadius() - 2)),
            new Vector3Int(0, -(gameMap.GetMapRadius() - 2), gameMap.GetMapRadius() - 2),
            new Vector3Int(-(gameMap.GetMapRadius() - 2), gameMap.GetMapRadius() - 2, 0),
            new Vector3Int(gameMap.GetMapRadius() - 2, -(gameMap.GetMapRadius() - 2), 0),
            new Vector3Int(-(gameMap.GetMapRadius() - 2), 0, gameMap.GetMapRadius() - 2),
            new Vector3Int(gameMap.GetMapRadius() - 2, 0, -(gameMap.GetMapRadius() - 2))
        };

        // Create players
        for (int i = 0; i < numPlayers; i++) {
            Vector3Int startTileCoords = Hex.HexToTileCoords(startLocations[i]);
            Player newPlayer = new Player(i, startTileCoords, gameMap, this);
            gameMap.AddPiece(GamePiece.CreateCastle(newPlayer), startTileCoords);

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
        for (int i = 0; i < numPlayers; i++) {
            players[i].StartFirstTurn();
        }
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
