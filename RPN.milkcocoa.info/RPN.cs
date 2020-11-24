using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace RPN {
  public class Token {
    public enum Kind {
      Number,
      Operator,
      Function
    }

    public int priority_;
    public Kind kind_;
    public string value_;
  }

  public class RPN {
    Regex op_;
    Regex cos_;
    Regex sin_;
    Regex num_;
    /// summary   : コンストラクタ
    public RPN() {
      op_ = new Regex(@"([+\-×*÷/=()])");
      sin_ = new Regex(@"sin$");
      cos_ = new Regex(@"cos$");
      num_ = new Regex(@"(-?[0-9]?\.[0-9]?|[0-9]*)");
    }


    private IEnumerable<Token> Tokenize(string _formula) {
      var tokenized = new List<Token>();

      _formula = _formula.Replace("(-", "(~");
      var token = Regex.Split(_formula, op_.ToString());
      foreach(var t in token) {
        if(t.Equals("")) {
          continue;
        } else if(Regex.IsMatch(t, op_.ToString())) {
          var priority = 0;

          if(t.Equals("(") | t.Equals(")")) {
            priority = 4;
          } else if(t.Equals("÷") || t.Equals("/")) {
            priority = 3;
          } else if(t.Equals("×") || t.Equals("*")) {
            priority = 2;
          } else {
            priority = 1;
          }
          tokenized.Add(new Token() { kind_ = Token.Kind.Operator, value_ = t.Replace("×", "*").Replace("÷", "/"), priority_ = priority });
        } else if(Regex.IsMatch(t, cos_.ToString()) |
                  Regex.IsMatch(t, sin_.ToString())) {
          tokenized.Add(new Token() { kind_ = Token.Kind.Function, value_ = t.Replace("~", "-"), priority_ = 5 });
        } else {
          tokenized.Add(new Token() { kind_ = Token.Kind.Number, value_ = t.Replace("~", "-"), priority_ = 0 });
        }
      }
      return tokenized;
    }

    private IEnumerable<Token> Convert(IEnumerable<Token> _token) {
      var converted = new List<Token>();
      var opStack = new List<List<Token>>();
      opStack.Add(new List<Token>());

      var quoted = 0;

      foreach(var t in _token) {
        if(t.kind_ == Token.Kind.Number) {
          converted.Add(t);
        } else {
          if(t.value_.Equals("(")) {
            quoted++;
            opStack.Add(new List<Token>());
          } else if(t.value_.Equals(")")) {
            opStack[quoted].Reverse();
            converted.AddRange(opStack[quoted]);
            quoted--;
          } else {
            while(opStack[quoted].Count() != 0 && t.priority_ <= opStack[quoted].Last().priority_) {
              converted.Add(opStack[quoted].Last());
              opStack[quoted].RemoveAt(opStack[quoted].Count() - 1);
            }

            opStack[quoted].Add(t);
          }
        }
      }

      opStack[0].Reverse();
      converted.AddRange(opStack[0]);

      return converted;
    }

    private double Calc(IEnumerable<Token> _token) {
      var stack = new Stack<double>();


      foreach(var t in _token) {
        if(t.kind_ == Token.Kind.Number) {
          stack.Push(double.Parse(t.value_));
        } else if(t.kind_ == Token.Kind.Function) {
          if(Regex.IsMatch(t.value_, cos_.ToString())) {
            var arg = stack.Pop();
            double amp;
            var ampStr = Regex.Match(t.value_, num_.ToString()).Value;
            if(ampStr.Equals("")) {
              amp = 1;
            } else if(ampStr.Equals(".")) {
              throw new Exception("この計算はできません．");
            } else {
              amp = double.Parse(ampStr);
            }
            stack.Push(amp * Math.Cos(arg));
          } else if(Regex.IsMatch(t.value_, sin_.ToString())) {
            var arg = stack.Pop();
            double amp;
            var ampStr = Regex.Match(t.value_, num_.ToString()).Value;
            if(ampStr.Equals("")) {
              amp = 1;
            } else if(ampStr.Equals(".")) {
              throw new Exception("この計算はできません．");
            } else {
              amp = double.Parse(ampStr);
            }
            stack.Push(amp * Math.Sin(arg));
          } else {
            throw new Exception("この計算はできません．");
          }
        } else {
          var lhs = 0.0;
          var rhs = 0.0;
          if(stack.Count() >= 2) {
            rhs = stack.Pop();
            lhs = stack.Pop();
          } else if(stack.Count == 1 && t.value_.Equals("-")) {
            lhs = 0;
            rhs = stack.Pop();
          } else {
            throw new Exception("この計算はできません．");
          }


          if(t.value_.Equals("+")) {
            stack.Push(lhs + rhs);
          } else if(t.value_.Equals("-")) {
            stack.Push(lhs - rhs);
          } else if(t.value_.Equals("*")) {
            stack.Push(lhs * rhs);
          } else if(t.value_.Equals("/")) {
            if(rhs == 0) {
              throw new Exception("この計算はできません．");
            }
            stack.Push(lhs / rhs);
          } else {
            throw new Exception("この計算はできません．");
          }
        }
      }

      if(stack.Count != 1) {
        throw new Exception("この計算はできません．");
      }
      return stack.Last();
    }



    public double Proc(string _formula) {
      return Calc(Convert(Tokenize(_formula)));
    }
  }
}
