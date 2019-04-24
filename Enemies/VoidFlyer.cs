using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidFlyer : Enemy {

    public float mov_speed;
    public float mov_friction;

    float direction;
    bool flag = false;

    IEnumerator Move()
    {
        if (!flag)
        {
            direction = ToPlayerAngle();
            mov_speed += Random.Range(-3f, 3f);
            flag = true;
        }
        mov_speed += mov_friction;
        transform.localPosition += new Vector3(mov_speed * Mathf.Cos(direction * Mathf.PI / 180f), mov_speed * Mathf.Sin(direction * Mathf.PI / 180f));
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        cct.Play();

        float randAngle = Random.Range(0f, 360f);

        ins = GetBullet("PETAL");
        if (ins != null) {
            ins.GetComponent<Bullet>().SetIdentity(
                transform.localPosition + new Vector3(50 * Mathf.Cos(randAngle * Mathf.PI / 180f), 50 * Mathf.Sin(randAngle * Mathf.PI / 180f)), 
                5f + GameController.rank, randAngle + 15f, 0.4f, 0.4f, Color.white, Color.white);
        }
        ins = GetBullet("PETAL");
        if (ins != null)
        {
            ins.GetComponent<Bullet>().SetIdentity(
                transform.localPosition + new Vector3(50 * Mathf.Cos(randAngle * Mathf.PI / 180f), 50 * Mathf.Sin(randAngle * Mathf.PI / 180f)),
                5f + GameController.rank, randAngle - 15f, 0.4f, 0.4f, Color.black, Color.black);
        }

        yield return new WaitForSeconds(0.2f - 0.017f * GameController.rank);
        StartCoroutine("Launch");
    }

    private void OnDestroy()
    {
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();
        lct.volume = 0.35f; lct.Play();

        ins = GetBullet("PETAL");
        if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 6 + GameController.rank / 2, ToPlayerAngle(),
            0.4f, 0.4f, Color.white, Color.white);
        ins = GetBullet("PETAL");
        if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 6 + GameController.rank / 2, ToPlayerAngle() + 20f,
            0.3f, 0.3f, Color.black, Color.black);
        ins = GetBullet("PETAL");
        if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 6 + GameController.rank / 2, ToPlayerAngle() - 20f,
            0.4f, 0.4f, Color.black, Color.black);

        Collapse();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
