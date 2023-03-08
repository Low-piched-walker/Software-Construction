using System;

abstract class Shape
{
    public abstract double Area { get; }
    public abstract bool IsValid();
}

class Rectangle : Shape
{
    private double _length;
    private double _width;

    public Rectangle(double length, double width)
    {
        _length = length;
        _width = width;
    }

    public override double Area
    {
        get
        {
            return _length * _width;
        }
    }

    public override bool IsValid()
    {
        return _length > 0 && _width > 0;
    }
}

class Square : Shape
{
    private double _side;

    public Square(double side)
    {
        _side = side;
    }

    public override double Area
    {
        get
        {
            return _side * _side;
        }
    }

    public override bool IsValid()
    {
        return _side > 0;
    }
}

class Triangle : Shape
{
    private double _a;
    private double _b;
    private double _c;

    public Triangle(double a, double b, double c)
    {
        _a = a;
        _b = b;
        _c = c;
    }

    public override double Area
    {
        get
        {
            double s = (_a + _b + _c) / 2;
            return Math.Sqrt(s * (s - _a) * (s - _b) * (s - _c));
        }
    }

    public override bool IsValid()
    {
        return _a + _b > _c && _a + _c > _b && _b + _c > _a;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Rectangle rectangle = new Rectangle(5, 10);
        Console.WriteLine("Rectangle area: " + rectangle.Area);
        Console.WriteLine("Rectangle is valid: " + rectangle.IsValid());

        Square square = new Square(7);
        Console.WriteLine("Square area: " + square.Area);
        Console.WriteLine("Square is valid: " + square.IsValid());

        Triangle triangle = new Triangle(3, 4, 5);
        Console.WriteLine("Triangle area: " + triangle.Area);
        Console.WriteLine("Triangle is valid: " + triangle.IsValid());
    }
}
