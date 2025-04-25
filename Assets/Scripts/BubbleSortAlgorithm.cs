using System.Collections;
using UnityEngine;

public class BubbleSortAlgorithm : MonoBehaviour
{
    public GameObject[] bars; // Assign these in the Inspector
    private bool advanceStep = false;

    void Start()
    {
        StartCoroutine(BubbleSortControlled());
    }

    void Update()
    {
        // Wait for user to press space to advance to next step
        if (Input.GetKeyDown(KeyCode.Space))
        {
            advanceStep = true;
        }
    }

    IEnumerator BubbleSortControlled()
    {
        int n = bars.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                SetColor(bars[j], Color.red);
                SetColor(bars[j + 1], Color.red);

                yield return WaitForUserInput(); // Pause until space is pressed

                float heightA = bars[j].transform.localScale.y;
                float heightB = bars[j + 1].transform.localScale.y;

                if (heightA > heightB)
                {
                    SwapBars(j, j + 1);
                }

                SetColor(bars[j], Color.white);
                SetColor(bars[j + 1], Color.white);
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

    void SwapBars(int indexA, int indexB)
    {
        GameObject temp = bars[indexA];
        bars[indexA] = bars[indexB];
        bars[indexB] = temp;

        Vector3 posA = bars[indexA].transform.position;
        Vector3 posB = bars[indexB].transform.position;

        bars[indexA].transform.position = new Vector3(posB.x, posA.y, posA.z);
        bars[indexB].transform.position = new Vector3(posA.x, posB.y, posB.z);
    }

    void SetColor(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }
}
