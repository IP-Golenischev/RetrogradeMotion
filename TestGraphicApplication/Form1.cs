using TestGraphicApplication.Models;

namespace TestGraphicApplication;

public partial class Form1 : Form
{
    private readonly PictureBox _functionGraphic = new()
    {
        Top = 100,
        Height = 900,
        Width = 1920
    };

    private int _divisionX = 20;
    private int _divisionY = 20;
    private readonly Label _divisionLabelX = new() { Text = "Цена деления:", Height = 100, Width = 50};
    private readonly TextBox _divisionBoxX = new() { Left = 110, Width = 50, Height = 100};
    private readonly Label _semiMajorAxisLabel = new() { Text = "Большая полуось", Left = 180, Width = 50, Height = 100};
    private readonly TextBox _semiMajorAxisBox = new() { Left = 250, Height = 100, Width = 50 };
    private readonly Label _eccentricityLabel = new() { Text = "Эксцентриситет", Left = 320, Height = 100, Width = 50 };
    private readonly TextBox _eccentricityBox = new() { Left = 390, Height = 100, Width = 50 };
    private readonly Label _inclinationLabel = new() { Text = "Наклонение", Left = 440, Height = 100, Width = 50 };
    private readonly TextBox _inclinationBox = new() { Left = 510, Height = 100, Width = 50 };
    private readonly Label _longitudeOfAscendingNodeLabel = new() { Text = "Долгота восходящего узла", Left = 680, Height = 100, Width = 50 };
    private readonly TextBox _longitudeOfAscendingNodeBox = new() { Left = 750, Height = 100, Width = 50 };
    private readonly Label _argumentOfPeriapsisLabel = new() { Text = "Аргумент перицентра:", Left = 820, Height = 100, Width = 50 };
    private readonly TextBox _argumentOfPeriapsisBox = new() { Left = 890, Height = 100, Width = 50 };
    private readonly Label _divisionLabelY = new() { Left = 950, Text = "Цена деления:", Height = 100, Width = 50};
    private readonly TextBox _divisionBoxY = new() { Left = 1020, Width = 50, Height = 100};

    private readonly Button _startButton = new()
    {
        Text = "Построить",
        Left = 1200,
        Width = 90
    };

    private readonly Button _clearButton = new()
    {
        Text = "Очистить",
        Left = 1300,
        Width = 70
    };

    private readonly Graphics _graphic;
    

    private readonly Planet _earth = new(0.01671123,1.00000261, 0.0, 0 , 1.81688775);
    private Planet? _planet;
    public Form1()
    {
        _functionGraphic.Parent = this;
        _startButton.Click += DrawFunctionGraphic;
        _startButton.Parent = this;
        _divisionLabelX.Parent = this;
        _semiMajorAxisBox.Parent = this;
        _eccentricityLabel.Parent = this;
        _eccentricityBox.Parent = this;
        _divisionLabelX.Parent = this;
        _divisionBoxX.Parent = this;
        _semiMajorAxisLabel.Parent = this;
        _inclinationLabel.Parent = this;
        _inclinationBox.Parent = this;
        _clearButton.Parent = this;
        _clearButton.Click+=Clear;
        _longitudeOfAscendingNodeLabel.Parent = this;
        _longitudeOfAscendingNodeBox.Parent = this;
        _argumentOfPeriapsisBox.Parent = this;
        _argumentOfPeriapsisLabel.Parent = this;
        _divisionLabelY.Parent = this;
        _divisionBoxY.Parent = this;
        InitializeComponent();
        _graphic = _functionGraphic.CreateGraphics();
    }

    private void Clear(object? sender, EventArgs e) => _functionGraphic.Image = null;


    private void DrawAxes()
    {
        /*_graphic.DrawLine(Pens.Red, new Point(_functionGraphic.Width / 2, 0),
            new Point(_functionGraphic.Width / 2, _functionGraphic.Height));*/
        _graphic.DrawLine(Pens.Red, new Point(0, _functionGraphic.Height / 2),
            new Point(_functionGraphic.Width, _functionGraphic.Height / 2));
    }

    private int CalculateXCoordinate(double value) => (int)( value * _divisionX);

    private int CalculateYCoordinate(double value) =>
        (int)(_functionGraphic.Height / 2.0 - value * _divisionY);


    private void DrawFunctionGraphic(object? sender, EventArgs e)
    {
        DrawAxes();
        using var file = File.OpenWrite("coordinates.txt");
        using var writer = new StreamWriter(file);
        _divisionX = int.Parse(_divisionBoxX.Text!);
        _divisionY = int.Parse(_divisionBoxY.Text!);
        var semiMajorAxisValue = double.Parse(_semiMajorAxisBox.Text!);
        var eccentricityValue = double.Parse(_eccentricityBox.Text!);
        var inclinationValue = double.Parse(_inclinationBox.Text!)/180 * Math.PI;
        var longitudeOfAscendingNodeValue = double.Parse(_longitudeOfAscendingNodeBox.Text!)/180*Math.PI;
        var argumentOfPeriapsisValue = double.Parse(_argumentOfPeriapsisBox.Text!)/180*Math.PI;
        _planet = new Planet(eccentricityValue, semiMajorAxisValue, inclinationValue, argumentOfPeriapsisValue,
            longitudeOfAscendingNodeValue);
        var points = new List<Point>();
        const double stepDays = 7.0;
        const double period = 779.94*10.0;
        _planet.TrueAnomaly = -_planet.ArgumentOfPeriapsis;
        _earth.TrueAnomaly = _planet.LongitudeOfAscendingNode-_earth.LongitudeOfAscendingNode;
        double t = 0.0, Dt = 0.0, dt = 0.00001;
        var n = 0;
        var l = 0.0;
        while(t < period/365.25636*2.0*Math.PI)
        {
            _earth.Step(dt);
            _planet.Step(dt);
            Dt += dt;
            t += dt;
        
            if (l > (_planet.DistanceToSun - _earth.DistanceToSun).Lambda
                &&Math.Abs((_planet.DistanceToSun - _earth.DistanceToSun).Lambda-l+2.0*Math.PI)<Math.Abs((_planet.DistanceToSun - _earth.DistanceToSun).Lambda-l))
            {
                n++;
            }
            if (l < (_planet.DistanceToSun - _earth.DistanceToSun).Lambda
                &&Math.Abs(-(_planet.DistanceToSun - _earth.DistanceToSun).Lambda+l+2.0*Math.PI)<Math.Abs((_planet.DistanceToSun- _earth.DistanceToSun).Lambda)-l)
            {
                n--;
            }
            l = (_planet.DistanceToSun - _earth.DistanceToSun).Lambda;

            if (!(Dt >= stepDays / 365.25636 * 2.0 * Math.PI)) 
                continue;
            writer.Write($"({n*2.0*Math.PI + (_planet.DistanceToSun - _earth.DistanceToSun).Lambda};{(_planet.DistanceToSun - _earth.DistanceToSun).Beta}) ");
            Console.WriteLine($"{n*2.0*Math.PI + (_planet.DistanceToSun - _earth.DistanceToSun).Lambda}\t{(_planet.DistanceToSun - _earth.DistanceToSun).Beta}");
            points.Add(new Point(CalculateXCoordinate(n*2.0*Math.PI + (_planet.DistanceToSun - _earth.DistanceToSun).Lambda),CalculateYCoordinate((_planet.DistanceToSun - _earth.DistanceToSun).Beta)));
            Dt -= (stepDays/365.25636)*2.0*Math.PI;
        }
        _graphic.DrawCurve(Pens.Blue, points.ToArray());
    }
}