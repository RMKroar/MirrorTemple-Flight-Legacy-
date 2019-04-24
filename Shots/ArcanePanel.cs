using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcanePanel : MonoBehaviour {

    public GameObject Fire;
    public GameObject Water;
    public GameObject RecentPanel;
    RectTransform RecentPanelTr;
    public bool enacra = false;

    GameObject Player;
    float chargeSpeed;
    float charge = 0;

    public void Start()
    {
        chargeSpeed = 1.5f; // Charge Max = 100
        Player = transform.parent.gameObject;
        RecentPanelTr = RecentPanel.GetComponent<RectTransform>();
        StartCoroutine(Working());
    }

    IEnumerator Working() {
        GameObject ins;
        Image ImgRecent = RecentPanel.GetComponent<Image>();
        Player playerData = Player.GetComponent<Player>();
        while (true) {
            if (Input.GetKey(KeyCode.Z)) {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    charge = (charge < 100) ? charge + chargeSpeed * (10 / playerData.subShotDelay) : 100;
                }
                else
                {
                    charge = (charge < 100) ? charge + chargeSpeed * (10 / playerData.mainShotDelay) : 100;
                }
            }
            else if (charge >= 50)
            {              
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (charge >= 100) {
                        int temp_cnt = 0;
                        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                        {
                            temp_cnt++;
                            ins = Instantiate(Water, GameObject.Find("TriggerPanel").transform);
                            if (ins != null)
                            {
                                if(enacra) ins.GetComponent<Shot>().SetIdentity(enemy.transform.localPosition, 0.01f, 0f, 2f, 2f, playerData.subShotDamage / 3);
                                else ins.GetComponent<Shot>().SetIdentity(enemy.transform.localPosition, 0.01f, 0f, 1f, 1f, playerData.subShotDamage / 3);
                            }
                        }

                        if (temp_cnt > 0) GetComponent<AudioSource>().Play();
                    }
                    
                }
                else {
                    StartCoroutine(FireBlast(((int)charge - 40) / 10));
                }
                charge = 0;
            }

            RecentPanelTr.sizeDelta = new Vector2(charge, 15);
            if (charge == 100)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    ImgRecent.color = BulletCode.COLOR_SKY;
                }
                else
                {
                    ImgRecent.color = Color.red;
                }
            }
            else if (charge >= 50)
            {
                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                {
                    ImgRecent.color = new Color(1f, 0.5f, 0f);
                }
                else ImgRecent.color = Color.white;
            }
            else ImgRecent.color = Color.white;

            yield return new WaitForSeconds(1 / 60f);
        }
    }

    IEnumerator FireBlast(int count) {
        Player playerData = Player.GetComponent<Player>();
        Vector3 savedPos = Player.transform.localPosition;
        GameObject ins;

        for (int i = 200; i <= 200 * count; i += 200) {
            ins = Instantiate(Fire, GameObject.Find("TriggerPanel").transform);
            if (ins != null)
            {
                if(enacra) ins.GetComponent<Shot>().SetIdentity(savedPos + new Vector3(i, 0), 0.01f, 0f, 2f, 2f, playerData.mainShotDamage / 4);
                else ins.GetComponent<Shot>().SetIdentity(savedPos + new Vector3(i, 0), 0.01f, 0f, 1f, 1f, playerData.mainShotDamage / 4);
            }

            yield return new WaitForSeconds(1 / 4f);
        }
    }
}
