using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum Direction
    {
        LEFT,
        UP,
        DOWN,
        RIGHT
    }

    [SerializeField] private Direction moveDirection;

    [SerializeField] private float delayStep; // tempo entre um passo e outro
    [SerializeField] private float step; // quantidade de movimentoa cada passo
    [SerializeField] private Transform head; // transform de cabeça da cobra

    [SerializeField] private List<Transform> tails;
    [SerializeField] private Transform food;
    [SerializeField] private GameObject tailPrefab;

    [SerializeField] private int col = 29;
    [SerializeField] private int rows = 15;

    [Header("UI")]
    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtHiScore;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelTitle;

    private int score;
    private int hiScore;

    private Vector3 lastPos;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(MoveSnake());
        SetFood();
        hiScore = PlayerPrefs.GetInt("hiScore");
        txtHiScore.text = "Hi-Score: " + hiScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        KeyPressed(); 
    }

    void KeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDirection = Direction.UP;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection = Direction.LEFT;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection = Direction.DOWN;

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection = Direction.RIGHT;
        }
    }

    IEnumerator MoveSnake()
    {
        yield return new WaitForSeconds(delayStep);
        Vector3 nextPosition = Vector3.zero;

        switch (moveDirection)
        {
            case Direction.DOWN:
                nextPosition = Vector3.down;
                head.rotation = Quaternion.Euler(0, 0, 90);
                break;

            case Direction.LEFT:
                nextPosition = Vector3.left;
                head.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.RIGHT:
                nextPosition = Vector3.right;
                head.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.UP:
                nextPosition = Vector3.up;
                head.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }

        nextPosition *= step;
        lastPos = head.position;
        head.position += nextPosition;

        foreach (Transform tail in tails)
        {
            Vector3 temp = tail.position;
            tail.position = lastPos;
            lastPos = temp;
            tail.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        StartCoroutine(MoveSnake());
    }


    public void Eat()
    {
        Vector3 tailPosition = head.position;

        if(tails.Count > 0)
        {
            tailPosition = tails[tails.Count - 1].position;
        }

        GameObject temp = Instantiate(tailPrefab, tailPosition, transform.localRotation);

        tails.Add(temp.transform);
        score += 10;
        txtScore.text = "Score: " + score.ToString();
        SetFood();
    }


    public void SetFood()
    {
        int x = Random.Range((col - 1) / 2 * -1, (col - 1) / 2);
        int y = Random.Range((rows - 1) / 2 * -1, (rows - 1) / 2);

        food.position = new Vector2(x * step, y * step);
    }

    public void GameOver()
    {
        if (score > hiScore)
        {
            PlayerPrefs.SetInt("hiScore", score);
            txtHiScore.text = "New Hi-Score: " + score.ToString(); 
        }
        head.gameObject.SetActive(false);
        panelGameOver.SetActive(true);
        Time.timeScale = 0;
    }

    public void Jogar()
    {
        Time.timeScale = 1;
        head.gameObject.SetActive(true);
        head.position = Vector3.zero;
        moveDirection = Direction.LEFT;

        foreach (Transform t in tails)
        {
            Destroy(t.gameObject);
        }

        tails.Clear();
        head.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        SetFood();
        score = 0;
        txtScore.text = "Score: " + score;
        hiScore = PlayerPrefs.GetInt("hiScore");
        txtHiScore.text = "Hi-Score: " + hiScore.ToString();
        panelGameOver.SetActive(false);
        panelTitle.SetActive(false);
    }
}
