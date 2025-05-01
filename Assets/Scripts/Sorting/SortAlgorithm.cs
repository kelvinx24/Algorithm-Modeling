using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SortAlgorithm : MonoBehaviour
{
    public int sortItemsAmount = 1;

    public GameObject sortPrefab;

    protected List<GameObject> arrayToSort = new List<GameObject>();

    protected int spacing = 2;

    protected bool advanceStep = false;

    protected System.Random rand = new System.Random();

    // Spawns random objects of varying heights to be sorted
    protected void SpawnStarting()
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

    // Sorts the arrayToSort
    protected abstract IEnumerator StartSort(GameObject[] unsorted);

    // Sets the color of the object
    protected void SetColor(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }

    // Sets the color of all objects in the arrayToSort to white
    // and the selected objects to the specified color
    protected void ResetAndColorSelected(GameObject[] selectedObj, Color colorWith)
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

    // Sets the color of selected objects to the specified color
    protected void ColorSelected(GameObject[] selectedObjs, Color colorWith)
    {
        foreach (GameObject go in selectedObjs)
        {
            if (go != null)
            {
                go.GetComponent<Renderer>().material.color = colorWith;

            }
        }
    }

    // Waits for user step
    protected IEnumerator WaitForStep()
    {
        advanceStep = false;
        while (!advanceStep)
        {
            yield return null; // Wait until next frame
        }

    }

    protected void SwapBars(GameObject[] unsorted, int indexA, int indexB)
    {
        GameObject temp = unsorted[indexA];
        unsorted[indexA] = unsorted[indexB];
        unsorted[indexB] = temp;

        Vector3 posA = unsorted[indexA].transform.position;
        Vector3 posB = unsorted[indexB].transform.position;

        unsorted[indexA].transform.position = new Vector3(posB.x, posA.y, posA.z);
        unsorted[indexB].transform.position = new Vector3(posA.x, posB.y, posB.z);
    }

    protected bool SwapCondition(GameObject a, GameObject b)
    {
        return a.transform.localScale.y > b.transform.localScale.y;
    }
}
