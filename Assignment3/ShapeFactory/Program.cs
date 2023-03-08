using System;
using System.Collections.Generic;

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

class ShapeFactory
{
    public static Shape Create(string shapeType)
    {
        Random random = new Random();
        switch (shapeType)
        {
            case "rectangle":
                return new Rectangle(random.NextDouble() * 10, random.NextDouble() * 10);
            case "square":
                return new Square(random.NextDouble() * 10);
            case "triangle":
                return new Triangle(random.NextDouble() * 10, random.NextDouble() * 10, random.NextDouble() * 10);
            default:
                throw new ArgumentException("Invalid shape type");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Shape> shapes = new List<Shape>();
        for (int i = 0; i < 10; i++)
        {
            string shapeType = "";
            switch (i % 3)
            {
                case 0:
                    shapeType = "rectangle";
                    break;
                case 1:
                    shapeType = "square";
                    break;
                case 2:
                    shapeType = "triangle";
                    break;
            }
            Shape shape = ShapeFactory.Create(shapeType);
            if (shape.IsValid())
            {
                shapes.Add(shape);
            }
        }
        double areaSum = 0;
        foreach (Shape shape in shapes)
        {
            areaSum += shape.Area;
        }
        Console.WriteLine("Total area: " + areaSum);
    }
}
