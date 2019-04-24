using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magician : Enemy {

    public float emergeSpeed;
    float emerge = 0;

    public GameObject HexA;
    public GameObject HexB;
    public GameObject HexSummon;

    IEnumerator Move()
    {
        emerge += emergeSpeed;
        if (emerge > 1)
        {
            gameObject.GetComponent<Image>().color = Color.white;
            yield return null;
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, emerge);
            yield return new WaitForSeconds(1 / 60f);
            StartCoroutine("Move");
        }
    }

    IEnumerator Launch()
    {
        AudioSource mct = GameObject.Find("Sound_EnemyMagic").GetComponent<AudioSource>();
        mct.volume = 0.7f; mct.Play();

        float rand = Random.Range(30f, 150f);
        float dir = Random.Range(0f, 360f);
        GameObject ins = Instantiate(HexA, GameObject.Find("TriggerPanel").transform);
        ins.transform.localPosition = transform.localPosition + new Vector3(rand * Mathf.Cos(dir * Mathf.PI / 180f), rand * Mathf.Sin(dir * Mathf.PI / 180f));
        ins.GetComponent<Hexagram>().SetIdentity(1, Random.Range(0f, 360f));

        yield return new WaitForSeconds(3f - GameController.rank * 0.1f);
        mct.volume = 0.7f; mct.Play();

        rand = Random.Range(30f, 150f);
        dir = Random.Range(0f, 360f);
        ins = Instantiate(HexB, GameObject.Find("TriggerPanel").transform);
        ins.transform.localPosition = transform.localPosition + new Vector3(rand * Mathf.Cos(dir * Mathf.PI / 180f), rand * Mathf.Sin(dir * Mathf.PI / 180f));
        ins.GetComponent<Hexagram>().SetIdentity(1, Random.Range(0f, 360f));

        yield return new WaitForSeconds(3f - GameController.rank * 0.1f);

        StartCoroutine("Launch");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (transform.localPosition.x <= 660) {
            GameObject ins = Instantiate(HexSummon, GameObject.Find("TriggerPanel").transform);
            ins.transform.localPosition = transform.localPosition;
            ins.GetComponent<Hexagram>().SetIdentity(0.7f, 0);

            Collapse();
        }
    }
}
