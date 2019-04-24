using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItem : MonoBehaviour {

    public int moveCode;
    GameObject player;

    private void Start() {
        StartCoroutine(Move());
    }

    public void SetPlayer(GameObject target) {
        player = target;
    }

    IEnumerator Move() {
        float rad = 0; int count = 0; GameObject ins;
        while (true) {
            if (moveCode == 0)
            { // Scorpion
                transform.localPosition = player.transform.localPosition + new Vector3(-60f, 0f);
            }
            else if (moveCode == 1)
            { // Iceflake
                rad += 0.026f; // 1.5 * 3.14 / 180
                transform.localPosition = player.transform.localPosition + new Vector3(100f * Mathf.Cos(rad), 100f * Mathf.Sin(rad));
            }
            else if (moveCode == 2)
            { // Greenmaple
                rad += 0.026f; // 1.5 * 3.14 / 180
                transform.localPosition += new Vector3(0, 2 * Mathf.Cos(rad));
                if (count >= 360)
                {
                    ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[6], GameObject.Find("TriggerPanel").transform);
                    ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 0.01f, 0, 1f, 1f, player.GetComponent<Player>().mainShotDamage / 2);
                    Destroy(gameObject);
                }
            }
            else if (moveCode == 3) { // Brickstone
                if (count >= 240)
                {
                    Destroy(gameObject);
                }
            }
            yield return new WaitForSeconds(1 / 120f);
            count++;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            collision.gameObject.GetComponent<Bullet>().UnableWithDebris();
            if (moveCode == 3) Destroy(gameObject);
        }
    }
}
