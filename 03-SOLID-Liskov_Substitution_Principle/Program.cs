using static System.Console;

/*
    El Liskov Substitution Principle (Principio de Sustitución de Liskov) es uno de los principios fundamentales de SOLID. Este principio establece que los objetos de una clase base deben poder ser sustituidos por objetos de sus clases derivadas sin alterar el comportamiento correcto del programa.

    En resumen, si una clase A es una subclase de una clase B, entonces A debe poder ser utilizada en lugar de B sin que esto genere errores o comportamientos inesperados en el programa.

    El principio se puede expresar en la siguiente afirmación: "Si S es un subtipo de T, entonces los objetos de tipo T pueden ser reemplazados por objetos de tipo S sin alterar la corrección del programa".
*/

namespace DotNetDesignPatternDemos.SOLID.LiskovSubstitutionPrinciple
{
  // using a classic example
  public class Rectangle
  {
    //public int Width { get; set; }
    //public int Height { get; set; }

    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public Rectangle()
    {
      
    }

    public Rectangle(int width, int height)
    {
      Width = width;
      Height = height;
    }

    public override string ToString()
    {
      return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
    }
  }

  public class Square : Rectangle
  {
    //public new int Width
    //{
    //  set { base.Width = base.Height = value; }
    //}

    //public new int Height
    //{ 
    //  set { base.Width = base.Height = value; }
    //}

    public override int Width // nasty side effects
    {
      set { base.Width = base.Height = value; }
    }

    public override int Height
    { 
      set { base.Width = base.Height = value; }
    }
  }

  public class Demo
  {
    static public int Area(Rectangle r) => r.Width * r.Height;

    static void Main(string[] args)
    {
      Rectangle rc = new Rectangle(2,3);
      WriteLine($"{rc} has area {Area(rc)}");

      // should be able to substitute a base type for a subtype
      /*Square*/ Rectangle sq = new Square();
      sq.Width = 4;
      WriteLine($"{sq} has area {Area(sq)}");
    }
  }
}
