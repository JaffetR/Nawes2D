using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO;

    public GameObject PlayerBulletGO;
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject ExplosionGO;

    public Text LivesUItext;

    const int MaxLives = 3;
    int lives;

    public float speed;

    public void Init()
    {
        lives = MaxLives;

        LivesUItext.text=lives.ToString();

        transform.position = new Vector2(0, 0);

        gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //aqui para disparar
        if (Input.GetKeyDown("space"))
        {

            GetComponent<AudioSource>().Play();
            //primera bala
            GameObject bullet01 = (GameObject)Instantiate(PlayerBulletGO);
            bullet01.transform.position = bulletPosition01.transform.position;

            GameObject bullet02 = (GameObject)Instantiate(PlayerBulletGO);
            bullet02.transform.position = bulletPosition02.transform.position;
        }


        float x = Input.GetAxisRaw("Horizontal");//el valor va de -1,0 o 1 izqui, centro, derecha
        float y = Input.GetAxisRaw("Vertical");// el valor va de -1 0 o 1 arriba medio o abajo

        //ahora la base del input de la computadora con el vector2(2D)
        Vector2 direction = new Vector2(x, y).normalized;

        //ahora aqui llamamos las funciones de la posicion del player

        Move(direction);
    }

    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));//aqui usamos los botones de la pantalla
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));//aqui de arriba abajo

        max.x = max.x - 0.225f; //half width
        min.x = min.x + 0.225f;//half height

        max.y = max.y - 0.285f; //half width
        min.y = min.y + 0.285f;//half height

        Vector2 pos = transform.position;

        pos += direction * speed * Time.deltaTime;

        //make sure su nueva position
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if((col.tag=="EnemyShipTag") || (col.tag=="EnemyBulletTag"))
        {
            PlayExplosion();
            lives--;
            LivesUItext.text = lives.ToString();

            if(lives ==0)
            {
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);

                gameObject.SetActive(false);
            }

            //Destroy(gameObject);
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExplosionGO);

        explosion.transform.position = transform.position;
    }

}
