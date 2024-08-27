using Invector;
using Invector.vCharacterController;
using System;
using System.Collections;
using UnityEngine;
public class AIHitBox : MonoBehaviour
{
    [SerializeField] private vThirdPersonController vThirdPersonController;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hit"))
        {
            vObjectDamage vObjectDamage = collision.gameObject.GetComponent<vObjectDamage>();
            int newHealthInt = Convert.ToInt32(vThirdPersonController.currentHealth);
            int damage = vObjectDamage.damage.damageValue / 2;
            vThirdPersonController.ChangeHealth(newHealthInt - damage);
            StartCoroutine(DestroyBullet(collision.gameObject));
        }
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(3f);
        Destroy(bullet);
    }
}