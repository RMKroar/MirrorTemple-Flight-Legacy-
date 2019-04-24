using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Frog : Enemy {

    public Sprite CastImage;
    public float mov_speed;
    float direction;
    bool flag = false;

    IEnumerator Move()
    {
        for (int i = 0; i <= 5; i++) {
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
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        GameObject ins;

        StopCoroutine(Animate());

        for (int j = 0; j < 3 + GameController.rank / 3; j++)
        {
            gameObject.GetComponent<Image>().sprite = CastImage;
            sct.Play();
            for (int i = 0; i < 6 + GameController.rank * 0.5f; i++)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(3f + j, 8f + j * 3), ToPlayerAngle() + Random.Range(-45f, 45f), 0.35f, 0.35f,
                Color.white, new Color(0, Random.Range(0.5f, 1f), 0f));
            }
            yield return new WaitForSeconds(1 / 4f);
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(Animate());
        yield return new WaitForSeconds(2f);
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
