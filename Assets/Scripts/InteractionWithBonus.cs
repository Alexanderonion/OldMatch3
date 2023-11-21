using UnityEngine;
using System;
using System.Linq;
using TMPro;

public class InteractionWithBonus : MonoBehaviour
{
    [SerializeField] private Board _boardScript;
    public static Action _untouchedObject;
    private LevelManager _levelManager;

    private void Start()
    {
        _boardScript = GameObject.Find("Field").GetComponent<Board>();
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void OnMouseDown()
    {
        Board._destroyObjectInPositionAfterMatch.Add(gameObject.GetComponent<Bonus>().Position);
        Board._lineBetweenToken.SetPosition(Board._destroyObjectInPositionAfterMatch.Count - 1, Board._lineBetweenToken.gameObject.transform.InverseTransformPoint(gameObject.transform.position));
    }

    private void OnMouseEnter()
    {
        if (Board._destroyObjectInPositionAfterMatch.Count != 0 && Input.GetMouseButton(0))
        {
            if (Board._destroyObjectInPositionAfterMatch.Count > 1 && gameObject.GetComponent<Bonus>().Position == Board._destroyObjectInPositionAfterMatch[^2])
            {
                Board._destroyObjectInPositionAfterMatch.RemoveAt(Board._destroyObjectInPositionAfterMatch.Count - 1);

                if (Board._lineBetweenToken.positionCount > 0)
                {
                    Board._lineBetweenToken.positionCount--;
                }
                
                if (_boardScript.IfTokenInChain(Board._destroyObjectInPositionAfterMatch) == false)
                {
                    InteractionWithToken._untouchedObject?.Invoke();
                }
            }
            else if (NearByToken(Board._destroyObjectInPositionAfterMatch[^1], gameObject.GetComponent<Bonus>().Position) && !Board._destroyObjectInPositionAfterMatch.Contains(gameObject.GetComponent<Bonus>().Position))
            {
                Board._destroyObjectInPositionAfterMatch.Add(gameObject.GetComponent<Bonus>().Position);
                Board._lineBetweenToken.positionCount++;
                Board._lineBetweenToken.SetPosition(Board._destroyObjectInPositionAfterMatch.Count - 1, Board._lineBetweenToken.gameObject.transform.InverseTransformPoint(gameObject.transform.position));
            }
        }
    }

    private void OnMouseUp()
    {
        Board._lineBetweenToken.positionCount = 1;
        InteractionWithToken._untouchedObject?.Invoke();
        _boardScript.RemoveAnObjectFromTheListOfDestroyObjects();
        _boardScript.MoveField();
        Board._destroyObjectInPositionAfterMatch.Clear();
        _levelManager.IncrementActionsCount();
    }

    private bool NearByToken(Coordinate firstPosition, Coordinate secondPosition)
    {
        if (firstPosition.Row == secondPosition.Row && firstPosition.Column == secondPosition.Column)
        {
            return false;
        }

        if (firstPosition.Row - secondPosition.Row < 2 && firstPosition.Column - secondPosition.Column < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}