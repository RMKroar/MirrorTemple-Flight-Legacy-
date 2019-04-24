using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hornet : Enemy {

    public float standing_freq;
    public float standing_magnitude;

    int count = 0;

    IEnumerator Move()
    {
        count++;
        if (count == 1)
        {
            StartCoroutine("Standing");
            GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            transform.localScale = new Vector3(0f, 0f, 1f);
            immortal = true;
            yield return new WaitForSeconds(1 / 60f);
            StartCoroutine("Move");
        }
        else {
            GetComponent<Image>().color += new Color(1/45f, 1 / 45f, 1 / 45f, 1 / 45f);
            transform.localScale += new Vector3(-1 / 45f, 1 / 45f);
            if (transform.localScale.x <= -1) {
                transform.localScale = new Vector3(-1, 1, 1);
                GetComponent<Image>().color = Color.white;
                immortal = false;
                yield return new WaitForSeconds(5f);
                count = 0;
                StartCoroutine("EndMove");
            }
            else {
                yield return new WaitForSeconds(1 / 60f);
                StartCoroutine("Move");
            }
        }
    }

    float mov = 0;
    IEnumerator Standing() {
        mov += standing_freq;
        transform.localPosition += new Vector3(0f, standing_magnitude * Mathf.Sin(mov));
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Standing");
    }

    IEnumerator EndMove()
    {
        count++;
        GetComponent<Image>().color -= new Color(1 / 45f, 1 / 45f, 1 / 45f, 1 / 45f);
        transform.localScale -= new Vector3(-1 / 45f, 1 / 45f);
        yield return new WaitForSeconds(1 / 60f);
        if (count >= 45) Destroy(gameObject);
        StartCoroutine("EndMove");
    }

    IEnumerator Launch() {
        GameObject ins;

        if (GameController.rank <= 0)
        {
            ins = GetBullet("THORN");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_HORNET", transform.localPosition, 2f, ToPlayerAngle(),
                0.4f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);

            yield return null;
        }
        else if (GameController.rank <= 3)
        {
            ins = GetBullet("THORN");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_HORNET", transform.localPosition, 2f, ToPlayerAngle(),
                0.4f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);

            yield return new WaitForSeconds(1f);
            StartCoroutine("Launch");
        }
        else if (GameController.rank <= 7)
        {
            ins = GetBullet("THORN");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_HORNET", transform.localPosition, 2f, ToPlayerAngle(),
                0.4f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);
            ins = GetBullet("THORN");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_HORNET", transform.localPosition, 2f, ToPlayerAngle() + 20f,
                0.4f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);
            ins = GetBullet("THORN");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_HORNET", transform.localPosition, 2f, ToPlayerAngle() - 20f,
                0.4f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);

            yield return new WaitForSeconds(1.2f);
            StartCoroutine("Launch");
        }
        else
        {
            ins = GetBullet("THORN");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_HORNET", transform.localPosition, 2f, ToPlayerAngle(),
                0.4f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);
            ins = GetBullet("THORN");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_HORNET", transform.localPosition, 2f, ToPlayerAngle() + 20f,
                0.4f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);
            ins = GetBullet("THORN");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_HORNET", transform.localPosition, 2f, ToPlayerAngle() - 20f,
                0.4f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);

            yield return new WaitForSeconds(1f);
            StartCoroutine("Launch");
        }
    }
}
