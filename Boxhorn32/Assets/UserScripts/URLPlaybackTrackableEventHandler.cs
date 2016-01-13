﻿/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using System;
using System.Collections;
using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class URLPlaybackTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES

        private TrackableBehaviour mTrackableBehaviour;

        private static bool DownloadWarningShown = false;

        #endregion // PRIVATE_MEMBER_VARIABLES

        #region PUBLIC_MEMBER_VARIABLES

        public string URL;

        public bool PlayVideo;

        public Color BgColor;

        public FullScreenMovieControlMode ControlMode;

        public FullScreenMovieScalingMode ScalingMode;

        #endregion PUBLIC_MEMBER_VARIABLES // PUBLIC_MEMBER_VARIABLES

        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS

        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                StartCoroutine(OnTrackingFound());
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS

        #region PRIVATE_METHODS


        private IEnumerator OnTrackingFound()
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (!DownloadWarningShown && Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    ShowDownloadWarning();
                    DownloadWarningShown = true;
                    yield return new WaitForSeconds(2);
                }

                if (PlayVideo)
                {
                    Screen.orientation = ScreenOrientation.LandscapeRight;
                    yield return new WaitForSeconds(0.5f);

                    Handheld.PlayFullScreenMovie(URL, BgColor, ControlMode, ScalingMode);

                    yield return new WaitForSeconds(3);
                    Screen.orientation = ScreenOrientation.Portrait;
                }
                else
                {
                    Application.OpenURL(URL);
                }
            }
            else
            {
                ShowNoConnectionError();
                yield return new WaitForSeconds(3);
            }
        }

        private void OnTrackingLost()
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        private void ShowDownloadWarning()
        {
            // TODO insert code to display alert note
            throw new NotImplementedException();
        }

        private void ShowNoConnectionError()
        {
            // TODO insert code to notify user that no internet connection exists
            throw new NotImplementedException();
        }

        #endregion // PRIVATE_METHODS
    }
}
