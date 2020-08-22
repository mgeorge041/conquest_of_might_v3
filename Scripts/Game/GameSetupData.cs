using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSetupData
{
    public static int mapRadius = 8;
    public static int numPlayers = 2;
    public static Dictionary<int, Race> playerRaces;
    public static Dictionary<int, Hero> playerHeroes;
    public static Dictionary<int, Deck> playerDecks;
    public static Dictionary<string, Card> cards;
    public static bool localPlay = true;

    public static int deckEditorPlayerIndex = 0;
}
