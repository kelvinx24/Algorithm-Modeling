using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinomeTiling : MonoBehaviour
{
    public BoardSpawner boardSpawner; // Reference to the BoardSpawner script
    private int tileID = 1;
    private Color[] colorPallete = { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoTiling(int startR, int startC, int size, int defectR, int defectC)
    {
        int halfSize = size / 2;
        int centerR = startR + halfSize;
        int centerC = startC + halfSize;

        (int, int) centerTopLeft = (centerR - 1, centerC - 1);
        (int, int) centerTopRight = (centerR - 1, centerC);
        (int, int) centerBottomLeft = (centerR, centerC - 1);
        (int, int) centerBottomRight = (centerR, centerC);

        // Create the trinome for the three quadrants
        int t = tileID++;
        Color currentColor = colorPallete[t % colorPallete.Length];

        if (size == 2)
        {
            if (defectR == 0 && defectC == 0)
            {
                boardSpawner.SetCellColor(centerTopRight.Item1, centerTopRight.Item2, currentColor);
                boardSpawner.SetCellColor(centerBottomLeft.Item1, centerBottomLeft.Item2, currentColor);
                boardSpawner.SetCellColor(centerBottomRight.Item1, centerBottomRight.Item2, currentColor);
            }
            else if (defectR == 0 && defectC == 1)
            {
                boardSpawner.SetCellColor(centerTopLeft.Item1, centerTopLeft.Item2, currentColor);
                boardSpawner.SetCellColor(centerBottomLeft.Item1, centerBottomLeft.Item2, currentColor);
                boardSpawner.SetCellColor(centerBottomRight.Item1, centerBottomRight.Item2, currentColor);
            }
            else if (defectR == 1 && defectC == 0)
            {
                boardSpawner.SetCellColor(centerTopLeft.Item1, centerTopLeft.Item2, currentColor);
                boardSpawner.SetCellColor(centerTopRight.Item1, centerTopRight.Item2, currentColor);
                boardSpawner.SetCellColor(centerBottomRight.Item1, centerBottomRight.Item2, currentColor);
            }
            else
            {
                boardSpawner.SetCellColor(centerTopLeft.Item1, centerTopLeft.Item2, currentColor);
                boardSpawner.SetCellColor(centerTopRight.Item1, centerTopRight.Item2, currentColor);
                boardSpawner.SetCellColor(centerBottomLeft.Item1, centerBottomLeft.Item2, currentColor);
            }
            return;
        }

        Debug.Log($"Tiling: Start({startR}, {startC}), Size: {size}, Defect: ({defectR}, {defectC})");
        Debug.Log($"Center: ({centerR}, {centerC})");

        // Determine the position of the defect
        //int defectQuadrantR = defectR < centerR ? 0 : 1;
        //int defectQuadrantC = defectC < centerC ? 0 : 1;

        if (defectR == 0 && defectC == 0)
        {
            boardSpawner.SetCellColor(centerTopRight.Item1, centerTopRight.Item2, currentColor);
            boardSpawner.SetCellColor(centerBottomLeft.Item1, centerBottomLeft.Item2, currentColor);
            boardSpawner.SetCellColor(centerBottomRight.Item1, centerBottomRight.Item2, currentColor);

            DoTiling(startR, startC, halfSize, defectR, defectC);
            DoTiling(startR, centerC, halfSize, 1, 0);
            DoTiling(centerR, startC, halfSize, 0, 1);
            DoTiling(centerR, centerC, halfSize, 0, 0);
        }
        else if (defectR == 0 && defectC == 1)
        {
            boardSpawner.SetCellColor(centerTopLeft.Item1, centerTopLeft.Item2, currentColor);
            boardSpawner.SetCellColor(centerBottomLeft.Item1, centerBottomLeft.Item2, currentColor);
            boardSpawner.SetCellColor(centerBottomRight.Item1, centerBottomRight.Item2, currentColor);

            DoTiling(startR, startC, halfSize, 1, 1);
            DoTiling(startR, centerC, halfSize, defectR, defectC);
            DoTiling(centerR, startC, halfSize, 0, 1);
            DoTiling(centerR, centerC, halfSize, 0, 0);
        }
        else if (defectR == 1 && defectC == 0   )
        {
            boardSpawner.SetCellColor(centerTopLeft.Item1, centerTopLeft.Item2, currentColor);
            boardSpawner.SetCellColor(centerTopRight.Item1, centerTopRight.Item2, currentColor);
            boardSpawner.SetCellColor(centerBottomRight.Item1, centerBottomRight.Item2, currentColor);

            DoTiling(startR, startC, halfSize, 1, 1);
            DoTiling(startR, centerC, halfSize, 1, 0);
            DoTiling(centerR, startC, halfSize, defectR, defectC);
            DoTiling(centerR, centerC, halfSize, 0, 0);
        }
        else
        {
            boardSpawner.SetCellColor(centerTopLeft.Item1, centerTopLeft.Item2, currentColor);
            boardSpawner.SetCellColor(centerTopRight.Item1, centerTopRight.Item2, currentColor);
            boardSpawner.SetCellColor(centerBottomLeft.Item1, centerBottomLeft.Item2, currentColor);

            DoTiling(startR, startC, halfSize, 1, 1);
            DoTiling(startR, centerC, halfSize, 1, 0);
            DoTiling(centerR, startC, halfSize, 0, 1);
            DoTiling(centerR, centerC, halfSize, defectR, defectC);
        }

        
    }
}
