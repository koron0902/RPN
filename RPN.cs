using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace FlickCalc {
  public class Token {
    public enum Kind {
      Number,
      Operator
    }

    public int priority_;
    public Kind kind_;
    public string value_;
  }

  public class RPN {
    Regex regex_;
    /// summary   : コンストラクタ
    public RPN() {
      regex_ = new Regex(@"([+\-*/=()])");
    }


    private IEnumerable<Token> Tokenize(string _formula) {
      var tokenized = new List<Token>();
      var token = Regex.Split(_formula, regex_.ToString());

      foreach(var t in token) {
        if(t.Equals("")) {
          continue;
        }
        if(Regex.IsMatch(t, regex_.ToString())) {
          var priority = 0;
          if(t.Equals("(") | t.Equals(")")) {
            priority = 4;
          } else if(t.Equals("/")) {
            priority = 3;
          } else if(t.Equals("*")) {
            priority = 2;
          } else {
            priority = 1;
          }
          tokenized.Add(new Token() { kind_ = Token.Kind.Operator, value_ = t, priority_ = priority });
        } else {
          tokenized.Add(new Token() { kind_ = Token.Kind.Number, value_ = t, priority_ = 0 });
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

    private String Calc(IEnumerable<Token> _token) {
      var stack = new List<double>();
      foreach(var t in _token) {
        if(t.kind_ == Token.Kind.Number) {
          stack.Add(double.Parse(t.value_));
        } else {
          var rhs = stack.Last();
          stack.RemoveAt(stack.Count() - 1);
          var lhs = stack.Last();
          stack.RemoveAt(stack.Count() - 1);

          if(t.value_.Equals("+")) {
            stack.Add(lhs + rhs);
          } else if(t.value_.Equals("-")) {
            stack.Add(lhs - rhs);
          } else if(t.value_.Equals("*")) {
            stack.Add(lhs * rhs);
          } else if(t.value_.Equals("/")) {
            stack.Add(lhs / rhs);
          }
        }
      }

      return stack.Last().ToString();
    }



    public String Proc(string _formula) {
      return Calc(Convert(Tokenize(_formula)));
    }
  }
}
