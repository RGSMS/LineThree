using UnityEngine;

namespace RGSMS
{
    public enum EDirection
    {
        None = -1,
        Up = 0,
        Down,
        Left,
        Right
    }

    [System.Serializable]
    public class Point
    {
        public EDirection direction = EDirection.None;
        public Vector2 point = Vector2.zero;
    }

    public class LinePath
    {
        public Point startPoint = null;
        public Point finalPoint = null;
        public float middlePosition = 0.0f;
        public int maxCurves = 0;

        public void Clear ()
        {
            maxCurves = 0;

            startPoint = null;
            finalPoint = null;

            middlePosition = 0.0f;
        }
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(LineRenderer))]
    public sealed class Line : MonoBehaviour
    {
        private bool _draw = false;

        private int _maxCurves = 0;
        private int _lineCurves = 0;

        private float _lerpTime = 0.0f;

        private Point _startPoint = null;
        private Point _finalPoint = null;

        private Vector2 _nextPoint = Vector2.zero;
        private Vector2 _currentPoint = Vector2.zero;

        private Vector3 _linePosition = Vector3.zero;

        private EDirection _direction = EDirection.None;

        [SerializeField]
        private LineRenderer _lineRenderer = null;

        private void Update()
        {
            if(_draw)
            {
                Draw();
            }
        }

        public void StartDrawLine (Point startPoint, Point finalPoint, float middlePosition, int maxCurves)
        {
            ClearDraw();

            _lineCurves = 0;
            _lerpTime = 0.0f;
            _maxCurves = maxCurves;
            _startPoint = startPoint;
            _finalPoint = finalPoint;
            _currentPoint = startPoint.point;
            _direction = _startPoint.direction;

            SetNewDirection(middlePosition);

            _lineRenderer.SetPosition(0, _currentPoint);

            _draw = true;
        }

        public void ClearDraw ()
        {
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                _lineRenderer.SetPosition(i, Vector3.zero);
            }
        }

        private void CalculateNewPoint()
        {
            _lineCurves++;

            if (_lineCurves > _maxCurves)
            {
                _draw = false;
                return;
            }

            _lerpTime = 0.0f;
            _currentPoint = _nextPoint;

            if (_maxCurves == 1)
            {
                _nextPoint = _finalPoint.point;
                _direction = GetInverseDirection(_finalPoint.direction);
                return;
            }

            if (_lineCurves == 1)
            {
                if (_direction == EDirection.Down ||
                    _direction == EDirection.Up)
                {
                    if(_startPoint.point.x > _finalPoint.point.x)
                    {
                        _direction = EDirection.Left;
                    }
                    else
                    {
                        _direction = EDirection.Right;
                    }
                }
                else if (_direction == EDirection.Left ||
                         _direction == EDirection.Right)
                {
                    if (_startPoint.point.y > _finalPoint.point.y)
                    {
                        _direction = EDirection.Down;
                    }
                    else
                    {
                        _direction = EDirection.Up;
                    }
                }

                switch (_direction)
                {
                    case EDirection.Down:
                    case EDirection.Up:
                        _nextPoint.x = _currentPoint.x;
                        _nextPoint.y = _finalPoint.point.y;
                        break;

                    case EDirection.Right:
                    case EDirection.Left:
                        _nextPoint.x = _finalPoint.point.x;
                        _nextPoint.y = _currentPoint.y;
                        break;
                }

                return;
            }

            _nextPoint = _finalPoint.point;
            _direction = _finalPoint.direction;
        }

        private void SetNewDirection (float middlePosition)
        {
            if(_maxCurves == 0)
            {
                _nextPoint = _finalPoint.point;
                return;
            }
            else if(_maxCurves == 1)
            {
                switch (_direction)
                {
                    case EDirection.Down:
                    case EDirection.Up:
                        _nextPoint.x = _startPoint.point.x;
                        _nextPoint.y = _finalPoint.point.y;
                        break;

                    case EDirection.Right:
                    case EDirection.Left:
                        _nextPoint.x = _finalPoint.point.x;
                        _nextPoint.y = _startPoint.point.y;
                        break;
                }
                return;
            }

            _nextPoint = _startPoint.point;

            switch(_direction)
            {
                case EDirection.Up:
                    _nextPoint.y += middlePosition;
                    break;

                case EDirection.Down:
                    _nextPoint.y -= middlePosition;
                    break;

                case EDirection.Right:
                    _nextPoint.x += middlePosition;
                    break;

                case EDirection.Left:
                    _nextPoint.x -= middlePosition;
                    break;
            }
        }

        private void Draw ()
        {
            _lerpTime += Time.deltaTime;

            switch (_direction)
            {
                case EDirection.Down:
                case EDirection.Up:
                    _linePosition.x = _currentPoint.x;
                    _linePosition.y = Mathf.Lerp(_currentPoint.y, _nextPoint.y, _lerpTime);
                    break;

                case EDirection.Right:
                case EDirection.Left:
                    _linePosition.y = _currentPoint.y;
                    _linePosition.x = Mathf.Lerp(_currentPoint.x, _nextPoint.x, _lerpTime);
                    break;
            }

            for(int i = _lineCurves + 1; i < _lineRenderer.positionCount; i++)
            {
                _lineRenderer.SetPosition(i, _linePosition);
            }

            if (_lerpTime >= 1.0f)
            {
                CalculateNewPoint();
            }
        }

        private EDirection GetInverseDirection (EDirection direction)
        {
            switch(direction)
            {
                case EDirection.Up:
                    return EDirection.Down;

                case EDirection.Down:
                    return EDirection.Up;

                case EDirection.Left:
                    return EDirection.Right;

                case EDirection.Right:
                    return EDirection.Left;

                default:
                    return EDirection.None;
            }
        }
    }
}
