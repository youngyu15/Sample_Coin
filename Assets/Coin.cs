using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    enum CoinState
    {
        Idle,
        MovingInAir,
        MovingTowardsPlayer,
        HitByPlayer
    }

    [SerializeField]
    private float rotationSpeedDegreesPerSecond;
    [SerializeField]
    private float coinAcceleration;

    [HideInInspector]
    public Transform playerTr;

    #region MoveInAir calculation fields
    public float coneHeightMinLength;
    public float coneHeightMaxLength;
    public float coneRadiusMinLength;
    public float coneRadiusMaxLength;
    public float yAxisRotation;
    public float zAxisRotation;
    public float lineLength;
    public Vector3 airPosition;
    #endregion

    private CoinState coinState;
    private Vector3 coinVelocity = Vector3.zero;
    private float currentCoinSpeed = 0f;

    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").transform;
        coinState = CoinState.Idle;
        CalculateAirPositionToMoveTo();
    }

    void Update()
    {
        RotateCoin();
        CheckCoinState();
    }

    void RotateCoin()
    {
        transform.Rotate(new Vector3(0f, rotationSpeedDegreesPerSecond, 0f) * Time.deltaTime, Space.World);
    }

    void CheckCoinState()
    {
        switch (coinState)
        {
            case CoinState.Idle:
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    coinState = CoinState.MovingInAir;
                }
                break;
            case CoinState.MovingInAir:
                MoveCoinInAir();
                if (Vector3.Distance(transform.position, airPosition) < 0.1f)
                {
                    coinState = CoinState.MovingTowardsPlayer;
                }
                break;
            case CoinState.MovingTowardsPlayer:
                MoveCoinTowardsPlayer();
                break;
            case CoinState.HitByPlayer:
                break;
        }
    }

    // 플레이어가 부딪치면 동전을 파괴시킴
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void CalculateAirPositionToMoveTo()
    {
        Vector3 initialMoveVector = new Vector3(0f, lineLength, 0f);
        airPosition = transform.position + (Quaternion.AngleAxis(yAxisRotation, Vector3.up) * Quaternion.AngleAxis(zAxisRotation, Vector3.forward) * initialMoveVector);
    }

    void MoveCoinInAir()
    {
        transform.position = Vector3.SmoothDamp(transform.position, airPosition, ref coinVelocity, 0.2f);
    }

    void MoveCoinTowardsPlayer()
    {
        currentCoinSpeed += coinAcceleration * Time.deltaTime;
        float coinDeltaPos = currentCoinSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerTr.position, coinDeltaPos);
    }
}
