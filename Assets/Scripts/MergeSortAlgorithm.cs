using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MergeSortAlgorithm : MonoBehaviour
{
    public List<GameObject> arrayToSort;

    private List<GameObject> visualizers;

    public int sortItemsAmount = 1;

    public GameObject sortPrefab;

    private bool stepReady = false;

    private int spacing = 2;

    private int overallLeft = 0;
    
    private int overallRight = 0;

    private System.Random rand;

    void Start()
    {
        rand = new System.Random();
        SpawnStarting();
        overallRight = arrayToSort.Count - 1;

        StartCoroutine(MergeSortCoroutine(arrayToSort.ToArray()));
    }

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

            /*
            Vector3 visPos = pos + new Vector3(0, 5, 0);
            GameObject vis = Instantiate(sortPrefab);
            vis.SetActive(false);
            vis.transform.localPosition = visPos;
            vis.transform.localScale = new Vector3(1, objYScale, 1);

            visualizers.Add(vis);
            */
        }
    }

    void Update()
    {
        // Wait for user input (e.g., space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stepReady = true;
        }
    }

    IEnumerator MergeSortCoroutine(GameObject[] unsorted)
    {
        GameObject[] result = null;
        yield return StartCoroutine(SortCoroutine(unsorted, val => result = val));

        // Reposition sorted result
        for (int i = 0; i < result.Length; i++)
        {
            result[i].transform.position = new Vector3(i * spacing, 0, 0);
            result[i].GetComponent<Renderer>().material.color = Color.green;
        }
    }

    IEnumerator SortCoroutine(GameObject[] unsorted, System.Action<GameObject[]> callback)
    {
        if (unsorted.Length <= 1)
        {
            ResetAndColorSelected(unsorted, Color.yellow);
            callback(unsorted);
            yield break;
        }


        int mid = unsorted.Length / 2;
        GameObject[] left = new GameObject[mid];
        GameObject[] right = new GameObject[unsorted.Length - mid];
        // Copies a range of elements from an Array starting at the specified source index and pastes them to another Array starting at the specified destination index
        // Last argument is length of copy
        System.Array.Copy(unsorted, 0, left, 0, mid);
        System.Array.Copy(unsorted, mid, right, 0, right.Length);

        // Color left and right halves for visualization
        ColorSelected(arrayToSort.ToArray(), Color.white);
        ColorSelected(left, Color.red);
        ColorSelected(right, Color.blue);


        yield return WaitForStep();

        GameObject[] sortedLeft = null;
        GameObject[] sortedRight = null;

        // Recursively sort the left and right halves
        // The callback is a lambda function that captures the sorted array
        // callback is called when the sorting is done
        // Either base case (first argument, as is) or merged is set to sortedLeft or sortedRight
        int originalLeft = overallLeft;
        int originalRight = overallRight;

        overallRight = originalLeft + mid - 1;

        Debug.Log("(LEFT) CURRENT LEFT: " + overallLeft + " CURRENT RIGHT: " + overallRight);
        yield return StartCoroutine(SortCoroutine(left, val => sortedLeft = val));
        overallRight = originalRight;
        yield return WaitForStep();
        overallLeft = originalLeft + mid;
        Debug.Log("(RIGHT) CURRENT LEFT: " + overallLeft + " CURRENT RIGHT: " + overallRight);
        yield return StartCoroutine(SortCoroutine(right, val => sortedRight = val));
        overallLeft = originalLeft;
        yield return WaitForStep();


        GameObject[] merged = null;

        // Merge the sorted halves, and set the merged array   
        ColorSelected(arrayToSort.ToArray(), Color.white);
        ColorSelected(left, Color.red);
        ColorSelected(right, Color.blue);
        yield return StartCoroutine(MergeCoroutine(sortedLeft, sortedRight, val => merged = val));

        callback(merged);
    }

    IEnumerator MergeCoroutine(GameObject[] left, GameObject[] right, System.Action<GameObject[]> callback)
    {
        int leftIndex = 0, rightIndex = 0;
        GameObject[] merged = new GameObject[left.Length + right.Length];
        GameObject[] visualizers = new GameObject[left.Length + right.Length];

        int overallIdx = 0;

        for (int i = 0; i < merged.Length; i++)
        {

            if (leftIndex < left.Length && rightIndex < right.Length)
            {
                if (left[leftIndex].transform.localScale.y <= right[rightIndex].transform.localScale.y)
                {
                    merged[i] = left[leftIndex++];
                }
                else
                {
                    merged[i] = right[rightIndex++];
                }
            }
            else if (leftIndex < left.Length)
            {
                merged[i] = left[leftIndex++];
            }
            else
            {
                merged[i] = right[rightIndex++];
            }


            Vector3 currentMergedPosition; 
            if (overallIdx < left.Length)
            {
                Debug.Log("Left: " + overallIdx);
                currentMergedPosition = left[overallIdx].transform.position;
            }
            else
            {
                Debug.Log("Right: " + overallIdx);
                currentMergedPosition = right[overallIdx - left.Length].transform.position;
            }

            currentMergedPosition = currentMergedPosition + new Vector3(0, 10, 0);
 

            GameObject visualizer = Instantiate(merged[i].gameObject);
            // Break material link
            Renderer copyRenderer = visualizer.GetComponent<Renderer>();
            if (copyRenderer != null)
            {
                copyRenderer.material = new Material(copyRenderer.material);
            }

            visualizer.transform.localPosition = currentMergedPosition;
            visualizers[overallIdx] = visualizer;
                
            merged[i].GetComponent<Renderer>().material.color = Color.black;

            // Update visual position
            overallIdx++;

            //merged[i].transform.position = new Vector3(i * spacing, 0, 0);
            //ResetAndColorSelected(merged, Color.black);

            yield return WaitForStep();
        }

        foreach (GameObject vis in visualizers)
        {
            vis.SetActive(false);
        }

        CorrectPositions(merged);
        callback(merged);
    }

    IEnumerator WaitForStep()
    {
        while (!stepReady)
            yield return null;

        stepReady = false;
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

    void CorrectPositions(GameObject[] sortedObjs)
    {
        for (int i = 0; i < sortedObjs.Length; i++)
        {
            GameObject go = sortedObjs[i];

            go.transform.localPosition = new Vector3((overallLeft + i) * spacing, 0, 0);
        }
    }
}