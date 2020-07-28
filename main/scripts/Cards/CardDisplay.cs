using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardDisplay : MonoBehaviour
{
    // Hand
    protected Player player;

    // Card data
    public Image cardArt;
    public Text cardName;
    public Image res1;
    public Text res1Cost;

    // Card images
    public Image cardBorder;

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
        Destroy(gameObject);
    }

    // Get and set card
    public abstract Card GetCard();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
