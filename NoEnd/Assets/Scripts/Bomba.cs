using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Interfaces;

public class Bomba : MonoBehaviour
{
    //explosion variables
    [SerializeField]
    public float fuseTimer = 3.0f;
    public float explosionRadius = 1.0f;
    public float explosionStart = 1.5f;
    public int damage = 5;
    private bool _exploded = false;
    
    //vfx
    [SerializeField]
    public Color flashColor;

    [SerializeField]
    public ParticleSystem explosionParticles; 

    private float _flashSpeed = 5.0f;
    private Color _originalColor;
    MeshRenderer _meshRenderer;
    
    //sound
    private AudioSource _explosionSfx; 
    
    void Start()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _originalColor = _meshRenderer.material.color;
        
        _explosionSfx = GetComponent<AudioSource>();
        explosionParticles.Stop();
    }

    void Update()
    {
        if (fuseTimer > 0)
        {
            fuseTimer -= Time.deltaTime;
            
            float flashFrequency = _flashSpeed * (1f - (fuseTimer / 3f));
            float lerp = Mathf.PingPong(Time.time * flashFrequency, 1f);
            _meshRenderer.material.color = Color.Lerp(_originalColor, flashColor, lerp);
        }
        else if (fuseTimer <= explosionStart && !_exploded)
        {
           Debug.Log("BOMBOCLAAAAT MI EXPLODED");
            _meshRenderer.enabled = false;
           explosionParticles.Play();
           _explosionSfx.Play();
           _exploded = true;
           Explosion(explosionRadius);
        }
        
        if (_exploded && !_explosionSfx.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }

    void Explosion(float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var collider in hitColliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log($"EXPLODED {collider.gameObject.name}");
                damageable.TakeDamage(damage);
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