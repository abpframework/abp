Prism.languages.scheme = {
	'comment': /;.*/,
	'string': {
		pattern: /"(?:[^"\\]|\\.)*"/,
		greedy: true
	},
	'symbol': {
		pattern: /'[^()#'\s]+/,
		greedy: true
	},
	'character': {
		pattern: /#\\(?:[ux][a-fA-F\d]+|[-a-zA-Z]+|\S)/,
		greedy: true,
		alias: 'string'
	},
	'lambda-parameter': [
		// https://www.cs.cmu.edu/Groups/AI/html/r4rs/r4rs_6.html#SEC30
		{
			pattern: /(\(lambda\s+)[^()'\s]+/,
			lookbehind: true
		},
		{
			pattern: /(\(lambda\s+\()[^()']+/,
			lookbehind: true
		}
	],
	'keyword': {
		pattern: /(\()(?:define(?:-library|-macro|-syntax|-values)?|defmacro|(?:case-)?lambda|let(?:(?:\*|rec)?(?:-values)?|-syntax|rec-syntax)|else|if|cond|begin|delay(?:-force)?|parameterize|guard|set!|(?:quasi-)?quote|syntax-(?:case|rules))(?=[()\s]|$)/,
		lookbehind: true
	},
	'builtin': {
		pattern: /(\()(?:(?:cons|car|cdr|list|call-with-current-continuation|call\/cc|append|abs|apply|eval)\b|null\?|pair\?|boolean\?|eof-object\?|char\?|procedure\?|number\?|port\?|string\?|vector\?|symbol\?|bytevector\?)(?=[()\s]|$)/,
		lookbehind: true
	},
	'number': {
		// This pattern (apart from the lookarounds) works like this:
		//
		// Decimal numbers
		// <dec real>       := \d*\.?\d+(?:[eE][+-]?\d+)?|\d+\/\d+
		// <dec complex>    := <dec real>(?:[+-]<dec real>i)?|<dec real>i
		// <dec prefix>     := (?:#d(?:#[ei])?|#[ei](?:#d)?)?
		// <dec number>     := <dec prefix>[+-]?<complex>
		//
		// Binary, octal, and hexadecimal numbers
		// <b.o.x. real>    := [\da-fA-F]+(?:\/[\da-fA-F]+)?
		// <b.o.x. complex> := <b.o.x. real>(?:[+-]<b.o.x. real>i)?|<b.o.x. real>i
		// <b.o.x. prefix>  := #[box](?:#[ei])?|#[ei](?:#[box])?
		// <b.o.x. number>  := <b.o.x. prefix>[+-]?<b.o.x. complex>
		//
		// <number>         := <dec number>|<b.o.x. number>
		pattern: /(^|[\s()])(?:(?:#d(?:#[ei])?|#[ei](?:#d)?)?[+-]?(?:(?:\d*\.?\d+(?:[eE][+-]?\d+)?|\d+\/\d+)(?:[+-](?:\d*\.?\d+(?:[eE][+-]?\d+)?|\d+\/\d+)i)?|(?:\d*\.?\d+(?:[eE][+-]?\d+)?|\d+\/\d+)i)|(?:#[box](?:#[ei])?|#[ei](?:#[box])?)[+-]?(?:[\da-fA-F]+(?:\/[\da-fA-F]+)?(?:[+-][\da-fA-F]+(?:\/[\da-fA-F]+)?i)?|[\da-fA-F]+(?:\/[\da-fA-F]+)?i))(?=[()\s]|$)/,
		lookbehind: true
	},
	'boolean': {
		pattern: /(^|[\s()])#[ft](?=[()\s]|$)/,
		lookbehind: true
	},
	'operator': {
		pattern: /(\()(?:[-+*%\/]|[<>]=?|=>?)(?=[()\s]|$)/,
		lookbehind: true
	},
	'function': {
		pattern: /(\()[^()'\s]+(?=[()\s]|$)/,
		lookbehind: true
	},
	'punctuation': /[()']/
};
