using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MergeSortAlgorithm : MonoBehaviour
{
    public GameObject[] arrayToSort;
    private bool stepReady = false;

    void Start()
    {
        StartCoroutine(MergeSortCoroutine(arrayToSort));
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
            result[i].transform.position = new Vector3(i, 0, 0);
            result[i].GetComponent<Renderer>().material.color = Color.green;
        }
    }

    IEnumerator SortCoroutine(GameObject[] unsorted, System.Action<GameObject[]> callback)
    {
        if (unsorted.Length <= 1)
        {
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
        for (int i = 0; i < left.Length; i++)
        {
            left[i].GetComponent<Renderer>().material.color = Color.red;
        }

        for (int i = 0; i < right.Length; i++)
        {
            right[i].GetComponent<Renderer>().material.color = Color.blue;
        }


        yield return WaitForStep();

        GameObject[] sortedLeft = null;
        GameObject[] sortedRight = null;

        // Recursively sort the left and right halves
        // The callback is a lambda function that captures the sorted array
        // callback is called when the sorting is done
        // Either base case (first argument, as is) or merged is set to sortedLeft or sortedRight
        yield return StartCoroutine(SortCoroutine(left, val => sortedLeft = val));
        yield return StartCoroutine(SortCoroutine(right, val => sortedRight = val));

        GameObject[] merged = null;

        // Merge the sorted halves, and set the merged array   
        yield return StartCoroutine(MergeCoroutine(sortedLeft, sortedRight, val => merged = val));

        callback(merged);
    }

    IEnumerator MergeCoroutine(GameObject[] left, GameObject[] right, System.Action<GameObject[]> callback)
    {
        int leftIndex = 0, rightIndex = 0;
        GameObject[] merged = new GameObject[left.Length + right.Length];

        for (int i = 0; i < merged.Length; i++)
        {
            yield return WaitForStep();

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

            // Update visual position
            merged[i].transform.position = new Vector3(i, 0, 0);
            merged[i].GetComponent<Renderer>().material.color = Color.yellow;
        }

        callback(merged);
    }

    IEnumerator WaitForStep()
    {
        while (!stepReady)
            yield return null;

        stepReady = false;
    }
}
