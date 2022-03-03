using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Gun Weapon;
    public Rigidbody2D rb;
    
    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Start()
    {
        StartCoroutine(destroyAfterTime(Weapon.BulletTravelTime));
        var direction = createRecoil(Weapon.ShootOrigin.transform.position - Player.Singleton.transform.position);
        
        rb.AddForce(direction * Weapon.BulletSpeed, ForceMode2D.Impulse);
    }

    private Vector2 createRecoil(Vector2 origin)
    {
        var recoil = Random.Range(Weapon.Recoil * -1, Weapon.Recoil);
        return new Vector2(origin.x + recoil, origin.y - recoil);
    }

    private void OnCollisionEnter2D(Collision2D other) => onCollision(other.collider);
    
    protected virtual void onCollision(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            Destroy(Instantiate(EffectLibrary.Particles["Blood"], transform.position, transform.rotation), 2f);
            coll.GetComponent<Enemy>().Health -= Weapon.Damage;
        }
        
        Destroy(gameObject);
    }

    private IEnumerator destroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
