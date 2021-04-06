using UnityEngine;

namespace RGSMS
{
    public enum EPieceType
    {
        None = 0,
        Type_01,
        Type_02,
        Type_03,
        Type_04,
    }

    public class Piece : MonoBehaviour
    {
        private bool _selected = false;

        private LevelManager _levelManager = null;

        public Tile Tile { private set; get; } = null;

        [SerializeField]
        private Transform _transform = null;
        public Transform Transform => _transform;

        [SerializeField]
        private Point[] _points = null;

        private void Start()
        {
            for(int i = 0; i <= (int)EDirection.Right; i++)
            {
                _points[i].direction = (EDirection)i;

                if(_points[i].direction == EDirection.Up)
                {
                    _points[i].point = new Vector2(_transform.position.x,
                                                   _transform.position.y + (_transform.localScale.y/2.0f));
                }
                else if(_points[i].direction == EDirection.Down)
                {
                    _points[i].point = new Vector2(_transform.position.x,
                                                   _transform.position.y - (_transform.localScale.y / 2.0f));
                }
                else if (_points[i].direction == EDirection.Right)
                {
                    _points[i].point = new Vector2(_transform.position.x + (_transform.localScale.x / 2.0f),
                                                   _transform.position.y);
                }
                else if (_points[i].direction == EDirection.Left)
                {
                    _points[i].point = new Vector2(_transform.position.x - (_transform.localScale.x / 2.0f),
                                                   _transform.position.y);
                }
            }
        }

        private void OnMouseDown()
        {
            if(_selected || !_levelManager.CanSelect)
            {
                return;
            }

            _selected = true;
            _levelManager.AddSelectedPiece(this);
        }

        public void SetLevelManager (LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void SetTile (Tile tile)
        {
            Tile = tile;
        }

        public void ResetSelection ()
        {
            _selected = false;
        }

        public Point GetPointByDirection (EDirection direction)
        {
            for(int i = 0; i < _points.Length; i++)
            {
                if(_points[i].direction == direction)
                {
                    return _points[i];
                }
            }

            return null;
        }

        public Point GetPointByInvertedDirection(EDirection direction)
        {
            for (int i = 0; i < _points.Length; i++)
            {
                if ((_points[i].direction == EDirection.Down && direction == EDirection.Up) ||
                    (_points[i].direction == EDirection.Up && direction == EDirection.Down) ||
                    (_points[i].direction == EDirection.Right && direction == EDirection.Left) ||
                    (_points[i].direction == EDirection.Left && direction == EDirection.Right))
                {
                    return _points[i];
                }
            }

            return null;
        }
    }
}
