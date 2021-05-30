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
            Sprite[] overlays = Resources.LoadAll<Sprite>(ENV.LIFEBAR_ART_RESOURCE_PATH);
            lifebarOverlay.sprite = overlays[value - 1];
            SetCardLifebarOverlay(lifebarOverlay.sprite);
        } 
    }
    public int currentHealth { get; set; }
    public int might { get; set; }
    public int range { get; set; }
    public int sightRange { get; set; }

    // Actions
    public bool canAttack { get; set; } = true;
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
    public abstract void SetCard(CardPiece cardPiece);

    // Create piece, decrease resources, and show hand playable cards
    public static T CreatePiece<T>(CardPiece cardPiece, Player player) where T : GamePiece
    {
        T newPiece = Instantiate(GetPiecePrefab<T>());
        newPiece.SetCard(cardPiece);
        newPiece.player = player;
        return newPiece;
    }

    // Create starting castle
    public static GamePiece CreateCastle(Player player) {
        CardBuilding castle = Resources.Load<CardBuilding>(ENV.CASTLE_CARD_RESOURCE_PATH);
        GamePiece castlePiece = CreatePiece<Building>(castle, player);
        return castlePiece;
    }

    // Get player ID
    public int GetPlayerId() {
        return player.playerId;
    }

    // Reset at beginning of turn
    public virtual void ResetPiece() {
        canAttack = true;
        CheckHasActions();
    }

    // End piece turn
    public virtual void EndTurn() {
        canAttack = false;
        CheckHasActions();
    }

    // Check whether piece has actions
    protected virtual void CheckHasActions() {
        if (!canAttack) {
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
    public void SetPosition(GameMap gameMap)
    {
        Vector3Int tileCoords = Hex.HexToTileCoords(gameHex.hexCoords);
        Vector3 tilePosition = gameMap.tileGrid.CellToWorld(tileCoords);
        Vector3 newPosition = new Vector3(tilePosition.x, tilePosition.y, 0);
        //StartCoroutine(MoveOverTime(newPosition));
        transform.position = new Vector3(tilePosition.x, tilePosition.y, 0);
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
        lifebar.fillAmount = currentHealth / (float)health;
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

    // Get piece prefab
    public static T GetPiecePrefab<T>() where T : GamePiece
    {
        if (typeof(T) == typeof(Unit))
        {
            T newUnit = Resources.Load<T>(ENV.UNIT_PREFAB_RESOURCE_PATH);
            return newUnit;
        }
        else if (typeof(T) == typeof(Building))
        {
            T newBuilding = Resources.Load<T>(ENV.BUILDING_PREFAB_RESOURCE_PATH);
            return newBuilding;
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetPiece();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
