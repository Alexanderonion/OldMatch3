using System;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using System.Linq;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Net;

public class InteractionWithToken : MonoBehaviour
{
    private Board _boardScript;
    public static Action _touchedToken;
    public static Action _insideFirstTokenInChainAfterBonus;
    public static Action _untouchedObject;
    private LevelManager _levelManager;

    private void OnEnable()
    {
        _touchedToken += TokensShadowOn;
        _untouchedObject += TokensShadowOff;
        _insideFirstTokenInChainAfterBonus += TokensShadowOn;
    }

    private void OnDestroy()
    {
        _touchedToken -= TokensShadowOn;
        _untouchedObject -= TokensShadowOff;
        _insideFirstTokenInChainAfterBonus -= TokensShadowOn;
    }

    private void Start()
    {
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        _boardScript = GameObject.Find("Field").GetComponent<Board>();
    }

    private void OnMouseDown()
    {
        if (LevelManager._stopInteraction) { return; }

        Board._chainTokenType = gameObject.GetComponent<Token>().TokenType;
        Board._destroyObjectInPositionAfterMatch.Add(gameObject.GetComponent<Token>().Position);
        _touchedToken?.Invoke();
        Board._lineBetweenToken.SetPosition(Board._destroyObjectInPositionAfterMatch.Count - 1, Board._lineBetweenToken.gameObject.transform.InverseTransformPoint(gameObject.transform.position));
    }

    private void OnMouseEnter()
    {
        if (LevelManager._stopInteraction) { return; }
        if (Board._destroyObjectInPositionAfterMatch.Count == 0) { return; }

        if (_boardScript.IfTokenInChain(Board._destroyObjectInPositionAfterMatch) == false && Input.GetMouseButton(0))
        {
            Board._chainTokenType = gameObject.GetComponent<Token>().TokenType;
            Board._destroyObjectInPositionAfterMatch.Add(gameObject.GetComponent<Token>().Position);
            Board._lineBetweenToken.positionCount++;
            Board._lineBetweenToken.SetPosition(Board._destroyObjectInPositionAfterMatch.Count - 1, Board._lineBetweenToken.gameObject.transform.InverseTransformPoint(gameObject.transform.position));
            _insideFirstTokenInChainAfterBonus?.Invoke();
        }
        else if (Board._chainTokenType == gameObject.GetComponent<Token>().TokenType && Input.GetMouseButton(0))
        {
            if (Board._destroyObjectInPositionAfterMatch.Count > 1 && gameObject.GetComponent<Token>().Position == Board._destroyObjectInPositionAfterMatch[^2])
            {
                Board._destroyObjectInPositionAfterMatch.RemoveAt(Board._destroyObjectInPositionAfterMatch.Count - 1);

                if (Board._lineBetweenToken.positionCount > 1)
                {
                    Board._lineBetweenToken.positionCount--;
                }

            }
            else if (!Board._destroyObjectInPositionAfterMatch.Contains(gameObject.GetComponent<Token>().Position) && NearByToken(Board._destroyObjectInPositionAfterMatch[^1], gameObject.GetComponent<Token>().Position))
            {
                Board._destroyObjectInPositionAfterMatch.Add(gameObject.GetComponent<Token>().Position);
                Board._lineBetweenToken.positionCount++;
                Board._lineBetweenToken.SetPosition(Board._destroyObjectInPositionAfterMatch.Count - 1, Board._lineBetweenToken.gameObject.transform.InverseTransformPoint(gameObject.transform.position));
            }
        }
    }

    private bool NearByToken(Coordinate objectPosition, Coordinate comparableObjectPosition)
    {
        if (objectPosition.Row == comparableObjectPosition.Row && objectPosition.Column == comparableObjectPosition.Column) { return false; }

        if (Math.Abs(objectPosition.Row - comparableObjectPosition.Row) < 2 && Math.Abs(objectPosition.Column - comparableObjectPosition.Column) < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnMouseUp()
    {
        _untouchedObject?.Invoke();

        if (Board._destroyObjectInPositionAfterMatch.Count < 3 && Board._destroyObjectInPositionAfterMatch.OfType<Bonus>().Any() == false)
        {
            Board._destroyObjectInPositionAfterMatch.Clear();
            Board._lineBetweenToken.positionCount = 1;
        }
        else
        {
            Board._lineBetweenToken.positionCount = 1;
            LevelManager._stopInteraction = true;
            _boardScript.RemoveAnObjectFromTheListOfDestroyObjects();
            _boardScript.MoveField();
        }
    }

    public void TokensShadowOn()
    {
        if (Board._chainTokenType != gameObject.GetComponent<Token>().TokenType)
        {
            gameObject.transform.Find("Shade").gameObject.SetActive(true);
        }
    }

    public void TokensShadowOff()
    {
        if (Board._chainTokenType != gameObject.GetComponent<Token>().TokenType)
        {
            gameObject.transform.Find("Shade").gameObject.SetActive(false);
        }
    }
}