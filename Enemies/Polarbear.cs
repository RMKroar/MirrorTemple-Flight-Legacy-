using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polarbear : Enemy {

    public float mov_speed;
    public GameObject snowRevolution;

    IEnumerator Move()
    {
        transform.localPosition += new Vector3(-mov_speed, 0);
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.35f;  pct.Play();

        GameObject ins = Instantiate(snowRevolution, GameObject.Find("TriggerPanel").transform);
        if (ins != null) {
            ins.transform.localPosition = transform.localPosition;
            ins.GetComponent<SnowRevolution>().SetIdentity(Random.Range(5, 7), ToPlayerAngle());
        }       
        yield return new WaitForSeconds(3f - GameController.rank * 0.1f);
        ins = Instantiate(snowRevolution, GameObject.Find("TriggerPanel").transform);
        if (ins != null)
        {
            ins.transform.localPosition = transform.localPosition;
            ins.GetComponent<SnowRevolution>().SetIdentity(Random.Range(5, 7), ToPlayerAngle());
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
