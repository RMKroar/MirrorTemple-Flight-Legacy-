using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGenerator : MonoBehaviour {

    public GameObject poison;

    Player player;   
    // Use this for initialization
    void Start() {
        player = GetPlayer().GetComponent<Player>();
        StartCoroutine(MakePoison());
    }

    IEnumerator MakePoison() {
        GameObject ins;
        float delay = ("Nemen".Equals(player.ID))? player.subShotDelay : 2f;
        float damage = player.subShotDamage / 10f;

        while (true) {
            if (transform.parent == null) Destroy(gameObject);

            ins = Instantiate(poison, GameObject.Find("TriggerPanel").transform);
            ins.GetComponent<Shot>().SetIdentity(transform.parent.localPosition, 0.01f, 0f, 1f, 1f, damage);
           
            yield return new WaitForSeconds(delay);
        }       
    }

    private GameObject GetPlayer()
    {
        GameObject returnObj = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            returnObj = obj;
        }

        return returnObj;
    }
}
