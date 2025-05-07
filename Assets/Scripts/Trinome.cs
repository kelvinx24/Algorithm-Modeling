using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trinome
{
    public Material trinomeMaterial; // Optional: assign for visual effect
    public enum TrinomeOrientation { TopLeft, TopRight, BottomLeft, BottomRight }

    private TrinomeOrientation orientation;

    public Trinome(TrinomeOrientation orientation)
    {
        this.orientation = orientation;
    }

    // Determiens the orientation of the trinome based on the positions
    public Trinome((int,int) pos1, (int,int) pos2, (int,int) pos3)
    {
        if (pos1.Item1 == pos2.Item1 && pos1.Item2 == pos3.Item2)
        {
            orientation = TrinomeOrientation.TopLeft;
        }
        else if (pos1.Item1 == pos2.Item1 && pos1.Item2 == pos3.Item2)
        {
            orientation = TrinomeOrientation.TopRight;
        }
        else if (pos1.Item1 == pos2.Item1 && pos1.Item2 == pos3.Item2)
        {
            orientation = TrinomeOrientation.BottomLeft;
        }
        else
        {
            orientation = TrinomeOrientation.BottomRight;
        }
    }

    public List<(int, int)> GetTrinomeOffsets(int r, int c)
    {
        var positions = new List<(int, int)>();

        switch (orientation)
        {
            case TrinomeOrientation.TopLeft:
                positions.Add((r, c));
                positions.Add((r - 1, c));
                positions.Add((r, c - 1));
                break;
            case TrinomeOrientation.TopRight:
                positions.Add((r, c));
                positions.Add((r - 1, c));
                positions.Add((r, c + 1));
                break;
            case TrinomeOrientation.BottomLeft:
                positions.Add((r, c));
                positions.Add((r + 1, c));
                positions.Add((r, c - 1));
                break;
            case TrinomeOrientation.BottomRight:
                positions.Add((r, c));
                positions.Add((r + 1, c));
                positions.Add((r, c + 1));
                break;
        }

        return positions;
    }
}
