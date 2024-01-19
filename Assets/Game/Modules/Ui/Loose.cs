using System;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Modules.Ui
{
    public class Loose : MonoBehaviour
    {
        #region Statements

        [SerializeField] private GameObject _replayButton;
        [SerializeField] private ScriptableEventNoParam _looseEvent;

        private void Start()
        {
            _looseEvent.OnRaised += LooseEventOnRaised;
        }

        #endregion

        #region Functions

        private void LooseEventOnRaised()
        {
            _replayButton.SetActive(true);
        }

        #endregion
    }
}
