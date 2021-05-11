using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class CardPieceDisplay : CardDisplay, IPointerClickHandler
{
    // Card data
    public Image res2;
    public Text res2Cost;

    // Card images
    public Sprite defaultBorder;
    public Sprite highlightedBorder;
    public Sprite unplayableBorder;

    // Card stats
    public Text cardMight;
    public Text cardRange;

    // Lifebar
    public Image lifebar;
    public Image deathbar;
    public Image lifebarOverlay;

    private bool isPlayable = true;
    private bool isHighlighted = false;

    // Get card
    public abstract CardPiece GetCardPiece();

    // Set player
    public void SetPlayerObject(PlayerObject playerObject) {
        this.playerObject = playerObject;
    }

    // Set playable card border status
    public void SetPlayableBorder() {
        cardBorder.sprite = defaultBorder;
        isPlayable = true;
    }

    // Set unplayable card border status
    public void SetUnplayableBorder() {
        cardBorder.sprite = unplayableBorder;
        isPlayable = false;
    }

    // Set card highlighted border
    public void SetHighlighted(bool highlighted) {
        isHighlighted = highlighted;
        if (highlighted) {
            cardBorder.sprite = highlightedBorder;
            
        }
        else {
            cardBorder.sprite = defaultBorder;
        }
    }

    // Get whether border is highlighted
    public bool IsHighlighted() {
        return isHighlighted;
    }

    // Set the width of the lifebar or deathbar
    private void SetLifebarWidth(Image bar, float overlayWidth) {
        RectTransform rectTransform = bar.transform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(overlayWidth, rectTransform.rect.height);
    }

    // Set the card display's healthbar
    public void SetCardLifebarOverlay(Sprite lifebarOverlaySprite) {
        lifebarOverlay.sprite = lifebarOverlaySprite;
        float overlayWidth = lifebarOverlaySprite.rect.width;

        // Set lifebar and deathbar widths
        SetLifebarWidth(lifebar, overlayWidth);
        SetLifebarWidth(deathbar, overlayWidth);
    }

    // Set highlighted status on click
    public void OnPointerClick(PointerEventData pointerEventData) {
        if (GetCardPiece().isPlayable && playerObject.IsTurn()) {

            // Deselect if already highlighted
            if (!isHighlighted) {
                SetHighlighted(true);
                playerObject.SetSelectedCard(this);
            }
            else {
                SetHighlighted(false);
                playerObject.SetSelectedCard(null);
            }
        }
    }

    // Show 2nd resource costs
    public void Set2ndResource(CardPiece cardPiece) {
        if (cardPiece.res2 != ResourceType.None) {
            res2.sprite = Resources.Load<Sprite>("Art/UI/" + cardPiece.res2.ToString());
            res2Cost.text = cardPiece.res2Cost.ToString();
        }
        else {
            res2.gameObject.SetActive(false);
            res2Cost.gameObject.SetActive(false);
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
