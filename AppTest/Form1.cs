using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Numerics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Reflection.Emit;

namespace AppTest
{
    public partial class Form1 : Form
    {
        public Game game;
        public GameData data;
        public Form1()
        {
            data = new GameData();
            game = new Game(data);
            InitializeComponent();
            game.GameInit(game.data);
            game.data.gameTicker = new Timer();
            game.data.gameTicker.Tick += new EventHandler(GameLoop);
            game.data.gameTicker.Interval = 17;
            game.data.gameTicker.Start();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    game.data.playerMovementDirection = Direction.up;
                    break;
                case Keys.A:
                    game.data.playerMovementDirection = Direction.left;
                    break;
                case Keys.S:
                    game.data.playerMovementDirection = Direction.down;
                    break;
                case Keys.D:
                    game.data.playerMovementDirection = Direction.right;
                    break;
            }
        }

        public void DrawMap(Map map, GameData data)
        {
            groupBox1.Controls.Clear();
            for (int x = 0; x < map.width; x++)
            {
                for (int y = 0; y < map.height; y++)
                {
                    PictureBox pBox = new PictureBox();
                    pBox.Location = new Point(x * data.cellSize, y * data.cellSize);
                    pBox.Size = new Size(data.cellSize, data.cellSize);
                    switch (game.GetMapValue(map, new MapLocation(x, y)))
                    {
                        case (int)CellType.empty:
                            pBox.BackColor = Color.Orange;
                            break;
                        case (int)CellType.wall:
                            pBox.BackColor = Color.Purple;
                            break;
                        case (int)CellType.player:
                            pBox.BackColor = Color.Cyan;
                            break;
                        case (int)CellType.exit:
                            pBox.BackColor = Color.Red;
                            break;
                        case (int)CellType.treasure:
                            pBox.BackColor = Color.Gold;
                            break;
                        case (int)CellType.bonus:
                            pBox.BackColor = Color.ForestGreen;
                            break;
                    }

                    groupBox1.Controls.Add(pBox);
                }
            }
        }
        public void GameLoop(Object myObject, EventArgs myEventArgs)
        {
            game.MovePlayer(game.data.playerMovementDirection, game.data);
            DrawMap(game.data.map, game.data);
            UpdateTurnAmount(game.data);
        }

        public void UpdateTurnAmount(GameData data)
        {
            label1.Text = data.turnAmount.ToString();
            label2.Text = "+ " + data.bonusTurnAmount.ToString();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Focus();
            this.KeyPreview = true;
            this.KeyUp += new KeyEventHandler(Form1_KeyUp);
        }
    }

    public class MapLocation
    {
        public int x;
        public int y;

        public MapLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return x == ((MapLocation)obj).x && y == ((MapLocation)obj).y;
            }
        }

        public override int GetHashCode()
        {
            return 0;
        }

    }

    public class Map
    {
        public int width;
        public int height;

        public int[,] gridArray;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;

            gridArray = new int[width, height];
        }

    }

    public class Node
    {
        public MapLocation pos;
        public float G;
        public float H;
        public float F;
        public Node parent;

        public Node(MapLocation pos, float g, float h, float f, Node p)
        {
            this.pos = pos;
            G = g;
            H = h;
            F = f;
            parent = p;
        }
    }

    public enum Direction
    {
        none,
        up,
        down,
        left,
        right
    }

    public enum CellType
    {
        empty,
        wall,
        player,
        exit,
        treasure,
        bonus
    }

    public class GameData
    {
        public int turnAmount;
        public int bonusTurnAmount;
        public Timer gameTicker;
        public MapLocation exitPos;
        public MapLocation playerPos;
        public MapLocation treasurePos;
        public List<MapLocation> bonusPosList;
        public bool isTreasureCollected;
        public Direction playerMovementDirection;
        public Map map;
        public int width;
        public int height;
        public int cellSize;
        public int spaceBetweenCells;
        public int level;
        public int bonusAmount;
        public List<Node> path;
        public List<Node> open;
        public List<Node> closed;
        public Node goalNode;
        public Node startNode;
        public Node lastPos;
        public bool done;

        public GameData()
        {
            done = false;
            path = new List<Node>();
            open = new List<Node>();
            closed = new List<Node>();
            bonusPosList = new List<MapLocation>();
            bonusTurnAmount = 0;
        }
    }

    public class Game
    {
        public GameData data;

        public Game(GameData data)
        {
            this.data = data;
        }

        public void GameInit(GameData data)
        {
            data.turnAmount = 0;
            data.level = 0;
            data.cellSize = 20;
            GenerateLevel(data);
        }

        public void GenerateLevel(GameData data)
        {
            SetMapSize(data, data.level);
            data.map = new Map(data.width, data.height);
            data.playerPos = new MapLocation(1, 1);
            data.exitPos = new MapLocation(data.width - 2, data.height - 2);
            data.playerMovementDirection = Direction.none;
            data.isTreasureCollected = false;
            data.bonusAmount = data.level / 2;
            data.bonusPosList.Clear();
            GenerateMaze(data);
            SetMapValue(data.map, data.playerPos, (int)CellType.player);
            SetRandomExitPosition(data.exitPos, data);
            SetRandomTreasurePosition(data);
            SetRandomBonusPosition(data, data.bonusAmount);
            data.turnAmount += CalculateShortestDistanceBetween(data.playerPos, data.treasurePos);
            data.turnAmount += CalculateShortestDistanceBetween(data.treasurePos, data.exitPos) + 1;
        }

        public void SetMapSize(GameData data, int level)
        {
            data.width = 7 + level * 2;
            data.height = 7 + level * 2;
        }

        public bool SetMapValue(Map map, MapLocation pos, int value)
        {
            if (pos.x >= 0 && pos.y >= 0 && pos.x < data.width && pos.y < data.height)
            {
                map.gridArray[pos.x, pos.y] = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetMapValue(Map map, MapLocation pos)
        {
            if (pos.x >= 0 && pos.y >= 0 && pos.x < data.width && pos.y < data.height)
            {
                return map.gridArray[pos.x, pos.y];
            }
            else
            {
                return -1;
            }
        }

        public bool MovePlayer(Direction dir, GameData data)
        {
            if (!CheckGameOver(data)){
                switch (dir)
                {
                    case Direction.none:
                        break;
                    case Direction.up:
                        ChangePlayerPosition(new MapLocation(data.playerPos.x, data.playerPos.y - 1), data);
                        data.playerMovementDirection = Direction.none;
                        break;
                    case Direction.down:
                        ChangePlayerPosition(new MapLocation(data.playerPos.x, data.playerPos.y + 1), data);
                        data.playerMovementDirection = Direction.none;
                        break;
                    case Direction.left:
                        ChangePlayerPosition(new MapLocation(data.playerPos.x - 1, data.playerPos.y), data);
                        data.playerMovementDirection = Direction.none;
                        break;
                    case Direction.right:
                        ChangePlayerPosition(new MapLocation(data.playerPos.x + 1, data.playerPos.y), data);
                        data.playerMovementDirection = Direction.none;
                        break;
                }
                return true;
            } else
            {
                return false;
            }
        }
        public bool ChangePlayerPosition(MapLocation targetPos, GameData data)
        {
            if (GetMapValue(data.map, targetPos) != (int)CellType.wall)
            {
                if (data.playerPos.Equals(data.exitPos))
                {
                    SetMapValue(data.map, data.playerPos, (int)CellType.exit);
                }
                else
                {
                    SetMapValue(data.map, data.playerPos, (int)CellType.empty);
                }
                SetMapValue(data.map, targetPos, (int)CellType.player);
                data.playerPos = targetPos;
                WithdrawMove(data);
                CheckTreasure(data);
                CheckExit(data);
                CheckBonus(data);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool WithdrawMove(GameData data)
        {
            if (data.turnAmount > 0)
            {
                data.turnAmount--;
                return false;
            } else
            {
                data.bonusTurnAmount--;
                return true;
            }
        }

        public void SetRandomExitPosition(MapLocation targetPos, GameData data)
        {
            data.exitPos = new MapLocation(0, 0);
            bool isExitPlaced = false;
            Random rng = new Random();
            while (!isExitPlaced)
            {
                data.exitPos.x = rng.Next(data.width);
                data.exitPos.y = rng.Next(data.height);
                if (GetMapValue(data.map, new MapLocation(data.exitPos.x, data.exitPos.y)) == (int)CellType.empty)
                {
                    isExitPlaced = true;
                    SetMapValue(data.map, new MapLocation(data.exitPos.x, data.exitPos.y), (int)CellType.exit);
                }
            }
        }
        public void SetRandomTreasurePosition(GameData data)
        {
            data.treasurePos = new MapLocation(0, 0);
            bool isTreasurePlaced = false;
            Random rng = new Random();
            while (!isTreasurePlaced)
            {
                data.treasurePos.x = rng.Next(data.width);
                data.treasurePos.y = rng.Next(data.height);
                if (GetMapValue(data.map, new MapLocation(data.treasurePos.x, data.treasurePos.y)) == (int)CellType.empty)
                {
                    isTreasurePlaced = true;
                    SetMapValue(data.map, new MapLocation(data.treasurePos.x, data.treasurePos.y), (int)CellType.treasure);
                }
            }
        }

        public void SetRandomBonusPosition(GameData data, int amount)
        {
            MapLocation bonusPos = new MapLocation(0, 0);
            Random rng = new Random();
            for (int i = 0; i < amount; i++)
            {
                bool isBonusPlaced = false;
                while (!isBonusPlaced)
                {
                    bonusPos.x = rng.Next(data.width);
                    bonusPos.y = rng.Next(data.height);
                    if (GetMapValue(data.map, new MapLocation(bonusPos.x, bonusPos.y)) == (int)CellType.empty)
                    {
                        isBonusPlaced = true;
                        SetMapValue(data.map, new MapLocation(bonusPos.x, bonusPos.y), (int)CellType.bonus);
                        data.bonusPosList.Add(new MapLocation(bonusPos.x, bonusPos.y));
                    }
                }
            }
        }
        public void CheckBonus(GameData data)
        {
            if (data.bonusPosList.Contains(data.playerPos))
            {
                data.bonusPosList.Remove(data.playerPos);
                data.bonusTurnAmount += 5;
            }
        }

        public void CheckTreasure(GameData data)
        {
            if (data.playerPos.Equals(data.treasurePos))
            {
                data.isTreasureCollected = true;
            }
        }
        public void CheckExit(GameData data)
        {
            if (data.playerPos.Equals(data.exitPos))
            {
                if (data.isTreasureCollected)
                {
                    data.level++;
                    GenerateLevel(data);
                }
                else
                {
                    Console.WriteLine("I will not go without a treasure");
                }
            }
        }
        public bool CheckGameOver(GameData data)
        {
            if (data.bonusTurnAmount + data.turnAmount <= 0)
            {
                Console.WriteLine("Out of moves!");
                return true;
            } else
            {
                return false;
            }
        }
        public void GenerateMaze(GameData data)
        {
            for (int y = 0; y < data.height; y++)
            {
                for (int x = 0; x < data.width; x++)
                {
                    if ((y % 2 == 1) && (x % 2 == 1))
                    {
                        SetMapValue(data.map, new MapLocation(x, y), 0);
                    }
                    else if (((y % 2 == 1) && (x % 2 == 0) && (x != 0) && (x != data.width - 1)) ||
                ((x % 2 == 1) && (y % 2 == 0) && (y != 0) && (y != data.height - 1)))
                    {
                        SetMapValue(data.map, new MapLocation(x, y), 0);
                    }
                    else
                    {
                        SetMapValue(data.map, new MapLocation(x, y), 1);
                    }
                }
            }
            //1
            List<int> rowSet = new List<int>(data.width / 2);
            for (int i = 0; i < data.width / 2; i++)
            {
                rowSet.Add(0);
            }
            int set = 1;
            Random rng = new Random();
            //rng.Next(10);

            for (int i = 0; i < data.height / 2; i++)
            {
                //2
                for (int j = 0; j < data.width / 2; j++)
                {
                    if (rowSet[j] == 0)
                    {
                        rowSet[j] = set++;
                    }
                }
                //3
                for (int j = 0; j < data.width / 2 - 1; j++)
                {
                    int rightWall = rng.Next(2);

                    if ((rightWall == 1) || (rowSet[j] == rowSet[j + 1]))
                    {
                        SetMapValue(data.map, new MapLocation(i * 2 + 1, j * 2 + 2), 1);
                    }
                    else
                    {
                        int changingSet = rowSet[j + 1];
                        for (int l = 0; l < data.width / 2; l++)
                        {
                            if (rowSet[l] == changingSet)
                            {
                                rowSet[l] = rowSet[j];
                            }
                        }
                    }
                }
                //4
                for (int j = 0; j < data.width / 2; j++)
                {
                    int bottomWall = rng.Next(2);
                    int countCurrentSet = 0;
                    for (int l = 0; l < data.width / 2; l++)
                    {
                        if (rowSet[j] == rowSet[l])
                        {
                            countCurrentSet++;
                        }
                    }
                    if ((bottomWall == 1) && (countCurrentSet != 1))
                    {
                        SetMapValue(data.map, new MapLocation(i * 2 + 2, j * 2 + 1), 1);
                    }
                }
                //
                if (i != data.height / 2 - 1)
                {
                    for (int j = 0; j < data.width / 2; j++)
                    {
                        int countHole = 0;
                        for (int l = 0; l < data.width / 2; l++)
                        {
                            if ((rowSet[l] == rowSet[j]) && (GetMapValue(data.map, new MapLocation(i * 2 + 2, l * 2 + 1)) == 0))
                            {
                                countHole++;
                            }
                        }
                        if (countHole == 0)
                        {
                            SetMapValue(data.map, new MapLocation(i * 2 + 2, j * 2 + 1), 0);
                        }
                    }
                }
                //5
                if (i != data.height / 2 - 1)
                {
                    for (int j = 0; j < data.width / 2; j++)
                    {
                        if (GetMapValue(data.map, new MapLocation(i * 2 + 2, j * 2 + 1)) == 1)
                        {
                            rowSet[j] = 0;
                        }
                    }
                }
            }
            for (int j = 0; j < data.width / 2 - 1; j++)
            {
                if (rowSet[j] != rowSet[j + 1])
                {
                    SetMapValue(data.map, new MapLocation(data.height - 2, j * 2 + 2), 0);
                }
            }
        }

        public int CalculateShortestDistanceBetween(MapLocation start, MapLocation target)
        {
            List<Node> path = GetPath(start, target, data);
            foreach (Node p in path)
            {
                Console.WriteLine("(" + p.pos.x + "," + p.pos.y + ")");
            }
            return path.Count - 1;

        }

        public void BeginSearch(MapLocation start, MapLocation goal, GameData data)
        {
            data.done = false;

            data.startNode = new Node(start, 0, 0, 0, null);
            data.goalNode = new Node(goal, 0, 0, 0, null);

            data.open.Clear();
            data.closed.Clear();
            data.path.Clear();
            data.open.Add(data.startNode);
            data.lastPos = data.startNode;
        }

        public void Search(Node thisNode, GameData data)
        {
            if (thisNode.pos.Equals(data.goalNode.pos))
            {
                data.done = true;
                return;
            }


            for (int dir = 0; dir < 4; dir++)
            {
                MapLocation neighbour = new MapLocation(0, 0);
                switch (dir)
                {
                    case 0:
                        neighbour = new MapLocation(thisNode.pos.x + 1, thisNode.pos.y);
                        break;
                    case 1:
                        neighbour = new MapLocation(thisNode.pos.x, thisNode.pos.y + 1);
                        break;
                    case 2:
                        neighbour = new MapLocation(thisNode.pos.x - 1, thisNode.pos.y);
                        break;
                    case 3:
                        neighbour = new MapLocation(thisNode.pos.x, thisNode.pos.y - 1);
                        break;
                }
                if (GetMapValue(data.map, neighbour) == 1) continue;
                if (GetMapValue(data.map, neighbour) == -1) continue;
                if (isClosed(neighbour, data)) continue;

                float G = Vector2.Distance(new Vector2(thisNode.pos.x, thisNode.pos.y), new Vector2(neighbour.x, neighbour.y)) + thisNode.G;
                float H = Vector2.Distance(new Vector2(neighbour.x, neighbour.y), new Vector2(data.goalNode.pos.x, data.goalNode.pos.y));
                float F = G + H;

                if (!UpdateMarker(neighbour, G, H, F, thisNode, data))
                    data.open.Add(new Node(neighbour, G, H, F, thisNode));
            }

            data.open = data.open.OrderBy(p => p.F).ToList<Node>();
            Node pm = (Node)data.open.ElementAt(0);
            data.closed.Add(pm);
            data.open.RemoveAt(0);
            data.lastPos = pm;
        }

        public bool isClosed(MapLocation pos, GameData data)
        {
            foreach (Node p in data.closed)
            {
                if (p.pos.Equals(pos)) return true;
            }
            return false;
        }

        public bool UpdateMarker(MapLocation pos, float g, float h, float f, Node prt, GameData data)
        {
            foreach (Node p in data.open)
            {
                if (p.pos.Equals(pos))
                {
                    p.G = g;
                    p.H = h;
                    p.F = f;
                    p.parent = prt;
                    return true;
                }
            }
            return false;
        }

        public List<Node> GetPath(MapLocation start, MapLocation target, GameData data)
        {
            BeginSearch(start, target, data);
            while (!data.done)
            {
                Search(data.lastPos, data);
            }

            Node begin = data.lastPos;

            while (!data.startNode.Equals(begin) && begin != null)
            {
                data.path.Add(begin);
                begin = begin.parent;
            }
            data.path.Add(begin);
            return data.path;
        }
    }
}
