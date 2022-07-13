
namespace AdmiralSunk
{
    public partial class Game : Form
    {
        public Game()
        {
            InitializeComponent();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            buttonSetLocation();
            _shipPoint = new[] {new Point(0,0),
                Mine1.Location,
                Mine2.Location,
                Mine3.Location,
                Cruiser1.Location,
                new Point(0,0),
                new Point(0,0),
                new Point(0,0),
                new Point(0,0),
                new Point(0,0),
                new Point(0,0),
                new Point(0,0),
                new Point(0,0)
            };
        }

        private void Mine1_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e,Mine1,1);
        }
        private void Mine1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_dragButton) return;
            Mine1.Left = e.X + Mine1.Left - (_getFirstPoint.X);
            Mine1.Top = e.Y + Mine1.Top - (_getFirstPoint.Y);
        }
        private void Mine1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragButton = false;
            buttonMatch(Mine1, true);
        }

        private void Mine2_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e, Mine2,2);
        }
        private void Mine2_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_dragButton) return;
            Mine2.Left = e.X + Mine2.Left - (_getFirstPoint.X);
            Mine2.Top = e.Y + Mine2.Top - (_getFirstPoint.Y);
        }
        private void Mine2_MouseUp(object sender, MouseEventArgs e)
        {
            _dragButton = false;
            buttonMatch(Mine2, true);
        }

        private void Mine3_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e, Mine3,3);
            
        }
        private void Mine3_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_dragButton) return;
            Mine3.Left = e.X + Mine3.Left - (_getFirstPoint.X);
            Mine3.Top = e.Y + Mine3.Top - (_getFirstPoint.Y);
        }
        private void Mine3_MouseUp(object sender, MouseEventArgs e)
        {
            _dragButton = false;
            buttonMatch(Mine3, true);
        }

        private void Cruiser1_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e, Cruiser1,4);
        }
        private void Cruiser1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_dragButton) return;
            Cruiser1.Left = e.X + Cruiser1.Left - (_getFirstPoint.X);
            Cruiser1.Top = e.Y + Cruiser1.Top - (_getFirstPoint.Y);
        }
        private void Cruiser1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragButton = false;
            buttonMatch(Cruiser1, true);
            if (e.Button != MouseButtons.Right) return;//  11.07.2022
            Cruiser1.Size = new Size(Cruiser1.Size.Height, Cruiser1.Size.Width);//  11.07.2022
            Cruiser1.AccessibleName = Cruiser1.AccessibleName is "H" ? "V" : "H";//  11.07.2022
        }


        private bool _dragButton;
        private Point _getFirstPoint;
        private Point[] _shipPoint;
        private string _oldButton;
        private Button _newButton = new();
        private int _counter;
        private string _shipXPoint = "";
        private int _pointX, _pointY;
        private readonly int[,] _buttonsOnFormLocationX = new int[10, 10];
        private readonly int[,] _buttonsOnFormLocationY = new int[10, 10];
        private readonly Button[] _buttonsOnPanel = new Button[100];
        private readonly List<string> _buttonIsEmpty = new();

        private void buttonSetLocation()
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    _buttonsOnFormLocationX[i, j] = panel1.Controls[_counter].Location.X + panel1.Location.X;
                    _buttonsOnFormLocationY[i, j] = panel1.Controls[_counter].Location.Y + panel1.Location.Y;
                    _buttonsOnPanel[_counter] = (Button) panel1.Controls[_counter];
                    _counter++;
                }
            }
        }
        private void buttonMatch(Control ship, bool downUp)
        {
            for (_pointX = 0; _pointX < 10; _pointX++)
            {
                if (ship.Location.X < _buttonsOnFormLocationX[0, _pointX] ||
                    ship.Location.X >= (_buttonsOnFormLocationX[0, _pointX] + 50)) continue;
                for (_pointY = 0; _pointY < 10; _pointY++)
                {
                    if (ship.Location.Y < _buttonsOnFormLocationY[_pointY, 0] ||
                        ship.Location.Y >= (_buttonsOnFormLocationY[_pointY, 0] + 50)) continue;
                    _shipXPoint = _pointX switch
                    {
                        0 => "A",
                        1 => "B",
                        2 => "C",
                        3 => "D",
                        4 => "E",
                        5 => "F",
                        6 => "G",
                        7 => "H",
                        8 => "I",
                        9 => "J",
                        _ => _shipXPoint
                    };
                    break;
                }
                break;
            }
            if (!downUp) return;
            if (_pointX != 10 && _pointY != 10)
            {
                buttonCheck(ship);
            }
            else
            {
                ship.Location = _shipPoint[0];
                InfoLabel.Text = "";
                if (_oldButton == "") return;
                _buttonIsEmpty.Remove(_oldButton);
            }
        }

        private void buttonCheck(Control ship)
        {
            foreach (var button in _buttonsOnPanel)
            {
                if (button.Name != _shipXPoint + (_pointY + 1)) continue;
                _newButton = button;
                break;
            }
            if (_oldButton != _newButton.Name)
            {
                if (_buttonIsEmpty.IndexOf(_shipXPoint + (_pointY + 1))==-1)
                {
                    ship.Location = new Point(_newButton.Location.X + panel1.Location.X + 3, _newButton.Location.Y + panel1.Location.Y + 3);
                    InfoLabel.Text = ship.Name + @" " + _newButton.Name + @" Bölgesine Yerleştirildi.";
                    _buttonIsEmpty.Add(_newButton.Name);
                    _buttonIsEmpty.Remove(_oldButton);
                }
                else
                {
                    InfoLabel2.Text = "Bu alan Dolu";
                    Timer1.Start();
                    if (_oldButton != "")
                    {
                        foreach (var variable in _buttonsOnPanel)
                        {
                            if (variable.Name != _oldButton) continue;
                            ship.Location =  new Point(variable.Location.X + panel1.Location.X + 3,
                                    variable.Location.Y + panel1.Location.Y + 3);
                            break;
                        }
                    }
                    else
                    {
                        ship.Location = _shipPoint[0];
                    }
                }
            }
            else
            {
                ship.Location = new Point(_newButton.Location.X + panel1.Location.X + 3, _newButton.Location.Y + panel1.Location.Y + 3);
                _oldButton = "";
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            InfoLabel2.Text = null;
        }

        protected void OnMouseDown(MouseEventArgs e,Button ship, int i)
        {
            base.OnMouseClick(e);
            Controls[ship.Name].BringToFront();//  11.07.2022
            _shipPoint[0] = _shipPoint[i];//  11.07.2022
            if (e.Button != MouseButtons.Left) return;
            _dragButton = true;
            _getFirstPoint = e.Location;
            buttonMatch(ship, false);
            _oldButton = "";
            //_shipPoint[0] = _shipPoint[i];  11.07.2022
            if (_pointX == 10) return;
            foreach (var variable in _buttonsOnPanel)
            {
                if (variable.Name != _shipXPoint + (_pointY + 1)) continue;
                _oldButton = variable.Name;
                break;
            }

        }


    }
}
