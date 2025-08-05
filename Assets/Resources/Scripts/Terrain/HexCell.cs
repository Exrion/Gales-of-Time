using UnityEngine;

public static class HexCellData
{
    public const float HEX_OUTER_RADIUS = 10f;
    public const float HEX_INNER_RADIUS = HEX_OUTER_RADIUS * 0.866025404f;
    public static Vector2[] DeriveCorners(Vector2 centre)
    {
        return new Vector2[]
        {
            centre,
            new Vector2(centre.x + HEX_OUTER_RADIUS / 2f, centre.y + HEX_INNER_RADIUS),
            new Vector2(centre.x + HEX_OUTER_RADIUS, centre.y),
            new Vector2(centre.x + HEX_OUTER_RADIUS / 2f, centre.y - HEX_INNER_RADIUS),
            new Vector2(centre.x - HEX_OUTER_RADIUS / 2f, centre.y - HEX_INNER_RADIUS),
            new Vector2(centre.x - HEX_OUTER_RADIUS, centre.y),
            new Vector2(centre.x - HEX_OUTER_RADIUS / 2f, centre.y + HEX_INNER_RADIUS)
        };
    }
}

public class HexCell
{

}
