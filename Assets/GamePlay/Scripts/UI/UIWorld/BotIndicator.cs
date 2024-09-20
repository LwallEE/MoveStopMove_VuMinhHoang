using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotIndicator : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private Vector3 offsetY;

    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private Image indicatorImg;
    
    public Transform target;  // The GameObject the indicator should follow
    private Camera mainCamera; // The main camera
    private Canvas canvas;     // The World Space Canvas
    private RectTransform indicator; // The UI Image for the indicator

    private RectTransform canvasRect;

    private void Start()
    {
        mainCamera = Camera.main;
        canvas = UICanvasWorld.Instance.MainCanvas;
        indicator = GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        ScreenOverlayFollow();
    }

    private void ScreenOverlayFollow()
    {
        if (target == null) return;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position + offsetY);
        bool isOffScreen = screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;
        
        if (isOffScreen)
        {
            if (screenPos.z < 0)
            {
                // If behind the camera, flip the indicator to the opposite side of the screen
                screenPos.x = Screen.width - screenPos.x;
                screenPos.y = Screen.height - screenPos.y;
                screenPos.z = 0;
            }
            nameTxt.gameObject.SetActive(false);
            screenPos.x = Mathf.Clamp(screenPos.x, offset, Screen.width-offset);
            screenPos.y = Mathf.Clamp(screenPos.y, offset, Screen.height-offset);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos,null, out var canvasPos);
          
            indicator.localPosition = canvasPos;
            return;
        }

        nameTxt.gameObject.SetActive(true);
        indicator.position = screenPos;
    }

    public void Init(string name, int currentLevel,Color color,Transform target)
    {
        if(mainCamera != null)
            indicator.position = mainCamera.WorldToScreenPoint(target.position + offsetY);
        this.target = target;
        nameTxt.text = name;
        currentLevelTxt.text = currentLevel.ToString();
        nameTxt.color = color;
        indicatorImg.color = color;
        transform.localScale = Vector3.one;
    }

    public void UpdateLevel(int level)
    {
        currentLevelTxt.text = level.ToString();
    }
    
}
