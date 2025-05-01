    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortAlgorithm : SortAlgorithm
{
    void Start()
    {
        SpawnStarting();
        StartCoroutine(StartSort(arrayToSort.ToArray()));
    }

    void Update()
    {
        // Wait for user to press space to advance to next step
        if (Input.GetKeyDown(KeyCode.Space))
        {
            advanceStep = true;
        }
    }

    protected override IEnumerator StartSort(GameObject[] unsorted)
    {
        rand = new System.Random();

        yield return StartCoroutine(BubbleSortControlled(unsorted));

        for (int i = 0; i < unsorted.Length; i++)
        {
            SetColor(unsorted[i], Color.green);
        }
    }

    /**
     * Bubble Sort Algorithm
     * For each position in the array, starting from the last element, find the largest element
     * If the element at position j is greater than the next element, swap them, j starts from 0
     * Repeat this process until no swaps are needed
     */
    IEnumerator BubbleSortControlled(GameObject[] unsorted)
    {
        int n = unsorted.Length;
        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false;
            SetColor(unsorted[n - i - 1], Color.green); // Color the position that the largest element is being found for

            for (int j = 0; j < n - i - 1; j++)
            {
                // Set the color of the current position to red (next position to be sorted for)
                // Set the color of the next position to be compared with to blue
                SetColor(unsorted[j], Color.red);
                SetColor(unsorted[j + 1], Color.blue);
                yield return WaitForStep(); // Pause until space is pressed

                // Compare the heights of the two bars
                if (SwapCondition(unsorted[j], unsorted[j+1]))
                {
                    SwapBars(unsorted, j, j + 1);
                    yield return WaitForStep(); // Pause until space is pressed
                    swapped = true;
                }

                // Reset the colors of the compared positions
                SetColor(unsorted[j], Color.white);
                SetColor(unsorted[j + 1], Color.white);
            }

            // if no swaps were made, the array is sorted
            if (!swapped)
            {
                ResetAndColorSelected(unsorted, Color.black);
                yield return WaitForStep(); // Pause until space is pressed
                break;
            }
        }
    }
}
