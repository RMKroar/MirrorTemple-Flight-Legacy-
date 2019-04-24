using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spirit : Enemy {

    public float retreatSpeed;
    public float emergeSpeed;

    float emerge = 0;
    float density;
    float launchDir;
    float launchDir2;
    bool flag = false;

    IEnumerator Move()
    {
        if (!flag) {
            density = 360f / (3 + GameController.rank);
            launchDir = Random.Range(0, 360f / (2 + GameController.rank));
            launchDir2 = launchDir;
        }
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

    IEnumerator EndMove() {
        transform.localPosition += new Vector3(retreatSpeed, 0f, 0f);
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("EndMove");
    }

    IEnumerator Launch()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.4f; pct.Play();

        for (float i = launchDir + 120; i < 240 + launchDir; i += density) {
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_SPLIT", transform.localPosition, 4f, i,
                0.4f, 0.4f, Color.white, BulletCode.COLOR_VIOLET);
        }
        for (float i = launchDir2 + 120; i < 240 + launchDir2; i += density)
        {
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_SPLIT", transform.localPosition, 4f, i,
                0.4f, 0.4f, Color.white, BulletCode.COLOR_VIOLET);
        }
        if (GameController.rank <= 2) yield return new WaitForSeconds(1.25f);
        else if (GameController.rank <= 5) yield return new WaitForSeconds(0.9f);
        else if (GameController.rank <= 8) yield return new WaitForSeconds(0.7f);
        else yield return new WaitForSeconds(0.3f);

        launchDir += density / 9.1f;
        launchDir2 -= density / 9.1f;
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
