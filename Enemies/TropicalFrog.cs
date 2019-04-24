using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TropicalFrog : Enemy {

    public Sprite CastImage;
    public float mov_speed;
    float direction;
    bool flag = false;

    IEnumerator Move()
    {
        for (int i = 0; i <= 5; i++)
        {
            if (!flag)
            {
                if (transform.localPosition.y >= -100f) direction = Random.Range(190f, 210f);
                else direction = Random.Range(150f, 170f);
                flag = true;
            }
            float radiusDir = direction * Mathf.PI / 180f;
            transform.localPosition += new Vector3(mov_speed * Mathf.Cos(radiusDir), mov_speed * Mathf.Sin(radiusDir));
            yield return new WaitForSeconds(1 / 60f);
        }
        yield return new WaitForSeconds(4f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        GameObject ins;

        StopCoroutine(Animate());
        float fixedAngle = ToPlayerAngle();

        for (int j = 0; j < 4 + GameController.rank / 3; j++)
        {
            gameObject.GetComponent<Image>().sprite = CastImage;
            cct.Play();

            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 20f, fixedAngle, 0.5f, 0.5f,
             Color.white, new Color(0, Random.Range(0.2f, 1f), 1f));
            if (GameController.rank > 3)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 20f, fixedAngle + 2.5f, 0.35f, 0.35f,
                 Color.white, new Color(0, Random.Range(0.2f, 1f), 1f));
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 20f, fixedAngle - 2.5f, 0.35f, 0.35f,
                 Color.white, new Color(0, Random.Range(0.2f, 1f), 1f));
            }
            if (GameController.rank > 7)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 20f, fixedAngle + 5f, 0.25f, 0.25f,
                 Color.white, new Color(0, Random.Range(0.2f, 1f), 1f));
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 20f, fixedAngle - 5f, 0.25f, 0.25f,
                 Color.white, new Color(0, Random.Range(0.2f, 1f), 1f));
            }

            yield return new WaitForSeconds(1 / 8f);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Animate());
        yield return new WaitForSeconds(0.5f);
        if (transform.localPosition.x >= 0) StartCoroutine(Launch());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
