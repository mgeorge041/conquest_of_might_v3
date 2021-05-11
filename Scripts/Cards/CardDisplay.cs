using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardDisplay : MonoBehaviour
{
    // Hand
    protected PlayerObject playerObject;

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

    // Get card unit display prefab
    private static CardUnitDisplay GetCardUnitDisplayPrefab() {
        CardUnitDisplay cardUnitDisplayPrefab = Resources.Load<CardUnitDisplay>("Prefabs/Card Unit Display");
        return cardUnitDisplayPrefab;
    }

    // Get card building display prefab
    private static CardBuildingDisplay GetCardBuildingDisplayPrefab() {
        CardBuildingDisplay cardBuildingDisplayPrefab = Resources.Load<CardBuildingDisplay>("Prefabs/Card Building Display");
        return cardBuildingDisplayPrefab;
    }

    // Get card resource display prefab
    private static CardResourceDisplay GetCardResourceDisplayPrefab() {
        CardResourceDisplay cardResourceDisplayPrefab = Resources.Load<CardResourceDisplay>("Prefabs/Card Resource Display");
        return cardResourceDisplayPrefab;
    }

    // Initialize into the parent transform
    public static CardDisplay Initialize(Card card, Transform parentTransform) {
        if (card.cardType == CardType.Unit) {
            CardUnitDisplay cardUnitDisplay = Instantiate(GetCardUnitDisplayPrefab(), parentTransform);
            cardUnitDisplay.SetCard((CardUnit)card);
            return cardUnitDisplay;
        }
        else if (card.cardType == CardType.Building) {
            CardBuildingDisplay cardBuildingDisplay = Instantiate(GetCardBuildingDisplayPrefab(), parentTransform);
            cardBuildingDisplay.SetCard((CardBuilding)card);
            return cardBuildingDisplay;
        }
        else if (card.cardType == CardType.Resource) {
            CardResourceDisplay cardResourceDisplay = Instantiate(GetCardResourceDisplayPrefab(), parentTransform);
            cardResourceDisplay.SetCard((CardResource)card);
            return cardResourceDisplay;
        }
        else {
            return null;
        }
        /*
        else if (cardType == CardType.Spell)
        {
            Debug.Log("i");
        }
        */
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
