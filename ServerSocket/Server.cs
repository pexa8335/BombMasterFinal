using System.Collections.Concurrent;
using System.Drawing.Drawing2D;
using System.Net.Sockets;
using System.Numerics;
using System.Threading.Tasks;

namespace ServerSocket
{
    public partial class Server : Form
    {
        private static List<PlayerTank> connectedPlayers = new List<PlayerTank>();
        //private static List<Lobby> lobbies = new List<Lobby>();
        private static readonly int port = 8989;
        private TcpListener server;

        private const int MAP_WIDTH = 1024;
        private const int MAP_HEIGHT = 768;
        private const int SPAWN_INTERVAL = 30000; // 30 seconds

        private Dictionary<string, System.Threading.Timer> lobbyTimers = new Dictionary<string, System.Threading.Timer>();
        private Dictionary<string, System.Threading.Timer> zombieSpawnTimers = new Dictionary<string, System.Threading.Timer>();
        const int fixedHeight = 620;

        // Thêm hàng đợi để lưu trữ các thông điệp
        private static BlockingCollection<(PlayerTank, string)> messageQueue 
            = new BlockingCollection<(PlayerTank, string)>(new ConcurrentQueue<(PlayerTank, string)>(), 1000); // Giới hạn kích thước

        public Server()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        class BaseObject
        {
            protected Rectangle rect;
            protected Bitmap bmpObject;

            #region properties
            public Rectangle Rect
            {
                get
                {
                    return rect;
                }

                set
                {
                    rect = value;
                }
            }
            public int RectX
            {
                get
                {
                    return rect.X;
                }

                set
                {
                    rect.X = value;
                }
            }
            public int RectY
            {
                get
                {
                    return rect.Y;
                }

                set
                {
                    rect.Y = value;
                }
            }
            public int RectWidth
            {
                get
                {
                    return rect.Width;
                }

                set
                {
                    rect.Width = value;
                }
            }
            public int RectHeight
            {
                get
                {
                    return rect.Height;
                }

                set
                {
                    rect.Height = value;
                }
            }
            public Bitmap BmpObject
            {
                get
                {
                    return bmpObject;
                }

                set
                {
                    bmpObject = value;
                }
            }
            #endregion

            // load ảnh đối tượng
            public void LoadImage(string path)
            {
                this.bmpObject = new Bitmap(path);
            }

            // vẽ đối tượng vào bitmap nền
            public virtual void Show(Bitmap bmpBack)
            {
                Common.PaintObject(bmpBack, this.bmpObject, rect.X, rect.Y,
                    0, 0, this.RectWidth, this.RectHeight);
            }
        }

        class Common
        {
            #region Hằng số các thông số cố định
            public const int SCREEN_WIDTH = 1100;
            public const int SCREEN_HEIGHT = 900;
            public const int NUMBER_OBJECT_WIDTH = 45;
            public const int NUMBER_OBJECT_HEIGHT = 40;
            public const int MAX_LEVEL = 10;
            public const int STEP = 20;
            public const int tankSize = 40;
            #endregion
            public static string path;

            // load hình ảnh
            public static Bitmap LoadImage(string fileName)
            {
                return new Bitmap(Common.path + fileName);
            }

            // vẽ lên bitmap
            public static void PaintObject(Bitmap bmpBack, Bitmap front, int x, int y, int xFrame, int yFrame, int wFrame, int hFrame)
            {
                Graphics g = Graphics.FromImage(bmpBack);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawImage(front, x, y, new Rectangle(xFrame, yFrame, wFrame, hFrame), GraphicsUnit.Pixel);
                //g.DrawRectangle(new Pen(Color.Yellow, 2)
                //    , x, y, wFrame, hFrame);
                g.Dispose();
            }

            // clear bitmap
            public static void PaintClear(Bitmap bmpBack)
            {
                Graphics g = Graphics.FromImage(bmpBack);
                g.Clear(Color.Black);
                g.Dispose();
            }

            // đọc map vào ma trận
            public static int[,] ReadMap(string path, int numberObjectHeight, int numberObjectWidth)
            {
                int[,] arrayObject;
                string s = "";
                using (StreamReader reader = new StreamReader(path))
                {
                    arrayObject = new int[numberObjectHeight, numberObjectWidth];
                    for (int i = 0; i < numberObjectHeight; i++)
                    {
                        s = reader.ReadLine();
                        for (int j = 0; j < numberObjectWidth; j++)
                            arrayObject[i, j] = int.Parse(s[j].ToString());
                    }
                    return arrayObject;
                }
            }

            // kiểm tra va chạm giữa hai hình chữ nhật
            public static bool IsCollision(Rectangle rect1, Rectangle rect2)
            {
                // góc dưới phải
                if (rect1.Left > rect2.Left && rect1.Left < rect2.Right)
                {
                    if (rect1.Top > rect2.Top && rect1.Top < rect2.Bottom)
                    {
                        return true;
                    }
                }
                // góc trên phải
                if (rect1.Left > rect2.Left && rect1.Left < rect2.Right)
                {
                    if (rect1.Bottom > rect2.Top && rect1.Bottom < rect2.Bottom)
                    {
                        return true;
                    }
                }
                // góc dưới trái
                if (rect1.Right > rect2.Left && rect1.Right < rect2.Right)
                {
                    if (rect1.Top > rect2.Top && rect1.Top < rect2.Bottom)
                    {
                        return true;
                    }
                }
                // góc trên trái
                if (rect1.Right > rect2.Left && rect1.Right < rect2.Right)
                {
                    if (rect1.Bottom > rect2.Top && rect1.Bottom < rect2.Bottom)
                    {
                        return true;
                    }
                }
                //=============================================================
                // góc dưới phải
                if (rect2.Left > rect1.Left && rect2.Left < rect1.Right)
                {
                    if (rect2.Top > rect1.Top && rect2.Top < rect1.Bottom)
                    {
                        return true;
                    }
                }
                // góc trên phải
                if (rect2.Left > rect1.Left && rect2.Left < rect1.Right)
                {
                    if (rect2.Bottom > rect1.Top && rect2.Bottom < rect1.Bottom)
                    {
                        return true;
                    }
                }
                // góc dưới trái
                if (rect2.Right > rect1.Left && rect2.Right < rect1.Right)
                {
                    if (rect2.Top > rect1.Top && rect2.Top < rect1.Bottom)
                    {
                        return true;
                    }
                }
                // góc trên trái
                if (rect2.Right > rect1.Left && rect2.Right < rect1.Right)
                {
                    if (rect2.Bottom > rect1.Top && rect2.Bottom < rect1.Bottom)
                    {
                        return true;
                    }
                }
                //=============================================================
                if (rect1.Left == rect2.Left && rect1.Right == rect2.Right &&
                    rect1.Top == rect2.Top && rect1.Bottom == rect2.Bottom)
                    return true;
                return false;
            }

        }

        #region
        public enum Direction
        {
            eLeft, eRight, eUp, eDown
        }
        public enum BulletType
        {
            eTriangleBullet, eRocketBullet
        }
        public enum ExplosionSize
        {
            eSmallExplosion, eBigExplosion
        }
        public enum EnemyTankType
        {
            // 0, 1
            eNormal, eMedium
        }
        public enum Skin
        {
            // 0, 1, 2, 3, 4, 5, 6, 7
            eGreen, eRed, eYellow, eBlue, ePurple, eLightBlue, eOrange, ePink
        }
        public enum ItemType
        {
            eItemHeart, eItemShield, eItemGrenade, eItemRocket
        }
        public enum InforStyle
        {
            eGameOver, eGameNext, eGameWin
        }
        #endregion
        class Bullet : BaseObject
        {
            private Direction directionBullet;
            private int speedBullet;
            private int power;

            // viên đạn di chuyển
            public void BulletMove()
            {
                switch (directionBullet)
                {
                    case Direction.eLeft:
                        this.RectX -= speedBullet;
                        break;
                    case Direction.eRight:
                        this.RectX += speedBullet;
                        break;
                    case Direction.eUp:
                        this.RectY -= speedBullet;
                        break;
                    case Direction.eDown:
                        this.RectY += speedBullet;
                        break;
                }
            }

            #region properties
            public Direction DirectionBullet
            {
                get
                {
                    return directionBullet;
                }

                set
                {
                    directionBullet = value;
                }
            }
            public int SpeedBullet
            {
                get
                {
                    return speedBullet;
                }

                set
                {
                    speedBullet = value;
                }
            }

            public int Power
            {
                get
                {
                    return power;
                }
                set
                {
                    power = value;
                }
            }
            #endregion
        }

        class Wall : BaseObject
        {
            private int wallNumber;

            public int WallNumber
            {
                get
                {
                    return wallNumber;
                }

                set
                {
                    wallNumber = value;
                }
            }
        }
        class Tank : BaseObject
        {
            #region số frame lớn nhất
            protected const int MAX_NUMBER_SPRITE_TANK = 7;
            protected const int MAX_NUMBER_SPRITE_EFFECT = 6;
            #endregion
            #region Số làm việc với frame (tank: có 8 frame 0-7; effect: có 6 frame 0-5)
            protected int frx_tank = 7;
            protected int frx_effect = 0;
            protected int fry_effect = 0;
            #endregion
            protected int moveSpeed;
            protected int tankBulletSpeed;
            protected int energy;
            private BulletType bulletType;
            protected Skin skinTank;
            protected bool isMove;
            private bool isActivate;
            protected bool left, right, up, down;
            protected Direction directionTank;
            private List<Bullet> bullets;
            protected Bitmap bmpEffect;

            public List<Bullet> Bullets { get; }
            public BulletType BulletTypes { get;}

            // contructor
            public Tank()
            {
                this.isActivate = false;
                this.RectWidth = Common.tankSize;
                this.RectHeight = Common.tankSize;
                this.Bullets = new List<Bullet>();
                this.BulletType = BulletType.eTriangleBullet;

            }

            // hiển thị xe tăng
            public override void Show(Bitmap background)
            {
                // nếu xe tăng đang bật chế độ hoạt động sẽ hiển thị xe tăng, 
                // ngược lại hiện thị hiệu ứng xuất hiện
                if (IsActivate)
                {
                    switch (directionTank)
                    {
                        case Direction.eUp:
                            Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                                   (int)skinTank * Common.tankSize, frx_tank * Common.tankSize, this.RectWidth, this.RectHeight);
                            break;
                        case Direction.eDown:
                            Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                                   (MAX_NUMBER_SPRITE_TANK - (int)skinTank) * Common.tankSize, frx_tank * Common.tankSize, this.RectWidth, this.RectHeight);
                            break;
                        case Direction.eLeft:
                            Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                                     frx_tank * Common.tankSize, (MAX_NUMBER_SPRITE_TANK - (int)skinTank) * Common.tankSize, this.RectWidth, this.RectHeight);
                            break;
                        case Direction.eRight:
                            Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                                frx_tank * Common.tankSize, (int)skinTank * Common.tankSize, this.RectWidth, this.RectHeight);
                            break;
                    }
                    // nếu xe tăng được di chuyển bánh xe sẽ xoay
                    if (this.isMove)
                    {
                        frx_tank--;
                        if (frx_tank == -1)
                            frx_tank = MAX_NUMBER_SPRITE_TANK;
                    }
                }
                else
                {
                    // hiển thị hiệu ứng xuất hiện
                    Common.PaintObject(background, this.bmpEffect, this.RectX, this.RectY,
                           frx_effect * this.RectWidth, fry_effect * this.RectHeight, this.RectWidth, this.RectHeight);
                    frx_effect++;
                    if (frx_effect == MAX_NUMBER_SPRITE_EFFECT)
                    {
                        frx_effect = 0;
                        fry_effect++;
                        if (fry_effect == MAX_NUMBER_SPRITE_EFFECT)
                        {
                            fry_effect = 0;
                            // hiệu ứng kết thúc, bật lại hoạt động của xe
                            IsActivate = true;
                        }
                    }
                }
            }

            // xoay frame xe tăng
            public void RotateFrame()
            {
                // xoay ảnh phù hợp với frame xe tăng
                if ((left == true && this.DirectionTank == Direction.eDown) ||
                    (right == true && this.DirectionTank == Direction.eUp) ||
                    (up == true && this.DirectionTank == Direction.eLeft) ||
                    (down == true && this.DirectionTank == Direction.eRight))
                {
                    this.bmpObject.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else if ((left == true && this.DirectionTank == Direction.eUp) ||
                   (right == true && this.DirectionTank == Direction.eDown) ||
                   (up == true && this.DirectionTank == Direction.eRight) ||
                   (down == true && this.DirectionTank == Direction.eLeft))
                {
                    this.bmpObject.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }
                else if ((left == true && this.DirectionTank == Direction.eRight) ||
                   (right == true && this.DirectionTank == Direction.eLeft) ||
                   (up == true && this.DirectionTank == Direction.eDown) ||
                   (down == true && this.DirectionTank == Direction.eUp))
                {
                    this.bmpObject.RotateFlip(RotateFlipType.Rotate180FlipNone);
                }
                else
                {
                    this.bmpObject.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                }
                // cập nhật hướng của xe tăng
                if (left)
                    directionTank = Direction.eLeft;
                else if (right)
                    directionTank = Direction.eRight;
                else if (up)
                    directionTank = Direction.eUp;
                else if (down)
                    directionTank = Direction.eDown;
            }

            // tạo đạn cho xe tăng
            public void CreatBullet(string pathRoundBullet, string pathRocketBullet)
            {
                if (this.bullets.Count == 0 && this.IsActivate)
                {
                    // đạn
                    Bullet bullet;
                    bullet = new Bullet();
                    bullet.SpeedBullet = this.TankBulletSpeed;

                    // set loại bullet
                    switch (this.bulletType)
                    {
                        case BulletType.eTriangleBullet:
                            bullet.LoadImage(Common.path + pathRoundBullet);
                            // đạn tam giác có kích thước 8x8
                            bullet.RectWidth = 8;
                            bullet.RectHeight = 8;
                            // năng lượng của đạn tam giác mặc định là 10
                            bullet.Power = 10;
                            break;
                        case BulletType.eRocketBullet:
                            bullet.LoadImage(Common.path + pathRocketBullet);
                            // đạn rocket có kích thước 12x12
                            bullet.RectWidth = 12;
                            bullet.RectHeight = 12;
                            // năng lượng của đạn rocket mặc định là 40
                            bullet.Power = 30;
                            break;
                    }
                    // hướng của xe tăng
                    switch (directionTank)
                    {
                        case Direction.eLeft:
                            bullet.DirectionBullet = Direction.eLeft;
                            bullet.BmpObject.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            bullet.RectX = this.RectX + bullet.RectWidth;
                            bullet.RectY = this.RectY + this.RectHeight / 2 - bullet.RectHeight / 2;
                            break;
                        case Direction.eRight:
                            bullet.DirectionBullet = Direction.eRight;
                            bullet.BmpObject.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            bullet.RectX = this.RectX + this.RectWidth - bullet.RectWidth;
                            bullet.RectY = this.RectY + this.RectHeight / 2 - bullet.RectHeight / 2;
                            break;
                        case Direction.eUp:
                            bullet.DirectionBullet = Direction.eUp;
                            bullet.RectY = this.RectY + bullet.RectHeight;
                            bullet.RectX = this.RectX + this.RectWidth / 2 - bullet.RectWidth / 2;
                            break;
                        case Direction.eDown:
                            bullet.DirectionBullet = Direction.eDown;
                            bullet.BmpObject.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            bullet.RectY = this.RectY + this.RectHeight - bullet.RectHeight;
                            bullet.RectX = this.RectX + this.RectWidth / 2 - bullet.RectWidth / 2;
                            break;
                    }
                    this.bullets.Add(bullet);
                    bullet = null;
                }
            }

            // hủy một viên đạn
            public void RemoveOneBullet(int index)
            {
                this.bullets[index] = null;
                this.bullets.RemoveAt(index);
            }

            // di chuyển và hiển thị đạn xe tăng
            public void ShowBulletAndMove(Bitmap background)
            {
                for (int i = 0; i < this.Bullets.Count; i++)
                {
                    this.Bullets[i].BulletMove();
                    this.Bullets[i].Show(background);
                }
            }

            // kiểm tra va chạm của xe tăng với một đối tượng
            public bool IsObjectCollision(Rectangle rectObj)
            {
                switch (this.directionTank)
                {
                    case Direction.eLeft:
                        if (this.Rect.Left == rectObj.Right)
                            if (this.Rect.Top >= rectObj.Top && this.Rect.Top < rectObj.Bottom ||
                                this.Rect.Bottom > rectObj.Top && this.Rect.Bottom <= rectObj.Bottom ||
                                this.Rect.Bottom > rectObj.Bottom && this.Rect.Top < rectObj.Top)
                            {
                                return true;
                            }
                        break;
                    case Direction.eRight:
                        if (this.Rect.Right == rectObj.Left)
                            if (this.Rect.Top >= rectObj.Top && this.Rect.Top < rectObj.Bottom ||
                                this.Rect.Bottom > rectObj.Top && this.Rect.Bottom <= rectObj.Bottom ||
                                this.Rect.Bottom > rectObj.Bottom && this.Rect.Top < rectObj.Top)
                            {
                                return true;
                            }
                        break;
                    case Direction.eUp:
                        if (this.Rect.Top == rectObj.Bottom)
                            if (this.Rect.Left < rectObj.Right && this.Rect.Left >= rectObj.Left ||
                                this.Rect.Right > rectObj.Left && this.Rect.Right <= rectObj.Right ||
                                this.Rect.Right > rectObj.Right && this.Rect.Left < rectObj.Left)
                            {
                                return true;
                            }
                        break;
                    case Direction.eDown:
                        if (this.Rect.Bottom == rectObj.Top)
                            if (this.Rect.Left < rectObj.Right && this.Rect.Left >= rectObj.Left ||
                                this.Rect.Right > rectObj.Left && this.Rect.Right <= rectObj.Right ||
                                this.Rect.Right >= rectObj.Right && this.Rect.Left <= rectObj.Left)
                            {
                                return true;
                            }
                        break;
                }
                return false;
            }

            // kiểm tra xe tăng chạm tường
            public bool IsWallCollision(List<Wall> walls, Direction directionTank)
            {
                foreach (Wall wall in walls)
                    // nếu không phải bụi cây thì xét va chạm
                    if (wall.WallNumber != 4)
                        if (IsObjectCollision(wall.Rect))
                            return true;
                return false;
            }

            // xe tăng di chuyển
            public void Move()
            {
                if (this.IsActivate)
                {
                    if (left)
                    {
                        this.RectX -= this.MoveSpeed;
                    }
                    else if (right)
                    {
                        this.RectX += this.MoveSpeed;
                    }
                    else if (up)
                    {
                        this.RectY -= this.MoveSpeed;
                    }
                    else if (down)
                    {
                        this.RectY += this.MoveSpeed;
                    }
                }
            }
            #region properties
            public int MoveSpeed
            {
                get
                {
                    return moveSpeed;
                }

                set
                {
                    moveSpeed = value;
                }
            }
            public int TankBulletSpeed
            {
                get
                {
                    return tankBulletSpeed;
                }

                set
                {
                    tankBulletSpeed = value;
                }
            }
            public int Energy
            {
                get
                {
                    return energy;
                }

                set
                {
                    energy = value;
                }
            }
            public bool Left
            {
                get
                {
                    return left;
                }

                set
                {
                    left = value;
                }
            }
            public bool Right
            {
                get
                {
                    return right;
                }

                set
                {
                    right = value;
                }
            }
            public bool Up
            {
                get
                {
                    return up;
                }

                set
                {
                    up = value;
                }
            }
            public bool Down
            {
                get
                {
                    return down;
                }

                set
                {
                    down = value;
                }
            }
            public Direction DirectionTank
            {
                get
                {
                    return directionTank;
                }

                set
                {
                    directionTank = value;
                }
            }
            public bool IsMove
            {
                get
                {
                    return isMove;
                }

                set
                {
                    isMove = value;
                }
            }
            private List<Bullet> bulletsList; // Renamed to bulletsList to avoid ambiguity

            public List<Bullet> bulletList
            {
                get
                {
                    return bulletsList;
                }

                set
                {
                    bulletsList = value;
                }
            }
            public Skin SkinTank
            {
                get
                {
                    return skinTank;
                }

                set
                {
                    skinTank = value;
                }
            }

            public bool IsActivate
            {
                get
                {
                    return isActivate;
                }

                set
                {
                    isActivate = value;
                }
            }

            public BulletType BulletType
            {
                get
                {
                    return bulletType;
                }
                set
                {
                    bulletType = value;
                }
            }
            #endregion property
        }
        class PlayerTank : Tank
        {
            private bool isShield;
            private Bitmap bmpShield;
            public int PositionX { get; set; }
            public int PositionY { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public bool isHost { get; set; }
            public string Id { get; set; }
            public string PlayerName { get; set; }
            public bool IsReady { get; set; } = false;

            public PlayerTank()
            {
                this.moveSpeed = 10;
                this.tankBulletSpeed = 20;
                this.energy = 100;
                this.SetLocation();
                this.DirectionTank = Direction.eUp;
                this.SkinTank = Skin.eYellow;
                bmpEffect = new Bitmap(Common.path + @"\Images\effect1.png");
                bmpShield = new Bitmap(Common.path + @"\Images\shield.png");
            }

            // cập nhật vị trí xe tăng player
            public void SetLocation()
            {
                int i = 17, j = 36;
                this.RectX = i * Common.STEP;
                this.RectY = j * Common.STEP;
            }

          
            

            // hiển thị xe tăng player
            public override void Show(Bitmap background)
            {
                // nếu xe tăng đang bật chế độ hoạt động sẽ hiển thị xe tăng, 
                // ngược lại hiện thị hiệu ứng xuất hiện
                if (IsActivate)
                {
                    switch (directionTank)
                    {
                        case Direction.eUp:
                            Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                                   (int)skinTank * Common.tankSize, frx_tank * Common.tankSize, this.RectWidth, this.RectHeight);
                            break;
                        case Direction.eDown:
                            Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                                   (MAX_NUMBER_SPRITE_TANK - (int)skinTank) * Common.tankSize, frx_tank * Common.tankSize, this.RectWidth, this.RectHeight);
                            break;
                        case Direction.eLeft:
                            Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                                     frx_tank * Common.tankSize, (MAX_NUMBER_SPRITE_TANK - (int)skinTank) * Common.tankSize, this.RectWidth, this.RectHeight);
                            break;
                        case Direction.eRight:
                            Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                                frx_tank * Common.tankSize, (int)skinTank * Common.tankSize, this.RectWidth, this.RectHeight);
                            break;
                    }
                    // nếu xe tăng player đang ở chế độ được bảo vệ -> show vòng tròn bảo vệ
                    if (this.isShield)
                    {
                        Common.PaintObject(background, this.bmpShield, rect.X, rect.Y, 0, 0, 40, 40);
                    }
                    //nếu xe tăng được di chuyển bánh xe sẽ xoay
                    if (this.isMove)
                    {
                        frx_tank--;
                        if (frx_tank == -1)
                            frx_tank = MAX_NUMBER_SPRITE_TANK;
                    }
                }
                else
                {
                    // hiển thị hiệu ứng xuất hiện
                    Common.PaintObject(background, this.bmpEffect, this.RectX, this.RectY,
                           frx_effect * this.RectWidth, fry_effect * this.RectHeight, this.RectWidth, this.RectHeight);
                    frx_effect++;
                    if (frx_effect == MAX_NUMBER_SPRITE_EFFECT)
                    {
                        frx_effect = 0;
                        fry_effect++;
                        if (fry_effect == MAX_NUMBER_SPRITE_EFFECT)
                        {
                            fry_effect = 0;
                            // hiệu ứng kết thúc, bật lại hoạt động của xe
                            IsActivate = true;
                        }
                    }
                }
            }

            #region properties
            public bool IsShield
            {
                get { return isShield; }
                set { isShield = value; }
            }
            #endregion 
        }
    }
}