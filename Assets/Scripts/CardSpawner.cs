using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class CardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private int cardsToSpawn = 5;

    private ARPlaneManager planeManager;
    private bool hasSpawned = false; // only spawn once, on the first plane found

    private void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        planeManager.trackablesChanged.AddListener(OnPlanesChanged);
    }

    private void OnDisable()
    {
        planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
    }

    private void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        if (hasSpawned || args.added.Count == 0)
            return;

        SpawnCardsOnPlane(args.added[0]);
        hasSpawned = true;
    }

    private void SpawnCardsOnPlane(ARPlane plane)
    {
        for (int i = 0; i < cardsToSpawn; i++)
        {
            Vector2 boundaryPoint = plane.boundary[Random.Range(0, plane.boundary.Length)];
            Vector2 localPoint = Vector2.Lerp(Vector2.zero, boundaryPoint, Random.Range(0.2f, 0.8f));

            Vector3 worldPoint = plane.transform.TransformPoint(new Vector3(localPoint.x, 0f, localPoint.y));

            Instantiate(cardPrefab, worldPoint, Quaternion.identity);
        }

        // Let the game manager know how many cards are in play so it
        // can track the win condition.
        if (GameManager.Instance != null)
            GameManager.Instance.SetTotalCards(cardsToSpawn);
    }
}