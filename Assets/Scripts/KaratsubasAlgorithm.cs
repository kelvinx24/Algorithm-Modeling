using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KaratsubasAlgorithm : MonoBehaviour
{
    public int firstOperand = 1234;
    public int secondOperand = 5678;

    public GameObject nodePrefab;
    public GameObject edgePrefab;

    private bool advanceStep = false;

    private TreeNode<int> rootNode;

    void Start()
    {
        StartCoroutine(StartAlgorithm());

    }

    void Update()
    {
        // Wait for user input (e.g., space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            advanceStep = true;
        }
    }

    private IEnumerator StartAlgorithm()
    {
        int result = 0;
        int n = Mathf.Max(firstOperand.ToString().Length, secondOperand.ToString().Length);

        yield return StartCoroutine(Karatsuba(firstOperand, secondOperand, n, null, val => result = val));

        Debug.Log($"Final Result: {firstOperand} * {secondOperand} = {result}");
    }

    private IEnumerator Karatsuba(int first, int second, int n, TreeNode<int> currentNode, System.Action<int> callback)
    {
        // Create a new node for the current operation
        TreeNode<int> node = new TreeNode<int>();
        GameObject nodeObject = Instantiate(nodePrefab);
        node.representation = nodeObject;
        if (currentNode != null && rootNode != null)
        {
            currentNode.addChild(node, edgePrefab);
        }
        else
        {
            rootNode = node;
        }

        TMP_Text textMesh = node.representation.GetComponentInChildren<TMP_Text>();
        textMesh.text = $"{first} * {second}";

        SetColor(nodeObject, Color.yellow); 

        if (n <= 1)
        {
            yield return WaitForStep(); // Wait for user input
            // Base case: single-digit multiplication
            int baseResult = first * second;
            Debug.Log($"Base case: {first} * {second} = {baseResult}");
            textMesh.text = baseResult.ToString();

            SetColor(nodeObject, Color.green); // Set color to green for completion
            yield return WaitForStep(); // Wait for user input
            callback(baseResult);

            yield break; // Exit the coroutine
        }

        int m = n / 2;
        int a = first / (int)Mathf.Pow(10, m);
        int b = first % (int)Mathf.Pow(10, m);
        int c = second / (int)Mathf.Pow(10, m);
        int d = second % (int)Mathf.Pow(10, m);
        int e = 0, f = 0, g = 0;

        Debug
            .Log($"Splitting: {first} = {a} * 10^{m} + {b}, {second} = {c} * 10^{m} + {d}");

        // Wait for user input before proceeding
        yield return WaitForStep();

        // Recursively calculate three products
        SetColor(nodeObject, Color.white);
        yield return StartCoroutine(Karatsuba(a, c, m, node, val => e = val));
        yield return StartCoroutine(Karatsuba(b, d, m, node, val => f = val));
        yield return StartCoroutine(Karatsuba(b-a,c-d, m, node, val => g = val));
        SetColor(nodeObject, Color.yellow); // Reset color to yellow for the next step

        Debug.Log($"Products: e = {e}, f = {f}, g = {g}");
        yield return WaitForStep(); // Wait for user input

        // Combine the results using the Karatsuba formula
        int result = (e * (int)Mathf.Pow(10, 2 * m)) + ((e + f + g) * (int)Mathf.Pow(10, m)) + f;
        Debug.Log($"Karatsuba: {first} * {second} = {result}");
        textMesh.text = result.ToString();
        yield return WaitForStep(); // Wait for user input

        // Set the color of the node to green to indicate completion
        SetColor(nodeObject, Color.green);
        callback(result);
    }

    protected IEnumerator WaitForStep()
    {
        advanceStep = false;
        while (!advanceStep)
        {
            yield return null; // Wait until next frame
        }

    }

    protected void SetColor(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }
}
