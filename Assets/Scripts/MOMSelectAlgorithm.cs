using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MOMSelectAlgorithm : MonoBehaviour
{
    public int numElements = 25;
    public int smallestK = 5; // The kth smallest element to find

    public GameObject modelPrefab;

    private int[] arrayToSort;
    private bool advanceStep = false;

    private Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
        GenerateNumbers();
        GenerateColors();
        StartCoroutine(StartAlgorithm());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartAlgorithm()
    {
        Debug.Log("Unsorted Array: " + string.Join(", ", arrayToSort));
        int kthSmallest = MOMSelect(arrayToSort, smallestK);
        Debug.Log($"The {smallestK}th smallest element is: {kthSmallest}");
        yield return null;
    }

    private void GenerateNumbers()
    {
        arrayToSort = new int[numElements];
        for (int i = 0; i < numElements; i++)
        {
            arrayToSort[i] = Random.Range(1, 100);
            
            /*
            GameObject model = Instantiate(modelPrefab);
            model.transform.position = new Vector3(i * 2, 0, 0);
            float modifiedHeight = arrayToSort[i] / 10f;
            model.transform.localScale = new Vector3(1, modifiedHeight, 1);
            */
        }
    }

    private void GenerateColors()
    {
        int numColors = numElements / 5;
        colors = new Color[numColors];

        float colorStep = 1f / numColors;

        for (int i = 0; i < numColors; i++)
        {
            Color addColor = new Color(0, 0, 1 - (i * colorStep));
            colors[i] = addColor;
        }
    }

    private int MOMPivotSelect(int[] selectPivotFrom)
    {
        if (selectPivotFrom.Length <= 5)
        {
            System.Array.Sort(selectPivotFrom);
            return FindMedian(selectPivotFrom);
        }

        int numSplits = selectPivotFrom.Length / 5;
        int[] meds = new int[numSplits];
        int[][] fifths = new int[numSplits][];

        for (int i = 0; i < numSplits; i++)
        {
            fifths[i] = new int[5];
            for (int j = 0; i * 5 + j < selectPivotFrom.Length && j < 5; j++)
            {
                fifths[i][j] = selectPivotFrom[i * 5 + j];
                //SetColor(modelPrefab, colors[i]); // Set color for each number
            }

            // Sort the group of 5 numbers
            System.Array.Sort(fifths[i]);
            // Find the median of the group
            int median = FindMedian(fifths[i]);
            meds[i] = median;
        }

        int pivot = MOMSelect(meds, meds.Length / 2 + 1);
        return pivot;
    }

    private int FindMedian(int[] nums)
    {
        int n = nums.Length;
        if (n % 2 == 0)
        {
            return (nums[n / 2 - 1] + nums[n / 2]) / 2;
        }
        else
        {
            return nums[n / 2];
        }
    }

    /**
     *  MOMSelect is a recursive function that selects the kth smallest element from the array nums.
     *  It uses the Median of Medians algorithm to select a pivot that is guaranteed to be close to the median.
     *  The function partitions the array into three lists: less than, equal to, and greater than the pivot.
     *  The kth smallest element is then determined based on the size of these partitions.
     *  
     *  Logic:
     *  The pivot gurantees that at least 30% of the elements are eliminated from consideration.
     *  The partitioning essentially determines what smallest element is the current pivot.
     *  k then checks if the partitioning is correct for the kth smallest element.
     *  If recurse left, then that means k is smaller than the pivot. 
     *  If recurse right, then that means k is larger than the pivot.
     *  If equal, then that means the pivot is the kth smallest element.
     */
    private int MOMSelect(int[] nums, int k)
    {
        // Sort the array if it has 5 or fewer elements and return the kth smallest element
        if (nums.Length <= 5)
        {
            System.Array.Sort(nums);
            return nums[k - 1];
        }

        // Select a pivot using the Median of Medians algorithm 
        // The pivot will eliminate at least 30% of the elements
        int p = MOMPivotSelect(nums);
         
        List<int> left = new List<int>();
        List<int> right = new List<int>();
        List<int> equal = new List<int>();

        // Partition the array into three lists: less than, equal to, and greater than the pivot
        // This will determine which side of the pivot the kth smallest element is on
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] < p)
            {
                left.Add(nums[i]);
            }
            else if (nums[i] > p)
            {
                right.Add(nums[i]);
            }
            else
            {
                equal.Add(nums[i]);
            }
        }

        // If k is less than or equal to the size of the left partition,
        // Then that means kth smallest is in the left partition since k 
        if (k <= left.Count)
        {
            return MOMSelect(left.ToArray(), k);
        }
        // The kth smallest element is in the equal partition 
        else if (k <= left.Count + equal.Count)
        {
            return equal[0];
        }
        else
        {
            // The kth smallest element is in the right partition
            return MOMSelect(right.ToArray(), k - left.Count - equal.Count);
     
        }
    }

    protected void SetColor(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }


}
