using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour
{
    private Dictionary<ResourceType, Text> resourceLabels;
    public Text foodLabel;
    public Text woodLabel;
    public Text manaLabel;

    // Set a single resource label
    public void SetResource(ResourceType resourceType, int amount) {
        resourceLabels[resourceType].text = amount.ToString();
    }

    // Update all resource label
    public void UpdateAllResources(Dictionary<ResourceType, int> playerResources) {
        foreach (KeyValuePair<ResourceType, int> pair in playerResources) {
            resourceLabels[pair.Key].text = pair.Value.ToString();
        }
    }

    // Update starting resource labels
    public void UpdateStartingResources(Dictionary<ResourceType, int> playerResources) {
        foreach (KeyValuePair<ResourceType, int> pair in playerResources) {
            if (pair.Key == ResourceType.Food) {
                foodLabel.text = pair.Value.ToString();
            }
            else if (pair.Key == ResourceType.Wood) {
                woodLabel.text = pair.Value.ToString();
            }
            else {
                manaLabel.text = pair.Value.ToString();
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("awakening");
        resourceLabels = new Dictionary<ResourceType, Text>() {
             {ResourceType.Food, foodLabel },
             {ResourceType.Wood, woodLabel },
             {ResourceType.Mana, manaLabel }
         };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
