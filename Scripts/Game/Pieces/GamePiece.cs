using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GamePiece : MonoBehaviour
{
    // Stats
    public string pieceName { get; set; }
    private int Health;
    public int health {
        get { return Health; }
        set {
            Health = value;
            Sprite[] overlays = Resources.LoadAll<Sprite>("Art/Cards/Healthbar Overlay");
            lifebarOverlay.sprite = overlays[value - 1];
            SetCardLifebarOverlay(lifebarOverlay.sprite);
        } 
    }
    public int currentHealth { get; set; }
    public int might { get; set; }
    public int range { get; set; }
    public int sightRange { get; set; }

    // Actions
    public bool canAttack { get; set; } = false;
    public bool canMove { get; set; } 
    public bool hasActions { get; protected set; } = true;

    public SpriteRenderer art;

    // Lifebar
    public Image lifebar;
    public Image deathbar;
    public Image lifebarOverlay;
    public Transform pieceCanvas;
    private const float MAX_HEALTHBAR_WIDTH = 0.64f;
    private const float HEALTHBAR_HEIGHT = 0.1f;

    // Collider
    public PieceType pieceType;
    public BoxCollider2D boxCollider;

    // Player
    public Player player { get; set; }

    // Game hex
    public GameHex gameHex { get; set; }

    public abstract CardPiece GetCard();

    // Create piece, decrease resources, and show hand playable cards
    public static GamePiece CreatePiece(CardPiece cardPiece, Player player) {
        if (cardPiece.cardType == CardType.Unit) {
            Unit newUnit = new Unit((CardUnit)cardPiece, player);
            return newUnit;
        }
        else {
            Building newBuilding = new Building((CardBuilding)cardPiece, player);
            return newBuilding;
        }
    }

    // Create starting castle
    public static GamePiece CreateCastle(Player player) {
        CardBuilding castle = Resources.Load<CardBuilding>("Cards/Building/Castle");
        GamePiece castlePiece = CreatePiece(castle, player);
        return castlePiece;
    }

    // Get player ID
    public int GetPlayerId() {
        return player.playerId;
    }

    // Reset at beginning of turn
    public void ResetPiece() {
        canAttack = true;
        canMove = true;
        CheckHasActions();
    }

    // End piece turn
    public void EndTurn() {
        canAttack = false;
        canMove = false;
        CheckHasActions();
    }

    // Check whether piece has actions
    protected void CheckHasActions() {
        if (!canAttack && !canMove) {
            hasActions = false;
            ShowPieceDisabled();
        }
        else {
            hasActions = true;
            ShowPieceDisabled();
        }
    }

    // Attack piece
    public int AttackPiece(GamePiece targetPiece) {
        int damageGiven = targetPiece.TakeDamage(might);
        //player.GetPlayerGameData().AddDamageGiven(damageGiven);
        //player.ClearSelectedPiece();
        EndTurn();
        return damageGiven;
    }

    // Take damage
    public int TakeDamage(int damage) {
        int damageTaken = Math.Min(damage, currentHealth);
        currentHealth -= damageTaken;
        //player.GetPlayerGameData().AddDamageTaken(damageTaken);
        return damageTaken;
    }

    // Set shared attributes
    public void SetCardStats(CardPiece cardPiece) {

        // Set stats
        pieceName = cardPiece.cardName;
        health = cardPiece.health;
        currentHealth = health;
        might = cardPiece.might;
        range = cardPiece.range;
        sightRange = cardPiece.sightRange;

        // Set artwork
        art.sprite = cardPiece.artwork;
        lifebarOverlay.sprite = cardPiece.lifebarOverlay;
        SetCardLifebarOverlay(lifebarOverlay.sprite);

        ResetPiece();
    }




    // Show piece as disabled
    public void ShowPieceDisabled()
    {
        if (hasActions)
        {
            art.color = new Color(1, 1, 1);
        }
        else
        {
            art.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    // Set piece object position
    public void SetPosition(GameMapObject gameMapObject)
    {
        Vector3Int tileCoords = Hex.HexToTileCoords(gameHex.hexCoords);
        Vector3 tilePosition = gameMapObject.tileGrid.CellToWorld(tileCoords);
        Vector3 newPosition = new Vector3(tilePosition.x, tilePosition.y, -1);
        StartCoroutine(MoveOverTime(newPosition));
        //transform.position = new Vector3(tilePosition.x, tilePosition.y, -1);
    }

    // Animate movement of piece
    public IEnumerator AnimateMove(Vector3 newPosition)
    {
        while (transform.position != newPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, 5 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    // Animate movement of piece
    public IEnumerator MoveOverTime(Vector3 newPosition)
    {
        float elapsedTime = 0;
        float seconds = 2;
        Vector3 startingPos = transform.position;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = newPosition;
    }

    // Set the width of the lifebar for current health
    public void SetLifebarCurrentHealth()
    {
        lifebar.fillAmount = (float)currentHealth / (float)health;
    }

    // Set the card display's healthbar
    public void SetCardLifebarOverlay(Sprite lifebarOverlaySprite)
    {
        lifebarOverlay.sprite = lifebarOverlaySprite;
        float overlayWidth = Math.Min((float)health / 10, MAX_HEALTHBAR_WIDTH);

        // Set lifebar sizes
        lifebarOverlay.rectTransform.sizeDelta = new Vector2(overlayWidth, HEALTHBAR_HEIGHT);
        lifebar.rectTransform.sizeDelta = new Vector2(overlayWidth, HEALTHBAR_HEIGHT);
        deathbar.rectTransform.sizeDelta = new Vector2(overlayWidth, HEALTHBAR_HEIGHT);

        // Set lifebar position for smaller sprites
        float spriteHeight = art.sprite.rect.height / 100;
        Vector3 position = pieceCanvas.position;
        pieceCanvas.localPosition = new Vector3(0, spriteHeight / 2, position.z);
    }

    // Get unit prefab
    public static GamePieceObject GetUnitPrefab()
    {
        GamePieceObject gamePieceObject = Resources.Load<GamePieceObject>("Prefabs/Unit");
        return gamePieceObject;
    }

    // Initialize from a card piece
    public static GamePieceObject InitializeFromCardPiece(CardPiece cardPiece, Transform parentTransform, Player player)
    {
        if (cardPiece.cardType == CardType.Unit)
        {
            GamePieceObject newUnitObject = Instantiate(GetUnitPrefab(), parentTransform);
            Unit newUnit = new Unit((CardUnit)cardPiece, player);
            newUnitObject.SetPiece(newUnit);
            return newUnitObject;
        }
        else if (cardPiece.cardType == CardType.Building)
        {
            GamePieceObject newBuildingObject = Instantiate(GetUnitPrefab(), parentTransform);
            Building newBuilding = new Building((CardBuilding)cardPiece, player);
            newBuildingObject.SetPiece(newBuilding);
            return newBuildingObject;
        }
        else
        {
            return null;
        }
    }

    // Initialize from a game piece
    public static GamePieceObject InitializeFromGamePiece(GamePiece piece, Transform parentTransform, int playerId)
    {
        CardPiece cardPiece = piece.GetCard();
        if (cardPiece.cardType == CardType.Unit)
        {
            GamePieceObject newUnitObject = Instantiate(GetUnitPrefab(), parentTransform);
            newUnitObject.SetPiece(piece);
            //piece.gamePieceObject = newUnitObject;
            return newUnitObject;
        }
        else if (cardPiece.cardType == CardType.Building)
        {
            GamePieceObject newBuildingObject = Instantiate(GetUnitPrefab(), parentTransform);
            newBuildingObject.SetPiece(piece);
            //piece.gamePieceObject = newBuildingObject;
            return newBuildingObject;
        }
        else
        {
            return null;
        }
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
