using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardResourceDisplay : CardDisplay
{
    // Card info
    private CardResource cardResource;

    // Set card
    public override void SetCard(Card card)
    {
        if (card is CardResource)
        {
            CardResource cardResource = (CardResource)card;
            this.cardResource = cardResource;
        }
    }

    // Get card
    public override Card GetCard() {
        return cardResource;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetSharedInfo(cardResource);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
