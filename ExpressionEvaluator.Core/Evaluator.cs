using System.Globalization;

namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix); 
        return EvaluatePostfix(postfix);    
    }

    private static string InfixToPostfix(string infix)
    {
        var output = "";
        var stack = new Stack<char>();
        var number = "";

        foreach (var c in infix.Replace(" ", ""))
        {
            
            if (char.IsDigit(c) || c == '.')
            {
                number += c;
            }
            else
            {
                
                if (number != "")
                {
                    output += number + " ";
                    number = "";
                }

                if (c == '(')
                {
                    stack.Push(c);
                }
                else if (c == ')')
                {
                    while (stack.Peek() != '(')
                        output += stack.Pop() + " ";
                    stack.Pop();
                }
                else if (IsOperator(c))
                {
                 
                    while (stack.Count > 0 &&
                           PriorityStack(stack.Peek()) >= PriorityInfix(c))
                    {
                        output += stack.Pop() + " ";
                    }
                    stack.Push(c);
                }
            }
        }

        
        if (number != "")
            output += number + " ";

        
        while (stack.Count > 0)
            output += stack.Pop() + " ";

        return output.Trim();
    }

    private static double EvaluatePostfix(string postfix)
    {
        var stack = new Stack<double>();
        var tokens = postfix.Split(' ');

        foreach (var token in tokens)
        {
            
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
            {
                stack.Push(num);
            }
            else
            {
                
                var b = stack.Pop();
                var a = stack.Pop();

                stack.Push(token switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    "^" => Math.Pow(a, b),
                    _ => throw new Exception("Error de sintaxis")
                });
            }
        }

        return stack.Pop();
    }

    private static int PriorityStack(char item) => item switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => 0
    };

    private static int PriorityInfix(char item) => item switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => 0
    };

    private static bool IsOperator(char c) => "+-*/^".Contains(c);
}