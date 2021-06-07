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

    private bool _isPlayable;
    public bool isPlayable
    {
        get { return _isPlayable; }
        set { 
            if (value)
            {
                cardBorder.sprite = defaultBorder;
            }
            else
            {
                cardBorder.sprite = unplayableBorder;
            }
            _isPlayable = value;
        }
    }

    // Get card
    public abstract CardPiece GetCardPiece();

    // Set or unset highlighted border
    public void SetHighlighted(bool highlighted)
    {
        if (highlighted && isPlayable)
        {
            cardBorder.sprite = highlightedBorder;
        }
        else if (!isPlayable)
        {
            cardBorder.sprite = unplayableBorder;
        }
        else
        {
            cardBorder.sprite = defaultBorder;
        }
    }

    // Get whether border is highlighted
    public bool IsHighlighted() {
        if (cardBorder.sprite == highlightedBorder)
            return true;
        return false;
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
        player.PlayerClickOnCardInHand(this);
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
