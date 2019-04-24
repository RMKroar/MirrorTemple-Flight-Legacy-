using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitSword : MonoBehaviour {

    public int moveCode;
    public float speed;
    public float direction;

    // Use this for initialization
    void Start () {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        StartCoroutine("Launch");
        StartCoroutine("Enable");
        StartCoroutine("Refresh");
    }

    IEnumerator Refresh()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, direction));
        float radDir = direction * Mathf.PI / 180f;
        transform.localPosition += new Vector3(speed * Mathf.Cos(radDir), speed * Mathf.Sin(radDir), 0f);

        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Refresh");
    }

    IEnumerator Launch()
    {
        float randValue;
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        if (moveCode == 0)
        {
            speed = 6f; direction = 270f;
            yield return new WaitForSeconds(2 / 3f);
            speed = 0f;
            yield return new WaitForSeconds(0.5f);

            for (; transform.localPosition.y > 0; transform.localPosition -= new Vector3(0f, 20f))
            {
                speed = 50f;
                yield return new WaitForSeconds(1 / 60f);
            }
            speed = 1f;

            for (int i = 0; i < 6 + GameController.rank; i++)
            {
                randValue = Random.Range(0f, 360f);
                cct.Play();
                for (float j = 0; j < 360; j += 360f / ((GameController.rank % 2) + 6))
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(0f, -80f, 0f),
                            8f, randValue + j, 0.35f, 0.35f, Color.white, Color.blue);
                    }
                }
                yield return new WaitForSeconds(4.5f / (6 + GameController.rank));
            }

            randValue = Random.Range(0f, 360f);
            for (float j = 0; j < 360; j += 360f / (GameController.rank + 5))
            {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3(0f, -80f, 0f),
                        12f, randValue + j, 0.35f, 0.35f, Color.blue, Color.white);
                }
                if (GameController.rank > 5) {
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3(0f, -80f, 0f),
                            6f, randValue + j + 180f / (GameController.rank + 5), 0.35f, 0.35f, BulletCode.COLOR_SKY, Color.white);
                    }
                }
                
            }
            InvokeUnable();
        }
        else if (moveCode >= 1) {
            speed = 3f; direction = (moveCode == 1)? 225f : 315f;
            yield return new WaitForSeconds(1.2f);
            speed = 0f;
            yield return new WaitForSeconds(0.5f);

            speed = 50f;
            while (transform.localPosition.y >= 0) {
                cct.Play();
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3((moveCode - 2) * Random.Range(800f, 1000f), Random.Range(-1000f, -800f)),
                        30f, direction + Random.Range(-30f, 30f), 0.35f, 0.35f, new Color(0f, 0f, 1f), Color.white);
                }
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3((moveCode - 2) * Random.Range(800f, 1000f), Random.Range(-1000f, -800f)),
                        25f, direction + Random.Range(-45f, 45f), 0.35f, 0.35f, new Color(0f, 1f, 0f), Color.white);
                }
                if (GameController.rank >= 2) {
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3((moveCode - 2) * Random.Range(800f, 1000f), Random.Range(-1000f, -800f)),
                            20f, direction + Random.Range(-60f, 60f), 0.35f, 0.35f, new Color(0f, 0.2f, 0.8f), Color.white);
                    }
                }
                if (GameController.rank >= 5)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3((moveCode - 2) * Random.Range(800f, 1000f), Random.Range(-1000f, -800f)),
                            15f, direction + Random.Range(-75f, 75f), 0.35f, 0.35f, new Color(0f, 0.4f, 0.6f), Color.white);
                    }
                }
                if (GameController.rank >= 7)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3((moveCode - 2) * Random.Range(800f, 1000f), Random.Range(-1000f, -800f)),
                            10f, direction + Random.Range(-90f, 90f), 0.35f, 0.35f, new Color(0f, 0.2f, 0.8f), Color.white);
                    }
                }
                if (GameController.rank >= 9)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3((moveCode - 2) * Random.Range(800f, 1000f), Random.Range(-1000f, -800f)),
                            5f, direction + Random.Range(-105f, 105f), 0.35f, 0.35f, BulletCode.COLOR_VIOLET, Color.white);
                    }
                }

                yield return new WaitForSeconds(1 / 120f);
            }

            speed = 10;
            InvokeUnable();
        }
        yield return null;
    }

    public GameObject GetBullet(string code)
    {
        return GameController.BulletPool.GetComponent<BulletPool>().GetChildMin(code);
    }

    public GameObject GetPlayer()
    {
        GameObject returnObj = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            returnObj = obj;
        }

        return returnObj;
    }

    public float ToPlayerAngle()
    {
        GameObject player = GetPlayer();
        Vector3 moveDirection = player.transform.localPosition - transform.parent.localPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            return angle;
        }
        else return 0;
    }

    IEnumerator Enable()
    {
        Image img = GetComponent<Image>();
        for (; img.color.a < 0.75f; img.color += new Color(0f, 0f, 0f, 1 / 15f))
        {
            yield return new WaitForSeconds(1 / 60f);
        }
        yield return null;
    }

    public void InvokeUnable()
    {
        StartCoroutine("Unable");
    }

    IEnumerator Unable()
    {
        StopCoroutine("Launch");
        Image img = GetComponent<Image>();
        for (; img.color.a > 0; img.color -= new Color(0f, 0f, 0f, 1 / 60f))
        {
            yield return new WaitForSeconds(1 / 60f);
        }
        Destroy(gameObject);
    }
}
