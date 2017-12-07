using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : MonoBehaviour {

    [SerializeField]
    private Resource[] resources = new Resource[Enum.GetNames(typeof(ResourceType)).Length];
	// Use this for initialization
	void Start () {
		for(int i = 0; i< resources.Length; i++)
        {
            resources[i] = new Resource((ResourceType)i);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
