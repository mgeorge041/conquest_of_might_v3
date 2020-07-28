using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPiece : Card
{
    public int health;
    public int might;
    public int range;
    public int sightRange;
    public Sprite lifebarOverlay;

    public ResourceType res2;
    public int res2Cost;

    // Get resource costs
    public Dictionary<ResourceType, int> GetResourceCosts() {
        Dictionary<ResourceType, int> resourceCosts = new Dictionary<ResourceType, int>();
        resourceCosts.Add(res1, res1Cost);
        if (res2 != ResourceType.None) {
            resourceCosts.Add(res2, res2Cost);
        }

        return resourceCosts;
    }
}
