using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSortAlgorithm : SortAlgorithm
{
    // Start is called before the first frame update
    void Start()
    {
        SpawnStarting();
        StartCoroutine(StartSort(arrayToSort.ToArray()));
    }

    // Update is called once per frame
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
        yield return StartCoroutine(SelectionSort(unsorted));

        for (int i = 0; i < unsorted.Length; i++)
        {
            SetColor(unsorted[i], Color.green);
        }
    }

    /**
     * Selection Sort Algorithm
     * For each position i in the array, find the minimum element in the unsorted part of the array
     * Swap the minimum element with the element at position i at the end of the unsorted part
     */
    private IEnumerator SelectionSort(GameObject[] unsorted)
    {
        for (int i = 0; i <= unsorted.Length - 1; i++)
        {
            // Set the color of the current position to red (next position to be sorted for)
            SetColor(unsorted[i], Color.red);
            int minPos = i;

            yield return WaitForStep(); // Pause until space is pressed

            for (int j = i + 1; j < unsorted.Length; j++)
            {
                // Set the color of the current position to blue (next position to be compared with min)
                SetColor(unsorted[j], Color.blue);
                yield return WaitForStep(); // Pause until space is pressed
                if (SwapCondition(unsorted[minPos], unsorted[j]))
                {
                    // If minPos is not the same as i, set the color of the previous minPos to white
                    if (minPos != i)
                    {
                        SetColor(unsorted[minPos], Color.white);
                    }

                    minPos = j;
                    SetColor(unsorted[j], Color.green); // Set the color of the new minPos to green
                    yield return WaitForStep(); // Pause until space is pressed

                }
                else
                {
                    // If the current position is not the minimum, set the color of the current position to white
                    SetColor(unsorted[j], Color.white);
                }

            }

            // Reset colors for next iteration
            SetColor(unsorted[i], Color.white);
            SetColor(unsorted[minPos], Color.white);
            SwapBars(unsorted, i, minPos);
        }

    }

}
