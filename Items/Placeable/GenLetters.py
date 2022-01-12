#!/usr/bin/env python3
import string
template = """
	public class LetterBlock{name}: LetterBlock<Blocks.LetterBlock{name}, Items.Placeable.LetterBlock{name}> {{
	    public override String _getLetter1() {{ return "{0}"; }}
		public override String _getLetter2() {{ return "{1}"; }}
	}}"""

# all names have a Z in front so they sort at the end of the list.
names = {
	'`':  "ZGrave",
	'-':  "ZHyphen",
	'=':  "ZEqual",
	'[':  "ZLeftBracket",
	']':  "ZRightBracket",
	'\\': "ZBackslash",
	';':  "ZSemicolon",
	"'":  "ZApostrophe",
	',':  "ZComma",
	'.':  "ZPeriod",
	'/':  "ZSlash",
	'!':  "ZExclamation",
	'@':  "ZAt",
	'#':  "ZHash",
	'$':  "ZDollar",
	'%':  "ZPercent",
	'^':  "ZCaret",
	'&':  "ZAmpersand",
	'*':  "ZAsterisk",
	'(':  "ZLeftParen",
	')':  "ZRightParen",
	'_':  "ZUnderscore",
	'+':  "ZPlus",
	'~':  "ZTilde",
	'{':  "ZLeftBrace",
	'}':  "ZRightBrace",
	'|':  "ZPipe",
	':':  "ZColon",
	'"':  "ZQuote",
	'<':  "ZLeftAngle",
	'>':  "ZRightAngle",
	'?':  "ZQuestion",
}
lowers = {
	'1': '!',
	'2': '@',
	'3': '#',
	'4': '$',
	'5': '%',
	'6': '^',
	'7': '&',
	'8': '*',
	'9': '(',
	'0': ')',
	'-': '_',
	'=': '+',
	'`': '~',
	'[': '{',
	']': '}',
	'\\': '|',
	';': ':',
	"'": '"', # not confusing at all
	',': '<',
	'.': '>',
	'/': '?',
}

for c in string.ascii_uppercase + string.digits + "`-=[]\\;',./":
	name  = names.get(c, c)
	c2    = lowers.get(c, c.lower())
	name2 = names.get(c2, c2)
	print(template.format(c, c2, name=name, name2=name2).replace('\t', '    '), end='')

# you will have to manually fix instances of \ and " in the output
