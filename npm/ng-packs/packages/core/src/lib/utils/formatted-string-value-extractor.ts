class ExtractionResult {
  public isMatch: boolean;
  public matches: any[];

  constructor(isMatch: boolean) {
    this.isMatch = isMatch;
    this.matches = [];
  }
}

enum FormatStringTokenType {
  ConstantText,
  DynamicValue,
}

class FormatStringToken {
  public text: string;
  public type: FormatStringTokenType;

  constructor(text: string, type: FormatStringTokenType) {
    this.text = text;
    this.type = type;
  }
}

class FormatStringTokenizer {
  tokenize(format: string, includeBracketsForDynamicValues: boolean = false): FormatStringToken[] {
    const tokens: FormatStringToken[] = [];

    let currentText = '';
    let inDynamicValue = false;

    for (let i = 0; i < format.length; i++) {
      const c = format[i];
      switch (c) {
        case '{':
          if (inDynamicValue) {
            throw new Error(
              'Incorrect syntax at char ' +
                i +
                '! format string can not contain nested dynamic value expression!',
            );
          }

          inDynamicValue = true;

          if (currentText.length > 0) {
            tokens.push(new FormatStringToken(currentText, FormatStringTokenType.ConstantText));
            currentText = '';
          }

          break;
        case '}':
          if (!inDynamicValue) {
            throw new Error(
              'Incorrect syntax at char ' +
                i +
                '! These is no opening brackets for the closing bracket }.',
            );
          }

          inDynamicValue = false;

          if (currentText.length <= 0) {
            throw new Error(
              'Incorrect syntax at char ' + i + '! Brackets does not containt any chars.',
            );
          }

          let dynamicValue = currentText;
          if (includeBracketsForDynamicValues) {
            dynamicValue = '{' + dynamicValue + '}';
          }

          tokens.push(new FormatStringToken(dynamicValue, FormatStringTokenType.DynamicValue));
          currentText = '';

          break;
        default:
          currentText += c;
          break;
      }
    }

    if (inDynamicValue) {
      throw new Error('There is no closing } char for an opened { char.');
    }

    if (currentText.length > 0) {
      tokens.push(new FormatStringToken(currentText, FormatStringTokenType.ConstantText));
    }

    return tokens;
  }
}

export class FormattedStringValueExtractor {
  extract(str: string, format: string): ExtractionResult {
    if (str === format) {
      return new ExtractionResult(true);
    }

    const formatTokens = new FormatStringTokenizer().tokenize(format);
    if (!formatTokens) {
      return new ExtractionResult(str === '');
    }

    const result = new ExtractionResult(true);

    for (let i = 0; i < formatTokens.length; i++) {
      const currentToken = formatTokens[i];
      const previousToken = i > 0 ? formatTokens[i - 1] : null;

      if (currentToken.type === FormatStringTokenType.ConstantText) {
        if (i === 0) {
          if (str.indexOf(currentToken.text) !== 0) {
            result.isMatch = false;
            return result;
          }

          str = str.substr(currentToken.text.length, str.length - currentToken.text.length);
        } else {
          const matchIndex = str.indexOf(currentToken.text);
          if (matchIndex < 0) {
            result.isMatch = false;
            return result;
          }

          result.matches.push({ name: previousToken.text, value: str.substr(0, matchIndex) });
          str = str.substring(0, matchIndex + currentToken.text.length);
        }
      }
    }

    const lastToken = formatTokens[formatTokens.length - 1];
    if (lastToken.type === FormatStringTokenType.DynamicValue) {
      result.matches.push({ name: lastToken.text, value: str });
    }

    return result;
  }

  isMatch(str: string, format: string): string[] {
    const result = new FormattedStringValueExtractor().extract(str, format);
    if (!result.isMatch) {
      return [];
    }

    const values = [];
    for (let i = 0; i < result.matches.length; i++) {
      values.push(result.matches[i].value);
    }

    return values;
  }
}
