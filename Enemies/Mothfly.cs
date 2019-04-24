using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mothfly : Enemy {

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
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        GameObject ins;
        cct.Play();

        float randDist = Random.Range(0f, 60f);
        float randDirRad = Random.Range(0f, 2f) * Mathf.PI;

        ins = GetBullet("CIRCLE");
        if (ins != null)
        {         
            ins.GetComponent<Bullet>().SetIdentityEx("MOTHFLY", transform.localPosition + new Vector3(-45f, 0f) + new Vector3(randDist * Mathf.Cos(randDirRad), randDist * Mathf.Sin(randDirRad))
                , 0.5f, Random.Range(0f, 360f), 0.35f, 0.35f, new Color(0.5f, 0f, 1f, 0.5f), new Color(1f, 0f, 1f, 0.5f));
        }

        ins = GetBullet("CIRCLE");
        if (ins != null)
        {
            ins.GetComponent<Bullet>().SetIdentityEx("MOTHFLY", transform.localPosition + new Vector3(45f, 0f) + new Vector3(randDist * Mathf.Cos(randDirRad), randDist * Mathf.Sin(randDirRad))
                , 0.5f, Random.Range(0f, 360f), 0.35f, 0.35f, new Color(0.5f, 0f, 1f, 0.5f), new Color(1f, 0f, 1f, 0.5f));
        }

        yield return new WaitForSeconds(0.7f - 0.063f * GameController.rank);
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
