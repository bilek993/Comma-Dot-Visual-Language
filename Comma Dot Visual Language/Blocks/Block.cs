using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Comma_Dot_Visual_Language.Helpers;
using System;
using System.Collections.Generic;

namespace Comma_Dot_Visual_Language.Blocks
{
    public abstract class Block : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; protected set; }
        public Block NextBlockPrimary { get; private set; }
        public Block NextBlockOptional { get; private set; }
        public List<Block> PreviousBlocks { get; private set; }

        protected Image Shape;

        private static readonly double ArrowLength = 5;

        private string _command = "";
        private double _connectionInputX;
        private double _connectionInputY;
        private double _connectionOutput1X;
        private double _connectionOutput1Y;
        private double _connectionOutput2X;
        private double _connectionOutput2Y;
        private double _connection1ArrowVector1X;
        private double _connection1ArrowVector1Y;
        private double _connection1ArrowVector2X;
        private double _connection1ArrowVector2Y;
        private double _connection2ArrowVector1X;
        private double _connection2ArrowVector1Y;
        private double _connection2ArrowVector2X;
        private double _connection2ArrowVector2Y;
        private readonly PropertiesManager _propertiesManager;
        private string _prefixCommand = "";
        private string _suffixCommand = "";
        private TextBlock _textBlockCommand;
        private Path _lineConnectionPrimary;
        private Path _lineConnectionOptional;
        private Line _connection1arrow1;
        private Line _connection1arrow2;
        private Line _connection2arrow1;
        private Line _connection2arrow2;
        private bool _isPressed;
        private readonly Canvas _canvasBlocks;
        private readonly int _maxConnectionsCount;
        protected static int _blocksCounter = 0;

        public string Command
        {
            get => _command;
            set
            {
                _command = value;
                _textBlockCommand.Text = _prefixCommand + value + _suffixCommand;
            }
        }

        public double ConnectionInputX
        {
            get => _connectionInputX;
            set
            {
                _connectionInputX = value;
                OnPropertyChanged("ConnectionInputX");
            }
        }
        public double ConnectionInputY
        {
            get => _connectionInputY;
            set
            {
                _connectionInputY = value;
                OnPropertyChanged("ConnectionInputY");
            }
        }
        public double ConnectionOutput1X
        {
            get => _connectionOutput1X;
            set
            {
                _connectionOutput1X = value;
                OnPropertyChanged("ConnectionOutput1X");
            }
        }
        public double ConnectionOutput1Y
        {
            get => _connectionOutput1Y;
            set
            {
                _connectionOutput1Y = value;
                OnPropertyChanged("ConnectionOutput1Y");
            }
        }
        public double ConnectionOutput2X
        {
            get => _connectionOutput2X;
            set
            {
                _connectionOutput2X = value;
                OnPropertyChanged("ConnectionOutput2X");
            }
        }
        public double ConnectionOutput2Y
        {
            get => _connectionOutput2Y;
            set
            {
                _connectionOutput2Y = value;
                OnPropertyChanged("ConnectionOutput2Y");
            }
        }
        public double Connection1ArrowVector1X
        {
            get => _connection1ArrowVector1X;
            set
            {
                _connection1ArrowVector1X = value;
                OnPropertyChanged("Connection1ArrowVector1X");
            }
        }
        public double Connection1ArrowVector1Y
        {
            get => _connection1ArrowVector1Y;
            set
            {
                _connection1ArrowVector1Y = value;
                OnPropertyChanged("Connection1ArrowVector1Y");
            }
        }
        public double Connection1ArrowVector2X
        {
            get => _connection1ArrowVector2X;
            set
            {
                _connection1ArrowVector2X = value;
                OnPropertyChanged("Connection1ArrowVector2X");
            }
        }
        public double Connection1ArrowVector2Y
        {
            get => _connection1ArrowVector2Y;
            set
            {
                _connection1ArrowVector2Y = value;
                OnPropertyChanged("Connection1ArrowVector2Y");
            }
        }
        public double Connection2ArrowVector1X
        {
            get => _connection2ArrowVector1X;
            set
            {
                _connection2ArrowVector1X = value;
                OnPropertyChanged("Connection2ArrowVector1X");
            }
        }
        public double Connection2ArrowVector1Y
        {
            get => _connection2ArrowVector1Y;
            set
            {
                _connection2ArrowVector1Y = value;
                OnPropertyChanged("Connection2ArrowVector1Y");
            }
        }
        public double Connection2ArrowVector2X
        {
            get => _connection2ArrowVector2X;
            set
            {
                _connection2ArrowVector2X = value;
                OnPropertyChanged("Connection2ArrowVector2X");
            }
        }
        public double Connection2ArrowVector2Y
        {
            get => _connection2ArrowVector2Y;
            set
            {
                _connection2ArrowVector2Y = value;
                OnPropertyChanged("Connection2ArrowVector2Y");
            }
        }

        protected Block(Canvas canvas, int maxConnectionsCount, PropertiesManager propertiesManager)
        {
            Id = _blocksCounter++;
            PreviousBlocks = new List<Block>();
            _canvasBlocks = canvas;
            _maxConnectionsCount = maxConnectionsCount;
            _propertiesManager = propertiesManager;
        }

        protected void AddShapeToCanvas()
        {
            _canvasBlocks.Children.Add(Shape);
            ChildrenAddEvents();
        }
        public void RemoveFromCanvas()
        {
            _canvasBlocks.Children.Remove(Shape);
            _canvasBlocks.Children.Remove(_textBlockCommand);
        }

        protected void AddTextBlockToCanvas(string prefix = "", string suffix = "")
        {
            _textBlockCommand = new TextBlock()
            {
                Foreground = new SolidColorBrush(Colors.White),
            };

            _prefixCommand = prefix;
            _suffixCommand = suffix;
            _canvasBlocks.Children.Add(_textBlockCommand);
            ChildrenAddEvents();
        }

        private void ChildrenAddEvents()
        {
            _canvasBlocks.Children[_canvasBlocks.Children.Count - 1].MouseLeftButtonDown += OnMouseLeftButtonDown;
            _canvasBlocks.Children[_canvasBlocks.Children.Count - 1].MouseLeftButtonUp += OnMouseLeftButtonUp;
            _canvasBlocks.Children[_canvasBlocks.Children.Count - 1].MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!_isPressed)
                return;

            var mousePosition = mouseEventArgs.GetPosition(_canvasBlocks);
            SetPosition(mousePosition.X - (Shape.ActualWidth / 2), mousePosition.Y - (Shape.ActualHeight / 2));
        }

        public void SetPosition(double left, double top)
        {
            Canvas.SetLeft(Shape, left);
            Canvas.SetTop(Shape, top);

            if (Shape.ActualWidth == 0 || Shape.ActualHeight == 0)
            {
                Shape.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                _textBlockCommand.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            Canvas.SetLeft(_textBlockCommand, left + 20f);
            Canvas.SetTop(_textBlockCommand, top + 9.5f);

            SetConnectionsPositions(left, top);

            SetArrowPositions();
            SetBezierCurvesPosition();
            foreach (Block block in PreviousBlocks)
            {
                block.SetArrowPositions();
                block.SetBezierCurvesPosition();
            }
        }

        public double getPositionX()
        {
            return Canvas.GetLeft(Shape);
        }

        public double getPositionY()
        {
            return Canvas.GetTop(Shape);
        }

        protected virtual void SetConnectionsPositions(double shapeLeft, double shapeTop)
        {

            ConnectionInputX = shapeLeft + Shape.ActualWidth / 2;
            ConnectionInputY = shapeTop + 4f;

            ConnectionOutput1X = ConnectionOutput2X = shapeLeft + Shape.ActualWidth / 2;
            ConnectionOutput1Y = ConnectionOutput2Y = shapeTop + Shape.ActualHeight - 14f;
        }

        private void SetBezierCurvesPosition()
        {
            if (_lineConnectionPrimary != null)
            {
                var pathGeometry = _lineConnectionPrimary.Data as PathGeometry;

                double xMod = 0;
                if (NextBlockPrimary.ConnectionInputY < ConnectionOutput1Y)
                {
                    xMod = (ConnectionOutput1X - NextBlockPrimary.ConnectionInputX);
                    if (xMod != 0)
                    {
                        xMod = xMod / Math.Abs(xMod) * 200;
                    }
                    else
                    {
                        xMod = 200;
                    }
                }

                pathGeometry.Figures[0].StartPoint = new Point(ConnectionOutput1X, ConnectionOutput1Y);
                (pathGeometry.Figures[0].Segments[0] as BezierSegment).Point1 = new Point(ConnectionOutput1X + xMod, ConnectionOutput1Y + 100);
                (pathGeometry.Figures[0].Segments[0] as BezierSegment).Point2 = new Point(NextBlockPrimary.ConnectionInputX + xMod, NextBlockPrimary.ConnectionInputY - 100);
                (pathGeometry.Figures[0].Segments[0] as BezierSegment).Point3 = new Point(NextBlockPrimary.ConnectionInputX, NextBlockPrimary.ConnectionInputY);
            }

            if (_lineConnectionOptional != null)
            {
                var pathGeometry = _lineConnectionOptional.Data as PathGeometry;

                double xMod = 0;
                if (NextBlockOptional.ConnectionInputY < ConnectionOutput2Y)
                {
                    xMod = (ConnectionOutput2X - NextBlockOptional.ConnectionInputX);
                    if (xMod != 0)
                    {
                        xMod = xMod / Math.Abs(xMod) * 200;
                    }
                    else
                    {
                        xMod = 200;
                    }
                }

                pathGeometry.Figures[0].StartPoint = new Point(ConnectionOutput2X, ConnectionOutput2Y);
                (pathGeometry.Figures[0].Segments[0] as BezierSegment).Point1 = new Point(ConnectionOutput2X + xMod, ConnectionOutput2Y + 100);
                (pathGeometry.Figures[0].Segments[0] as BezierSegment).Point2 = new Point(NextBlockOptional.ConnectionInputX + xMod, NextBlockOptional.ConnectionInputY - 100);
                (pathGeometry.Figures[0].Segments[0] as BezierSegment).Point3 = new Point(NextBlockOptional.ConnectionInputX, NextBlockOptional.ConnectionInputY);
            }
        }

        private void SetArrowPositions()
        {
            if (NextBlockPrimary != null)
            {
                Connection1ArrowVector1X = NextBlockPrimary.ConnectionInputX - ArrowLength;
                Connection1ArrowVector1Y = NextBlockPrimary.ConnectionInputY - ArrowLength;
                Connection1ArrowVector2X = NextBlockPrimary.ConnectionInputX + ArrowLength;
                Connection1ArrowVector2Y = NextBlockPrimary.ConnectionInputY - ArrowLength;
            }

            if (NextBlockOptional != null)
            {
                Connection2ArrowVector1X = NextBlockOptional.ConnectionInputX - ArrowLength;
                Connection2ArrowVector1Y = NextBlockOptional.ConnectionInputY - ArrowLength;
                Connection2ArrowVector2X = NextBlockOptional.ConnectionInputX + ArrowLength;
                Connection2ArrowVector2Y = NextBlockOptional.ConnectionInputY - ArrowLength;
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Shape.ReleaseMouseCapture();
            _isPressed = false;
        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (BlockManager.IsAddConnectionMode)
            {
                if (BlockManager.FirstBlockForConnection == null)
                    BlockManager.FirstBlockForConnection = this;
                else
                {
                    Mouse.OverrideCursor = null;
                    BlockManager.SecondBlockForConnection = this;
                    BlockManager.AddConnection();
                }
            }

            _propertiesManager.SelectedBlock = this;
            _propertiesManager.Update();

            Shape.CaptureMouse();
            _isPressed = true;
        }

        public void AddConnection(Block block)
        {
            if (this == block)
            {
                MessageBox.Show("Self connection is not possible due to potential endless loop.","Connection error!",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            if (_maxConnectionsCount == 0)
            {
                MessageBox.Show("Outputs are not supported by this block.", "Connection error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NextBlockPrimary == null)
            {
                NextBlockPrimary = block;
                NextBlockPrimary.PreviousBlocks.Add(this);

                _lineConnectionPrimary = CreateBezierPath(block, 1);
                SetBezierCurvesPosition();
            }
            else if (_maxConnectionsCount == 2 && NextBlockOptional == null)
            {
                NextBlockOptional = block;
                NextBlockOptional.PreviousBlocks.Add(this);

                _lineConnectionOptional = CreateBezierPath(block, 2);
                SetBezierCurvesPosition();
            }
            else
            {
                MessageBox.Show("You cannot add another connection, because all outputs are already used.", "Connection error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Path CreateBezierPath(Block secondBlock, int outputIndex)
        {
            BezierSegment bezier = new BezierSegment(new Point(), new Point(), new Point(), true);

            PathSegmentCollection segmentCollection = new PathSegmentCollection();
            segmentCollection.Add(bezier);

            PathFigure pathFigure = new PathFigure();
            pathFigure.Segments = segmentCollection;
            pathFigure.StartPoint = new Point();

            PathFigureCollection figureCollection = new PathFigureCollection();
            figureCollection.Add(pathFigure);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = figureCollection;

            Path path = new Path();
            path.Data = pathGeometry;
            path.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            path.StrokeThickness = 2;

            _canvasBlocks.Children.Add(path);

            createConnectionArrow(secondBlock, outputIndex);

            return path;
        }

        private void createConnectionArrow(Block secondBlock, int outputIndex)
        {
            var arrowLine1 = new Line()
            {
                Stroke = Brushes.White
            };

            _canvasBlocks.Children.Add(arrowLine1);


            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Connection" + outputIndex + "ArrowVector1X")
            };
            arrowLine1.SetBinding(Line.X1Property, binding);

            binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Connection" + outputIndex + "ArrowVector1Y")
            };
            arrowLine1.SetBinding(Line.Y1Property, binding);

            binding = new Binding
            {
                Source = secondBlock,
                Path = new PropertyPath("ConnectionInputX")
            };
            arrowLine1.SetBinding(Line.X2Property, binding);

            binding = new Binding
            {
                Source = secondBlock,
                Path = new PropertyPath("ConnectionInputY")
            };
            arrowLine1.SetBinding(Line.Y2Property, binding);

            var arrowLine2 = new Line()
            {
                Stroke = Brushes.White
            };

            _canvasBlocks.Children.Add(arrowLine2);


            binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Connection" + outputIndex + "ArrowVector2X")
            };
            arrowLine2.SetBinding(Line.X1Property, binding);

            binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Connection" + outputIndex + "ArrowVector2Y")
            };
            arrowLine2.SetBinding(Line.Y1Property, binding);

            binding = new Binding
            {
                Source = secondBlock,
                Path = new PropertyPath("ConnectionInputX")
            };
            arrowLine2.SetBinding(Line.X2Property, binding);

            binding = new Binding
            {
                Source = secondBlock,
                Path = new PropertyPath("ConnectionInputY")
            };
            arrowLine2.SetBinding(Line.Y2Property, binding);

            switch (outputIndex)
            {
                case 1:
                    _connection1arrow1 = arrowLine1;
                    _connection1arrow2 = arrowLine2;
                    break;
                case 2:
                    _connection2arrow1 = arrowLine1;
                    _connection2arrow2 = arrowLine2;
                    break;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RemoveLine(int connectionId)
        {
            switch (connectionId)
            {
                case 0:
                    _canvasBlocks.Children.Remove(_lineConnectionPrimary);
                    _canvasBlocks.Children.Remove(_connection1arrow1);
                    _canvasBlocks.Children.Remove(_connection1arrow2);
                    NextBlockPrimary = null;
                    _lineConnectionPrimary = null;
                    break;
                case 1:
                    _canvasBlocks.Children.Remove(_lineConnectionOptional);
                    _canvasBlocks.Children.Remove(_connection2arrow1);
                    _canvasBlocks.Children.Remove(_connection2arrow2);
                    NextBlockOptional = null;
                    _lineConnectionOptional = null;
                    break;
            }
        }

        public abstract Block Run();

        public static void ResetBlocksCounter()
        {
            _blocksCounter = 0;
        }
    }
}
