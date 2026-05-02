using UnityEngine;
using UnityEngine.UI;

public class BoardDisplayer : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite BlackRook;
    [SerializeField] private Sprite BlackKnight;
    [SerializeField] private Sprite BlackBishop;
    [SerializeField] private Sprite BlackQueen;
    [SerializeField] private Sprite BlackKing;
    [SerializeField] private Sprite BlackPawn;
    [SerializeField] private Sprite WhiteRook;
    [SerializeField] private Sprite WhiteKnight;
    [SerializeField] private Sprite WhiteBishop;
    [SerializeField] private Sprite WhiteQueen;
    [SerializeField] private Sprite WhiteKing;
    [SerializeField] private Sprite WhitePawn;
    [SerializeField] private Sprite Empty;
    [SerializeField] private Sprite Rock;
    [SerializeField] private Sprite GummyBear;
    [SerializeField] private Sprite ThumbTack;
    [SerializeField] private Sprite Estrogen;
    [SerializeField] private Sprite Amazon;

    [Header("Grid Settings")]
    [SerializeField] private SpriteRenderer[][] squares = new SpriteRenderer[8][];
    [SerializeField] private GameObject EmptySquare;
    public float SquareWidth;

    private char[][] board;

    public static BoardDisplayer Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupSquares();
    }

    private void Awake()
    {
        Instance = this;
    }

    void InitializeArray()
    {
        squares[0] = new SpriteRenderer[8];
        squares[1] = new SpriteRenderer[8];
        squares[2] = new SpriteRenderer[8];
        squares[3] = new SpriteRenderer[8];
        squares[4] = new SpriteRenderer[8];
        squares[5] = new SpriteRenderer[8];
        squares[6] = new SpriteRenderer[8];
        squares[7] = new SpriteRenderer[8];
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
                squares[i][j] = s.GetComponent<SpriteRenderer>();
            }
        }
    }

    private Sprite SpriteFromChar(char c)
    {
        switch (c){
            case ' ':
                return Empty;
            case 'r':
            case 'h':
                return BlackRook;
            case 'n':
                return BlackKnight;
            case 'b':
            case 'j':
                return BlackBishop;
            case 'q':
            case 'f':
                return BlackQueen;
            case 'p':
                return BlackPawn;
            case 'k':
                return BlackKing;
            case 'R':
            case 'H':
                return WhiteRook;
            case 'N':
                return WhiteKnight;
            case 'B':
            case 'J':
                return WhiteBishop;
            case 'Q':
            case 'F':
                return WhiteQueen;
            case 'P':
                return WhitePawn;
            case 'K':
                return WhiteKing;
            case 'Z':
                return Rock;
            case 'G':
                return GummyBear;
            case 'T':
                return ThumbTack;
            case 'S':
                return Estrogen;
            case 'O':
                return Amazon;
            default:
                return null;
        } 
    } 

    public void UpdateBoard(char[][] state)
    {
        board = state;
    }

    private void RenderBoard()
    {
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[i].Length; j++)
            {
                squares[i][j].sprite = SpriteFromChar(board[i][j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RenderBoard();
    }
}
