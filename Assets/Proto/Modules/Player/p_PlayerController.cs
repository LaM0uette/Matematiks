using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Obvious.Soap;
using Proto.Modules.NumBall;
using Proto.Modules.Player.Inputs;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Proto.Modules.Player
{
    public class p_PlayerController : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Properties")]
        [SerializeField] private float _maxDistanceBetweenNumBalls = 1.3f;
        [SerializeField] private GameObject _ballPrefab;
        
        [Space, Title("Soap")]
        [SerializeField] private BoolVariable _isDownVariable;
        [SerializeField] private ScriptableEventP_NumBall _numBallSelectedEvent;
        [SerializeField] private ScriptableListP_NumBall _numBallsSelected;

        private p_PlayerInputsReader _inputsReader;
        private LineRenderer _lineRenderer;
        private readonly List<float> _positionsToSpawnBalls = new();

        private void Awake()
        {
            _inputsReader = GetComponent<p_PlayerInputsReader>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        #endregion

        #region Events

        private void OnEnable()
        {
            _inputsReader.PressAction += OnPress;
            _inputsReader.ReleaseAction += OnRelease;
            _numBallSelectedEvent.OnRaised += OnNumBallSelected;
        }

        private void OnDisable()
        {
            _inputsReader.PressAction -= OnPress;
            _inputsReader.ReleaseAction -= OnRelease;
            _numBallSelectedEvent.OnRaised -= OnNumBallSelected;
        }

        #endregion

        #region Functions

        private void OnPress()
        {
            _isDownVariable.Value = true;
        }
        
        private void OnRelease()
        {
            if (_isDownVariable.Value == false) 
                return;
            
            StartCoroutine(AnimateAndMergeBalls());
            _isDownVariable.Value = false;
        }
        
        private IEnumerator AnimateAndMergeBalls() 
        {
            if (_numBallsSelected.Count < 3)
            {
                _numBallsSelected.Clear();
                _lineRenderer.positionCount = 0;
                yield break;
            }

            DisablePhysicsForAllBalls();
            
            for (var i = 0; i < _numBallsSelected.Count - 1; i++) 
            {
                var currentBall = _numBallsSelected[i];
                var nextBall = _numBallsSelected[i + 1];
                
                _positionsToSpawnBalls.Add(currentBall.gameObject.transform.position.x);
                
                // Animation
                const float duration = 0.1f;
                var elapsed = 0f;
                var startPosition = currentBall.transform.position;
                var endPosition = nextBall.transform.position;
                
                RemoveFirstPosition(_lineRenderer);
                
                while (elapsed < duration) 
                {
                    currentBall.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                
                Destroy(currentBall.gameObject);
            }

            // Mettre à jour la dernière ball
            var lastNumBall = _numBallsSelected[^1];
            lastNumBall.SetNum(lastNumBall.Num + 1);

            // Réinitialiser les variables après la fusion
            EnablePhysicsForAllBalls();
            _numBallsSelected.Clear();
            _positionsToSpawnBalls.Clear();
        }
        
        private void DisablePhysicsForAllBalls() 
        {
            foreach (var ball in FindObjectsOfType<p_NumBall>()) 
            {
                ball.IsBlocked = true;
            }
        }
        
        private void EnablePhysicsForAllBalls() 
        {
            foreach (var ball in FindObjectsOfType<p_NumBall>()) 
            {
                ball.IsBlocked = false;
            }
        }
        
        private void RemoveFirstPosition(LineRenderer lineRenderer) {
            int positionsCount = lineRenderer.positionCount;
            if (positionsCount <= 1) {
                // Si le LineRenderer n'a qu'une ou aucune position, réinitialisez-le simplement.
                lineRenderer.positionCount = 0;
                return;
            }

            // Créer un tableau temporaire pour stocker les nouvelles positions
            Vector3[] newPositions = new Vector3[positionsCount - 1];

            // Copier toutes les positions sauf la première dans le nouveau tableau
            for (int i = 1; i < positionsCount; i++) {
                newPositions[i - 1] = lineRenderer.GetPosition(i);
            }

            // Appliquer le nouveau tableau de positions au LineRenderer et ajuster le nombre de positions
            lineRenderer.positionCount = positionsCount - 1;
            lineRenderer.SetPositions(newPositions);
        }
        
        private void OnNumBallSelected(p_NumBall pNumBall)
        {
            if (_isDownVariable.Value == false) 
                return;
            
            if (_numBallsSelected.Count == 0)
            {
                _numBallsSelected.Add(pNumBall);
                _lineRenderer.positionCount = 1;
                _lineRenderer.SetPosition(0, pNumBall.transform.position);
                return;
            }
            
            if (_numBallsSelected.Contains(pNumBall) && _numBallsSelected.Count > 1)
            {
                if (_numBallsSelected[^2] == pNumBall)
                {
                    _numBallsSelected.Remove(_numBallsSelected[^1]);
                    _lineRenderer.positionCount = _numBallsSelected.Count;
                }
                
                return;
            }

            var firstNumBall = _numBallsSelected[0];
            if (firstNumBall.Num != pNumBall.Num) 
                return;
            
            _numBallsSelected.Add(pNumBall);
            
            if (_numBallsSelected.Count >= 2)
            {
                var distance = Vector3.Distance(_numBallsSelected[^2].transform.position, _numBallsSelected[^1].transform.position);

                if (distance > _maxDistanceBetweenNumBalls)
                {
                    _numBallsSelected.Remove(_numBallsSelected[^1]);
                    _lineRenderer.positionCount = _numBallsSelected.Count;
                    return;
                }
            }
            
            _lineRenderer.positionCount = _numBallsSelected.Count;
            
            var newYPosition = pNumBall.transform.position;
            
            foreach (var selectedBall in _numBallsSelected)
            {
                if (!(Mathf.Abs(selectedBall.transform.position.y - pNumBall.transform.position.y) <= 0.2f)) 
                    continue;
                
                newYPosition.y = selectedBall.transform.position.y;
                break;
            }
            
            _lineRenderer.SetPosition(_numBallsSelected.Count - 1, newYPosition);
        }

        #endregion
    }
}
