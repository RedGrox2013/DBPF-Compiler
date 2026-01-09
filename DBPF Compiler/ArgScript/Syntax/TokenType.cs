namespace DBPF_Compiler.ArgScript.Syntax
{
    public readonly struct TokenType(string name, string regex)
    {
        public readonly string Name = name;
        public readonly string Regex = regex;

        public override readonly string ToString() => Name;

        public static bool operator==(TokenType left, TokenType right) => left.Equals(right);
        public static bool operator!=(TokenType left, TokenType right) => !left.Equals(right);

        public override bool Equals(object? obj) =>
            obj is TokenType t && t.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase);

        public override int GetHashCode() => Name.GetHashCode();

        #region Token types
        public static readonly TokenType NUMBER = new(nameof(NUMBER), @"\d*\.?\d+");
        public static readonly TokenType HASH = new(nameof(HASH), @"0x([0-9A-Fa-f]{1,8})");
        //public static readonly TokenType SET = new(nameof(SET), @"\bset\w?\b");
        public static readonly TokenType ARGUMENT = new(nameof(ARGUMENT), @"\w+");
        public static readonly TokenType FLAG = new(nameof(FLAG), @"\-+\w+");
        public static readonly TokenType SPACE = new(nameof(SPACE), @"[ \t\x0B\f\r]+");
        public static readonly TokenType ENDL = new(nameof(ENDL), @"\n");
        ////public static readonly TokenType QUOT = new(nameof(QUOT), "\"\"");
        public static readonly TokenType STR = new(nameof(STR), @"\""[^\""\r\n]*\""");
        public static readonly TokenType BRACEEXPR = new(nameof(BRACEEXPR), @"[\(\{].*[\)\}]");
        public static readonly TokenType LPAR = new(nameof(LPAR), @"[\(\{]");
        public static readonly TokenType RPAR = new(nameof(RPAR), @"[\)\}]");
        //public static readonly TokenType LBRACE = new(nameof(LBRACE), @"\{");
        //public static readonly TokenType RBRACE = new(nameof(RBRACE), @"\}");
        public static readonly TokenType PLUS = new(nameof(PLUS), @"\+");
        public static readonly TokenType MINUS = new(nameof(MINUS), @"\-");
        public static readonly TokenType MULTIPLY = new(nameof(MULTIPLY), @"\*");
        public static readonly TokenType DEVIDE = new(nameof(DEVIDE), @"\/");
        public static readonly TokenType MOD = new(nameof(MOD), "%");
        public static readonly TokenType POWER = new(nameof(POWER), @"\^");
        public static readonly TokenType GREAT = new(nameof(GREAT), @"\>");
        public static readonly TokenType SMALL = new(nameof(SMALL), @"\<");
        public static readonly TokenType EQUAL = new(nameof(EQUAL), @"==");
        public static readonly TokenType NOTEQUAL = new(nameof(NOTEQUAL), @"!=");
        public static readonly TokenType MULTILCOMMENT = new(nameof(MULTILCOMMENT), @"\#\<[\w\s]*\#\>"); // не все символы работают, исправить
        public static readonly TokenType COMMENT = new(nameof(COMMENT), @"\#.*");
        public static readonly TokenType AND = new(nameof(AND), @"\band\b");
        public static readonly TokenType OR = new(nameof(OR), @"\bor\b");
        public static readonly TokenType NOT = new(nameof(NOT), @"\bnot\b");
        //public static readonly TokenType VARIABLE = new(nameof(VARIABLE), @"\$\w+");
        //public static readonly TokenType END = new(nameof(END), @"\bend\b");
        public static readonly TokenType DOLLAR = new(nameof(DOLLAR), @"\$");

        //public static readonly TokenType[] MainTokens = [NUMBER, SET, AND, OR, NOT, END, ARGUMENT,
        //    ENDL, SPACE, STR, BRACEEXPR, LPAR, RPAR, LBRACE, RBRACE, PLUS, MINUS, MULTIPLY, DEVIDE,
        //    MOD, POWER, VARIABLE, MULTILCOMMENT, GREAT, SMALL, EQUAL, NOTEQUAL, COMMENT];

        //public static readonly TokenType[] Keywords = [SET, END, ARGUMENT];
        //public static readonly TokenType[] BinOperators = [PLUS, MINUS, MULTIPLY, DEVIDE, MOD,
        //    POWER, GREAT, SMALL, EQUAL, NOTEQUAL, AND, OR];
        ////public static readonly TokenType[] UnarOperators = [NOT];
        //public static readonly TokenType[] ExpressionsTokens = [PLUS, MINUS, MULTIPLY, DEVIDE, MOD,
        //    POWER, GREAT, SMALL, EQUAL, NOTEQUAL, AND, OR, LPAR, RPAR, NUMBER, VARIABLE, ARGUMENT];

        public static readonly TokenType[] MainTokens = [HASH, NUMBER, ARGUMENT, FLAG, ENDL, SPACE, STR, BRACEEXPR, MULTILCOMMENT, COMMENT];
        public static readonly TokenType[] ExpressionsTokens = [AND, OR, NOT, .. MainTokens, LPAR, RPAR, /*LBRACE, RBRACE, */PLUS, MINUS, MULTIPLY,
            DEVIDE, MOD, POWER, GREAT, SMALL, EQUAL, NOTEQUAL];
        #endregion
    }
}
