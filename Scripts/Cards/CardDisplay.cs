using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardDisplay : MonoBehaviour
{
    // Hand
    public Player player { get; set; }

    // Card data
    public Card card;
    public Image cardArt;
    public Text cardName;
    public Image res1;
    public Text res1Cost;
    public GameObject cardBack;

    // Card images
    public Image cardBorder;

    // Card rotation
    protected int flipTimer;

    // Sets info shared by all cards
    public void SetSharedInfo(Card card) {
        cardArt.sprite = card.artwork;
        cardName.text = card.cardName;

        // Resource images
        res1.sprite = Resources.Load<Sprite>("Art/UI/" + card.res1.ToString());
        res1Cost.text = card.res1Cost.ToString();
    }

    // Destroys card
    public void Destroy() {
        C.Destroy(gameObject);
    }

    // Get and set card
    public abstract Card GetCard();
    public abstract void SetCard(Card card);

    // Get piece prefab
    public static CardDisplay GetCardDisplayPrefab(Card card)
    {
        if (card.cardType == CardType.Unit)
        {
            CardUnitDisplay newUnit = Resources.Load<CardUnitDisplay>(ENV.CARD_UNIT_DISPLAY_PREFAB_RESOURCE_PATH);
            return newUnit;
        }
        else if (card.cardType == CardType.Building)
        {
            CardBuildingDisplay newBuilding = Resources.Load<CardBuildingDisplay>(ENV.CARD_BUILDING_DISPLAY_PREFAB_RESOURCE_PATH);
            return newBuilding;
        }
        else if (card.cardType == CardType.Resource)
        {
            CardResourceDisplay newResource = Resources.Load<CardResourceDisplay>(ENV.CARD_RESOURCE_DISPLAY_PREFAB_RESOURCE_PATH);
            return newResource;
        }
        return null;
    }

    // Initialize into the parent transform
    public static CardDisplay CreateCardDisplay(Card card)
    {
        CardDisplay newCardDisplay = Instantiate(GetCardDisplayPrefab(card));
        newCardDisplay.SetCard(card);
        return newCardDisplay;
    }

    // Initialize into the parent transform
    public static CardDisplay CreateCardDisplay(Card card, Transform parentTransform)
    {
        CardDisplay newCardDisplay = Instantiate(GetCardDisplayPrefab(card), parentTransform);
        newCardDisplay.SetCard(card);
        return newCardDisplay;
    }

    // Set is drawn
    public void SetIsDrawn() {
        transform.localScale = new Vector3(0, 0, 1);
        transform.Rotate(new Vector3(0, -180, 0));
        cardBack.SetActive(true);
        StartCoroutine(RotateDrawnCard());
    }

    // Rotate drawn card
    public IEnumerator RotateDrawnCard() {
        float scaleIncrease = 1f / 180f;
        for (int i = 0; i < 180; i++) {
            yield return null;
            transform.Rotate(new Vector3(0, 1, 0));
            transform.localScale = new Vector3(transform.localScale.x + scaleIncrease, transform.localScale.y + scaleIncrease, 1);
            flipTimer++;

            if (flipTimer == 90) { 
                Debug.Log("setting false at: " + transform.rotation.y);
                cardBack.SetActive(false);
            }
        }
        flipTimer = 0;
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
