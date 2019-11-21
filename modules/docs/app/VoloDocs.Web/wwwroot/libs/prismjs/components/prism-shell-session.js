Prism.languages['shell-session'] = {
	'command': {
		pattern: /\$(?:[^\r\n'"<]|(["'])(?:\\[\s\S]|\$\([^)]+\)|`[^`]+`|(?!\1)[^\\])*\1|((?:^|[^<])<<\s*)["']?(\w+?)["']?\s*(?:\r\n?|\n)(?:[\s\S])*?(?:\r\n?|\n)\3)+/,
		inside: {
			'bash': {
				pattern: /(\$\s*)[\s\S]+/,
				lookbehind: true,
				alias: 'language-bash',
				inside: Prism.languages.bash
			},
			'sh': {
				pattern: /^\$/,
				alias: 'important'
			}
		}
	},
	'output': {
		pattern: /.(?:.*(?:\r\n?|\n|.$))*/
		// output highlighting?
	}
}
