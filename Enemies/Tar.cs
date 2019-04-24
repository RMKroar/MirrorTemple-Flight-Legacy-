using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tar : Enemy {

    public float mov_speed;
    float direction;
    bool flag = false;

    IEnumerator Move()
    {
        if (!flag)
        {
            if (transform.localPosition.y >= -100f) direction = Random.Range(190f, 200f);
            else direction = Random.Range(160f, 170f);
            flag = true;
        }
        float radiusDir = direction * Mathf.PI / 180f;
        transform.localPosition += new Vector3(mov_speed * Mathf.Cos(radiusDir), mov_speed * Mathf.Sin(radiusDir));
        yield return new WaitForSeconds(1 / 60f);

        if (transform.localPosition.x < 0)
        {
            Image myImage = GetComponent<Image>();
            myImage.color -= new Color(0f, 0f, 0f, 1 / 15f);
            if (myImage.color.a <= 0) Destroy(gameObject);
        }

        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        GameObject ins;

        for (int i = 0; i < 3 + GameController.rank * 2; i++) {
            cct.Play();
            ins = GetBullet("CIRCLE");
            float bulletScale = (GameController.rank >= 5) ? Random.Range(0.25f, 0.5f) : 0.3f;
            if (ins != null) {
                ins.GetComponent<Bullet>().SetIdentityEx("TAR", transform.localPosition, 15f + GameController.rank,
                                                         ToPlayerAngle() + Random.Range(-40f, 40f), bulletScale, bulletScale, Color.grey, Color.black);
            }

            yield return new WaitForSeconds(1f / (3 + GameController.rank * 2));
        }

        yield return new WaitForSeconds(1.5f - 0.1f * GameController.rank);
        if (transform.localPosition.x >= 150) StartCoroutine(Launch());
    }
}
