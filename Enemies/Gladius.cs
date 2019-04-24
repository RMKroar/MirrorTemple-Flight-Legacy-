using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gladius : Enemy {
    public Sprite frame1;
    public Sprite frame2;

	// Ignore initial setting in 'Enemy' script
	void Start () {
        StartCoroutine(Emerge());	
	}

    IEnumerator Emerge() {
        GameObject ins;
        Image myImage = GetComponent<Image>();
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();

        myImage.color = new Color(1f, 1f, 1f, 0f);
        for (; myImage.color.a < 1; myImage.color += new Color(0f, 0f, 0f, 1 / 30f)) {
            yield return new WaitForSeconds(1 / 30f);
        }

        yield return new WaitForSeconds(0.5f);

        myImage.sprite = frame1;
        pct.volume = 0.35f; pct.Play();

        for (float j = Random.Range(40f, 50f); j < 200; )
        {
            for (int i = 0; i < 3; i++) {
                ins = GetBullet("THORN");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(110 * Mathf.Cos(j * Mathf.PI / 180f), 110 * Mathf.Sin(j * Mathf.PI / 180f)),
                    Random.Range(12f, 15f), j + Random.Range(-3f, 3f), 0.4f, 0.4f, Color.white, Color.blue);
                ins = GetBullet("THORN");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(105 * Mathf.Cos(j * Mathf.PI / 180f), 105 * Mathf.Sin(j * Mathf.PI / 180f)),
                    Random.Range(9f, 12f), j + Random.Range(-3f, 3f), 0.35f, 0.35f, Color.white, new Color(0f, 0.5f, 1f));
                if (GameController.rank >= 3)
                {
                    ins = GetBullet("THORN");
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(100 * Mathf.Cos(j * Mathf.PI / 180f), 105 * Mathf.Sin(j * Mathf.PI / 180f)),
                        Random.Range(7f, 9f), j + Random.Range(-3f, 3f), 0.35f, 0.35f, Color.white, new Color(0f, 1f, 1f));
                }
                if (GameController.rank >= 6)
                {
                    ins = GetBullet("THORN");
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(95 * Mathf.Cos(j * Mathf.PI / 180f), 100 * Mathf.Sin(j * Mathf.PI / 180f)),
                        Random.Range(6f, 8f), j + Random.Range(-30f, 30f), 0.3f, 0.3f, Color.white, new Color(0f, 1f, 0.5f));
                }
                if (GameController.rank >= 8)
                {
                    ins = GetBullet("THORN");
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(90 * Mathf.Cos(j * Mathf.PI / 180f), 100 * Mathf.Sin(j * Mathf.PI / 180f)),
                        Random.Range(5f, 7f), j + Random.Range(-30f, 30f), 0.3f, 0.3f, Color.white, Color.green);
                }

                j += (48f / (GameController.rank * 0.5f + 1));
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
