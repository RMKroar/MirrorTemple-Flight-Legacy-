using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannoneer : Enemy {

    public float mov_speed;

    IEnumerator Move()
    {
        transform.localPosition += new Vector3(-mov_speed, 0);
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        GameObject ins;
        if (GameController.rank >= 0) {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CANNON", transform.localPosition, Random.Range(20f, 30f), 90f + Random.Range(0f, 12f), 0.7f, 0.7f, Color.black, Color.black);
        }
        if (GameController.rank >= 5)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CANNON", transform.localPosition, Random.Range(20f, 30f), 90f + Random.Range(8f, 20f), 0.7f, 0.7f, Color.black, Color.black);
        }
        if (GameController.rank >= 10)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CANNON", transform.localPosition, Random.Range(20f, 30f), 90f + Random.Range(2f, 14f), 0.7f, 0.7f, Color.black, Color.black);
        }
        yield return null;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
