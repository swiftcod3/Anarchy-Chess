using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PeiceMoveDisplayer : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private PieceHighlighter[][] squares = new PieceHighlighter[8][];
    [SerializeField] private GameObject EmptySquare;
    public float SquareWidth;

    private char[][] board;
    private Vector2 selectedPosition;

    public static PeiceMoveDisplayer Instance;
    private FairyFishManager _manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupSquares();
        _manager = FairyFishManager.Instance;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateBoard(char[][] state)
    {
        board = state;
    }

    private void ClearHighlights()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                squares[i][j].highlighted = false;
            }
        }
    }

    public void UpdateSelectedPosition(Vector2 newPosition)
    {
        selectedPosition = newPosition;
        if (!_manager.waitingForPlayer)
        {
            selectedPosition = Vector2.down;
            return;
        }

        char pieceType = board[(int)selectedPosition.x][(int)selectedPosition.y];
        print($"Clicked on {(int)selectedPosition.x}, {(int)selectedPosition.y} ({pieceType})");
        
        ClearHighlights();
        foreach (var move in ValidPositionsFromPosAndType(selectedPosition, pieceType))
        {
            int x = (int)move.x;
            int y = (int)move.y;
            try
            {
                squares[x][y].highlighted = true;
            }
            catch (System.Exception)
            {

            }
            
        }
    }

    public void Capture(Vector2 endPos)
    {
        _manager.Capture(selectedPosition, endPos);
        ClearHighlights() ;
    }

    List<Vector2> ValidPositionsFromPosAndType(Vector2 position, char pieceType)
    {
        List<Vector2> output = new();

        switch (pieceType)
        {
            case 'R':
                Vector2[] directions = new Vector2[]
                {
                new Vector2(0, 1),   // up
                new Vector2(0, -1),  // down
                new Vector2(1, 0),   // right
                new Vector2(-1, 0)   // left
                };

                foreach (var dir in directions)
                {
                    int x = (int)position.x;
                    int y = (int)position.y;

                    while (true)
                    {
                        x += (int)dir.x;
                        y += (int)dir.y;

                        if (x < 0 || x >= 8 || y < 0 || y >= 8)
                            break;

                        char target = board[x][y];

                        if (target == ' ')
                        {
                            output.Add(new Vector2(x, y));
                        }
                        else
                        {
                            output.Add(new Vector2(x, y));
                            break; // blocked :3
                        }
                    }
                }
                break;
            case 'B':
                Vector2[] directions2 = new Vector2[]
                {
                new Vector2(1, 1),   // up
                new Vector2(1, -1),  // down
                new Vector2(-1, 1),   // right
                new Vector2(-1, -1)   // left
                };

                foreach (var dir in directions2)
                {
                    int x = (int)position.x;
                    int y = (int)position.y;

                    while (true)
                    {
                        x += (int)dir.x;
                        y += (int)dir.y;

                        if (x < 0 || x >= 8 || y < 0 || y >= 8)
                            break;

                        char target = board[x][y];

                        if (target == ' ')
                        {
                            output.Add(new Vector2(x, y));
                        }
                        else
                        {
                            output.Add(new Vector2(x, y));
                            break; // blocked :3
                        }
                    }
                }
                break;
            case 'Q':
                Vector2[] directions3 = new Vector2[]
                {
                new Vector2(1, 1),   // upright
                new Vector2(1, -1),  // upleft
                new Vector2(-1, 1),   // downright
                new Vector2(-1, -1),   // downleft
                new Vector2(0, 1),   // up
                new Vector2(0, -1),  // down
                new Vector2(1, 0),   // right
                new Vector2(-1, 0)   // left
                };

                foreach (var dir in directions3)
                {
                    int x = (int)position.x;
                    int y = (int)position.y;

                    while (true)
                    {
                        x += (int)dir.x;
                        y += (int)dir.y;

                        if (x < 0 || x >= 8 || y < 0 || y >= 8)
                            break;

                        char target = board[x][y];

                        if (target == ' ')
                        {
                            output.Add(new Vector2(x, y));
                        }
                        else
                        {
                            output.Add(new Vector2(x, y));
                            break; // blocked :3
                        }
                    }
                }
                break;
            case 'N':
                int x1 = (int)position.x;
                int y1 = (int)position.y;

                Vector2[] directions4 = new Vector2[]
                {
                new Vector2(1, 2),
                new Vector2(1, -2),
                new Vector2(-1, 2),
                new Vector2(-1, -2),
                new Vector2(2, 1),
                new Vector2(-2, -1),
                new Vector2(2, -1),
                new Vector2(-2, 1)
                };
                foreach (var dir in directions4) {
                    try
                    {
                        output.Add(new Vector2(x1 + dir.x, y1 + dir.y));
                    }
                    catch (System.Exception)
                    {
                    }
                
                }
                
                break;
            case 'P':
                {
                    int x = (int)position.x;
                    int y = (int)position.y;

                    try
                    {
                        if (board[x + 1][y] == ' ')
                        {
                            output.Add(new Vector2(x + 1, y));
                            if (board[x + 2][y] == ' ' && x == 1)
                            {
                                output.Add(new Vector2(x + 2, y));
                            }
                        }
                    }
                    catch (System.Exception){ }
                try
                    {
                        if (board[x + 1][y + 1] != ' ')
                        {
                            output.Add(new Vector2(x + 1, y + 1));
                        }
                    }
                    catch (System.Exception){}
                    try
                    {
                        if (board[x + 1][y - 1] != ' ')
                        {
                            output.Add(new Vector2(x + 1, y - 1));
                        }
                    }
                    catch (System.Exception){}
                    

                    break;
                }
            case 'K':
            case 'F':
                int x3 = (int)position.x;
                int y3 = (int)position.y;

                Vector2[] directions5 = new Vector2[]
                {
                new Vector2(1, -1),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1),
                new Vector2(0, -1),
                new Vector2(0, 1)
                };
                foreach (var dir in directions5)
                {
                    try
                    {
                        output.Add(new Vector2(x3 + dir.x, y3 + dir.y));
                    }
                    catch (System.Exception)
                    {
                    }

                }
                break;
            case 'H':
                int x4 = (int)position.x;
                int y4 = (int)position.y;

                Vector2[] directions6 = new Vector2[]
                {
                    new Vector2(0, 1),
                    new Vector2(0, -1),
                    new Vector2(1, 0),
                    new Vector2(-1, 0)
                };
                foreach (var dir in directions6)
                {
                    try
                    {
                        output.Add(new Vector2(x4 + dir.x, y4 + dir.y));
                    }
                    catch (System.Exception)
                    {
                    }

                }
                break;
            case 'J':
                int x5 = (int)position.x;
                int y5 = (int)position.y;

                Vector2[] directions7 = new Vector2[]
                {
                    new Vector2(1, 1),
                    new Vector2(1, -1),
                    new Vector2(-1, 1),
                    new Vector2(-1, -1)
                };
                foreach (var dir in directions7)
                {
                    try
                    {
                        output.Add(new Vector2(x5 + dir.x, y5 + dir.y));
                    }
                    catch (System.Exception)
                    {
                    }

                }
                break;
            case 'O':
                Vector2[] directions8 = new Vector2[]
                {
                new Vector2(1, 1),   // upright
                new Vector2(1, -1),  // upleft
                new Vector2(-1, 1),   // downright
                new Vector2(-1, -1),   // downleft
                new Vector2(0, 1),   // up
                new Vector2(0, -1),  // down
                new Vector2(1, 0),   // right
                new Vector2(-1, 0)   // left
                };

                foreach (var dir in directions8)
                {
                    int x = (int)position.x;
                    int y = (int)position.y;

                    while (true)
                    {
                        x += (int)dir.x;
                        y += (int)dir.y;

                        if (x < 0 || x >= 8 || y < 0 || y >= 8)
                            break;

                        char target = board[x][y];

                        if (target == ' ')
                        {
                            output.Add(new Vector2(x, y));
                        }
                        else
                        {
                            output.Add(new Vector2(x, y));
                            break; // blocked :3
                        }
                    }
                }

                int x6 = (int)position.x;
                int y6 = (int)position.y;

                Vector2[] directions9 = new Vector2[]
                {
                new Vector2(1, 2),
                new Vector2(1, -2),
                new Vector2(-1, 2),
                new Vector2(-1, -2),
                new Vector2(2, 1),
                new Vector2(-2, -1),
                new Vector2(2, -1),
                new Vector2(-2, 1)
                };
                foreach (var dir in directions9)
                {
                    try
                    {
                        output.Add(new Vector2(x6 + dir.x, y6 + dir.y));
                    }
                    catch (System.Exception)
                    {
                    }

                }
                break;

        }

        return output;
    }

    void InitializeArray()
    {
        squares[0] = new PieceHighlighter[8];
        squares[1] = new PieceHighlighter[8];
        squares[2] = new PieceHighlighter[8];
        squares[3] = new PieceHighlighter[8];
        squares[4] = new PieceHighlighter[8];
        squares[5] = new PieceHighlighter[8];
        squares[6] = new PieceHighlighter[8];
        squares[7] = new PieceHighlighter[8];
    }
    void SetupSquares()
    {
        InitializeArray();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var s = Instantiate(EmptySquare);
                s.transform.SetParent(transform, false);
                s.transform.localPosition = new Vector2(1 * j, 1 * i) * SquareWidth;
                s.GetComponent<PieceHighlighter>().Position = new Vector2(i, j);
                squares[i][j] = s.GetComponent<PieceHighlighter>();
            }
        }
    }

    private void RenderBoard()
    {
        
    }

}
