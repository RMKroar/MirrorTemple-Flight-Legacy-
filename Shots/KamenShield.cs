using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KamenShield : MonoBehaviour {

    public GameObject ShieldPanel;
    GameObject Player;
    Player PlayerData;
    Image myImage;
    int charge;
    float capacity;
    GameObject _panel;

    public GameObject Slash;

    public void Initialize()
    {
        Player = transform.parent.gameObject;
        PlayerData = Player.GetComponent<Player>();
        myImage = GetComponent<Image>();
        transform.SetParent(GameObject.Find("TriggerPanel").transform);
        myImage.color = new Color(1f, 1f, 1f, 0.5f);
        charge = 0;
        capacity = 0;
        _panel = Instantiate(ShieldPanel, transform);
        _panel.transform.localPosition = new Vector3(20, 80);
        StartCoroutine("Charge");
    }

    IEnumerator Charge() {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            charge++;
            if (charge >= PlayerData.subShotDelay)
            {
                myImage.color = new Color(1f, 0.5f, 0f, myImage.color.a);
            }

            transform.localPosition = Player.transform.localPosition + new Vector3(40f, 0f, 0f);

            if (myImage.color.a < 1)
            {
                capacity += (0.3f * 110f / PlayerData.subShotDelay);
                if (capacity >= 30) myImage.color += new Color(0f, 0f, 0f, 0.5f);
            }
            else if (capacity <= 0) {
                capacity = 0;
                myImage.color -= new Color(0f, 0f, 0f, 0.5f);
            }
            _panel.transform.GetChild(1).gameObject.GetComponent<Text>().text = ((int)capacity).ToString() + " / 30";
        }
        else {
            Unable();
        }

        yield return new WaitForSeconds(1 / 120f);
        StartCoroutine("Charge");
    }

    public void Unable() {
        StopCoroutine("Charge");
        if (myImage.color.b <= 0) {
            GameObject ins = Instantiate(Slash, GameObject.Find("TriggerPanel").transform);
            if (ins != null)
            {
                if (PlayerData.mars)
                {
                    ins.GetComponent<Shot>().SetIdentity(Player.transform.localPosition, 0.01f, 60f, 1f, 1f, PlayerData.subShotDamage * 2);
                    ins.GetComponent<Shot>().additionalCode = "MARS";
                }
                else {
                    ins.GetComponent<Shot>().SetIdentity(Player.transform.localPosition, 0.01f, 60f, 1f, 1f, PlayerData.subShotDamage);
                }
            }
        }
        transform.SetParent(Player.transform);
        Destroy(_panel);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (myImage.color.a >= 1)
        {
            if (collision.tag == "Bullet" && !"DEBRIS".Equals(collision.gameObject.GetComponent<Bullet>().moveCode))
            {
                collision.gameObject.GetComponent<Bullet>().UnableWithDebris();
                capacity -= 1;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (myImage.color.a >= 1)
        {
            if (collision.tag == "Bullet" && !"DEBRIS".Equals(collision.gameObject.GetComponent<Bullet>().moveCode))
            {
                collision.gameObject.GetComponent<Bullet>().UnableWithDebris();
                capacity -= 1;
            }
        }
    }
}
