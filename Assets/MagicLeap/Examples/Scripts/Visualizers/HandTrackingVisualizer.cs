// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2018-present, Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap
{
    /// <summary>
    /// Component used to hook into the Hand Tracking script and attach
    /// primitive game objects to it's detected keypoint positions for
    /// each hand.
    /// </summary>
    public class HandTrackingVisualizer : MonoBehaviour
    {
        #region Private Variables
        [SerializeField, Tooltip("The hand to visualize.")]
        private MLHandType _handType = MLHandType.Left;

        [SerializeField]
        private Transform _center = null;

        [SerializeField]
        private Transform _thumb = null;

        [SerializeField]
        private Transform _index = null;

        [System.NonSerialized]
        public bool isTracking = false;
        #endregion

        #region Private Properties
        /// <summary>
        /// Returns the hand based on the hand type.
        /// </summary>
        public MLHand Hand
        {
            get
            {
                if (_handType == MLHandType.Left)
                {
                    return MLHands.Left;
                }
                else
                {
                    return MLHands.Right;
                }
            }
        }
        #endregion

        #region Unity Methods
        /// <summary>
        /// Initializes MLHands API.
        /// </summary>
        void Start()
        {
            MLResult result = MLHands.Start();
            if (!result.IsOk)
            {
                Debug.LogErrorFormat("Error: HandTrackingVisualizer failed starting MLHands, disabling script. Reason: {0}", result);
                enabled = false;
                return;
            }
        }

        /// <summary>
        /// Stops the communication to the MLHands API.
        /// </summary>
        void OnDestroy()
        {
            if (MLHands.IsStarted)
            {
                MLHands.Stop();
            }
        }

        /// <summary>
        /// Update the keypoint positions.
        /// </summary>
        void Update()
        {
            if (MLHands.IsStarted)
            {
                isTracking = true;
                if (_index != null)
                {
                    _index.localPosition = Hand.Index.KeyPoints[Hand.Index.KeyPoints.Count - 1].Position;
                    _index.gameObject.SetActive(Hand.IsVisible);
                }

                if (_thumb != null)
                {
                    _thumb.localPosition = Hand.Thumb.KeyPoints[Hand.Thumb.KeyPoints.Count - 1].Position;
                    _thumb.gameObject.SetActive(Hand.IsVisible);
                }

                if (_center != null)
                {
                    _center.localPosition = Hand.Center;
                    _center.gameObject.SetActive(Hand.IsVisible);
                }
            }
            else
                isTracking = false;
        }
        #endregion
    }
}
