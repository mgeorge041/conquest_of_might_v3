using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObject : MonoBehaviour
{
    // Hand
    public Transform cardRegion;

    // Display variables
    float height;
    private bool collapsing = false;

    // Show or hide hand
    public void ToggleHand() {
        collapsing = !collapsing;
        if (collapsing) {
            Hand.CollapseHand(transform, height);
        }
        else {
            Hand.ExpandHand(transform, height);
        }
    }

    // Start is called before the first frame update
    void Start() {
        height = transform.GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update() {

    }
}