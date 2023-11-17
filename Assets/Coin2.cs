using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin2 : MonoBehaviour
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
    [SerializeField]
    private float initialCoinSpeed;

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

    public static UnityEvent CoinCollisionWithPlayer = new UnityEvent();

    private CoinState coinState;
    private float currentCoinSpeed = 0f;
    private Rigidbody rb;

    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
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
                    MoveCoinInAir();
                    coinState = CoinState.MovingInAir;
                }
                break;
            case CoinState.MovingInAir:
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
            CoinCollisionWithPlayer.Invoke();
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
        rb.AddForce(initialCoinSpeed * (airPosition - transform.position).normalized, ForceMode.Impulse);
    }

    void MoveCoinTowardsPlayer()
    {
        currentCoinSpeed += coinAcceleration * Time.deltaTime;
        float coinDeltaPos = currentCoinSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerTr.position, coinDeltaPos);
    }
}
