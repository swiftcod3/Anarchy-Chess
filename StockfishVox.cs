using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class FairyFishManager : MonoBehaviour
{
    public string exeFileName = "stockfish-windows-x86-64-sse41-popcnt.exe"; // Specify the file name of the .exe file
    public string variantsFileName = "variants.ini"; // Specify the file name of the .exe file
    private Process stockfishProcess;
    private Thread outputReaderThread;
    public bool waitingForMove = true;
    public bool waitingForPlayer = false;
    public bool UpgradeChosen = true;
    public bool isReady = false;
    public bool enemyMoved = false;
    public int points = 500;

    private string Move;

    private BoardDisplayer _boardDisplayer;
    private PeiceMoveDisplayer _peiceMoveDisplayer;

    public char[][] board;
    public static FairyFishManager Instance;
    public TMP_Text score_display;

    private int pointsFromCapture(char c)
    {
        switch (c)
        {
            case 'P':
                return 0;
            case 'B':
            case 'K':
                return 2;
            case 'R':
                return 4;
            case 'Q':
                return 8;
            default:
                return 0;
        }
    }

    public void TryPotOfGreed()
    {
        if (points >= 7)
        {
            points -= 7;
            AddRandom();
            UpgradeChosen = true;
        }
    }

    public void TryEstrogen()
    {
        if (points >= 6)
        {
            points -= 6;
            AddEstrogen();
            UpgradeChosen = true;
        }
    }

    public void TryAmazon()
    {
        if(points >= 9)
        {
            points -= 9;
            AddAmazon();
            UpgradeChosen = true;
        }
    }

    public void TryAddRocks()
    {
        if (points >= 3)
        {
            points-=3;
            AddRocks();
            UpgradeChosen = true;
        }
    }

    public void TryAddGummybears()
    {
        if (points >= 4)
        {
            points-=4;
            AddGummyBears();
            UpgradeChosen = true;
        }
    }

    public void TryAddThumbTacks()
    {
        if (points >= 3)
        {
            points-=3;
            AddThumbTacks();
            UpgradeChosen = true;
        }
    }

    public void TryFan()
    {
        if (points >= 5)
        {
            points-=5;
            ShiftBoard(0, 1);
            UpgradeChosen = true;
        }
    }


    private void Awake()
    {
        Instance = this;
    }

    private void InitializeBoard()
    {
        board = new char[][]
        {
            new char[] {'R','N','B','Q','K','B','N','R'},
            new char[] {'P','P','P','P','P','P','P','P'},
            new char[] {' ',' ',' ',' ',' ',' ',' ',' '},
            new char[] {' ',' ',' ',' ',' ',' ',' ',' '},
            new char[] {' ',' ',' ',' ',' ',' ',' ',' '},
            new char[] {' ',' ',' ',' ',' ',' ',' ',' '},
            new char[] {'p','p','p','p','p','p','p','p'},
            new char[] {'r','n','b','q','k','b','n','r'}
        };
    }

    public void ShiftBoard(int dx, int dy)
    {
        int rows = board.Length;
        int cols = board[0].Length;

        char[][] newBoard = new char[rows][];
        for (int i = 0; i < rows; i++)
        {
            newBoard[i] = new char[cols];
            for (int j = 0; j < cols; j++)
            {
                newBoard[i][j] = ' ';
            }
        }

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (board[y][x] != ' ')
                {
                    int newX = x + dx;
                    int newY = y + dy;

                    if (newX < 0 || newX >= cols || newY < 0 || newY >= rows)
                    {
                        newBoard[y][x] = board[y][x];
                    }
                }
            }
        }

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (board[y][x] != ' ')
                {
                    int newX = x + dx;
                    int newY = y + dy;

                    if (newX < 0 || newX >= cols || newY < 0 || newY >= rows)
                        continue;

                    if (newBoard[newY][newX] == ' ')
                    {
                        newBoard[newY][newX] = board[y][x];
                    }
                    else
                    {
                        newBoard[y][x] = board[y][x];
                    }
                }
            }
        }

        board = newBoard;

        _boardDisplayer.UpdateBoard(board);
        _peiceMoveDisplayer.UpdateBoard(board);
    }

    public void AddEstrogen()
    {
        int i = 0;
        while (i < 1)
        {
            char temp = 'X';
            int x = 0;
            int y = 0;
            int iterations = 0;
            while (temp != ' ' && iterations < 1000)
            {
                x = Random.Range(0, 8);
                y = Random.Range(0, 4);
                temp = board[x][y];
                iterations++;
            }
            board[x][y] = 'S';
            i++;
        }

        _boardDisplayer.UpdateBoard(board);
        _peiceMoveDisplayer.UpdateBoard(board);
    }

    public void AddRandom()
    {
        int i = 0;
        char[] chars = new char[] {'R', 'B', 'N', 'Q', 'P'};
        while (i < 2)
        {
            char temp = 'X';
            int x = 0;
            int y = 0;
            int iterations = 0;
            while (temp != ' ' && iterations < 1000)
            {
                x = Random.Range(0, 8);
                y = Random.Range(0, 8);
                temp = board[x][y];
                iterations++;
            }
            board[x][y] = chars[Random.Range(0, chars.Length)];
            i++;
        }

        _boardDisplayer.UpdateBoard(board);
        _peiceMoveDisplayer.UpdateBoard(board);
    }

    public void AddRocks()
    {
        int i = 0;
        while (i < 3)
        {
            char temp = 'X';
            int x = 0;
            int y = 0;
            int iterations = 0;
            while (temp != ' ' && iterations < 1000)
            {
                x = Random.Range(0, 8);
                y = Random.Range(0, 8);
                temp = board[x][y];
                iterations ++;
            }
            board[x][y] = 'Z';
            i++;
        }

        _boardDisplayer.UpdateBoard(board);
        _peiceMoveDisplayer.UpdateBoard(board);
    }

    public void AddAmazon()
    {
        int i = 0;
        while (i < 1)
        {
            char temp = 'X';
            int x = 0;
            int y = 0;
            int iterations = 0;
            while (temp != ' ' && iterations < 1000)
            {
                x = Random.Range(0, 8);
                y = Random.Range(0, 8);
                temp = board[x][y];
                iterations++;
            }
            board[x][y] = 'O';
            i++;
        }

        _boardDisplayer.UpdateBoard(board);
        _peiceMoveDisplayer.UpdateBoard(board);
    }

    public void AddGummyBears()
    {
        int i = 0;
        while (i < 2)
        {
            char temp = 'X';
            int x = 0;
            int y = 0;
            int iterations = 0;
            while (temp != ' ' || iterations > 1000)
            {
                x = Random.Range(0, 8);
                y = Random.Range(0, 8);
                temp = board[x][y];
                iterations++;
            }
            board[x][y] = 'G';
            i++;
        }

        _boardDisplayer.UpdateBoard(board);
        _peiceMoveDisplayer.UpdateBoard(board);
    }

    public void AddThumbTacks()
    {
        int i = 0;
        while (i < 2)
        {
            char temp = 'X';
            int x = 0;
            int y = 0;
            int iterations = 0;
            while (temp != ' ' || iterations > 1000)
            {
                x = Random.Range(0, 8);
                y = Random.Range(0, 8);
                temp = board[x][y];
                iterations++;
            }
            board[x][y] = 'T';
            i++;
        }

        _boardDisplayer.UpdateBoard(board);
        _peiceMoveDisplayer.UpdateBoard(board);
    }


    private char injure(char c)
    {
        switch (c)
        {
            case 'B':
                return 'J';
            case 'b':
                return 'j';
            case 'R':
                return 'H';
            case 'r':
                return 'h';
            case 'q':
                return 'f';
            case 'Q':
                return 'F';
            default:
                return c;
        }
    }

    public void Capture(Vector2 start, Vector2 end)
    {
        if (board[(int)end.x][(int)end.y] != 'G')
        {
            waitingForPlayer = false;
        }
        if (board[(int)end.x][(int)end.y] == 'T')
        {
            board[(int)end.x][(int)end.y] = injure(board[(int)start.x][(int)start.y]);
            board[(int)start.x][(int)start.y] = ' ';
            return;
        }
        if (board[(int)end.x][(int)end.y] == 'S' && board[(int)start.x][(int)start.y] == 'P')
        {
            board[(int)end.x][(int)end.y] = 'Q';
            board[(int)start.x][(int)start.y] = ' ';
            return;
        }

        points += pointsFromCapture(board[(int)end.x][(int)end.y]);
        board[(int)end.x][(int)end.y] = board[(int)start.x][(int)start.y];
        board[(int)start.x][(int)start.y] = ' ';
    }

    private string BoardToFen()
    {
        StringBuilder output = new StringBuilder();

        for (int i = 7; i >= 0; i--)
        {
            int whitespaces = 0;

            for (int j = 0; j < 8; j++)
            {
                char item = board[i][j];

                if (item == ' ')
                {
                    whitespaces++;
                }
                else
                {
                    if (whitespaces > 0)
                    {
                        output.Append(whitespaces);
                        whitespaces = 0;
                    }
                    output.Append(item);
                }
            }

            if (whitespaces > 0)
                output.Append(whitespaces);

            if (i > 0)
                output.Append("/");
        }

        output.Append(" b - - 0 1");
        return output.ToString();
    }

    private void DebugBoard()
    {
        foreach (char[] cs in board)
        {
            string chrs = "";
            foreach (char c in cs)
            {
                chrs += c;
            }
            print(chrs);
        }
    }
    // TODO reverse move to make the right cplor
    private void MovePieceOnBoard(string move)
    {
        if (move == "O-O")
        {

        }


        move = move.Trim();
        int startColumn = columnFromChar(move[0]);
        int startRow = rowFromChar(move[1]);
        int endColumn = columnFromChar(move[2]);
        int endRow = rowFromChar(move[3]);

        board[endRow][endColumn] = board[startRow][startColumn].ToString()[0];
        board[startRow][startColumn] = ' ';
        _boardDisplayer.UpdateBoard(board);
    }

    int columnFromChar(char ch)
    {
        switch (ch)
        {
            case 'a':
                return 0;
            case 'b':
                return 1;
            case 'c':
                return 2;
            case 'd':
                return 3;
            case 'e':
                return 4;
            case 'f':
                return 5;
            case 'g':
                return 6;
            case 'h':
                return 7;
            default:
                SceneManager.LoadScene("Victory");
                return 0;
        }
    }
    int rowFromChar(char ch)
    {
        switch (ch)
        {
            case '1':
                return 0;
            case '2':
                return 1;
            case '3':
                return 2;
            case '4':
                return 3;
            case '5':
                return 4;
            case '6':
                return 5;
            case '7':
                return 6;
            case '8':
                return 7;
            default:
                //throw new System.Exception($"Invalid Row! {ch}");
                return 0;
        }
    }


    public void EnterStageLeft()
    {
        string exeFilePath = Path.Combine(Application.dataPath, exeFileName);
        InitializeBoard();
        _boardDisplayer = BoardDisplayer.Instance;
        _peiceMoveDisplayer = PeiceMoveDisplayer.Instance;
        _boardDisplayer.UpdateBoard(board);

        if (File.Exists(exeFilePath))
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = exeFilePath;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            stockfishProcess = new Process
            {
                StartInfo = startInfo
            };
            stockfishProcess.Start();

            // Start a thread to continuously read from Stockfish's standard output
            outputReaderThread = new Thread(ReadStockfishOutput);
            outputReaderThread.Start();

            // Send "uci" command to Stockfish
            StartCoroutine(
                SendCommands(new string[] {
                "uci",
                $"setoption name VariantPath value variants.ini.ini",
                $"setoption name UCI_Variant value anarchy",
                "setoption name UCI_Elo value 2850",
                "setoption name EvalFile value nn-46832cfbead3.nnue"
                })
            );
            print(BoardToFen());
            
        }
        else
        {
            UnityEngine.Debug.LogError("Stockfish executable file not found: " + exeFilePath);
        }
    }

    private IEnumerator SendCommands(string[] cmds)
    {
        foreach (string cmd in cmds)
        {
            SendCommandToStockfish(cmd);
            yield return new WaitForSeconds(1);
            SendCommandToStockfish("isready");
            while (!isReady)
            {
                yield return new WaitForSeconds(0.05f);
            }
            isReady = false;
        }
        StartCoroutine(GetMove(BoardToFen()));
    }

    private void Update()
    {
        if (!waitingForMove && !enemyMoved)
        {
            MovePieceOnBoard(Move);
            enemyMoved = true;
        }
        score_display.text = $"Points: {points}";
        bool win = true;
        foreach (char[] cs in board)
        {
            foreach(char c in cs)
            {
                if (c == 'k') win = false;
            }
        }
        if (win)
        {
            SceneManager.LoadScene("Victory");
        }
    }

    private IEnumerator GetMove(string fen)
    {
        while (!Input.GetKeyDown(KeyCode.Z))
        {
            waitingForMove = true;
            enemyMoved = false;
            UpgradeChosen = false;
            waitingForPlayer = true;
            points++;
            print(BoardToFen());
            _boardDisplayer.UpdateBoard(board);
            _peiceMoveDisplayer.UpdateBoard(board);
            while (waitingForPlayer)
            {
                yield return new WaitForSeconds(0.2f);
            }


            _boardDisplayer.UpdateBoard(board);
            _peiceMoveDisplayer.UpdateBoard(board);
            print(BoardToFen());
            SendCommandToStockfish("ucinewgame");
            SendCommandToStockfish($"position fen {BoardToFen()}");
            SendCommandToStockfish("go movetime 4000");
            waitingForMove = true;
            while (waitingForMove)
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private void RecieveBestMove(string m)
    {
        waitingForMove = false;
        Move = m;
    }


    private void ReadStockfishOutput()
    {
        while (!stockfishProcess.StandardOutput.EndOfStream)
        {
            string l = stockfishProcess.StandardOutput.ReadLine();
            print(l);
            if (l.StartsWith("bestmove"))
            {
                print(l);
                l = l.Substring(9, 4);
                RecieveBestMove(l);
            }
            if (l.StartsWith("readyok"))
            {
                isReady = true;
            }
        }
    }

    private void SendCommandToStockfish(string command)
    {
        if (stockfishProcess != null && !stockfishProcess.HasExited)
        {
            stockfishProcess.StandardInput.WriteLine(command);
            stockfishProcess.StandardInput.Flush();
        }
    }

    public void Start()
    {
        EnterStageLeft();
    }

    private void OnDestroy()
    {
        if (stockfishProcess != null && !stockfishProcess.HasExited)
        {
            stockfishProcess.StandardInput.WriteLine("quit");
            stockfishProcess.StandardInput.Flush();
            stockfishProcess.WaitForExit();
            stockfishProcess.Close();

            // Stop the output reader thread
            outputReaderThread.Join();
        }
    }
}
