using NUnit.Framework;
using RPN.milkcocoa.info;

namespace RPN.milkcocoa.info.test {
  public class Tests {
    [SetUp]
    public void Setup() {
    }

    [Test]
    public void Add() {
      RPN rpn = new RPN();
      Assert.AreEqual(3, rpn.Proc("1+2"));
    }

    [Test]
    public void Sub() {
      RPN rpn = new RPN();
      Assert.AreEqual((1 - 2), rpn.Proc("1-2"));
    }

    [Test]
    public void Minus() {
      RPN rpn = new RPN();
      Assert.AreEqual(-1, rpn.Proc("-1"));
    }

    [Test]
    public void Mul() {
      RPN rpn = new RPN();
      Assert.AreEqual(10, rpn.Proc("2*5"));
      Assert.AreEqual(10, rpn.Proc("2×5"));
    }

    [Test]
    public void Div() {
      RPN rpn = new RPN();
      Assert.AreEqual(1.5, rpn.Proc("3/2"));
      Assert.AreEqual(1.5, rpn.Proc("3÷2"));
      Assert.That(() => {
        RPN rpn = new RPN();
        rpn.Proc("1/0");
      }, Throws.Exception);
    }

    [Test]
    public void Quart() {
      RPN rpn = new RPN();
      Assert.AreEqual((1 + 2) * 3, rpn.Proc("(1+2)*3"));
      Assert.AreEqual((1.0 + 2.0) * 3.0 / 4.0, rpn.Proc("(1+2)*3/4"));
      Assert.AreEqual(1 + 2 * (3 + 4) * 5, rpn.Proc("1+2*(3+4)*5"));
    }

    [Test]
    public void Cos() {
      RPN rpn = new RPN();
      Assert.AreEqual(-1, rpn.Proc("cos(3.141592653979)"), 0.1);
      Assert.AreEqual(1, rpn.Proc("cos(0)"));
      Assert.AreEqual(0, rpn.Proc("cos(1.5707963268)"), 0.1);
      Assert.AreEqual(0, rpn.Proc("cos(1.5707963268)"), 0.1);
      Assert.AreEqual(-2, rpn.Proc("2cos(3.141592653979)"), 0.1);
      Assert.AreEqual(2, rpn.Proc("2cos(0)"));
      Assert.AreEqual(1.5, rpn.Proc("1.5cos(0)"));
      Assert.AreEqual(0.5, rpn.Proc(".5cos(0)"));
      Assert.AreEqual(1, rpn.Proc("1.cos(0)"));
      Assert.That(() => {
        RPN rpn = new RPN();
        rpn.Proc(".cos(0)");
      }, Throws.Exception);
    }

    [Test]
    public void Sin() {
      RPN rpn = new RPN();
      Assert.AreEqual(0, rpn.Proc("sin(3.141592653979)"), 0.1);
      Assert.AreEqual(0, rpn.Proc("sin(0)"));
      Assert.AreEqual(1, rpn.Proc("sin(1.5707963268)"), 0.1);
      Assert.AreEqual(-1, rpn.Proc("sin(-1.5707963268)"), 0.1);
      Assert.AreEqual(2, rpn.Proc("2*sin(1.5707963268)"), 0.1);
      Assert.AreEqual(-2, rpn.Proc("2*sin(-1.5707963268)"), 0.1);
      Assert.AreEqual(1.5, rpn.Proc("1.5sin(1.5707963268)"), 0.1);
      Assert.AreEqual(0.5, rpn.Proc(".5sin(1.5707963268)"), 0.1);
      Assert.AreEqual(1, rpn.Proc("1.sin(1.5707963268)"), 0.1);
      Assert.That(() => {
        RPN rpn = new RPN();
        rpn.Proc(".sin(0)");
      }, Throws.Exception);
    }
  }
}