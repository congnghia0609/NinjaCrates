using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]
public class ClickAndSwipe : MonoBehaviour
{
    private GameManager gameManager;
    private Camera cam;
    private Vector3 mousePos;
    private TrailRenderer trail;
    private BoxCollider col;
    private bool swiping = false;

    void Awake()
    {
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();
        col = GetComponent<BoxCollider>();
        trail.enabled = false;
        col.enabled = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            TouchSwipe();
            MouseSwipe();
        }
    }

    void TouchSwipe()
    {
        // Kiểm tra nếu có ít nhất một ngón tay chạm vào màn hình.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Khi người dùng chạm (tap) bắt đầu.
            if (touch.phase == TouchPhase.Began)
            {
                swiping = true;
                UpdateComponents();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swiping = false;
                UpdateComponents();
            }
            if (swiping)
            {
                UpdateTouchPosition();
            }
        }
    }

    void UpdateTouchPosition()
    {
        Touch touch = Input.GetTouch(0);
        // The reason we use 10.0f on the z axis, is because the camera has the z position of -10.0f.
        mousePos = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10.0f));
        transform.position = mousePos;
    }

    void MouseSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swiping = true;
            UpdateComponents();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swiping = false;
            UpdateComponents();
        }
        if (swiping)
        {
            UpdateMousePosition();
        }
    }

    void UpdateMousePosition()
    {
        // The reason we use 10.0f on the z axis, is because the camera has the z position of -10.0f.
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePos;
    }

    void UpdateComponents()
    {
        trail.enabled = swiping;
        col.enabled = swiping;
    }

    void OnCollisionEnter(Collision collision)
    {
        var target = collision.gameObject.GetComponent<Target>();
        if (target)
        {
            // Destroy the target
            target.DestroyTarget();
        }
    }
}
