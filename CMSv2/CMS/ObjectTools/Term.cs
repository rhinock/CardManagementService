using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ObjectTools
{
    public class Term
    {
        public const string NullValue = "null";
        public const string QuoteValue = "\"";
        public const string EqualValue = "==";
        public const string NotEqualValue = "!=";
        public const string GreaterThanValue = ">";
        public const string LessThanValue = "<";
        public const string GreaterThanOrEqualValue = ">=";
        public const string LessThanOrEqualValue = "<=";
        public const string AndValue = "&&";
        public const string OrValue = "||";

        private const string SpaceValue = "#space#";

        private readonly string _input;
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string> { { OrValue, "OrElse" }, { AndValue, "AndAlso" } };

        private Term(string input)
        {
            _input = input;
        }

        public Term Add(string what, string value)
        {
            if (_dictionary.ContainsKey(what))
            {
                what = _dictionary[what];
            }

            if(what == QuoteValue || value == QuoteValue)
            {
                return new Term(_input.Replace($"{what}", $"{value}"));
            }
            else
            {
                return new Term(_input.Replace($" {what} ", $" {value} "));
            }
        }

        public override string ToString()
        {
            return _input;
        }

        public Expression<Func<TIn, bool>> ToBoolExpression<TIn>()
        {
            string line = _input;
            ParameterExpression parameter = Expression.Parameter(typeof(TIn), "x");

            Regex stringTemplate = new Regex($" {QuoteValue}.*{QuoteValue}");
            MatchCollection stringMathes = stringTemplate.Matches(_input);

            foreach (Match match in stringMathes)
            {
                string lastPartValue = match.Value.Trim();
                string newPartValue = lastPartValue.Replace(" ", SpaceValue);
                line = line.Replace(lastPartValue, newPartValue);
            }

            PropertyInfo[] properties = typeof(TIn).GetProperties();
            string[] parts = line.Split(' ');

            if ((parts.Length % 2) == 0)
            {
                throw new Exception("Expression is difficult");
            }

            BinaryExpression current = null;
            BinaryExpression last = null;
            string unionSymbol = null;

            for (int index = 0; index < parts.Length;)
            {
                if (index > 0 && (index % 3) == 0)
                {
                    unionSymbol = parts[index];
                    index++;
                }
                else
                {
                    PropertyInfo property = properties.FirstOrDefault(x => x.Name == parts[index]);
                    object value = ConvertValue(parts[index + 2], property.PropertyType);
                    Expression left = Expression.Property(parameter, parts[index]);
                    Expression right = Expression.Constant(value);

                    switch (parts[index + 1])
                    {
                        case EqualValue:
                            current = Expression.Equal(left, right);
                            break;
                        case NotEqualValue:
                            current = Expression.NotEqual(left, right);
                            break;
                        case GreaterThanValue:
                            current = Expression.GreaterThan(left, right);
                            break;
                        case LessThanValue:
                            current = Expression.LessThan(left, right);
                            break;
                        case GreaterThanOrEqualValue:
                            current = Expression.GreaterThanOrEqual(left, right);
                            break;
                        case LessThanOrEqualValue:
                            current = Expression.LessThanOrEqual(left, right);
                            break;
                    }
                    if (unionSymbol != null)
                    {
                        switch (unionSymbol)
                        {
                            case AndValue:
                                current = Expression.And(last, current);
                                break;
                            case OrValue:
                                current = Expression.Or(last, current);
                                break;
                        }
                        unionSymbol = null;
                    }
                    last = current;
                    index += 3;
                }
            }

            return Expression.Lambda<Func<TIn, bool>>(current, parameter);
        }

        private object ConvertValue(string value, Type type)
        {
            if (value == NullValue)
            {
                return null;
            }

            switch (type.Name)
            {
                case "Int16":
                    return Int16.Parse(value);
                case "Int32":
                    return Int32.Parse(value);
                case "Int64":
                    return Int64.Parse(value);
                case "Decimal":
                    return Decimal.Parse(value);
                case "Single":
                    return Single.Parse(value);
                case "Double":
                    return Double.Parse(value);
                case "Guid":
                    return Guid.Parse(value);
                case "Boolean":
                    return Boolean.Parse(value);
                case "String":
                    return value.Trim(QuoteValue[0]).Replace(SpaceValue, " ");
                default:
                    throw new Exception($"Unsupported type {type.Name}");
            }
        }

        public static Term Create(string expression)
        {
            return new Term(expression);
        }

        public static Term Create<TIn, TOut>(Expression<Func<TIn, TOut>> expression)
        {
            Func<TIn, TOut> func = expression.Compile();
            string expressionString = expression.Body.ToString();
            string paramName = expression.Parameters.FirstOrDefault()?.Name;

            FieldInfo[] funcFields = func.Target.GetType().GetFields();
            foreach (FieldInfo field in funcFields)
            {
                object fieldValue = field.GetValue(func.Target);
                if (fieldValue != null && fieldValue is object[] fieldValueMembers)
                {
                    foreach (object valueMember in fieldValueMembers)
                    {
                        FieldInfo[] memberFields = valueMember.GetType().GetFields();
                        foreach (FieldInfo memberField in memberFields)
                        {
                            string variableName = $"value({memberField.DeclaringType.FullName}).{memberField.Name}";
                            string value = GetVaribaleValue(memberField.GetValue(valueMember));
                            if(expressionString.Contains($"Convert({variableName}, "))
                            {
                                expressionString = expressionString.Replace($"{variableName}", "variable");
                                Regex convertPattern = new Regex("Convert\\(variable, .*\\)");
                                expressionString = convertPattern.Replace(expressionString, value);
                            }
                            else
                            {
                                expressionString = expressionString.Replace($"{variableName}", value);
                            }
                        }
                    }
                }
            }

            expressionString = expressionString.Replace($"{paramName}.", "").Replace("(", "").Replace(")", "");

            return new Term(expressionString);
        }

        private static string GetVaribaleValue(object value)
        {
            if (value == null)
            {
                return NullValue;
            }
            else if (value is string)
            {
                return $"{QuoteValue}{value}{QuoteValue}";
            }
            else
            {
                return value.ToString();
            }
        }
    }
}