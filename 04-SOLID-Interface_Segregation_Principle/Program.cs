using System;

/*
    Con gusto te explico el Interface Segregation Principle (Principio de Segregación de Interfaces).

    El Interface Segregation Principle (ISP) es uno de los principios SOLID que establece que los clientes no deben depender de interfaces que no utilizan por completo. En otras palabras, es mejor tener interfaces especializadas y específicas para cada cliente en lugar de tener una única interfaz grande que abarque todas las posibles funcionalidades.

    El objetivo del ISP es evitar que las clases dependan de métodos o funcionalidades que no necesitan. Al tener interfaces más pequeñas y específicas, se evita la sobrecarga y la dependencia innecesaria de código que no se utilizará.

    Un ejemplo práctico sería el siguiente:

    Supongamos que tenemos una interfaz llamada IAnimal que tiene métodos como Caminar(), Nadar() y Volar(). Ahora, si tenemos una clase llamada Perro que implementa la interfaz IAnimal, tendríamos un problema porque el perro no puede volar y no necesita tener ese método.

    En lugar de tener una única interfaz con todas las funcionalidades de los animales, se podría dividir la interfaz en interfaces más pequeñas y especializadas, como ICaminante y INadador. De esta manera, la clase Perro solo implementaría la interfaz ICaminante, que contiene el método Caminar().

    Este enfoque cumple con el principio de segregación de interfaces al permitir que las clases dependan solo de las interfaces que necesitan realmente y eviten la dependencia de métodos o funcionalidades irrelevantes para ellas.

    En resumen, el Interface Segregation Principle promueve la creación de interfaces más pequeñas y especializadas, evitando la dependencia innecesaria y mejorando la cohesión y flexibilidad de las clases.
*/

namespace DotNetDesignPatternDemos.SOLID.InterfaceSegregationPrinciple
{
  public class Document
  {
  }

  public interface IMachine
  {
    void Print(Document d);
    void Fax(Document d);
    void Scan(Document d);
  }

  // ok if you need a multifunction machine
  public class MultiFunctionPrinter : IMachine
  {
    public void Print(Document d)
    {
      //
    }

    public void Fax(Document d)
    {
      //
    }

    public void Scan(Document d)
    {
      //
    }
  }

  public class OldFashionedPrinter : IMachine
  {
    public void Print(Document d)
    {
      // yep
    }

    public void Fax(Document d)
    {
      throw new System.NotImplementedException();
    }

    public void Scan(Document d)
    {
      throw new System.NotImplementedException();
    }
  }

  public interface IPrinter
  {
    void Print(Document d);
  }

  public interface IScanner
  {
    void Scan(Document d);
  }

  public class Printer : IPrinter
  {
    public void Print(Document d)
    {
      
    }
  }

  public class Photocopier : IPrinter, IScanner
  {
    public void Print(Document d)
    {
      throw new System.NotImplementedException();
    }

    public void Scan(Document d)
    {
      throw new System.NotImplementedException();
    }
  }

  public interface IMultiFunctionDevice : IPrinter, IScanner //
  {
    
  }

  public struct MultiFunctionMachine : IMultiFunctionDevice
  {
    // compose this out of several modules
    private IPrinter printer;
    private IScanner scanner;

    public MultiFunctionMachine(IPrinter printer, IScanner scanner)
    {
      if (printer == null)
      {
        throw new ArgumentNullException(paramName: nameof(printer));
      }
      if (scanner == null)
      {
        throw new ArgumentNullException(paramName: nameof(scanner));
      }
      this.printer = printer;
      this.scanner = scanner;
    }

    public void Print(Document d)
    {
      printer.Print(d);
    }

    public void Scan(Document d)
    {
      scanner.Scan(d);
    }
  }
}
