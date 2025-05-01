using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSortAlgoirthm : SortAlgorithm
{
    void Start()
    {
        SpawnStarting();

        StartCoroutine(StartSort(arrayToSort.ToArray()));
    }

    void Update()
    {
        // Wait for user input (e.g., space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            advanceStep = true;
        }
    }

    protected override IEnumerator StartSort(GameObject[] unsorted)
    {
        yield return StartCoroutine(InsertionSortCoroutine(unsorted));
        for (int i = 0; i < unsorted.Length; i++)
        {
            SetColor(unsorted[i], Color.green);
        }
    }

    /**
     * Insertion Sort Algorithm
     * For each element i in the array, find the correct position for the element in the sorted part of the array
     * Shift all elements greater than the current element to the right
     */
    public IEnumerator InsertionSortCoroutine(GameObject[] unsorted)
    {
        for (int i = 1; i < unsorted.Length; i++)
        {
            ColorSelected(unsorted[0..i], Color.white); // Set the color of the sorted part to white
            SetColor(unsorted[i], Color.red); // Set the color of the current position to red (next position to be sorted for)
            for (int j = i - 1; j >= 0; j--)
            {
                // Set the color of the current position to blue (next position to be compared with min)
                SetColor(unsorted[j], Color.blue);
                yield return WaitForStep(); // Pause until space is pressed

                if (SwapCondition(unsorted[j], unsorted[j + 1]))
                {
                    // Set the color of the swapped to white, so it doesn't get confused with the next position to be compared with
                    SetColor(unsorted[j], Color.white); 
                    SwapBars(unsorted, j, j + 1);
                    yield return WaitForStep(); // Pause until space is pressed
                }
                else 
                {
                    break; // If the current element is in the correct position, break out of the inner loop
                }
            }

            SetColor(unsorted[i], Color.white); // Reset the color of the current position to white
            int sortedLength = i + 1;
            ColorSelected(unsorted[0..sortedLength], Color.green); // Set the color of the sorted part to green
            yield return WaitForStep(); // Pause until space is pressed
        }


    }



}