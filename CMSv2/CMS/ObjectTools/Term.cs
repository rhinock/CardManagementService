using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectTools
{
    public class Term
    {
        private const string NullValue = "null";
        private const string QuoteValue = "\"";

        private readonly string _input;
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string> { { "||", "OrElse" }, { "&&", "AndAlso" } };

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
            return new Term(_input.Replace(what, value));
        }

        public override string ToString()
        {
            return _input;
        }

        public Func<TIn, bool> ToDelegate<TIn>()
        {
            var parameter = Expression.Parameter(typeof(TIn), "x");

            string[] parts = _input.Split(' ');

            if((parts.Length % 3) != 0)
            {
                throw new Exception("Expression is difficult");
            }

            return null;
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
                            string value = GetVaribaleValue(memberField.GetValue(valueMember));
                            expressionString = expressionString.Replace($"value({memberField.DeclaringType.FullName}).{memberField.Name}", value);
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