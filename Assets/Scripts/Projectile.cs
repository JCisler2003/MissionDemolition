using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))] // Ensures AudioSource is present

public class Projectile : MonoBehaviour
{
    const int LOOKBACK_COUNT = 10;
    static List<Projectile> PROJECTILES = new List<Projectile>();

    [SerializeField]
    private bool _awake = true;
    public bool awake
    {
        get { return _awake; }
        private set { _awake = value; }
    }

    private Vector3 prevPos;
    private List<float> deltas = new List<float>();
    private Rigidbody rigid;
    private AudioSource audioSource;
    private bool hasLaunched = false; // Tracks if projectile has been launched

    private const float launchThreshold = 0.2f; // Minimum speed to count as launched
    private const float stopThreshold = 0.05f;  // Minimum speed to stop sound

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;   // Loop sound while moving
        audioSource.playOnAwake = false; // Don't start automatically

        awake = true;
        prevPos = transform.position;
        deltas.Add(1000);

        PROJECTILES.Add(this);
    }

    void FixedUpdate()
    {
        if (rigid.isKinematic || !awake) return;

        Vector3 deltaV3 = transform.position - prevPos;
        deltas.Add(deltaV3.magnitude);
        prevPos = transform.position;

        while (deltas.Count > LOOKBACK_COUNT)
        {
            deltas.RemoveAt(0);
        }

        float maxDelta = 0;
        foreach (float f in deltas)
        {
            if (f > maxDelta) maxDelta = f;
        }

        float currentSpeed = rigid.linearVelocity.magnitude; // Get current speed

        if (!hasLaunched && currentSpeed > launchThreshold)
        {
            hasLaunched = true; // Mark as launched
        }

        if (hasLaunched && currentSpeed > stopThreshold)
        {
            if (!audioSource.isPlaying) audioSource.Play(); // Start sound
        }
        else if (hasLaunched && currentSpeed <= stopThreshold)
        {
            if (audioSource.isPlaying) audioSource.Stop(); // Stop sound when fully stopped
            awake = false;
            rigid.Sleep();
        }
    }

    private void OnDestroy()
    {
        PROJECTILES.Remove(this);
    }

    static public void DESTROY_PROJECTILES()
    {
        foreach (Projectile p in PROJECTILES)
        {
            Destroy(p.gameObject);
        }
    }
}


