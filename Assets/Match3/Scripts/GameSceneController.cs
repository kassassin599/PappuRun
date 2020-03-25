using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    public int boardWidth = 6;
    public int boardheight = 6;
    public float pieceSpacing = 1.4f;

    public Camera gameCamera;
    public Transform levelContainer;

    public GameObject piecePrefab;

    int score;
    bool gameover;

    Piece[,] board;
    Piece selectedPiece;

    private void Start()
    {
        BuildBoard();
    }

    private void Update()
    {
        ProcessInput();
    }

    void BuildBoard()
    {
        board = new Piece[boardWidth, boardheight];

        for (int y = 0; y < boardheight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                GameObject pieceObject = Instantiate(piecePrefab);
                pieceObject.transform.SetParent(levelContainer);
                pieceObject.transform.localPosition = new Vector3(
                    (-boardWidth*pieceSpacing)/2f+(pieceSpacing/2f)+x*pieceSpacing,
                    (-boardheight*pieceSpacing)/2f+(pieceSpacing/2f)+y*pieceSpacing,
                    0
                    );

                Piece piece = pieceObject.GetComponent<Piece>();
                piece.coordinates = new Vector2(x, y);

                board[x, y] = piece;
            }
        }
    }

    void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (hitCollider!=null && hitCollider.gameObject.GetComponent<Piece>()!=null)
            {
                Piece hitPiece = hitCollider.gameObject.GetComponent<Piece>();

                if (selectedPiece == null)
                {
                    selectedPiece = hitPiece;

                    iTween.ScaleTo(selectedPiece.gameObject, iTween.Hash(
                        "scale", Vector3.one/3f,
                        "time", 0.2f
                        ));
                }
                else
                {
                    if (hitPiece == selectedPiece || hitPiece.IsNeighbour(selectedPiece) == false)
                    {
                        iTween.ScaleTo(selectedPiece.gameObject, iTween.Hash(
                            "scale", Vector3.one / 2f,
                            "time",0.2f
                            ));
                    }
                    else if (hitPiece.IsNeighbour(selectedPiece))
                    {
                        AttemptMatch(selectedPiece, hitPiece);
                    }

                    selectedPiece = null;
                }
            }
        }
    }

    void AttemptMatch(Piece piece1, Piece piece2)
    {
        StartCoroutine(AttemptMatchRoutine(piece1, piece2));
    }

    IEnumerator AttemptMatchRoutine(Piece piece1, Piece piece2)
    {
        iTween.Stop(piece1.gameObject);
        iTween.Stop(piece2.gameObject);

        piece1.transform.localScale = Vector3.one / 2f;
        piece2.transform.localScale = Vector3.one / 2f;

        Vector2 coordinates1 = piece1.coordinates;
        Vector2 coordinates2 = piece2.coordinates;

        Vector3 position1 = piece1.transform.position;
        Vector3 position2 = piece2.transform.position;

        iTween.MoveTo(piece1.gameObject, iTween.Hash(
            "position", position2,
            "time", 0.5f
            ));

        iTween.MoveTo(piece2.gameObject, iTween.Hash(
            "position", position1,
            "time", 0.5f
            ));

        piece1.coordinates = coordinates2;
        piece2.coordinates = coordinates1;

        board[(int)piece1.coordinates.x, (int)piece2.coordinates.y] = piece1;
        board[(int)piece2.coordinates.x, (int)piece2.coordinates.y] = piece2;

        yield return new WaitForSeconds(0.5f);

        List<Piece> matchingPiece = CheckMatch(piece1);
        if (matchingPiece.Count == 0)
        {
            matchingPiece = CheckMatch(piece2);
        }

        if (matchingPiece.Count<3)
        {
            iTween.MoveTo(piece1.gameObject, iTween.Hash(
                    "position",position1,
                    "time",0.3f
                ));

            iTween.MoveTo(piece2.gameObject, iTween.Hash(
                    "position",position2,
                    "time",0.3f
                ));

            piece1.coordinates = coordinates1;
            piece2.coordinates = coordinates2;

            board[(int)piece1.coordinates.x, (int)piece2.coordinates.y] = piece1;
            board[(int)piece2.coordinates.x, (int)piece2.coordinates.y] = piece2;

            yield return new WaitForSeconds(1.0f);

            CheckGameOver();
        }
        else
        {
            foreach (Piece piece in matchingPiece)
            {
                piece.destroyed = true;

                score += 100;

                iTween.ScaleTo(piece.gameObject, iTween.Hash(
                        "scale",Vector3.zero,
                        "time",0.3f
                    ));
            }

            yield return new WaitForSeconds(0.3f);

            DropPieces();
            AddPieces();

            yield return new WaitForSeconds(1.0f);

            CheckGameOver();
        }
    }

    private List<Piece> CheckMatch(Piece piece)
    {
        List<Piece> matchingNeighbours = new List<Piece>();

        int x = 0;
        int y = (int)piece.coordinates.y;
        bool reachedPiece = false;

        while (x<boardWidth)
        {
            if (board[x,y].destroyed == false && board[x,y].index == piece.index)
            {
                matchingNeighbours.Add(board[x, y]);

                if (board[x,y] == piece)
                {
                    reachedPiece = true;
                }
            }
            else
            {
                if (reachedPiece == false)
                {
                    //Didn't reach the matching piece
                    matchingNeighbours.Clear();
                }
                else if (matchingNeighbours.Count>=3)
                {
                    //rEACHED A GOOD MATCH
                    return matchingNeighbours;
                }
                else
                {
                    //reachedPiece matching piece but got few pieces
                    matchingNeighbours.Clear();
                }
            }

            x++;
        }

        if (matchingNeighbours.Count>=3)
        {
            return matchingNeighbours;
        }

        //Vertical matches

        x = (int)piece.coordinates.x;
        y = 0;
        reachedPiece = false;
        matchingNeighbours.Clear();

        while (y<boardheight)
        {
            if (board[x,y].destroyed == false && board[x,y].index == piece.index)
            {
                matchingNeighbours.Add(board[x, y]);
                if (board[x,y] == piece)
                {
                    reachedPiece = true;
                }
            }
            else
            {
                if (reachedPiece == false)
                {
                    matchingNeighbours.Clear();
                }
                else if (matchingNeighbours.Count >=3)
                {
                    return matchingNeighbours;
                }
                else
                {
                    matchingNeighbours.Clear();
                }
            }

            y++;
        }

        return matchingNeighbours;
    }

    void DropPieces()
    {
        for (int y = 0; y < boardheight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                if (board[x,y].destroyed)
                {
                    bool dropped = false;

                    for (int j = y+1; j < boardheight && dropped == false; j++)
                    {
                        if (board[x,j].destroyed == false)
                        {
                            Vector2 coordinates1 = board[x, y].coordinates;
                            Vector2 coordinates2 = board[x, j].coordinates;

                            board[x, y].coordinates = coordinates2;
                            board[x, j].coordinates = coordinates1;

                            iTween.MoveTo(board[x, j].gameObject, iTween.Hash(
                                    "position", board[x, y].transform.position,
                                    "time", 0.3f
                                ));

                            board[x, y].transform.position = board[x, j].transform.position;

                            Piece fallingPiece = board[x, j];
                            board[x, j] = board[x, y];
                            board[x, y] = fallingPiece;

                            dropped = true;
                        }
                    }
                }

            }
        }
    }

    void AddPieces()
    {
        int firstY = -1;

        for (int y = 0; y < boardheight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                if (board[x,y].destroyed)
                {
                    if (firstY == -1)
                    {
                        firstY = y;
                    }

                    Piece oldPiece = board[x, y];

                    GameObject pieceObject = Instantiate(piecePrefab);
                    pieceObject.transform.SetParent(levelContainer);
                    pieceObject.transform.position = new Vector3(
                        oldPiece.transform.position.x,
                        10,
                        0
                        );

                    iTween.MoveTo(pieceObject, iTween.Hash(
                            "position", oldPiece.transform.position,
                            "time", 0.3f,
                            "delay",0.1f*(y-firstY)
                        ));

                    Piece piece = pieceObject.GetComponent<Piece>();
                    piece.coordinates = oldPiece.coordinates;

                    board[x, y] = piece;

                    Destroy(oldPiece.gameObject);
                }
            }
        }
    }

    void CheckGameOver()
    {
        int possibleMatches = 0;

        for (int y = 0; y < boardheight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                Piece piece1 = board[x, y];
                Vector2 coordinates1 = piece1.coordinates;

                Piece piece2;
                Vector2 coordinates2;

                //Horizontal swap

                if (x<boardWidth-1)
                {
                    piece2 = board[x + 1, y];
                    coordinates2 = piece2.coordinates;

                    piece1.coordinates = coordinates2;
                    piece2.coordinates = coordinates1;

                    board[(int)piece1.coordinates.x, (int)piece1.coordinates.y] = piece1;
                    board[(int)piece2.coordinates.x, (int)piece2.coordinates.y] = piece2;

                    if (CheckMatch(piece1).Count>=3 || CheckMatch(piece2).Count>=3)
                    {
                        possibleMatches++;
                    }

                    piece1.coordinates = coordinates1;
                    piece2.coordinates = coordinates2;

                    board[(int)piece1.coordinates.x, (int)piece1.coordinates.y] = piece1;
                    board[(int)piece2.coordinates.x, (int)piece2.coordinates.y] = piece2;
                }

                //Vetical Swap

                if (y<boardheight-1)
                {
                    piece2 = board[x, y + 1];
                    coordinates2 = piece2.coordinates;

                    piece1.coordinates = coordinates2;
                    piece2.coordinates = coordinates1;

                    board[(int)piece1.coordinates.x, (int)piece1.coordinates.y] = piece1;
                    board[(int)piece2.coordinates.x, (int)piece2.coordinates.y] = piece2;

                    if (CheckMatch(piece1).Count>=3 || CheckMatch(piece2).Count>=3)
                    {
                        possibleMatches++;
                    }

                    piece1.coordinates = coordinates1;
                    piece2.coordinates = coordinates2;

                    board[(int)piece1.coordinates.x, (int)piece1.coordinates.y] = piece1;
                    board[(int)piece2.coordinates.x, (int)piece2.coordinates.y] = piece2;
                }
            }
        }

        if (possibleMatches == 0)
        {
            OnGameOver();
        }
    }

    void OnGameOver()
    {
        Debug.Log("GAME OVER!!!!");
    }
}
