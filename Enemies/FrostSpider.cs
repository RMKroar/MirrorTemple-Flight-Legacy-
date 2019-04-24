using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrostSpider : Enemy {

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
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        GameObject ins;

        sct.Play();
        if (GameController.rank <= 3)
        {
            for (float i = 180 + Random.Range(-40f, -30f); i <= 215f; i += 40)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 10f, i, 0.35f, 0.35f, 
                    BulletCode.COLOR_SKY, Color.white);
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 10f, i + 180, 0.35f, 0.35f,
                    BulletCode.COLOR_SKY, Color.white);
            }
        }
        else if (GameController.rank <= 6)
        {
            for (float i = 180 + Random.Range(-40f, -30f); i <= 215f; i += 30)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 10f, i, 0.35f, 0.35f,
                    BulletCode.COLOR_SKY, Color.white);
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 10f, i + 180, 0.35f, 0.35f,
                    BulletCode.COLOR_SKY, Color.white);
            }
        }
        else if (GameController.rank <= 9)
        {
            for (float i = 180 + Random.Range(-40f, -30f); i <= 215f; i += 20)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 10f, i, 0.35f, 0.35f,
                    BulletCode.COLOR_SKY, Color.white);
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 10f, i + 180, 0.35f, 0.35f,
                    BulletCode.COLOR_SKY, Color.white);
            }
        }
        else
        {
            for (float i = 180 + Random.Range(-40f, -30f); i <= 215f; i += 10)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 10f, i, 0.35f, 0.35f,
                    BulletCode.COLOR_SKY, Color.white);
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 10f, i + 180, 0.35f, 0.35f,
                    BulletCode.COLOR_SKY, Color.white);
            }
        }
        yield return new WaitForSeconds(6f - 0.4f * GameController.rank);
        if (transform.localPosition.x >= 150) StartCoroutine(Launch());
    }
}
