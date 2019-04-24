using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soul : Enemy {

    public int elementalCode; // 0 - fire, 1 - ice, 2 - wind, 3 - earth
    public float standing_freq;
    public float standing_magnitude;

    int count = 0;
    float speed = 0;
    float direction = 0;
    bool beEasy = false;

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
        Image myImage = GetComponent<Image>();
        for (; myImage.color.a > 0; myImage.color -= new Color(0, 0, 0, 1 / 30f)) {
            yield return new WaitForSeconds(1 / 60f);
        }
        Destroy(gameObject);
    }

    IEnumerator Launch()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        cct.Play();
        if (elementalCode == 0) {
            if (!beEasy)
            {
                for (int i = 0; i < 2 + GameController.rank / 6; i++)
                {
                    cct.Play();
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentityEx("LAVABALL", transform.localPosition, 1f + GameController.rank, ToPlayerAngle() + Random.Range(-30f, 30f), 0.5f, 0.5f, Color.white, Color.red);
                    }

                    yield return new WaitForSeconds(1 / 2f);
                }
            }
            else {
                cct.Play();
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("LAVABALL", transform.localPosition, 1f + GameController.rank, ToPlayerAngle() + Random.Range(-30f, 30f), 0.5f, 0.5f, Color.white, Color.red);
                }
            }
            yield return new WaitForSeconds(10f); // Can't Launch Anymore
        }

        if (elementalCode == 1)
        {
            if (!beEasy)
            {
                for (int i = 0; i < 3 + GameController.rank / 4; i++)
                {
                    cct.Play();
                    ins = GetBullet("THORN");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f + GameController.rank * 0.4f, ToPlayerAngle(), 0.4f, 0.4f, Color.white, Color.blue);
                    }

                    yield return new WaitForSeconds(1 / 10f);
                }
            }
            else {
                for (int i = 0; i < 1 + GameController.rank / 6; i++)
                {
                    cct.Play();
                    ins = GetBullet("THORN");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f + GameController.rank * 0.4f, ToPlayerAngle(), 0.4f, 0.4f, Color.white, Color.blue);
                    }

                    yield return new WaitForSeconds(1 / 10f);
                }
            }
            
            yield return new WaitForSeconds(1.5f - 0.1f * GameController.rank);
        }

        if (elementalCode == 2) {
            int dens = 0;
            if (!beEasy) dens = 3 + GameController.rank / 4;
            else dens = 1 + GameController.rank / 6;

            for(int i = 1; i <= dens; i++)
            {
                float tempDir;
                for (float j = Random.Range(-40f, -30f); j < 35f; j += 85f / (4 + GameController.rank / 2)) {
                    ins = GetBullet("CIRCLE");
                    tempDir = ToPlayerAngle() + j;
                    if (ins != null) {
                        ins.GetComponent<Bullet>().SetIdentityEx("WIDDY", transform.localPosition + 30 * i * new Vector3(Mathf.Cos(tempDir * Mathf.PI / 180f), Mathf.Sin(tempDir * Mathf.PI / 180f)),
                            dens - i + 1, tempDir, 0.3f, 0.3f, Color.white, Color.green);
                    }
                }
                yield return new WaitForSeconds(1f / dens);
            }

            yield return new WaitForSeconds(10f); // Can't Launch Anymore
        }

        if (elementalCode == 3)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("GNOME", transform.localPosition, 0.01f, 180, 0.25f, 0.25f, Color.white, BulletCode.COLOR_BROWN);
            }

            if (!beEasy) yield return new WaitForSeconds(0.3f - 0.023f * GameController.rank);
            else yield return new WaitForSeconds(0.6f - 0.04f * GameController.rank);
        }

        StartCoroutine(Launch());
    }

    public void SetIdentity(Vector3 position, float _speed, float _direction) {
        transform.localPosition = position;
        speed = _speed;
        direction = _direction;
        beEasy = true;
        health = 1;
        StartCoroutine(SoulMove());
    }

    IEnumerator SoulMove() {
        Vector3 moveVector = new Vector3(speed * Mathf.Cos(direction * Mathf.PI / 180f), speed * Mathf.Sin(direction * Mathf.PI / 180f));

        while (true) {
            transform.localPosition += moveVector;
            yield return new WaitForSeconds(1 / 30f);
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
