using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeavyWeightAlgorithm : MonoBehaviour
{
    public int exponent = 2;
    public GameObject coinPrefab;

    public int spacing = 2;

    private GameObject fakeCoin;
    private GameObject[] coins;

    private Dictionary<GameObject, int> coinWeights = new Dictionary<GameObject, int>();

    private bool advanceStep = false;

    // Start is called before the first frame update
    void Start()
    {
        coins = new GameObject[(int)Math.Pow(3, exponent)];
        SpawnCoins();
        StartSearch();
    }

    void Update()
    {
        // Wait for user input (e.g., space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            advanceStep = true;
        }
    }

    private void SpawnCoins()
    {
        int totalCoins = (int)Math.Pow(3, exponent);
        for (int i = 0; i < totalCoins; i++)
        {
            coins[i] = Instantiate(coinPrefab, new Vector3(i * spacing, 0, 0), Quaternion.identity);
            coinWeights[coins[i]] = 1; // Set the default weight to 1
        }

        int fakeCoinIdx = UnityEngine.Random.Range(0, totalCoins);
        fakeCoin = coins[fakeCoinIdx];
        coinWeights[fakeCoin] = 2; // Set the fake coin weight to 2
        //SetColor(fakeCoin, Color.red);
        fakeCoin.transform.localScale = new Vector3(1, 2, 1); // Make the fake coin taller
    }

    private void StartSearch()
    {
        // Start the sorting algorithm
        StartCoroutine(SearchCoins(coins));
    }

    /**
     * Heavy Weight Algorithm
     * Divide the coins into three piles and compare their weights
     * The pile with the different weight is the one with the fake coin
     * Repeat until only one coin is left
     */
    private IEnumerator SearchCoins(GameObject[] coinsToSearch)
    {
        GameObject[] searchPile = coinsToSearch;

        int leftToSearch = searchPile.Length;

        while (searchPile.Length > 1)
        {
            ColorSelected(searchPile, Color.yellow); // Color coins to weigh yellow
            //SetColor(fakeCoin, Color.red);
            int pileSize = leftToSearch / 3;

            yield return WaitForStep();

            // Divide the coins into three piles and color them uniquely
            GameObject[] pile1 = searchPile[0..pileSize];
            GameObject[] pile2 = searchPile[pileSize..(pileSize * 2)];
            GameObject[] pile3 = searchPile[(pileSize * 2)..leftToSearch];

            ColorSelected(pile1, Color.red);
            ColorSelected(pile2, Color.blue);
            ColorSelected(pile3, Color.black);

            //SetColor(fakeCoin, Color.red);

            yield return WaitForStep();

            // Compare the first two piles weights
            int weight1 = GetWeight(pile1);
            int weight2 = GetWeight(pile2);

            if (weight1 == weight2)
            {
                searchPile = pile3;
            }
            else if (weight1 > weight2)
            {
                searchPile = pile1;
            }
            else
            {
                searchPile = pile2;
            }

            leftToSearch = searchPile.Length;
            ColorSelected(coins, Color.white);
        }

        SetColor(searchPile[0], Color.green); // Color the found coin green
    }

    private int GetWeight(GameObject[] coinsToCheck)
    {
        int totalWeight = 0;
        foreach (GameObject coin in coinsToCheck)
        {
            if (coin != null)
            {
                totalWeight += coinWeights[coin];
            }
        }
        return totalWeight;
    }

    private IEnumerator WaitForStep()
    {
        advanceStep = false;
        while (!advanceStep)
        {
            yield return null; // Wait until next frame
        }

    }

    private void ColorSelected(GameObject[] selectedObjs, Color colorWith)
    {
        foreach (GameObject go in selectedObjs)
        {
            if (go != null)
            {
                go.GetComponent<Renderer>().material.color = colorWith;

            }
        }
    }

    // Sets the color of the object
    private void SetColor(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }
}
