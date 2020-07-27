using System;

namespace RPN {
  class MainClass {
    public static void Main(string[] args) {
      var rpn = new FlickCalc.RPN();
      Console.WriteLine($"1+2={rpn.Proc("1+2")}");
      Console.WriteLine($"4-2={rpn.Proc("4-2")}");
      Console.WriteLine($"9*3={rpn.Proc("9*3")}");
      Console.WriteLine($"8/2={rpn.Proc("8/2")}");
      Console.WriteLine($"5+4-3/2+1={rpn.Proc("5+4-3/2+1")}");
      Console.WriteLine($"6+5+4-3/2*(1+5)={rpn.Proc("6+5+4-3/2*(1+5)")}");

      Console.WriteLine("Hello World!");
    }
  }
}
