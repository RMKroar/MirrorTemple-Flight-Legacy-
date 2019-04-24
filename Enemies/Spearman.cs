using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : Enemy {

    public float moveSpeed;

    int moveDirection = 1;

    IEnumerator Move()
    {
        GameObject ins;
        float dir = 180f - moveDirection * 20f;
        float sp = 10f;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        for (int i = 0; i <= 30; i++) {
            if (GameController.rank <= 1)
            {
                if (i % 15 == 0)
                {
                    cct.Play();

                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp, dir, 0.35f, 0.35f, Color.white, Color.red);
                    dir += moveDirection * 20f;
                    sp -= 1f;
                }
            }

            else if (GameController.rank <= 3)
            {
                if (i % 10 == 0)
                {
                    cct.Play();

                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 2, dir, 0.35f, 0.35f, Color.white, Color.red);
                    dir += moveDirection * 10f;
                    sp -= 1f;
                }
            }

            else if (GameController.rank <= 6)
            {
                if (i % 5 == 0)
                {
                    cct.Play();

                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 4, dir, 0.35f, 0.35f, Color.white, Color.red);
                    dir += moveDirection * 8f;
                    sp -= 1f;
                }
            }

            else if (GameController.rank <= 9)
            {
                if (i % 3 == 0)
                {
                    cct.Play();

                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 5, dir, 0.35f, 0.35f, Color.white, Color.red);
                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 2, dir, 0.25f, 0.25f, Color.white, BulletCode.COLOR_ORANGE);
                    dir += moveDirection * 7f;
                    sp -= 1f;
                }
            }

            else {
                if (i % 3 == 0)
                {
                    cct.Play();

                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 5, dir, 0.35f, 0.35f, Color.white, Color.red);
                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 2, dir + 13.3f, 0.25f, 0.25f, Color.white, BulletCode.COLOR_ORANGE);
                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 2, dir - 13.3f, 0.25f, 0.25f, Color.white, BulletCode.COLOR_ORANGE);
                    dir += moveDirection * 7f;
                    sp -= 1f;
                }
            }

            transform.localPosition -= new Vector3(10f, moveDirection * Random.Range(2.1f, 5.6f));
            yield return new WaitForSeconds(1 / 200f);
        }
        moveDirection *= -1;
        yield return new WaitForSeconds(1f - GameController.rank * 0.08f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
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
