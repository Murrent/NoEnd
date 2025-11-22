using UnityEngine;
using UnityEngine.Serialization;

public class Bomba : MonoBehaviour
{
    [SerializeField]
    public float fuseTimer = 3.0f;
    public float explosionRadius = 1.0f;
    
    private bool _exploded = false;
    public Player player;
    private AudioSource _explosionSfx; 
    
    void Start()
    {
        _explosionSfx = GetComponent<AudioSource>();
        GameObject.FindAnyObjectByType<Player>();
    }

    void Update()
    {
        if (fuseTimer > 0)
        {
            fuseTimer -= Time.deltaTime;
        }
        else if (fuseTimer <= 0 && _exploded == false)
        {
           Debug.Log("BOMBOCLAAAAT EXPLOSION");
           Explosion(explosionRadius);
           _explosionSfx.Play();
           _exploded = true;
        }
    }

    void Explosion(float radius)
    {
        // Physics.OverlapSphereNonAlloc(Vector3.zero, 0.5f, new Collider[]);
        Collider[] hitColliders = Physics.OverlapSphere(Vector3.zero, radius);
        foreach (var VARIABLE in hitColliders)
        {
            if (VARIABLE.tag == "Player")
            {
                Debug.Log("EXPLODED PLAYER");
            }
        }
    }
}
