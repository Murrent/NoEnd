using UnityEditor;
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
        player = GameObject.FindAnyObjectByType<Player>();
    }

    void Update()
    {
        if (fuseTimer > 0)
        {
            fuseTimer -= Time.deltaTime;
        }
        else if (fuseTimer <= 0 && !_exploded)
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var VARIABLE in hitColliders)
        {
            if (VARIABLE.tag == "Player")
            {
                Debug.Log("EXPLODED PLAYER");
                //damage call
            }
        }
    }
    
    void OnDrawGizmos()
    {
        if (_exploded)
        {
            Gizmos.color = new Color(1, 0, 0, 0.8f);
            Gizmos.DrawSphere(transform.position, explosionRadius);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
