using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lutadia : Enemy {

    public Sprite frame1;
    public Sprite frame2;

    // Ignore initial setting in 'Enemy' script
    void Start()
    {
        StartCoroutine(Emerge());
    }

    IEnumerator Emerge()
    {
        GameObject ins;
        Image myImage = GetComponent<Image>();
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();

        myImage.color = new Color(1f, 1f, 1f, 0f);
        for (; myImage.color.a < 1; myImage.color += new Color(0f, 0f, 0f, 1 / 30f))
        {
            yield return new WaitForSeconds(1 / 30f);
        }

        yield return new WaitForSeconds(0.5f);

        myImage.sprite = frame1;
        pct.volume = 0.35f; pct.Play();

        for (float j = Random.Range(130f, 140f); j > -20;)
        {
            for (int i = 0; i < 2; i++)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3(110 * Mathf.Cos(j * Mathf.PI / 180f), 110 * Mathf.Sin(j * Mathf.PI / 180f)),
                    Random.Range(8f, 9f), j + Random.Range(-3f, 3f), 0.35f, 0.35f, Color.yellow, Color.white);
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3(105 * Mathf.Cos(j * Mathf.PI / 180f), 105 * Mathf.Sin(j * Mathf.PI / 180f)),
                    Random.Range(7f, 8f), j + Random.Range(-3f, 3f), 0.32f, 0.32f, new Color(1f, 0.25f, 0f), Color.white);
                if (GameController.rank >= 3)
                {
                    ins = GetBullet("CIRCLE");
                    ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3(100 * Mathf.Cos(j * Mathf.PI / 180f), 100 * Mathf.Sin(j * Mathf.PI / 180f)),
                        Random.Range(6f, 7f), j + Random.Range(-30f, 30f), 0.3f, 0.3f, new Color(1f, 0.5f, 0f), Color.white);
                }
                if (GameController.rank >= 7)
                {
                    ins = GetBullet("CIRCLE");
                    ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition + new Vector3(95 * Mathf.Cos(j * Mathf.PI / 180f), 100 * Mathf.Sin(j * Mathf.PI / 180f)),
                        Random.Range(5f, 6f), j + Random.Range(-30f, 30f), 0.28f, 0.28f, new Color(1f, 0.75f, 0f), Color.white);
                }

                j -= (80f / (GameController.rank * 0.5f + 1));
            }
            yield return new WaitForSeconds(1 / 120f);
        }

        myImage.sprite = frame2;

        for (; myImage.color.a > 0; myImage.color -= new Color(0f, 0f, 0f, 1 / 30f))
        {
            yield return new WaitForSeconds(1 / 30f);
        }
        Destroy(gameObject);
    }
}
