using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip coinAudioClip;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Coin2.CoinCollisionWithPlayer.AddListener(OnCoinCollisionWithPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCoinCollisionWithPlayer()
    {
        audioSource.PlayOneShot(coinAudioClip);
    }
}
