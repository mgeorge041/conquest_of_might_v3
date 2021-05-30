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
        //piece.gamePieceObject = this;
    }

    // Destroy this game object
    public void Destroy() {
        Destroy(gameObject);
    }

    // Show piece as disabled
    public void ShowPieceDisabled() {
        if (piece.hasActions) {
            art.color = new Color(1, 1, 1);
        }
        else {
            art.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    // Set piece object position
    public void SetPosition(GameMapObject gameMapObject) {
        Vector3Int tileCoords = Hex.HexToTileCoords(piece.gameHex.hexCoords);
        Vector3 tilePosition = gameMapObject.tileGrid.CellToWorld(tileCoords);
        Vector3 newPosition = new Vector3(tilePosition.x, tilePosition.y, -1);
        StartCoroutine(MoveOverTime(newPosition));
        //transform.position = new Vector3(tilePosition.x, tilePosition.y, -1);
    }

    // Animate movement of piece
    public IEnumerator AnimateMove(Vector3 newPosition) {
        while (transform.position != newPosition) {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, 5 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    // Animate movement of piece
    public IEnumerator MoveOverTime(Vector3 newPosition) {
        float elapsedTime = 0;
        float seconds = 2;
        Vector3 startingPos = transform.position;
        while (elapsedTime < seconds) {
            transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = newPosition;
    }

    // Set the width of the lifebar or deathbar
    public void SetLifebarFullWidth(Image lifebar, float overlayWidth) {
        RectTransform rectTransform = lifebar.transform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(overlayWidth, rectTransform.rect.height / 100);
    }

    // Set the width of the lifebar for current health
    public void SetLifebarCurrentHealth() {
        lifebar.fillAmount = (float)piece.currentHealth / (float)piece.health;
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
        GamePieceObject gamePieceObject = Resources.Load<GamePieceObject>(ENV.UNIT_PREFAB_RESOURCE_PATH);
        return gamePieceObject;
    }

    // Initialize from a card piece
    public static GamePieceObject InitializeFromCardPiece(CardPiece cardPiece, Transform parentTransform, Player player) {
        if (cardPiece.cardType == CardType.Unit) {
            GamePieceObject newUnitObject = Instantiate(GetUnitPrefab(), parentTransform);
            Unit newUnit = GamePiece.CreatePiece<Unit>(cardPiece, player);
            newUnitObject.SetPiece(newUnit);
            return newUnitObject;
        }
        else if (cardPiece.cardType == CardType.Building) {
            GamePieceObject newBuildingObject = Instantiate(GetUnitPrefab(), parentTransform);
            Building newBuilding = GamePiece.CreatePiece<Building>(cardPiece, player);
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
            //piece.gamePieceObject = newUnitObject;
            return newUnitObject;
        }
        else if (cardPiece.cardType == CardType.Building) {
            GamePieceObject newBuildingObject = Instantiate(GetUnitPrefab(), parentTransform);
            newBuildingObject.SetPiece(piece);
            //piece.gamePieceObject = newBuildingObject;
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
