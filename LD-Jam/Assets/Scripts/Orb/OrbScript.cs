using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : MonoBehaviour
{
    [SerializeField]
    private Collider2D explosionTrigger;

    private int damage;

    private float speed;

    private bool isReady;
    private bool exploded;

    private Vector2 target;

    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();

        ToggleExplosionTrigger(false);
    }

    private void FixedUpdate()
    {
        if (isReady)
        {
            if ((Vector2)transform.position == target)
            {
                Explode();
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
        }
    }

    public void ProjectTo(Vector2 mousePos, float shootSpeed, int orbDamage = 2)
    {
        target = mousePos;
        speed = shootSpeed;
        damage = orbDamage;

        isReady = true;
    }

    private void Explode()
    {
        FindObjectOfType<SoundFXPlayer>().PlaySpellLandSound();
        myAnimator.SetTrigger("Explode");

        CameraScript.instance.ShakeCamera();

        isReady = false;
        exploded = true;
    }

    public void ToggleExplosionTrigger(bool toggle)
    {
        explosionTrigger.enabled = toggle;
    }

    public void Dismiss()
    {
        exploded = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReady)
        {
            Explode();
        }
        if (exploded)
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                other.GetComponent<IDamageable>().TakeDamage(damage);
            }

            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerScript>().Immunity();
            }
        }
    }
}
