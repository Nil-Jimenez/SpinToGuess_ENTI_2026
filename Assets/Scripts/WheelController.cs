using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WheelController : MonoBehaviour
{
    [Header("Referencias 3D")]
    [SerializeField] private Transform wheelTransform; 
    [SerializeField] private GameObject pinPrefab;     

    [Header("Dimensiones de la Ruleta")]
    [SerializeField] private float tileRadius = 0.5f; 

    [Header("Configuración de Clavijas")]
    [SerializeField] private float pinRadius = 1f;          
    [SerializeField] private float pinHeight = 0.1f;        
    [SerializeField] private Vector3 pinScale = Vector3.one; 
    [SerializeField] private Vector3 pinPositionOffset = Vector3.zero; 
    [SerializeField] private Vector3 pinRotationOffset = Vector3.zero; 

    [Header("Configuración de Giro")]
    [SerializeField] private AnimationCurve spinCurve;
    [SerializeField] private float spinDuration = 4f;
    [SerializeField] private int minFullRotations = 5;

    [Header("Casillas Modulares")]
    public List<WheelTile> tiles = new List<WheelTile>();

    [Header("Eventos")]
    public UnityEvent<WheelTile> OnSpinFinished;
    public UnityEvent OnPinPassed;

    private bool isSpinning = false;
    private float currentAngle = 0f;
    private int lastPinIndex = -1;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        BuildWheel();
    }

    public void BuildWheel()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null) Destroy(obj);
        }
        spawnedObjects.Clear();

        if (tiles.Count == 0) return;

        float anglePerTile = 360f / tiles.Count;

        for (int i = 0; i < tiles.Count; i++)
        {
            float tileAngleDeg = i * anglePerTile;
            float tileAngleRad = tileAngleDeg * Mathf.Deg2Rad;

            // instanciar el gajo
            if (tiles[i].tileVisualPrefab != null)
            {
                Vector3 tilePos = new Vector3(
                    Mathf.Sin(tileAngleRad) * tileRadius,
                    0.05f, 
                    Mathf.Cos(tileAngleRad) * tileRadius
                );

                GameObject tileObj = Instantiate(tiles[i].tileVisualPrefab, wheelTransform);
                tileObj.transform.localPosition = tilePos;
                tileObj.transform.localRotation = Quaternion.Euler(0f, tileAngleDeg, 0f);

                spawnedObjects.Add(tileObj);
            }

            //instanciar la clavija que marca los bordes de los gajos
            if (pinPrefab != null)
            {
                float pinAngleDeg = tileAngleDeg + (anglePerTile / 2f);
                float pinAngleRad = pinAngleDeg * Mathf.Deg2Rad;

                Vector3 pinPos = new Vector3(
                    Mathf.Sin(pinAngleRad) * pinRadius,
                    pinHeight,
                    Mathf.Cos(pinAngleRad) * pinRadius
                ) + pinPositionOffset;

                GameObject pinObj = Instantiate(pinPrefab, wheelTransform);
                pinObj.transform.localPosition = pinPos;
                pinObj.transform.localRotation = Quaternion.Euler(pinRotationOffset);
                pinObj.transform.localScale = pinScale;

                spawnedObjects.Add(pinObj);
            }
        }
    }


    public void SpinWheel()
    {
        if (isSpinning || tiles.Count == 0) return;
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;

        float elapsed = 0f;
        float startAngle = currentAngle;
        float randomOffset = Random.Range(0f, 360f);
        float targetAngle = startAngle + (360f * minFullRotations) + randomOffset;

        float anglePerTile = 360f / tiles.Count;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / spinDuration);
            float curveValue = spinCurve.Evaluate(t);

            currentAngle = Mathf.Lerp(startAngle, targetAngle, curveValue);
            wheelTransform.localEulerAngles = new Vector3(0f, currentAngle, 0f);

          
            float adjustedAngle = (currentAngle + (anglePerTile / 2f)) % 360f;
            int currentPinIndex = Mathf.FloorToInt(adjustedAngle / anglePerTile);

            if (currentPinIndex != lastPinIndex)
            {
                lastPinIndex = currentPinIndex;
                OnPinPassed?.Invoke();
            }

            yield return null;
        }

        currentAngle = targetAngle % 360f;
        wheelTransform.localEulerAngles = new Vector3(0f, currentAngle, 0f);
        isSpinning = false;

        WheelTile winningTile = GetWinningTile();
        Debug.Log($"¡Resultado: {winningTile.tileName} (+{winningTile.points} pts)!");
        OnSpinFinished?.Invoke(winningTile);
    }

    private WheelTile GetWinningTile()
    {
        float anglePerTile = 360f / tiles.Count;

        float normalizedAngle = (360f - (currentAngle % 360f) + (anglePerTile / 2f)) % 360f;
        int winningIndex = Mathf.FloorToInt(normalizedAngle / anglePerTile);

        return tiles[Mathf.Clamp(winningIndex, 0, tiles.Count - 1)];
    }
}
