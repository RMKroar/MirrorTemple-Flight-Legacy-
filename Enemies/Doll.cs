using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doll : Enemy {

    int count = 0;

    IEnumerator Move()
    {
        GameObject ins;
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
                yield return new WaitForSeconds(4f);

                AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
                sct.Play();
                float density = 360 / (8 + GameController.rank * 3);
                for (float i = Random.Range(0f, density); i < 360; i += density)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) {
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, i, 0.3f, 0.3f,
                            Color.white, Color.black);
                    }                 
                }

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
        count++;
        transform.localPosition += new Vector3(2 + count * 0.05f, 0f);
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("EndMove");
    }

    IEnumerator Launch()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        cct.Play();

        float tempAngle = ToPlayerAngle() + Random.Range(-10f, 10f);
        ins = GetBullet("THORN");
        if (ins != null) {
            ins.GetComponent<Bullet>().SetIdentityEx("DOLL", transform.localPosition, 15 + GameController.rank, tempAngle,
                0.3f, 0.3f, Color.white, Color.black);
        }

        if (GameController.rank >= 5) {
            ins = GetBullet("THORN");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("DOLL", transform.localPosition, 15 + GameController.rank, tempAngle - 2f,
                    0.3f, 0.3f, Color.white, Color.black);
            }
            ins = GetBullet("THORN");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("DOLL", transform.localPosition, 15 + GameController.rank, tempAngle + 2f,
                    0.3f, 0.3f, Color.white, Color.black);
            }
        }

        yield return new WaitForSeconds(1.2f - 0.04f * GameController.rank);
        StartCoroutine(Launch());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
