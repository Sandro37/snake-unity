using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class head : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Food":
                _gameController.Eat();
                break;
            case "Tail":
                _gameController.GameOver();
                break;
        }
    }
}
