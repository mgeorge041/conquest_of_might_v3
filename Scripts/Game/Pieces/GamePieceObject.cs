using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePieceObject : MonoBehaviour
{
    public SpriteRenderer art;
    private GamePiece piece;

    // Lifebar
    public Image lifebar;
    public Image deathbar;
    public Image lifebarOverlay;
    public Transform pieceCanvas;

    // Collider
    public BoxCollider2D boxCollider;
    public PieceType pieceType;

    // Get game piece
    public GamePiece GetPiece() {
        return piece;
    }

    // Set game piece
    public void SetPiece(GamePiece piece) {
        this.piece = piece;
        piece.SetGamePieceObject(this);
    }

    // Destroy this game object
    public void Destroy() {
        Destroy(gameObject);
    }

    // Show piece as disabled
    public void ShowPieceDisabled() {
        if (piece.HasActions()) {
            art.color = new Color(1, 1, 1);
        }
        else {
            art.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    // Set piece object position
    public void SetPosition(GameMapObject gameMapObject) {
        Vector3Int tileCoords = Map.ConvertHexToTileCoords(piece.GetGameHex().GetHexCoords());
        Vector3 tilePosition = gameMapObject.tileGrid.CellToWorld(tileCoords);
        transform.position = new Vector3(tilePosition.x, tilePosition.y, -1);
    }

    // Set the width of the lifebar or deathbar
    public void SetLifebarFullWidth(Image lifebar, float overlayWidth) {
        RectTransform rectTransform = lifebar.transform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(overlayWidth, rectTransform.rect.height / 100);
    }

    // Set the width of the lifebar for current health
    public void SetLifebarCurrentHealth() {
        lifebar.fillAmount = (float)piece.GetCurrentHealth() / (float)piece.GetHealth();
    }

    // Set the card display's healthbar
    public void SetCardLifebarOverlay(Sprite lifebarOverlaySprite) {
        lifebarOverlay.sprite = lifebarOverlaySprite;
        float overlayWidth = lifebarOverlaySprite.rect.width / 100;

        // Set lifebar and deathbar widths
        SetLifebarFullWidth(lifebar, overlayWidth);
        SetLifebarFullWidth(deathbar, overlayWidth);

        // Set lifebar position for smaller sprites
        float spriteHeight = art.sprite.rect.height / 100;
        Vector3 position = pieceCanvas.position;
        pieceCanvas.localPosition = new Vector3(0, spriteHeight / 2, position.z);
    }

    // Get unit prefab
    public static GamePieceObject GetUnitPrefab() {
        GamePieceObject gamePieceObject = Resources.Load<GamePieceObject>("Prefabs/Unit");
        return gamePieceObject;
    }

    // Initialize from a card piece
    public static GamePieceObject InitializeFromCardPiece(CardPiece cardPiece, Transform parentTransform, Player player) {
        if (cardPiece.cardType == CardType.Unit) {
            GamePieceObject newUnitObject = Instantiate(GetUnitPrefab(), parentTransform);
            Unit newUnit = new Unit((CardUnit)cardPiece, player);
            newUnitObject.SetPiece(newUnit);
            return newUnitObject;
        }
        else if (cardPiece.cardType == CardType.Building) {
            GamePieceObject newBuildingObject = Instantiate(GetUnitPrefab(), parentTransform);
            Building newBuilding = new Building((CardBuilding)cardPiece, player);
            newBuildingObject.SetPiece(newBuilding);
            return newBuildingObject;
        }
        else {
            return null;
        }
    }

    // Initialize from a game piece
    public static GamePieceObject InitializeFromGamePiece(GamePiece piece, Transform parentTransform, int playerId) {
        CardPiece cardPiece = piece.GetCard();
        if (cardPiece.cardType == CardType.Unit) {
            GamePieceObject newUnitObject = Instantiate(GetUnitPrefab(), parentTransform);
            newUnitObject.SetPiece(piece);
            piece.SetGamePieceObject(newUnitObject);
            return newUnitObject;
        }
        else if (cardPiece.cardType == CardType.Building) {
            GamePieceObject newBuildingObject = Instantiate(GetUnitPrefab(), parentTransform);
            newBuildingObject.SetPiece(piece);
            piece.SetGamePieceObject(newBuildingObject);
            return newBuildingObject;
        }
        else {
            return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        art.sprite = piece.GetCard().artwork;
        lifebarOverlay.sprite = piece.GetCard().lifebarOverlay;
        SetCardLifebarOverlay(lifebarOverlay.sprite);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
