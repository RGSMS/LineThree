using UnityEngine;

namespace RGSMS
{
    public class Tile
    {
        public Tile Father { private set; get; } = null;

        public Piece Piece { private set; get; } = null;
        public Vector2Int Coordenates { private set; get; } = Vector2Int.zero;

        public int F => G + H;
        public int G { private set; get; } = 0;
        public int H { private set; get; } = 0;

        public bool IsEmpty => (Piece == null);

        public Tile(Vector2Int coordenates, Piece piece)
        {
            Piece = piece;
            Coordenates = coordenates;
        }

        public void SetDistances(Tile firstTile, Tile lastTile)
        {
            G = Mathf.RoundToInt(Vector2Int.Distance(Coordenates, firstTile.Coordenates));
            H = Mathf.RoundToInt(Vector2Int.Distance(Coordenates, lastTile.Coordenates));
        }

        public void SetFather (Tile father)
        {
            Father = father;
        }

        public void ClearPathCalc()
        {
            Father = null;
            G = 0;
            H = 0;
        }

        public void ClearPiece()
        {
            Piece = null;
        }
    }
}
