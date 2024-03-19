using System.Collections.Generic;
using Game.Modules.Board.Balls;
using Game.Modules.Board.Cells;
using Game.Modules.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Screenshoter
{
    public class Screenshoter : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Material _lineRendererMaterial;
        
        [SerializeField] private GameObject[] _lstLineRenderer01;
        [SerializeField] private GameObject[] _lstLineRenderer02;
        [SerializeField] private GameObject[] _lstLineRenderer03;
        [SerializeField] private GameObject[] _lstLineRenderer04;
        
        [Button]
        public void Reset()
        {
            int[] lstNum = {1,1,1,1,1, 1,1,1,1,1, 1,1,1,1,1, 1,1,1,1,1, 1,1,1,1,1, 1,1,1,1,1, 1,1,1,1,1};
            SetBalls(lstNum);

            _lineRenderer.positionCount = 0;
            _lineRendererMaterial.color = ColorVar.Ball1Color;
        }
        
        [Button]
        public void SetScreen01()
        {
            int[] lstNum = {1,1,2,3,2, 2,4,3,5,3, 2,3,2,3,1, 2,4,3,1,2, 2,2,3,4,2, 3,3,2,2,1, 3,3,2,4,2};
            SetBalls(lstNum);
            SetLineRenderer(_lstLineRenderer01, ColorVar.Ball3Color);
        }
        
        [Button]
        public void SetScreen02()
        {
            int[] lstNum = {4,4,4,5,6, 8,5,5,3,8, 8,4,5,5,8, 8,6,8,6,8, 8,8,6,8,8, 8,5,2,4,8, 6,5,5,5,6};
            SetBalls(lstNum);
            SetLineRenderer(_lstLineRenderer02, ColorVar.Ball8Color);
        }
        
        [Button]
        public void SetScreen03()
        {
            int[] lstNum = {13,14,15,15,15, 14,15,16,13,13, 14,16,14,16,13, 16,15,12,14,16, 16,13,16,17,16, 17,16,13,16,18, 13,15,15,15,17};
            SetBalls(lstNum);
            SetLineRenderer(_lstLineRenderer03, ColorVar.Ball6Color);
        }
        
        [Button]
        public void SetScreen04()
        {
            int[] lstNum = {92,87,87,82,82, 86,86,86,86,86, 86,83,83,87,86, 92,82,86,83,83, 83,83,83,82,85, 86,86,82,86,86, 86,86,82,86,86};
            SetBalls(lstNum);
            SetLineRenderer(_lstLineRenderer04, ColorVar.Ball6Color);
        }
        
        private static void SetBalls(IReadOnlyList<int> lstNum)
        {
            var board = new GameObject[7, 5];
            var cells = FindObjectsOfType<Cell>();
            
            foreach (var cell in cells)
            {
                if (cell.transform.CompareTag("CellCache")) 
                    continue;

                var position = cell.ExtractCellPosition();
                board[position.x, position.y] = cell.gameObject;
            }
            
            var width = board.GetLength(0);
            var height = board.GetLength(1);
            
            List<Ball> balls = new();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var ball = board[x, y].transform.GetComponentInChildren<Ball>();
                    balls.Add(ball);
                }
            }

            for (var i = 0; i < balls.Count; i++)
            {
                var ball = balls[i];
                ball.SetNum(lstNum[i]);
            }
        }
        
        private void SetLineRenderer(IReadOnlyList<GameObject> lstLineRenderer, Color color)
        {
            _lineRenderer.positionCount = lstLineRenderer.Count;
            _lineRendererMaterial.color = color;
            
            for (var i = 0; i < lstLineRenderer.Count; i++)
            {
                _lineRenderer.SetPosition(i, lstLineRenderer[i].transform.position);
            }
        }
    }
}
