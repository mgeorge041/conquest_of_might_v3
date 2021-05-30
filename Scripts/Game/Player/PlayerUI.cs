using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Dictionary<ResourceType, Text> resourceLabels;
    public Text foodLabel;
    public Text woodLabel;
    public Text manaLabel;

    // Get resource count
    public int GetResourceCount(ResourceType resourceType)
    {
        if (resourceType == ResourceType.Food)
        {
            return int.Parse(foodLabel.text);
        }
        else if (resourceType == ResourceType.Wood)
        {
            return int.Parse(woodLabel.text);
        }
        else if (resourceType == ResourceType.Mana)
        {
            return int.Parse(manaLabel.text);
        }
        return 0;
    }

    // Set a single resource label
    public void SetResource(ResourceType resourceType, int amount)
    {
        resourceLabels[resourceType].text = amount.ToString();
    }

    // Update all resource label
    public void UpdateAllResources(Dictionary<ResourceType, int> playerResources)
    {
        foodLabel.text = playerResources[ResourceType.Food].ToString();
        woodLabel.text = playerResources[ResourceType.Wood].ToString();
        manaLabel.text = playerResources[ResourceType.Mana].ToString();
    }

    // Update starting resource labels
    public void UpdateStartingResources(Dictionary<ResourceType, int> playerResources)
    {
        foreach (KeyValuePair<ResourceType, int> pair in playerResources)
        {
            if (pair.Key == ResourceType.Food)
            {
                foodLabel.text = pair.Value.ToString();
            }
            else if (pair.Key == ResourceType.Wood)
            {
                woodLabel.text = pair.Value.ToString();
            }
            else
            {
                manaLabel.text = pair.Value.ToString();
            }
        }
    }
}
