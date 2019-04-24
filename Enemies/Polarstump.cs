using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polarstump : Enemy {

    public float mov_speed;

    IEnumerator Move()
    {
        transform.localPosition += new Vector3(-mov_speed, 0);
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        cct.Play();

        GameObject ins = GetBullet("CIRCLE");
        if (ins != null)
        {
            ins.GetComponent<Bullet>().SetIdentityEx("SNOWSOUL", transform.localPosition + new Vector3(-35f, 60f),
            Random.Range(15f, 30f), 90f + Random.Range(0f, 45f), 0.35f, 0.35f, Color.white, Color.blue);
        }
        yield return new WaitForSeconds(0.5f - 0.04f * GameController.rank);
        if (transform.localPosition.x >= 100) StartCoroutine(Launch());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
