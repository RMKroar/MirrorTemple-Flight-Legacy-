using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrostKimera : Enemy {

    int count = 0;
    IEnumerator Move()
    {
        count++;
        if (count == 1)
        {
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            immortal = true;
            yield return new WaitForSeconds(1 / 60f);
            StartCoroutine("Move");
        }
        else
        {
            GetComponent<Image>().color += new Color(0, 0, 0, 1 / 45f);
            if (GetComponent<Image>().color.a >= 1)
            {
                GetComponent<Image>().color = Color.white;
                immortal = false;
                yield return new WaitForSeconds(6f);

                StartCoroutine("EndMove");
                count = 0;
            }
            else
            {
                yield return new WaitForSeconds(1 / 60f);
                StartCoroutine("Move");
            }
        }
    }

    IEnumerator EndMove()
    {
        while (true) {
            GameObject ins;
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f + GameController.rank, ToPlayerAngle(), 0.45f, 0.45f,
                 Color.white, Color.red);

            GetComponent<Image>().color = Color.red;
            count++;
            transform.localPosition -= new Vector3(5 + count, 0f);
            yield return new WaitForSeconds(1 / 60f);
        }   
    }

    IEnumerator Launch()
    {
        GameObject ins;
        int deltaDir = 1;
        int[] density = new int[11] { 30, 27, 25, 22, 18, 16, 15, 14, 13, 12, 10 };
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        for (float i = Random.Range(55, 180); ; i += deltaDir * density[GameController.rank]) {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 10f, i, 0.35f, 0.35f,
                 Color.blue, Color.white);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 6f, i, 0.35f, 0.35f,
                 BulletCode.COLOR_SKY, Color.white);

            if (i < 55 || i > 180) deltaDir *= -1;

            yield return new WaitForSeconds(0.7f - 0.055f * GameController.rank);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
