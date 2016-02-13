using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(RectTransform))]
public class MiniGestureRecognizerTransition : MonoBehaviour
{

    public RectTransform LeftScreen;

    public RectTransform RightScreen;

    public RectTransform UpperScreen;

    public RectTransform LowerScreen;

    private RectTransform CurrentScreen;

    private Vector2 fingerStartPos = Vector2.zero;

    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private bool movedFarEnough = false;

    private bool startedHorizontal = false;
    private bool startedVertical = false;

    private bool transitionInProgess = false;
    private RectTransform transitionTarget;
    private float correctionRate = 0.1f;

    void Awake()
    {
        CurrentScreen = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transitionInProgess)
        {
            transitionToScreen(transitionTarget, startedVertical);
        } else if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isSwipe = true;
                        fingerStartPos = touch.position;
                        break;
                    case TouchPhase.Moved:
                        float gestureDist = (touch.position - fingerStartPos).magnitude;

                        if (isSwipe && (movedFarEnough || gestureDist > minSwipeDist))
                        {
                            movedFarEnough = true;
                            Vector2 direction = touch.position - fingerStartPos;
                            Vector2 swipeDirection = Vector2.zero;

                            if (!startedVertical && !startedHorizontal)
                            {
                                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                                {
                                    // the swipe is horizontal:
                                    startedHorizontal = true;
                                    if (LeftScreen != null)
                                    {
                                        LeftScreen.gameObject.SetActive(true);
                                        setScreenToPosition(LeftScreen, Vector2.left);
                                    }

                                    if (RightScreen != null)
                                    {
                                        RightScreen.gameObject.SetActive(true);
                                        setScreenToPosition(RightScreen, Vector2.right);
                                    }
                                }
                                else
                                {
                                    // the swipe is vertical:
                                    startedVertical = true;
                                    if (UpperScreen != null)
                                    {
                                        UpperScreen.gameObject.SetActive(true);
                                        setScreenToPosition(UpperScreen, Vector2.up);
                                    }

                                    if (LowerScreen != null)
                                    {
                                        LowerScreen.gameObject.SetActive(true);
                                        setScreenToPosition(LowerScreen, Vector2.down);
                                    }
                                }
                            }

                            if (startedHorizontal)
                            {
                                swipeDirection = Vector2.right * (direction.x / Screen.width);
                                if (LeftScreen != null)
                                {
                                    setScreenToPosition(LeftScreen, Vector2.left + swipeDirection);
                                }
                                setScreenToPosition(CurrentScreen, swipeDirection);
                                if (RightScreen != null)
                                {
                                    setScreenToPosition(RightScreen, Vector2.right + swipeDirection);
                                }
                            }
                            else
                            {
                                swipeDirection = Vector2.up * (direction.y / Screen.height);
                                if (UpperScreen != null)
                                {
                                    setScreenToPosition(UpperScreen, Vector2.up + swipeDirection);
                                }
                                setScreenToPosition(CurrentScreen, swipeDirection);
                                if (LowerScreen != null)
                                {
                                    setScreenToPosition(LowerScreen, Vector2.down + swipeDirection);
                                }
                            }
                        }
                        break;
                    case TouchPhase.Canceled:
                        isSwipe = false;
                        movedFarEnough = false;
                        transitionTarget = CurrentScreen;
                        transitionToScreen(CurrentScreen, startedVertical);
                        startedVertical = false;
                        startedHorizontal = false;
                        break;
                    case TouchPhase.Ended:
                        movedFarEnough = false;
                        isSwipe = false;
                        RectTransform closestToCenter = calculateSmallest(startedVertical);
                        transitionTarget = closestToCenter;
                        transitionToScreen(closestToCenter, startedVertical);
                        startedVertical = false;
                        startedHorizontal = false;
                        break;
                }
            }
        }
    }

    private RectTransform calculateSmallest(bool isVertical)
    {
        if (isVertical)
        {
            float yMin = Mathf.Abs(CurrentScreen.anchorMin.y);
            RectTransform minScreen = CurrentScreen;
            if (UpperScreen != null)
            {
                float upperY = Mathf.Abs(UpperScreen.anchorMin.y);
                if (upperY < yMin)
                {
                    yMin = upperY;
                    minScreen = UpperScreen;
                }
            }
            if (LowerScreen != null)
            {
                float lowerY = Mathf.Abs(LowerScreen.anchorMin.y);
                if (lowerY < yMin)
                {
                    yMin = lowerY;
                    minScreen = LowerScreen;
                }
            }
            return minScreen;
        } else
        {
            float xMin = Mathf.Abs(CurrentScreen.anchorMin.x);
            RectTransform minScreen = CurrentScreen;
            if (LeftScreen != null)
            {
                float leftX = Mathf.Abs(LeftScreen.anchorMin.x);
                if (leftX < xMin)
                {
                    xMin = leftX;
                    minScreen = LeftScreen;
                }
            }
            if (RightScreen != null)
            {
                float rightX = Mathf.Abs(RightScreen.anchorMin.x);
                if (rightX < xMin)
                {
                    xMin = rightX;
                    minScreen = RightScreen;
                }
            }
            return minScreen;
        }
    }

    private void transitionToScreen(RectTransform target, bool isVertical)
    {
        transitionInProgess = true;
        if (isVertical)
        {
            if (Mathf.Abs(target.anchorMin.y) < 0.1f)
            {
                transitionInProgess = false;
                if (UpperScreen != null)
                {
                    setScreenToPosition(UpperScreen, Vector2.zero);
                    UpperScreen.gameObject.SetActive(UpperScreen.Equals(target));
                }
                setScreenToPosition(CurrentScreen, Vector2.zero);
                CurrentScreen.gameObject.SetActive(CurrentScreen.Equals(target));
                if (LowerScreen != null)
                {
                    setScreenToPosition(LowerScreen, Vector2.zero);
                    LowerScreen.gameObject.SetActive(LowerScreen.Equals(target));
                }
            } else
            {
                Vector2 dir;
                if (target.anchorMin.y < 0)
                {
                    dir = Vector2.up * 0.05f;
                } else
                {
                    dir = Vector2.down * 0.05f;
                }
                moveByAmount(UpperScreen, dir);
                moveByAmount(CurrentScreen, dir);
                moveByAmount(LowerScreen, dir);
            }
        } else
        {
            if (Mathf.Abs(target.anchorMin.x) < 0.1f)
            {
                transitionInProgess = false;
                if (LeftScreen != null)
                {
                    setScreenToPosition(LeftScreen, Vector2.zero);
                    LeftScreen.gameObject.SetActive(LeftScreen.Equals(target));
                }
                setScreenToPosition(CurrentScreen, Vector2.zero);
                CurrentScreen.gameObject.SetActive(CurrentScreen.Equals(target));
                if (RightScreen != null)
                {
                    setScreenToPosition(RightScreen, Vector2.zero);
                    RightScreen.gameObject.SetActive(RightScreen.Equals(target));
                }
            }
            else
            {
                Vector2 dir;
                if (target.anchorMin.x < 0)
                {
                    dir = Vector2.right * 0.05f;
                }
                else
                {
                    dir = Vector2.left * 0.05f;
                }
                moveByAmount(LeftScreen, dir);
                moveByAmount(CurrentScreen, dir);
                moveByAmount(RightScreen, dir);
            }
        }
    }

    private void moveByAmount(RectTransform screen, Vector2 amount)
    {
        if (screen != null)
        {
            setScreenToPosition(screen, screen.anchorMin + amount);
        }
    }

    private void setScreenToPosition(RectTransform screen, Vector2 bottomLeftPoint)
    {
        screen.anchorMin = bottomLeftPoint;
        screen.anchorMax = bottomLeftPoint + new Vector2(1, 1);
    }
}
