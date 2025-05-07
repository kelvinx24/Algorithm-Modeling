using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinomeTiling : MonoBehaviour
{
    public BoardSpawner boardSpawner; // Reference to the BoardSpawner script
    private int tileID = 1;
    private Color[] colorPallete = { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta };

    private Dictionary<(int, int), int> tileIDMap = new Dictionary<(int, int), int>();
    private Dictionary<int, Color> colorMap = new Dictionary<int, Color>();

    
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
        Color currentColor = Color.white;
        (int, int)[] values;

        if (size == 2)
        {
            if (defectR == 0 && defectC == 0)
            {

                values = new (int, int)[] { centerTopRight, centerBottomLeft, centerBottomRight };
                tileIDMap[centerTopRight] = t;

                tileIDMap[centerBottomLeft] = t;

                tileIDMap[centerBottomRight] = t;

            }
            else if (defectR == 0 && defectC == 1)
            {
                values = new (int, int)[] { centerTopLeft, centerBottomLeft, centerBottomRight };

                tileIDMap[centerTopLeft] = t;

                tileIDMap[centerBottomRight] = t;

                tileIDMap[centerBottomLeft] = t;
            }
            else if (defectR == 1 && defectC == 0)
            {
                values = new (int, int)[] { centerTopLeft, centerTopRight, centerBottomRight };
                tileIDMap[centerTopLeft] = t;
                tileIDMap[centerTopRight] = t;
                tileIDMap[centerBottomRight] = t;
            }
            else
            {
                values = new (int, int)[] { centerTopLeft, centerTopRight, centerBottomLeft };
                tileIDMap[centerTopLeft] = t;
                tileIDMap[centerTopRight] = t;
                tileIDMap[centerBottomLeft] = t;
            }

            // Set the color for the trinome
            currentColor = DetermineColor(values, t);
            for (int i = 0; i < values.Length; i++)
            {
                boardSpawner.SetCellColor(values[i].Item1, values[i].Item2, currentColor);
            }

            return;
        }

        // Determine the position of the defect
        //int defectQuadrantR = defectR < centerR ? 0 : 1;
        //int defectQuadrantC = defectC < centerC ? 0 : 1;

        if (defectR == 0 && defectC == 0)
        {
            values = new (int, int)[] { centerTopRight, centerBottomLeft, centerBottomRight };
            tileIDMap[centerTopRight] = t;

            tileIDMap[centerBottomLeft] = t;

            tileIDMap[centerBottomRight] = t;

            DoTiling(startR, startC, halfSize, defectR, defectC);
            DoTiling(startR, centerC, halfSize, 1, 0);
            DoTiling(centerR, startC, halfSize, 0, 1);
            DoTiling(centerR, centerC, halfSize, 0, 0);
        }
        else if (defectR == 0 && defectC == 1)
        {
            values = new (int, int)[] { centerTopLeft, centerBottomLeft, centerBottomRight };

            tileIDMap[centerTopLeft] = t;

            tileIDMap[centerBottomRight] = t;

            tileIDMap[centerBottomLeft] = t;

            DoTiling(startR, startC, halfSize, 1, 1);
            DoTiling(startR, centerC, halfSize, defectR, defectC);
            DoTiling(centerR, startC, halfSize, 0, 1);
            DoTiling(centerR, centerC, halfSize, 0, 0);
        }
        else if (defectR == 1 && defectC == 0)
        {
            values = new (int, int)[] { centerTopLeft, centerTopRight, centerBottomRight };
            tileIDMap[centerTopLeft] = t;
            tileIDMap[centerTopRight] = t;
            tileIDMap[centerBottomRight] = t;

            DoTiling(startR, startC, halfSize, 1, 1);
            DoTiling(startR, centerC, halfSize, 1, 0);
            DoTiling(centerR, startC, halfSize, defectR, defectC);
            DoTiling(centerR, centerC, halfSize, 0, 0);
        }
        else
        {
            values = new (int, int)[] { centerTopLeft, centerTopRight, centerBottomLeft };
            tileIDMap[centerTopLeft] = t;
            tileIDMap[centerTopRight] = t;
            tileIDMap[centerBottomLeft] = t;

            DoTiling(startR, startC, halfSize, 1, 1);
            DoTiling(startR, centerC, halfSize, 1, 0);
            DoTiling(centerR, startC, halfSize, 0, 1);
            DoTiling(centerR, centerC, halfSize, defectR, defectC);
        }

        currentColor = DetermineColor(values, t);
        for (int i = 0; i < values.Length; i++)
        {
            boardSpawner.SetCellColor(values[i].Item1, values[i].Item2, currentColor);
        }
    }

    private Color DetermineColor((int,int)[] positions, int currentID)
    {
        HashSet<Color> neighborColors = new HashSet<Color>();
        (int, int)[] directions = { (0, 1), (1, 0), (0, -1), (-1, 0) }; // Up, Right, Down, Left


        foreach (var position in positions)
        {
           foreach (var direction in directions) {

                (int, int) neighborPos = (position.Item1 + direction.Item1, position.Item2 + direction.Item2);
                if (tileIDMap.TryGetValue(neighborPos, out int neighborID))
                {
                    if (colorMap.ContainsKey(neighborID))
                    {
                        neighborColors.Add(colorMap[neighborID]); // Add the neighbor color to the list

                    }
                }
                
            }
        }



        List<Color> availableColors = new List<Color>();
        foreach (Color color in colorPallete)
        {
            if (!neighborColors.Contains(color))
            {
                availableColors.Add(color);
            }
        }

        if (availableColors.Count > 0)
        {
            int randomIndex = Random.Range(0, availableColors.Count);
            Color selectedColor = availableColors[randomIndex];
            colorMap[currentID] = selectedColor;
            return selectedColor;
        }
        else
        {
            return Color.white; // Default color if no available colors
        }
    }
}
