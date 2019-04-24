using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcadrobotA : Enemy {

    public float mov_speed;
    public GameObject AlcadrobotC;
    public int mode;

    IEnumerator Move()
    {
        transform.localPosition += new Vector3(-mov_speed, 0);
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        if (mode == 0)
        {
            bool rev = (transform.localPosition.x > GetPlayer().transform.localPosition.x);
            GameObject ins;
            
            if (rev)
            {
                sct.Play();

                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROCHASE", transform.localPosition, 10f + GameController.rank * 1.2f,
                    180f, 0.5f, 0.5f, Color.white, Color.black);
                yield return new WaitForSeconds(1.2f);
            }
            else
            {
                cct.Play();

                ins = GetBullet("CIRCLE");
                float sc = Random.Range(0.25f, 0.45f);
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(6f * GameController.rank * 0.2f, 8f + GameController.rank * 0.6f),
                    Random.Range(-90f, 90f), sc, sc, Color.white, Color.black);
                yield return new WaitForSeconds(0.5f - 0.047f * GameController.rank);
            }

            StartCoroutine("Launch");
        }
        else yield return null;
    }

    private void OnDestroy()
    {
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();

        if (mode == 0)
        {
            ins = Instantiate(AlcadrobotC, GameObject.Find("TriggerPanel").transform);
            if (ins != null) ins.transform.localPosition = new Vector3(660f, Random.Range(-305f, 305f), 0f);
            ins = Instantiate(AlcadrobotC, GameObject.Find("TriggerPanel").transform);
            if (ins != null) ins.transform.localPosition = new Vector3(660f, Random.Range(-305f, 305f), 0f);
        }
        else {
            if (health <= 0)
            {
                float launchDir = 1 + GameController.rank;
                lct.volume = 0.35f; lct.Play();
                for (float i = Random.Range(0f, 360f / launchDir); i < 360; i += (360f / launchDir))
                {
                    ins = GetBullet("PETAL");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 5f + GameController.rank * 0.5f, i,
                        0.3f, 0.3f, Color.white, Color.yellow);
                }
            }
            else {
                for (int i = 0; i < (GameController.rank + 1); i++) {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(2f, 3 + GameController.rank * 0.2f), Random.Range(-90f, 90f),
                        0.25f, 0.25f, Color.white, BulletCode.COLOR_SKY);
                }
            }
        }

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
