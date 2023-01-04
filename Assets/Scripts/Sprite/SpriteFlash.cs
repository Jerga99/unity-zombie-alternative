﻿using System.Collections;
using UnityEngine;

namespace Eincode.ZombieSurvival.Sprite
{
    public class SpriteFlash : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [Tooltip("Material to switch to during the flash.")]
        public Material FlashMaterial;

        [Tooltip("Duration of the flash.")]
        public float Duration;

        #endregion
        #region Private Fields

        // The SpriteRenderer that should flash.
        private SpriteRenderer spriteRenderer;

        // The material that was in use, when the script started.
        private Material originalMaterial;

        // The currently running coroutine.
        private Coroutine flashRoutine;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalMaterial = spriteRenderer.material;
            FlashMaterial = new Material(FlashMaterial);
        }

        #endregion

        public void Flash(Color color)
        {
            // If the flashRoutine is not null, then it is currently running.
            if (flashRoutine != null)
            {
                // In this case, we should stop it first.
                // Multiple FlashRoutines the same time would cause bugs.
                StopCoroutine(flashRoutine);
            }

            // Start the Coroutine, and store the reference for it.
            flashRoutine = StartCoroutine(FlashRoutine(color));
        }

        private IEnumerator FlashRoutine(Color color)
        {
            // Swap to the flashMaterial.
            spriteRenderer.material = FlashMaterial;

            // Set the desired color for the flash.
            FlashMaterial.color = color;

            // Pause the execution of this function for "duration" seconds.
            yield return new WaitForSeconds(Duration);

            // After the pause, swap back to the original material.
            spriteRenderer.material = originalMaterial;

            // Set the flashRoutine to null, signaling that it's finished.
            flashRoutine = null;
        }

        #endregion
    }
}