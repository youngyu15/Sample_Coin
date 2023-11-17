using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private float numberOfCoins;

    #region Debug cone fields
    [SerializeField]
    private float coneRadiusLength;
    [SerializeField]
    private float coneHeight;
    [SerializeField]
    private float yAxisRotation;
    [SerializeField]
    private float zAxisRotation;
    [SerializeField]
    private float yAxisRotationMin;
    [SerializeField]
    private float yAxisRotationMax;
    [SerializeField]
    private float lineLength;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GenerateCoins();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateCoins();
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < 360; i++)
        {
            Vector3 gizmoLine = new Vector3(coneRadiusLength, coneHeight, 0f);
            Vector3 direction = Quaternion.AngleAxis(i, Vector3.up) * gizmoLine.normalized;
            Vector3 newDestinationPoint = transform.position + (direction * gizmoLine.magnitude);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, newDestinationPoint);
        }
        Vector3 testGizmoLine = new Vector3(0f, lineLength, 0f);
        Vector3 testDestinationPoint = transform.position + (Quaternion.AngleAxis(yAxisRotation, Vector3.up) * Quaternion.AngleAxis(zAxisRotation, Vector3.forward) * testGizmoLine);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, testDestinationPoint);
    }

    void GenerateCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            CalculateAirMoveParameters();
            GameObject spawnedCoin = Instantiate(coinPrefab, transform.position, Quaternion.Euler(0f, 0f, 90f));
            //Coin coinScript = spawnedCoin.GetComponent<Coin>();
            //coinScript.yAxisRotation = yAxisRotation;
            //coinScript.zAxisRotation = zAxisRotation;
            //coinScript.lineLength = lineLength;
            Coin2 coinScript = spawnedCoin.GetComponent<Coin2>();
            coinScript.yAxisRotation = yAxisRotation;
            coinScript.zAxisRotation = zAxisRotation;
            coinScript.lineLength = lineLength;
            Debug.Log($"{yAxisRotation}, {zAxisRotation}, {lineLength}");
        }
    }

    void CalculateAirMoveParameters()
    {
        lineLength = Random.Range(1.5f, new Vector3(coneRadiusLength, coneHeight).magnitude);
        float angle = Mathf.Atan2(coneRadiusLength, coneHeight) * Mathf.Rad2Deg;
        zAxisRotation = Random.Range(-angle, angle);
        if (angle >= 0)
        {
            yAxisRotation = Random.Range(-yAxisRotationMax, -yAxisRotationMin);
        }
        else
        {
            yAxisRotation = Random.Range(yAxisRotationMin, yAxisRotationMax);
        }
    }
}
