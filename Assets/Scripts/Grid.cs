using System.Collections.Generic;
using UnityEngine;
using System;

namespace RGSMS
{
    public enum EPathFindResult
    {
        Continue = 0,
        InvalidPath,
        Found
    }

    public class Grid : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _position = Vector3.zero;

        [SerializeField, Range(2, 10)]
        private int _sizeX = 2;
        [SerializeField, Range(2, 10)]
        private int _sizeY = 2;

        [SerializeField]
        private GameObject _piecePrefab = null;

        [SerializeField]
        private LevelManager _levelManager = null;

        private LinePath _linePath = null;

        private Tile[,] _tiles = null;
        //private int GridXSize => _tiles.GetLength(0);
        //private int GridYSize => _tiles.GetLength(1);

        private Piece[] _pieces = null;

        public List<Tile> _tilesToCalculate = null;
        public HashSet<Tile> _calculatedTiles = null;

        private Comparison<Tile> _sortTilesByDistance = null;

        private void Start()
        {
            _linePath = new LinePath();

            _tilesToCalculate = new List<Tile>();
            _calculatedTiles = new HashSet<Tile>();

            _sortTilesByDistance = SortByDistance;

            CreateGrid();
        }

        private void CreateGrid ()
        {
            _tiles = new Tile[_sizeX+2, _sizeY+2];

            Piece piece;
            GameObject pieceGo;

            for (int x = 0; x < _sizeX+2; x++)
            {
                for (int y = 0; y < _sizeY+2; y++)
                {
                    piece = null;

                    if( (x > 0 && y > 0) &&
                        (x <= _sizeX && y <= _sizeY))
                    {
                        pieceGo = Instantiate(_piecePrefab, _position, Quaternion.identity);

                        piece = pieceGo.GetComponent<Piece>();
                        piece.SetLevelManager(_levelManager);
                    }

                    _tiles[x, y] = new Tile(new Vector2Int(x, y), piece);

                    if (piece != null)
                    {
                        piece.SetTile(_tiles[x, y]);
                    }

                    _position.y -= 1.0f;
                }

                _position.y += _sizeY+2;
                _position.x += 1.0f;
            }
        }

        public LinePath CalculateLinePath (Piece[] pieces)
        {
            _pieces = pieces;
            _linePath.Clear();
            _tilesToCalculate.Add(pieces[0].Tile);
            
            if(GetSideTiles(_tilesToCalculate[0], ref _linePath) == EPathFindResult.InvalidPath)
            {
                return null;
            }

            return _linePath;
        }

        private EPathFindResult GetSideTiles (Tile tile, ref LinePath linePath)
        {
            Vector2Int coordenates = tile.Coordenates;

            List<Tile> sideTiles = new List<Tile>();

            //Get Up Tile
            Tile nextTile = _tiles[coordenates.x, coordenates.y + 1];
            if(CheckIfIsAValidTile(nextTile))
            {
                nextTile.SetFather(tile);
                _tilesToCalculate.Add(nextTile);
            }

            //Get Down Tile
            nextTile = _tiles[coordenates.x, coordenates.y - 1];
            if (CheckIfIsAValidTile(nextTile))
            {
                nextTile.SetFather(tile);
                _tilesToCalculate.Add(nextTile);
            }

            //Get Right Tile
            nextTile = _tiles[coordenates.x + 1, coordenates.y];
            if (CheckIfIsAValidTile(nextTile))
            {
                nextTile.SetFather(tile);
                _tilesToCalculate.Add(nextTile);
            }

            //Get Left Tile
            nextTile = _tiles[coordenates.x - 1, coordenates.y];
            if (CheckIfIsAValidTile(nextTile))
            {
                nextTile.SetFather(tile);
                _tilesToCalculate.Add(nextTile);
            }

            _calculatedTiles.Add(tile);
            _tilesToCalculate.Remove(tile);

            //Sort Tile List by Tiles Distance
            _tilesToCalculate.Sort(_sortTilesByDistance);

            return GetSideTiles(_tilesToCalculate[0], ref _linePath);
        }

        private bool CheckIfIsAValidTile (Tile tile)
        {
            if (tile.IsEmpty)
            {
                if (!_tilesToCalculate.Contains(tile) &&
                    !_calculatedTiles.Contains(tile))
                {
                    return true;
                }
            }

            return false;
        }

        private EPathFindResult SetTileToTilesList (Tile tile)
        {
            if (tile.IsEmpty)
            {
                if (!_tilesToCalculate.Contains(tile) &&
                    !_calculatedTiles.Contains(tile))
                {
                    _tilesToCalculate.Add(tile);
                }
            }
            else
            {
                if(tile.Piece == _pieces[1])
                {
                    Debug.Log("Achou o caminho");
                    //parar aqui
                    return EPathFindResult.Found;
                }
            }

            return EPathFindResult.Continue;
        }

        private int SortByDistance(Tile tileX, Tile tileY)
        {
            if (tileX.F < tileY.F)
            {
                return -1;
            }
            else if (tileX.F > tileY.F)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
