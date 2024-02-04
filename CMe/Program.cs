// Main Entry Point
using CMe;

var source = File.ReadAllText("../../../Examples/Parse1.txt");
var prog = new Parser(new Lexer(source)).Parse();
var pr = new Printer();
prog.Accept(pr);
Console.WriteLine(pr.Result);
