using System;
using System.Collections.Generic;
using static System.Console;

/*
    El principio Open-Closed (OCP) es otro de los principios del SOLID, y se refiere a la capacidad de una entidad (clase, módulo, componente, etc.) de estar abierta para su extensión pero cerrada para su modificación.

    En pocas palabras, esto significa que el código de una entidad debe ser fácilmente extensible sin necesidad de realizar modificaciones directas en su implementación existente. En lugar de cambiar el código existente, se deben poder agregar nuevas funcionalidades o comportamientos mediante la adición de nuevas clases, métodos, interfaces, etc.

    El objetivo del OCP es lograr un diseño flexible y fácil de mantener, evitando efectos secundarios no deseados al realizar modificaciones en el código existente. Al seguir este principio, se promueve la reutilización de código, la modularidad y la escalabilidad.

    Un ejemplo práctico podría ser un sistema de envío de notificaciones donde se tienen diferentes tipos de notificaciones (email, SMS, notificación push, etc.). En lugar de modificar directamente la clase que se encarga del envío de notificaciones cada vez que se agrega un nuevo tipo, se puede aplicar el OCP definiendo una interfaz general de notificación y creando clases separadas para cada tipo de notificación que implementen dicha interfaz. De esta manera, se puede extender el sistema agregando nuevas clases sin modificar la lógica existente.
*/

namespace DotNetDesignPatternDemos.SOLID.OCP
{
  public enum Color
  {
    Red, Green, Blue
  }

  public enum Size
  {
    Small, Medium, Large, Yuge
  }

  public class Product
  {
    public string Name;
    public Color Color;
    public Size Size;

    public Product(string name, Color color, Size size)
    {
      Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
      Color = color;
      Size = size;
    }
  }

  public class ProductFilter
  {
    // let's suppose we don't want ad-hoc queries on products
    public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
    {
      foreach (var p in products)
        if (p.Color == color)
          yield return p;
    }
    
    public static IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
    {
      foreach (var p in products)
        if (p.Size == size)
          yield return p;
    }

    public static IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
    {
      foreach (var p in products)
        if (p.Size == size && p.Color == color)
          yield return p;
    } // state space explosion
      // 3 criteria = 7 methods

    // OCP = open for extension but closed for modification
  }

  // we introduce two new interfaces that are open for extension

  public interface ISpecification<T>
  {
    bool IsSatisfied(Product p);
  }

  public interface IFilter<T>
  {
    IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
  }

  public class ColorSpecification : ISpecification<Product>
  {
    private Color color;

    public ColorSpecification(Color color)
    {
      this.color = color;
    }

    public bool IsSatisfied(Product p)
    {
      return p.Color == color;
    }
  }

  public class SizeSpecification : ISpecification<Product>
  {
    private Size size;

    public SizeSpecification(Size size)
    {
      this.size = size;
    }

    public bool IsSatisfied(Product p)
    {
      return p.Size == size;
    }
  }

  // combinator
  public class AndSpecification<T> : ISpecification<T>
  {
    private ISpecification<T> first, second;

    public AndSpecification(ISpecification<T> first, ISpecification<T> second)
    {
      this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
      this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
    }

    public bool IsSatisfied(Product p)
    {
      return first.IsSatisfied(p) && second.IsSatisfied(p);
    }
  }

  public class BetterFilter : IFilter<Product>
  {
    public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
    {
      foreach (var i in items)
        if (spec.IsSatisfied(i))
          yield return i;
    }
  }

  public class Demo
  {
    static void Main(string[] args)
    {
      var apple = new Product("Apple", Color.Green, Size.Small);
      var tree = new Product("Tree", Color.Green, Size.Large);
      var house = new Product("House", Color.Blue, Size.Large);

      Product[] products = {apple, tree, house};

      var pf = new ProductFilter();
      WriteLine("Green products (old):");
      foreach (var p in pf.FilterByColor(products, Color.Green))
        WriteLine($" - {p.Name} is green");

      // ^^ BEFORE

      // vv AFTER
      var bf = new BetterFilter();
      WriteLine("Green products (new):");
      foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
        WriteLine($" - {p.Name} is green");

      WriteLine("Large products");
      foreach (var p in bf.Filter(products, new SizeSpecification(Size.Large)))
        WriteLine($" - {p.Name} is large");

      WriteLine("Large blue items");
      foreach (var p in bf.Filter(products,
        new AndSpecification<Product>(new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large)))
      )
      {
        WriteLine($" - {p.Name} is big and blue");
      }
    }
  }
}
