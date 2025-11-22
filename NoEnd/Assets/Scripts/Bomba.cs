using UnityEngine;

public class Bomba : MonoBehaviour
{
    [SerializeField]
    public float fuseTimer = 3.0f;
    public float explosionRadius = 1.0f;
    
    private bool exploded = false;
    public Player _player;
    public AudioSource audio; 
    
    void Start()
    {
        audio = GetComponent<AudioSource>();
        GameObject.FindAnyObjectByType<Player>();
    }

    void Update()
    {
        if (fuseTimer > 0)
        {
            fuseTimer -= Time.deltaTime;
        }
        else if (fuseTimer <= 0 && exploded == false)
        {
           Debug.Log("BOMBOCLAAAAT EXPLOSION");
           Explosion(explosionRadius);
           audio.Play();
           exploded = true;
        }
    }

    void Explosion(float radius)
    {
        // Physics.OverlapSphereNonAlloc(Vector3.zero, 0.5f, new Collider[]);
        //only take in player collider
        
        Collider[] hitColliders = Physics.OverlapSphere(Vector3.zero, radius);
        foreach (var VARIABLE in hitColliders)
        {
            Debug.Log("BOMBOCLAAAAT HIT");
            //damage code
        }
    }
}
