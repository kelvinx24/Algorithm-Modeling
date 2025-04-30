    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortAlgorithm : MonoBehaviour
{
    public List<GameObject> arrayToSort;

    public int sortItemsAmount = 1;

    public GameObject sortPrefab;

    private int spacing = 2;

    private bool advanceStep = false;

    private System.Random rand;

    void SpawnStarting()
    {
        for (int i = 0; i < sortItemsAmount; i++)
        {
            Vector3 pos = new Vector3(spacing * i, 0, 0);
            GameObject spawnedObject = Instantiate(sortPrefab);
            spawnedObject.transform.position = pos;

            int objYScale = Random.Range(1, 8);
            spawnedObject.transform.localScale = new Vector3(1, objYScale, 1);

            arrayToSort.Add(spawnedObject);
        }
    }

    void Start()
    {
        SpawnStarting();
        StartCoroutine(BubbleSortCoroutine(arrayToSort.ToArray()));
    }

    void Update()
    {
        // Wait for user to press space to advance to next step
        if (Input.GetKeyDown(KeyCode.Space))
        {
            advanceStep = true;
        }
    }

    IEnumerator BubbleSortCoroutine(GameObject[] unsorted)
    {
        rand = new System.Random();

        yield return StartCoroutine(BubbleSortControlled(unsorted));

        for (int i = 0; i < unsorted.Length; i++)
        {
            SetColor(unsorted[i], Color.green);
        }
    }

    IEnumerator BubbleSortControlled(GameObject[] unsorted)
    {
        int n = unsorted.Length;
        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false;
            SetColor(unsorted[n - i - 1], Color.green); // Color the position that the largest element is being found for

            for (int j = 0; j < n - i - 1; j++)
            {
                SetColor(unsorted[j], Color.red);
                SetColor(unsorted[j + 1], Color.blue);
                yield return WaitForUserInput(); // Pause until space is pressed

                float heightA = unsorted[j].transform.localScale.y;
                float heightB = unsorted[j + 1].transform.localScale.y;

                if (heightA > heightB)
                {
                    SwapBars(unsorted, j, j + 1);
                    SetColor(unsorted[j], Color.blue);
                    SetColor(unsorted[j + 1], Color.red);
                    yield return WaitForUserInput(); // Pause until space is pressed
                    swapped = true;
                }

                SetColor(unsorted[j], Color.white);
                SetColor(unsorted[j + 1], Color.white);
            }

            if (!swapped)
            {
                ResetAndColorSelected(unsorted, Color.black);
                yield return WaitForUserInput(); // Pause until space is pressed
                break; // No swaps means the array is sorted
            }
        }
    }

    IEnumerator WaitForUserInput()
    {
        advanceStep = false;
        while (!advanceStep)
        {
            yield return null; // Wait until next frame
        }
    }

    void SwapBars(GameObject[] unsorted,  int indexA, int indexB)
    {
        GameObject temp = unsorted[indexA];
        unsorted[indexA] = unsorted[indexB];
        unsorted[indexB] = temp;

        Vector3 posA = unsorted[indexA].transform.position;
        Vector3 posB = unsorted[indexB].transform.position;

        unsorted[indexA].transform.position = new Vector3(posB.x, posA.y, posA.z);
        unsorted[indexB].transform.position = new Vector3(posA.x, posB.y, posB.z);
    }

    void SetColor(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }

    void ResetAndColorSelected(GameObject[] selectedObj, Color colorWith)
    {
        foreach (GameObject go in arrayToSort)
        {
            go.GetComponent<Renderer>().material.color = Color.white;
        }

        foreach (GameObject go in selectedObj)
        {
            if (go != null)
            {
                go.GetComponent<Renderer>().material.color = colorWith;

            }
        }

    }

    void ColorSelected(GameObject[] selectedObjs, Color colorWith)
    {
        foreach (GameObject go in selectedObjs)
        {
            if (go != null)
            {
                go.GetComponent<Renderer>().material.color = colorWith;

            }
        }
    }
}
