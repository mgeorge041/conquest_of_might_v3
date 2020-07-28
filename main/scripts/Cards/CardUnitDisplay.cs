using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUnitDisplay : CardPieceDisplay
{
    // Card info
    private CardUnit cardUnit;
    
    // Unit stats
    public Text cardSpeed;

    // Set card unit
    public void SetCard(CardUnit cardUnit) {
        this.cardUnit = cardUnit;
    }

    // Get card
    public override CardPiece GetCardPiece() {
        return cardUnit;
    }

    // Get card
    public override Card GetCard() {
        return cardUnit;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetSharedInfo(cardUnit);
        Set2ndResource(cardUnit);
        SetCardLifebarOverlay(cardUnit.lifebarOverlay);
        cardMight.text = cardUnit.might.ToString();
        cardRange.text = cardUnit.range.ToString();
        cardSpeed.text = cardUnit.speed.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
