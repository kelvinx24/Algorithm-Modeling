using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreeNode<T>
{
    public TreeNode<T> parent;

    public List<TreeNode<T>> children = new List<TreeNode<T>>();

    public GameObject representation;

    private List<GameObject> edges = new List<GameObject>();


    public void addChild(TreeNode<T> child)
    {
        children.Add(child);
    }

    public void addChild(TreeNode<T> child, GameObject edgePrefab)
    {
        children.Add(child);

        // Calculate position of child
        child.representation.transform.parent = representation.transform;

        // Add and configure edge
        if (edgePrefab != null)
        {
            GameObject edge = GameObject.Instantiate(edgePrefab);
            edge.transform.parent = representation.transform;
            edges.Add(edge);
        }



        PositionChildren();
    }

    private void PositionChildren()
    {
        float verticalOffset = -1.5f; // Distance below the parent
        float horizontalSpacing = 2.0f; // Horizontal space between children

        int count = children.Count;
        float totalWidth = (count - 1) * horizontalSpacing;

        for (int i = 0; i < count; i++)
        {
            TreeNode<T> child = children[i];
            if (child != null && child.representation != null)
            {
                // Center the children around x = 0
                // Calculate new leftest position then add by current object index * spacing
                float x = -totalWidth / 2f + i * horizontalSpacing;
                float y = verticalOffset;

                Vector3 childPos = new Vector3(x, y, 0f);
                child.representation.transform.localPosition = childPos;

                if (i < edges.Count && edges[i] != null)
                {
                    GameObject edge = edges[i];

                    Vector3 parentGlobalPos = edge.transform.position;
                    Vector3 childGlobalPos = child.representation.transform.position;

                    Vector3 edgeDirection = childGlobalPos - parentGlobalPos;
                    float edgeLength = edgeDirection.magnitude;
                    Vector3 edgeCenter = parentGlobalPos + edgeDirection / 2;

                    edge.transform.localPosition = edgeCenter;
                    edge.transform.up = edgeDirection.normalized;
                    edge.transform.localScale = new Vector3(1, edgeLength / 2, 1);
                }
            }
        }
    }

}
