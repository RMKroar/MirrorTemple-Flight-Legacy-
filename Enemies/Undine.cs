using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Undine : Enemy {

    public float retreatSpeed;
    public float emergeSpeed;

    float emerge = 0;

    IEnumerator Move()
    {
        emerge += emergeSpeed;
        if (emerge > 1)
        {
            immortal = false;
            gameObject.GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(3f);
            StartCoroutine("EndMove");
        }
        else
        {
            immortal = true;
            gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, emerge);
            yield return new WaitForSeconds(1 / 60f);
            StartCoroutine("Move");
        }
    }

    IEnumerator EndMove()
    {
        transform.localPosition += new Vector3(retreatSpeed, 0f, 0f);
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("EndMove");
    }

    IEnumerator Launch()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        if (GameController.rank <= 2)
        {
            float di = 4 + GameController.rank;
            for (float i = Random.Range(0f, 360 / di); i < 360; i += 360 / di)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentityEx("UNDINE", transform.localPosition, 6f, i, 0.3f, 0.3f, Color.white,
                    new Color(0f, Random.Range(0f, 0.8f), 1f));
            }
            yield return new WaitForSeconds(1.9f - 0.2f * GameController.rank);
        }
        else if (GameController.rank <= 5)
        {
            float di = 8 + GameController.rank;
            float sc = Random.Range(0.2f, 0.5f);
            for (float i = Random.Range(0f, 360 / di); i < 360; i += 360 / di)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentityEx("UNDINE", transform.localPosition, 7f, i, sc, sc, Color.white,
                    new Color(0f, Random.Range(0f, 0.8f), 1f));
            }
            yield return new WaitForSeconds(2.8f - 0.3f * GameController.rank);
        }
        else {
            float di = 9 + GameController.rank;
            float sc = Random.Range(0.2f, 0.4f);
            for (float i = Random.Range(0f, 360 / di); i < 360; i += 360 / di)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentityEx("UNDINE", transform.localPosition, 8f + 0.6f * (GameController.rank - 6), i, sc, sc, Color.white,
                    new Color(0f, Random.Range(0f, 0.8f), 1f));
            }
            yield return new WaitForSeconds(1.2f - 0.2f * (GameController.rank - 6));
        }
        StartCoroutine("Launch");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
