using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maneaterdrake : Enemy {

    public float mov_speed;
    float direction;
    bool flag = false;

    IEnumerator Move()
    {
        if (!flag) {
            if (transform.localPosition.y >= -100f) direction = Random.Range(190f, 210f);
            else direction = Random.Range(150f, 170f);

            StartCoroutine(Launch2());
            flag = true;
        }
        float radiusDir = direction * Mathf.PI / 180f;
        transform.localPosition += new Vector3(mov_speed * Mathf.Cos(radiusDir), mov_speed * Mathf.Sin(radiusDir));
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        GameObject ins;

        sct.Play();
        if (GameController.rank <= 3) {
            for (float i = 180 + Random.Range(-40f, -30f); i <= 215f; i += 15) {
                ins = GetBullet("CIRCLE");
                if (ins != null) {
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(0f, 40f), 12f, i, 0.35f, 0.35f, Color.white, Color.red);
                }
            }
        }
        else if (GameController.rank <= 6)
        {
            for (float i = 180 + Random.Range(-40f, -30f); i <= 215f; i += 10)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(0f, 40f), 12f, i, 0.35f, 0.35f, Color.white, Color.red);
                }
            }
        }
        else if (GameController.rank <= 9)
        {
            for (float i = 180 + Random.Range(-40f, -30f); i <= 215f; i += 8)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(0f, 40f), 12f, i, 0.35f, 0.35f, Color.white, Color.red);
                }
            }
        }
        else
        {
            for (float i = 180 + Random.Range(-40f, -30f); i <= 215f; i += 7)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(0f, 40f), 12f, i, 0.35f, 0.35f, Color.white, Color.red);
                }
            }
        }
        yield return new WaitForSeconds(2f - 0.05f * GameController.rank);
        if(transform.localPosition.x >= 0) StartCoroutine(Launch());
    }

    IEnumerator Launch2() {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        
        yield return new WaitForSeconds(1f - 0.06f * GameController.rank);
        ins = GetBullet("CIRCLE");
        if (ins != null) {
            ins.GetComponent<Bullet>().SetIdentityEx("MANEATERDRAKE", transform.localPosition - new Vector3(0f, 40f),
                10f, ToPlayerAngle(), 0.4f, 0.4f, Color.white, Color.green);
        }
        sct.Play();
        yield return new WaitForSeconds(1f - 0.06f * GameController.rank);
        if (transform.localPosition.x >= 0) StartCoroutine(Launch2());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
