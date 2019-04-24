using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jellyfish : Enemy {

    public float standing_freq;
    public float standing_magnitude;

    int count = 0;

    IEnumerator Move()
    {
        GameObject ins;
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
            GetComponent<Image>().color += new Color(0,0,0, 1 / 45f);
            if (GetComponent<Image>().color.a >= 1)
            {
                GetComponent<Image>().color = Color.white;
                immortal = false;
                yield return new WaitForSeconds(4f);
                AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
                sct.Play();

                if (GameController.rank >= 7)
                {
                    for (int i = 0; i < (GameController.rank - 6) * 18; i++)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(4f, 16f), ToPlayerAngle() + Random.Range(-40f, 40f), 0.35f, 0.35f,
                        Color.white, new Color(0, Random.Range(0f, 0.75f), 1f));
                    }
                }
                else if (GameController.rank >= 4) {
                    for (int i = 0; i < (GameController.rank - 3) * 5; i++) {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 10f), ToPlayerAngle() + Random.Range(-30f, 30f), 0.35f, 0.35f,
                        Color.white, new Color(0, Random.Range(0f, 0.75f), 1f));
                    }
                }

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

    int launchCnt = 0;
    IEnumerator Launch() {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        cct.Play();

        launchCnt++;
        if (GameController.rank <= 3)
        {
            if (launchCnt <= 2 + GameController.rank * 2)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 3 + launchCnt, ToPlayerAngle(), 0.35f, 0.35f,
                    Color.white, new Color(0, 0.75f - 0.08f * launchCnt, 1f));
                yield return new WaitForSeconds(0.3f);
                StartCoroutine("Launch");
            }
            else yield return null;
        }
        else if (GameController.rank <= 6)
        {
            if (launchCnt <= 4 + GameController.rank * 2)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 5 + launchCnt * 0.5f, ToPlayerAngle(), 0.35f, 0.35f,
                    Color.white, new Color(0, 0.75f - 0.04f * launchCnt, 1f));
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 5 + launchCnt * 0.5f, ToPlayerAngle() + 30f, 0.35f, 0.35f,
                    Color.white, new Color(0, 0.75f - 0.04f * launchCnt, 1f));
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 5 + launchCnt * 0.5f, ToPlayerAngle() - 30f, 0.35f, 0.35f,
                    Color.white, new Color(0, 0.75f - 0.04f * launchCnt, 1f));
                yield return new WaitForSeconds(0.1f);
                StartCoroutine("Launch");
            }
            else yield return null;
        }
        else if (GameController.rank <= 9)
        {
            if (launchCnt <= 4 + GameController.rank * 2)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6 + launchCnt * 0.5f, ToPlayerAngle(), 0.35f, 0.35f,
                    Color.white, new Color(0, 0.75f - 0.03f * launchCnt, 1f));
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6 + launchCnt * 0.5f, ToPlayerAngle() + 20f, 0.35f, 0.35f,
                    Color.white, new Color(0, 0.75f - 0.03f * launchCnt, 1f));
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6 + launchCnt * 0.5f, ToPlayerAngle() - 20f, 0.35f, 0.35f,
                    Color.white, new Color(0, 0.75f - 0.03f * launchCnt, 1f));
                yield return new WaitForSeconds(0.07f);
                StartCoroutine("Launch");
            }
            else yield return null;
        }
        // level insane
        else {
            if (launchCnt <= 30)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8 + launchCnt * 0.5f, ToPlayerAngle(), 0.35f, 0.35f,
                    Color.white, new Color(0, 0.75f - 0.025f * launchCnt, 1f));
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8 + launchCnt * 0.5f, ToPlayerAngle() + 15f, 0.35f, 0.35f,
                    Color.white, new Color(0, 0.75f - 0.025f * launchCnt, 1f));
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8 + launchCnt * 0.5f, ToPlayerAngle() - 15f, 0.35f, 0.35f,
                    Color.white, new Color(0, 0.75f - 0.025f * launchCnt, 1f));
                yield return new WaitForSeconds(0.05f);
                StartCoroutine("Launch");
            }
            else yield return null;
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
