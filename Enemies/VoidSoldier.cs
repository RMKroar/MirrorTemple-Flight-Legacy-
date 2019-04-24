using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoidSoldier : Enemy {

    public float standing_freq;
    public float standing_magnitude;

    int count = 0;

    IEnumerator Move()
    {
        count++;
        if (count == 1)
        {
            StartCoroutine("Standing");
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
                StopCoroutine("Standing");
            }
            else
            {
                yield return new WaitForSeconds(1 / 60f);
                StartCoroutine("Move");
            }
        }
    }

    float mov = 0;
    IEnumerator Standing()
    {
        mov += standing_freq;
        transform.localPosition += new Vector3(0f, standing_magnitude * Mathf.Sin(mov));
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Standing");
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

        float savedAngle = ToPlayerAngle();
        float randValue = Random.Range(30f, 55f);
        for (float i = Random.Range(-60f, -50f); i < 55f; i += 30 - 2.2f * GameController.rank) {
            ins = GetBullet("CIRCLE");
            if (ins != null) {
                ins.GetComponent<Bullet>().SetIdentityEx("WIDDY", transform.localPosition, 2f + GameController.rank / 2, savedAngle + i, 0.45f, 0.45f, Color.white, Color.white);
            }

            ins = GetBullet("CIRCLE");
            if (ins != null) {
                ins.GetComponent<Bullet>().SetIdentityEx("WIDDY", 
                    transform.localPosition + new Vector3(60 * Mathf.Cos((savedAngle + i) * Mathf.PI / 180f), 60 * Mathf.Sin((savedAngle + i) * Mathf.PI / 180f)),
                    2f + GameController.rank / 2, savedAngle + i + randValue, 0.35f, 0.35f, Color.black, Color.black);
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("WIDDY",
                    transform.localPosition + new Vector3(60 * Mathf.Cos((savedAngle + i) * Mathf.PI / 180f), 60 * Mathf.Sin((savedAngle + i) * Mathf.PI / 180f)),
                    2f + GameController.rank / 2, savedAngle + i - randValue, 0.35f, 0.35f, Color.black, Color.black);
            }
        }

        yield return new WaitForSeconds(1.5f);
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
