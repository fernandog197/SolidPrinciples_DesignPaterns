using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

/*
    El Dependency Inversion Principle establece que los módulos de alto nivel no deben depender de los módulos de bajo nivel, ambos deben depender de abstracciones. Además, las abstracciones no deben depender de los detalles concretos, los detalles concretos deben depender de las abstracciones. En pocas palabras, este principio busca invertir la dependencia entre los diferentes componentes de un sistema.

    El DIP se basa en el uso de interfaces o clases abstractas para definir las dependencias entre los módulos. En lugar de que un módulo de alto nivel dependa directamente de los detalles de implementación de un módulo de bajo nivel, se establece una abstracción que ambos utilizan.

    Esto permite una mayor flexibilidad y extensibilidad en el sistema, ya que los módulos de alto nivel pueden ser independientes de los detalles de implementación de los módulos de bajo nivel. Además, facilita la sustitución de componentes y la realización de pruebas unitarias, ya que se pueden utilizar implementaciones diferentes de una misma abstracción.
*/

namespace DotNetDesignPatternDemos.SOLID.DependencyInversionPrinciple
{
  // hl modules should not depend on low-level; both should depend on abstractions
  // abstractions should not depend on details; details should depend on abstractions

  public enum Relationship
  {
    Parent,
    Child,
    Sibling
  }

  public class Person
  {
    public string Name;
    // public DateTime DateOfBirth;
  }

  public interface IRelationshipBrowser
  {
    IEnumerable<Person> FindAllChildrenOf(string name);
  }

  public class Relationships : IRelationshipBrowser // low-level
  {
    private List<(Person,Relationship,Person)> relations
      = new List<(Person, Relationship, Person)>();

    public void AddParentAndChild(Person parent, Person child)
    {
      relations.Add((parent, Relationship.Parent, child));
      relations.Add((child, Relationship.Child, parent));
    }

    public List<(Person, Relationship, Person)> Relations => relations;

    public IEnumerable<Person> FindAllChildrenOf(string name)
    {
      return relations
        .Where(x => x.Item1.Name == name
                    && x.Item2 == Relationship.Parent).Select(r => r.Item3);
    }
  }

  public class Research
  {
    public Research(Relationships relationships) 
    {
      // high-level: find all of john's children
      //var relations = relationships.Relations;
      //foreach (var r in relations
      //  .Where(x => x.Item1.Name == "John"
      //              && x.Item2 == Relationship.Parent))
      //{
      //  WriteLine($"John has a child called {r.Item3.Name}");
      //}
    }

    public Research(IRelationshipBrowser browser) {
      foreach (var p in browser.FindAllChildrenOf("John"))
      {
        WriteLine($"John has a child called {p.Name}");
      }
    }

    static void Main(string[] args)
    {
      var parent = new Person {Name = "John"};
      var child1 = new Person {Name = "Chris"};
      var child2 = new Person {Name = "Matt"};

      // low-level module
      var relationships = new Relationships();
      relationships.AddParentAndChild(parent, child1);
      relationships.AddParentAndChild(parent, child2);

      new Research(relationships);
      
    }
  }
}
