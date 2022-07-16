
using System.Runtime.CompilerServices;

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
            _shipsInitialPoint = new[] {new Point(0,0),
                Mine1.Location,
                Mine2.Location,
                Mine3.Location,
                MineLayer1.Location,
                MineLayer2.Location,
                MineLayer3.Location,
                MineLayer4.Location,
                Cruiser1.Location,
                Cruiser2.Location,
                Cruiser3.Location,
                Frigate1.Location,
                Frigate2.Location,
                FlagShip1.Location
            };
        }




        private bool _dragButton; // suruklenmedurumu
        private Point _getFirstPoint; // ilkkonumAl
        private Point[] _shipsInitialPoint; //gemilerin ilk konumu
        private readonly List<string> _oldButton = new(); // eski butonun ismi
        private Button _newButton = new(); // yeni buton
        private int _counter; // sayaç
        private int _setArrayX, _setArrayY;// geminin dizideki alanını belirtme
        private int _pointX, _pointY; // i ve j yerine geçiyor
        private readonly string[] _shipXPointArray = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J"}; // GemiHarfKonumu
        private string _shipXPoint = "";
        private readonly int[,] _buttonsOnFormLocationX = new int[10, 10]; // butonlarFormKonumuX
        private readonly int[,] _buttonsOnFormLocationY = new int[10, 10]; // butonlarFormKonumuY
        private readonly Button[] _buttonsOnPanel = new Button[100]; // paneldeki butonlar
        private readonly List<string> _buttonIsEmpty = new(); // buton boş mu
        private readonly List<string> _tryStrings = new();//isim değişecek
        private bool _changeShipRotate;

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
            for (_pointX = 0; _pointX < _setArrayX; _pointX++)
            {
                if (ship.Location.X < _buttonsOnFormLocationX[0, _pointX] ||
                    ship.Location.X >= (_buttonsOnFormLocationX[0, _pointX] + 50)) continue;
                for (_pointY = 0; _pointY < _setArrayY; _pointY++)
                {
                    if (ship.Location.Y < _buttonsOnFormLocationY[_pointY, 0] ||
                        ship.Location.Y >= (_buttonsOnFormLocationY[_pointY, 0] + 50)) continue;
                        _shipXPoint = _shipXPointArray[_pointX];
                        findButtonsOfShip(ship);
                    break;
                }
                break;
            }
            if (!downUp) return;
            if (_pointX != _setArrayX && _pointY != _setArrayY)
            {
                buttonCheck(ship);
            }
            else
            {
                ship.Location = _shipsInitialPoint[0];
                InfoLabel.Text = "";
                foreach (var t in _oldButton)
                {
                    _buttonIsEmpty.Remove(t);
                }
               
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

            if (!buttonsAreEmpty(_oldButton,0))
            {
                if (!buttonsAreEmpty(_buttonIsEmpty,1))
                {
                    ship.Location = new Point(_newButton.Location.X + panel1.Location.X + 3, _newButton.Location.Y + panel1.Location.Y + 3);
                    string gecici = null;//silinecek
                    foreach (var a in _tryStrings)
                    {
                        _buttonIsEmpty.Add(a);
                        gecici += a+"-";//silinecek

                    }
                    InfoLabel.Text = ship.Name + @" " + gecici + @" Bölgesine Yerleştirildi.";//silinecek
                    foreach (var a in _oldButton)
                    {
                        _buttonIsEmpty.Remove(a);
                    }
                }
                else
                {
                    InfoLabel2.Text = "Bu alan Dolu";
                    Timer1.Start();
                    if (_oldButton.Count>0)
                    {
                        foreach (var variable in _buttonsOnPanel)
                        {
                            if (_oldButton.IndexOf(variable.Name) == -1) continue;
                            if (_changeShipRotate && ((ship.Size.Width / 44) > 1 || (ship.Size.Height / 44) > 1))
                            {
                                ship.Size = new Size(ship.Size.Height, ship.Size.Width);
                                ship.AccessibleName = ship.AccessibleName is "H" ? "V" : "H";
                            }
                            _changeShipRotate = false;
                            ship.Location =  new Point(variable.Location.X + panel1.Location.X + 3,
                                    variable.Location.Y + panel1.Location.Y + 3);


                            break;
                        }
                    }
                    else
                    {
                        ship.Location = _shipsInitialPoint[0];
                    }
                }
            }
            else
            {
                ship.Location = new Point(_newButton.Location.X + panel1.Location.X + 3, _newButton.Location.Y + panel1.Location.Y + 3);
                _oldButton.Clear();
            }
        }
        private bool buttonsAreEmpty(ICollection<string> a,int i)
        {
            var c = false;
            var counter2 = 0;
            foreach (var t in _tryStrings)
            {
                if (_tryStrings.Count > 1 && i == 1)
                {
                    counter2++;
                    if (_oldButton.Contains(t))
                    {
                        _buttonIsEmpty.Remove(_oldButton.FirstOrDefault(t));//hatamız burada.
                        if (_tryStrings.Count == counter2)
                        {
                            return c;
                        }
                    }
                    else
                    {
                        c = a.Contains(t);
                        if (c) return true;
                        if (_tryStrings.Count == counter2)
                        {
                            return c;
                        }
                    }
                }
                c = a.Contains(t);
                if (_tryStrings.Count == 1 && i == 1) return c; 
                if (i == 0 && !c) return false;
            }
            if (i == 0 && c) return true;
            return c;

        }
        private void findButtonsOfShip(Control ship)
        {
            _tryStrings.Clear();
            switch (ship.AccessibleName)
            {
                case @"H":
                {
                    for (var i = 0; i < (ship.Size.Width / 44); i++)
                    {
                        _tryStrings.Add(_shipXPointArray[_pointX + i] + (_pointY + 1));
                    }

                    break;
                }
                case @"V":
                {
                    for (var i = 0; i < (ship.Size.Height / 44); i++)
                    {
                        _tryStrings.Add(_shipXPointArray[_pointX] + (_pointY + 1 + i));
                    }
                    break;
                }
            }
        }


        protected void OnMouseDown(MouseEventArgs e,Button ship, int i)
        {
            base.OnMouseClick(e);
            Controls[ship.Name].BringToFront();
            _shipsInitialPoint[0] = _shipsInitialPoint[i];
            if (e.Button == MouseButtons.Left)
            {
                _dragButton = true;
            }
            _getFirstPoint = e.Location;
            buttonMatch(ship, false);
            _oldButton.Clear();
            if (_pointX == _setArrayX) return;
            if (_buttonsOnPanel.All(variable => variable.Name != _shipXPoint + (_pointY + 1))) return;
            findButtonsOfShip(ship);
            foreach (var t in _tryStrings)
            {
                _oldButton.Add(t);
            }
        }
        protected void OnMouseMove(MouseEventArgs e, Button ship)
        {
            base.OnMouseMove(e);
            if (!_dragButton) return;
            ship.Left = e.X + ship.Left - (_getFirstPoint.X);
            ship.Top = e.Y + ship.Top - (_getFirstPoint.Y);
        }
        protected void OnMouseUp(MouseEventArgs e,Button ship)
        {
            base.OnMouseUp(e);
            _dragButton = false;
            if (e.Button == MouseButtons.Right)
            {
                if ((ship.Size.Width / 44) > 1 || (ship.Size.Height / 44) > 1)
                {
                    ship.Size = new Size(ship.Size.Height, ship.Size.Width);
                    ship.AccessibleName = ship.AccessibleName is "H" ? "V" : "H";
                    _changeShipRotate = true;
                }
                else
                {
                    return;
                }
            }
            else
            {
                _changeShipRotate = false;
            }
            switch (ship.AccessibleName)
            {
                case @"H":
                    _setArrayX = 10 - (ship.Size.Width / 44) + 1;
                    _setArrayY = 10;
                    break;
                case @"V":
                    _setArrayX = 10;
                    _setArrayY = 10 - (ship.Size.Height / 44) + 1;
                    break;
            }

            buttonMatch(ship, true);

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            InfoLabel2.Text = null;
        }

     
        private void mouseDown(object sender, MouseEventArgs e)
        {
            switch (sender)
            {
                case var value when value == Mine1:
                {
                    OnMouseDown(e, Mine1, 1);
                    break;
                }
                case var value when value == Mine2:
                {
                    OnMouseDown(e, Mine2, 2);
                    break;
                }
                case var value when value == Mine3:
                {
                    OnMouseDown(e, Mine3, 3);
                    break;
                }
                case var value when value == MineLayer1:
                {
                    OnMouseDown(e, MineLayer1, 4);
                    break;
                }
                case var value when value == MineLayer2:
                {
                    OnMouseDown(e, MineLayer2, 5);
                    break;
                }
                case var value when value == MineLayer3:
                {
                    OnMouseDown(e, MineLayer3, 6);
                    break;
                }
                case var value when value == MineLayer4:
                {
                    OnMouseDown(e, MineLayer4, 7);
                    break;
                }
                case var value when value == Cruiser1:
                {
                    OnMouseDown(e, Cruiser1, 8);
                    break;
                }
                case var value when value == Cruiser2:
                {
                    OnMouseDown(e, Cruiser2, 9);
                    break;
                }
                case var value when value == Cruiser3:
                {
                    OnMouseDown(e, Cruiser3, 10);
                    break;
                }
                case var value when value == Frigate1:
                {
                    OnMouseDown(e, Frigate1, 11);
                    break;
                }
                case var value when value == Frigate2:
                {
                    OnMouseDown(e, Frigate2, 12);
                    break;
                }
                case var value when value == FlagShip1:
                {
                    OnMouseDown(e, FlagShip1, 13);
                    break;
                }
            }

        }
        private void mouseUp(object sender, MouseEventArgs e)
        {
            switch (sender)
            {
                case var value when value == Mine1:
                    {
                        OnMouseUp(e, Mine1);
                        break;
                    }
                case var value when value == Mine2:
                    {
                        OnMouseUp(e, Mine2);
                        break;
                    }
                case var value when value == Mine3:
                    {
                        OnMouseUp(e, Mine3);
                        break;
                    }
                case var value when value == MineLayer1:
                    {
                        OnMouseUp(e, MineLayer1);
                        break;
                    }
                case var value when value == MineLayer2:
                    {
                        OnMouseUp(e, MineLayer2);
                        break;
                    }
                case var value when value == MineLayer3:
                    {
                        OnMouseUp(e, MineLayer3);
                        break;
                    }
                case var value when value == MineLayer4:
                    {
                        OnMouseUp(e, MineLayer4);
                        break;
                    }
                case var value when value == Cruiser1:
                    {
                        OnMouseUp(e, Cruiser1);
                        break;
                    }
                case var value when value == Cruiser2:
                    {
                        OnMouseUp(e, Cruiser2);
                        break;
                    }
                case var value when value == Cruiser3:
                    {
                        OnMouseUp(e, Cruiser3);
                        break;
                    }
                case var value when value == Frigate1:
                    {
                        OnMouseUp(e, Frigate1);
                        break;
                    }
                case var value when value == Frigate2:
                    {
                        OnMouseUp(e, Frigate2);
                        break;
                    }
                case var value when value == FlagShip1:
                    {
                        OnMouseUp(e, FlagShip1);
                        break;
                    }
            }

        }
        private void mouseMove(object sender, MouseEventArgs e)
        {
            switch (sender)
            {
                case var value when value == Mine1:
                    {
                        OnMouseMove(e, Mine1);
                        break;
                    }
                case var value when value == Mine2:
                    {
                        OnMouseMove(e, Mine2);
                        break;
                    }
                case var value when value == Mine3:
                    {
                        OnMouseMove(e, Mine3);
                        break;
                    }
                case var value when value == MineLayer1:
                    {
                        OnMouseMove(e, MineLayer1);
                        break;
                    }
                case var value when value == MineLayer2:
                    {
                        OnMouseMove(e, MineLayer2);
                        break;
                    }
                case var value when value == MineLayer3:
                    {
                        OnMouseMove(e, MineLayer3);
                        break;
                    }
                case var value when value == MineLayer4:
                    {
                        OnMouseMove(e, MineLayer4);
                        break;
                    }
                case var value when value == Cruiser1:
                    {
                        OnMouseMove(e, Cruiser1);
                        break;
                    }
                case var value when value == Cruiser2:
                    {
                        OnMouseMove(e, Cruiser2);
                        break;
                    }
                case var value when value == Cruiser3:
                    {
                        OnMouseMove(e, Cruiser3);
                        break;
                    }
                case var value when value == Frigate1:
                    {
                        OnMouseMove(e, Frigate1);
                        break;
                    }
                case var value when value == Frigate2:
                    {
                        OnMouseMove(e, Frigate2);
                        break;
                    }
                case var value when value == FlagShip1:
                    {
                        OnMouseMove(e, FlagShip1);
                        break;
                    }
            }

        }
        

    }
}