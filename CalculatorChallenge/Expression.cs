using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorChallenge
{
    public enum ExpressionType
    {
        X,
        Number,
        Expression
    }

    public enum Operator
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public class Expression
    {
        public ExpressionType ExpressionType { get; set; }
        public Expression LeftExpression { get; set; }
        public Expression RightEpression { get; set; }
        public Operator Operator { get; set; }
        public int Value { get; set; }
        public bool HasX
        {
            get
            {
                if (ExpressionType == ExpressionType.X)
                    return true;

                if (ExpressionType == ExpressionType.Expression)
                    return LeftExpression.HasX || RightEpression.HasX;

                return false;
            }
        }
        public bool DidHaveX { get; set; }

        public Expression(Queue<string> expressionQueue)
        {
            var firstElement = expressionQueue.Dequeue();
            var isNumber = int.TryParse(firstElement, out int number);

            if (isNumber)
            {
                ExpressionType = ExpressionType.Number;
                Value = number;
            }
            else
            {
                if (firstElement == "X")
                    ExpressionType = ExpressionType.X;
                else
                {
                    ExpressionType = ExpressionType.Expression;
                    switch (firstElement)
                    {
                        case "+":
                            Operator = Operator.Addition;
                            break;
                        case "-":
                            Operator = Operator.Subtraction;
                            break;
                        case "*":
                            Operator = Operator.Multiplication;
                            break;
                    }
                    if (expressionQueue.Count > 0)
                    {
                        LeftExpression = new Expression(expressionQueue);
                        RightEpression = new Expression(expressionQueue);
                    }
                }
            }
        }

        private Expression(int value)
        {
            ExpressionType = ExpressionType.Number;
            Value = value;
        }

        private Expression() { }

        public Expression BuildReverseExpression(int number)
        {
            var newExpression = new Expression(number);
            var finalExpression = ReverseExpression(this, newExpression);
            return finalExpression;
        }

        public double CalculateExpression()
        {
            if (HasX)
                throw new Exception();

            if (ExpressionType == ExpressionType.Number)
                return Value;

            var leftExpressionValue = LeftExpression.CalculateExpression();
            var rightExpressionValue = RightEpression.CalculateExpression();

            double expressionValue = 0;
            switch (Operator)
            {
                case Operator.Addition:
                    expressionValue = leftExpressionValue + rightExpressionValue;
                    break;
                case Operator.Subtraction:
                    expressionValue = leftExpressionValue - rightExpressionValue;
                    break;
                case Operator.Multiplication:
                    expressionValue = leftExpressionValue * rightExpressionValue;
                    break;
                case Operator.Division:
                    expressionValue = leftExpressionValue / rightExpressionValue;
                    break;
            }

            return expressionValue;
        }

        private static Expression ReverseExpression(Expression oldExpression, Expression newExpression)
        {
            if (oldExpression.ExpressionType == ExpressionType.X)
                return newExpression;

            if (oldExpression.ExpressionType == ExpressionType.Expression)
            {
                var hasLeftX = oldExpression.LeftExpression.HasX;
                var nextExpression = new Expression
                {
                    ExpressionType = ExpressionType.Expression,
                    LeftExpression = newExpression,
                    RightEpression = hasLeftX ? oldExpression.RightEpression : oldExpression.LeftExpression
                };

                switch (oldExpression.Operator)
                {
                    case Operator.Addition:
                        nextExpression.Operator = Operator.Subtraction;
                        break;
                    case Operator.Subtraction:
                        nextExpression.Operator = hasLeftX ? Operator.Addition : Operator.Subtraction;
                        if (!hasLeftX)
                        {
                            nextExpression.LeftExpression = nextExpression.RightEpression;
                            nextExpression.RightEpression = newExpression;
                        }
                        break;
                    case Operator.Multiplication:
                        nextExpression.Operator = Operator.Division;
                        break;
                }

                var expression = ReverseExpression(hasLeftX ? oldExpression.LeftExpression : oldExpression.RightEpression, nextExpression);
                return expression;
            }

            return newExpression;
        }
    }
}
