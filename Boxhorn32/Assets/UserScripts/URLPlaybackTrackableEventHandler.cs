/*==============================================================================
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

        private static float LastOpenTime;

        private const float OpenDelay = 2f;

        private TrackableBehaviour mTrackableBehaviour;

        private Color BgColor = new Color(155, 185, 255, 0);

        private FullScreenMovieControlMode ControlMode = FullScreenMovieControlMode.Full;

        private FullScreenMovieScalingMode ScalingMode = FullScreenMovieScalingMode.AspectFit;

        private static Stack QueuedLinks = new Stack();

        #endregion // PRIVATE_MEMBER_VARIABLES

        #region PUBLIC_MEMBER_VARIABLES

        public string URL;

        public bool PlayVideo;

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

        void Update()
        {
            if (Time.time > LastOpenTime + OpenDelay && QueuedLinks.Count > 0)
            {
                Application.OpenURL((string)QueuedLinks.Pop());
                LastOpenTime = Time.time;
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
            GameObject scanScreen = GameObject.FindGameObjectWithTag("ScanUI");
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                if (PlayVideo)
                {
                    OpenVideo(URL);
                }
                else if (!QueuedLinks.Contains(URL) && scanScreen.activeSelf)
                {
                    QueuedLinks.Push(URL);
                }
            }
        }

        public void OpenURL(string URL)
        {
            Application.OpenURL(URL);
        }

        public void OpenVideo(string URL)
        {
            StartCoroutine(DisplayVideo(URL));
        }

        public void OpenMail(string Receiver)
        {
            string MailString = "mailto:" + Receiver + "?subject=" + MyEscapeURL("") + "&body=" + MyEscapeURL("");
            OpenURL(MailString);
        }

        #endregion // PUBLIC_METHODS

        #region PRIVATE_METHODS

        private IEnumerator DisplayVideo(string URL)
        {
            Screen.orientation = ScreenOrientation.LandscapeRight;
            yield return new WaitForSeconds(0.5f);

            Handheld.PlayFullScreenMovie(URL, BgColor, ControlMode, ScalingMode);

            yield return new WaitForSeconds(3);
            Screen.orientation = ScreenOrientation.Portrait;
        }

        private string MyEscapeURL(string URL)
        {
            return WWW.EscapeURL(URL).Replace("+", "%20");
        }

        #endregion // PRIVATE_METHODS
    }
}
