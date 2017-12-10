using System;
using UnityEngine;

[Serializable]
public class Resource{
    [SerializeField]
    private bool isResearch;
    [SerializeField]
    private ResourceType resourceType;
    [SerializeField]
    private int maxValue;
    [SerializeField]
    private int currentValue;

    public Resource()
    {
        isResearch = true;
        resourceType = ResourceType.Gold;
        maxValue = -1;
        currentValue = 0;
    }

    public Resource(ResourceType resourceType, int currentValue = 0, int maxValue = -1, bool isResearch = true)
    {
        this.isResearch = isResearch;
        this.resourceType = resourceType;
        this.currentValue = currentValue;
        this.maxValue = maxValue;
    }

    public int CurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            currentValue = value;
        }
    }

    public string GetResourceName()
    {
        return resourceType.ToString();
    }

    public ResourceType ResourceType
    {
        get
        {
            return resourceType;
        }

        set
        {
            resourceType = value;
        }
    }
}

public enum ResourceType{
    Gold,
    Wood,
    Iron
}
