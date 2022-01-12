#!/usr/bin/env python3
import string
template = """
	public class LetterBlock{name}: LetterBlock<Items.Placeable.LetterBlock{name}> {{
		public override int _getFrame() {{ return {frame}; }}
	}}"""

# all names have a Z in front so they sort at the end of the list.
# (this does not apply to blocks, but they need to match the items.)
# funny how blocks that don't do anything have taken the most effort
# so far, aside from trying to make the stupid gravity shit work.
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
}

for i, c in enumerate(string.ascii_uppercase + string.digits + "`-=[]\\;',./"):
	name = names.get(c, c)
	print(template.format(name=name, frame=i).replace('\t', '    '), end='')

# you will have to manually fix instances of \ and " in the output
