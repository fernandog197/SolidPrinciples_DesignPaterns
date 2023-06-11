using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.Console;

/*
    El principio de responsabilidad única, conocido como Single Responsibility Principle (SRP) en inglés, es uno de los principios de diseño de software dentro del acrónimo SOLID. Este principio establece que una clase debe tener una única razón para cambiar, es decir, debe tener una única responsabilidad.

    La idea detrás del SRP es que cada clase o módulo del software debe ser responsable de una única tarea o función. Esto promueve la cohesión y evita la superposición de responsabilidades en una misma clase. Al seguir el SRP, se busca que los cambios relacionados con una determinada responsabilidad tengan un impacto mínimo en otras partes del sistema.

    Un beneficio importante del SRP es que facilita el mantenimiento y la evolución del software. Al tener clases enfocadas en una única responsabilidad, es más sencillo entender su funcionamiento, realizar cambios y corregir errores sin afectar otras áreas del sistema. Además, el SRP favorece la reutilización de código, ya que las clases especializadas en una tarea específica pueden ser utilizadas en diferentes contextos.
*/

namespace DotNetDesignPatternDemos.SOLID.SRP
{
  public class Journal
  {
    private readonly List<string> entries = new List<string>();

    private static int count = 0;

    public int AddEntry(string text)
    {
      entries.Add($"{++count}: {text}");
      return count; // memento pattern!
    }

    public void RemoveEntry(int index)
    {
      entries.RemoveAt(index);
    }

    public override string ToString()
    {
      return string.Join(Environment.NewLine, entries);
    }

    // Aqui rompemos el SRP(Single Responsability Principle). Asignamos a la clase Journal metodos de Persistencia de datos.

    // public void Save(string filename, bool overwrite = false)
    // {
    //   File.WriteAllText(filename, ToString());
    // }

    // public void Load(string filename)
    // {
      
    // }

    // public void Load(Uri uri)
    // {
      
    // }
  }

  // Declaramos una clase Persistence que manejara la misma
  public class Persistence
  {
    public void SaveToFile(Journal journal, string filename, bool overwrite = false)
    {
      if (overwrite || !File.Exists(filename))
        File.WriteAllText(filename, journal.ToString());
    }
  }

  public class Demo
  {
    static void Main(string[] args)
    {
      var j = new Journal();
      j.AddEntry("I cried today.");
      j.AddEntry("I ate a bug.");
      WriteLine(j);

      var p = new Persistence();
      var filename = @"c:\temp\journal.txt";
      p.SaveToFile(j, filename);
      Process.Start(filename);
    }
  }
}