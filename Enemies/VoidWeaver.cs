using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoidWeaver : Enemy {

    public float standing_freq;
    public float standing_magnitude;

    int count = 0;

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
        count++;
        transform.localPosition += new Vector3(2 + count * 0.05f, 0f);
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("EndMove");
    }

    IEnumerator Launch()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        if (GameController.rank <= 2) {
            ins = GetBullet("CIRCLE");
            if (ins != null) {
                ins.GetComponent<Bullet>().SetIdentityEx("VOID", transform.localPosition, 2f, ToPlayerAngle(), 0.45f, 0.45f,
                    Color.black, Color.black);
            }
        }
        else if (GameController.rank <= 6) {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("VOID", transform.localPosition, 2f, ToPlayerAngle() + 20f, 0.45f, 0.45f,
                    Color.black, Color.black);
            }

            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("VOID", transform.localPosition, 2f, ToPlayerAngle() - 20f, 0.45f, 0.45f,
                    Color.black, Color.black);
            }
        }
        else {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("VOID", transform.localPosition, 2f, ToPlayerAngle(), 0.45f, 0.45f,
                    Color.black, Color.black);
            }

            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("VOID", transform.localPosition, 2f, ToPlayerAngle() - 30f, 0.45f, 0.45f,
                    Color.black, Color.black);
            }

            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("VOID", transform.localPosition, 2f, ToPlayerAngle() + 30f, 0.45f, 0.45f,
                    Color.black, Color.black);
            }
        }
        yield return null; // no more shooting
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
