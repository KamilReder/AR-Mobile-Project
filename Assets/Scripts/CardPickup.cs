using UnityEngine;
using UnityEngine.InputSystem;

public class CardPickup : MonoBehaviour
{
    [SerializeField] private float popDuration = 0.15f;

    private static Camera arCamera;
    private bool collected = false;

    private void Awake()
    {
        if (arCamera == null)
            arCamera = Camera.main;
    }

    private void Update()
    {
        if (collected || Touchscreen.current == null)
            return;

        if (!Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            return;

        Vector2 screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.gameObject == gameObject)
                Collect();
        }
    }

    private void Collect()
    {
        collected = true;
        if (GameManager.Instance != null)
            GameManager.Instance.ReportCardCollected();

        StartCoroutine(PopAndDestroy());
    }

    private System.Collections.IEnumerator PopAndDestroy()
    {
        Vector3 startScale = transform.localScale;
        Vector3 bigScale = startScale * 1.3f;
        float t = 0f;

        while (t < popDuration)
        {
            t += Time.deltaTime;
            float normalized = t / popDuration;
            transform.localScale = Vector3.Lerp(startScale, bigScale, normalized);
            yield return null;
        }

        Destroy(gameObject);
    }
}