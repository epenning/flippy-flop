﻿using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {

    public float thickness;

    public float numBlocks;

    public GameObject blockPrefab;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().enabled = false;

        numBlocks = transform.localScale.x;
        transform.localScale = Vector3.one;

        GetComponent<BoxCollider2D>().size = new Vector2(numBlocks, 1);

        thickness = transform.localScale.y;

        float xPos = -(numBlocks) / 2 + 0.5f;
        for(int i = 0; i < numBlocks; i++)
        {
            GameObject newBlock = GameObject.Instantiate(blockPrefab);
            newBlock.transform.parent = transform;
            newBlock.transform.localPosition = new Vector3(xPos, 0, 0);
            xPos += 1;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
