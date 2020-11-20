using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject hitParticle;
    private int damage;
    private float speed;
    private float destroyAfterSeconds = 5.0f;
    private bool friendly;
    private float timer;
    private Vector3 target;
    private GameObject attacker;

    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;
    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        timer += Time.deltaTime;
        if (timer >= destroyAfterSeconds) Destroy(gameObject);
    }
    public void SetStartValues(int _damage, float _speed, float _destroyAfterSeconds, bool _friendly, Transform _target, GameObject _attacker)
    {
        damage = _damage;
        speed = _speed;
        destroyAfterSeconds = _destroyAfterSeconds;
        friendly = _friendly;
        if(_target != null)
        {
            if (_target.Find("EnemyRayTarget") != null) target = _target.Find("EnemyRayTarget").position;
            else target = _target.position;
        }
        else
        {
            target = _attacker.transform.forward * 10;
        }
        transform.LookAt(target);
        attacker = _attacker;
    }
    void Splash()
    {
        // Instantiate the splach prefab
        GameObject splash = Instantiate(hitParticle, collisionEvents[0].intersection, Quaternion.identity, null);
        // Destroy the projectile
        Destroy(gameObject);
    }
    void OnParticleCollision(GameObject other)
    {
        if(other != attacker)
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            print("Particle collided with name: " + other.name + " tag: " + other.tag + ".");
            int i = 0;

            while (i < numCollisionEvents)
            {
                // Projectile is fired by an enemy
                if (!friendly)
                {
                    // Hurt player
                    if (other.CompareTag("Player"))
                    {
                        other.GetComponent<Health>().TakeDamage(damage);
                        // If a particle hits a character, destroy the gameobject because it's very likely that many particles hit the player which will cause the damage to be delt several times over
                        // This is a flawed way since it doesn't allow damage to be dealth to more than one character. A better way is to check if damage has been dealt to a specific NPC and only damage them if it hasn't been damaged by this projectile
                        Splash();
                    }
                }
                // Projectile is fired by a friend
                else
                {
                    // Hurt enemies
                    Splash();
                }
                i++;
            }
            Splash();
            
        }
        
       

        
    }
}
