using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skullcrawler : Enemy {
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
                yield return new WaitForSeconds(4f);

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
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();

        for (int i = 0; i < 2 + GameController.rank / 4; i++) {
            lct.volume = 0.35f; lct.Play();
            ins = GetBullet("PETAL");
            float randAngle = ToPlayerAngle() + Random.Range(-30f, 30f);
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 10f + GameController.rank * 0.5f, randAngle,
                0.3f, 0.3f, Color.white, Color.red);

            for (float j = Random.Range(-(5 + GameController.rank * 4), -(15 + GameController.rank * 4)); j < 10 + GameController.rank * 4;
                       j += 20 - GameController.rank) {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f + GameController.rank * 0.5f, randAngle + j,
                    0.3f, 0.3f, Color.white, Color.red);
            }

            yield return new WaitForSeconds(0.5f - 0.1f * (GameController.rank / 4));
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
