using UnityEngine;

namespace RGSMS
{
    public class LevelManager : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool DeletePiece = false;
#endif

        private bool _canSelect = true;
        public bool CanSelect => _canSelect;

        [SerializeField]
        private Line _line = null;

        [SerializeField]
        private Grid _grid = null;

        private LinePath _linePath = null;

        private int _currentPieceIndex = 0;
        private Piece[] _pieces = new Piece[] { null, null };

        public void AddSelectedPiece (Piece piece)
        {
#if UNITY_EDITOR
            if (DeletePiece)
            {
                piece.Tile.ClearPiece();
                Destroy(piece.gameObject);
                return;
            }
#endif

            _pieces[_currentPieceIndex] = piece;

            _currentPieceIndex++;

            if(_currentPieceIndex == 2)
            {
                _canSelect = false;

                //calculate line path
                if(FindLinePath())
                {
                    //_line.StartDrawLine(_linePath.startPoint, _linePath.finalPoint, _linePath.middlePosition, _linePath.maxCurves);
                    return;
                }

                RestoreGameplay();
            }
        }

        private bool FindLinePath ()
        {
            _linePath = _grid.CalculateLinePath(_pieces);

            return (_linePath != null);
        }

        private void RestoreGameplay ()
        {
            _canSelect = false;
            _currentPieceIndex = 0;
            for (int i = 0; i < 2; i++)
            {
                _pieces[i] = null;
            }
        }
    }
}
