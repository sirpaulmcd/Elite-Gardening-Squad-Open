﻿// using System; // For events
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

namespace EGS
{
    /// <summary>
    /// The prompt UI for an OT event. Upon enable, prints prompt text to the
    /// players and waits for a PromptOption to be selected.
    /// </summary>
    public class Prompt : MonoBehaviour
    {
        //=====================================================================
        #region Instance variables
        //=====================================================================
        /// <summary>
        /// The scriptable object associated with the prompt.
        /// </summary>
        [SerializeField] private PromptSO _scriptableObject = default;
        /// <summary>
        /// The TextMeshPro component that displays the text.
        /// </summary>
        [SerializeField] private TextMeshProUGUI _textDisplayer = default;
        /// <summary>
        /// The options available for the prompt.
        /// </summary>
        [SerializeField] private PromptOption[] _options = default;
        #endregion

        //=====================================================================
        #region MonoBehavior
        //=====================================================================
        /// <summary>
        /// Called when the object is Enabled.
        /// </summary>
        private void OnEnable()
        {
            InitOnEnable();
        }
        #endregion

        //=====================================================================
        #region Public methods
        //=====================================================================
        /// <summary>
        /// Deactivates the prompt UI and activates the response UI.
        /// </summary>
        public void ActivateResponse(GameObject responseUI)
        {
            this.gameObject.SetActive(false);
            responseUI.SetActive(true);
        }
        #endregion

        //=====================================================================
        #region Private methods
        //=====================================================================
        /// <summary>
        /// Initialises the component in Start().
        /// </summary>
        private void InitOnEnable()
        {
            InitVars();
            StartCoroutine(WriteText(_scriptableObject.text));
        }

        /// <summary>
        /// Sources and initializes component variables.
        /// </summary>
        private void InitVars()
        {
            transform.parent.name = "PromptCanvas";
            // Link option info from PromptSO to PromptOption button functionality
            if (_options.Length == _scriptableObject.optionInfo.Length)
            {
                for (int i = 0; i < _options.Length; i++)
                {
                    _options[i].Init(_scriptableObject.optionInfo[i]);
                }
            }
            else { Debug.LogWarning("Prompt not wired correctly.", this.gameObject); }
        }

        /// <summary>
        /// Prints input text to the text displayer.
        /// </summary>
        /// <param name="text">
        /// The text to be printed on the text displayer.
        /// </param>
        private IEnumerator WriteText(string text)
        {
            _textDisplayer.text = "";
            for (int i = 0; i < text.Length; i++)
            {
                _textDisplayer.text += text[i];
                yield return new WaitForSeconds(_scriptableObject.typeDelaySeconds);
            }
        }
        #endregion
    }
}
