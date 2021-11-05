import { Generic } from '../models';

export class GenericsCollector {
  private _generics: Generic[] = [];
  get generics() {
    return this._generics;
  }

  apply = (value: string, index: number) => {
    const generic = this.get(index);
    if (generic) {
      if (!generic.type) generic.setType(value);
      return value + generic.default;
    }

    return value;
  };

  constructor(private getTypeIdentifier = (type: string) => type) {}

  private createGeneric(type: string, ref: string, defaultValue: string) {
    const _default = this.getTypeIdentifier(defaultValue);
    const refs = [generateRefWithPlaceholders(ref)];
    const generic = new Generic({ type, default: _default, refs });
    return generic;
  }

  private register(index: number, generic: Generic) {
    const existing = this.get(index);
    if (existing) {
      existing.setDefault(generic.default);
      existing.setType(generic.type);
    } else this.set(index, generic);
  }

  collect(generics: string[], genericArguments: string[]) {
    generics.forEach((ref, i) => {
      const generic = this.createGeneric(
        genericArguments[i],
        ref,
        genericArguments.includes(ref) ? '' : ref,
      );
      this.register(i, generic);
    });
  }

  get(index: number) {
    return this.generics[index];
  }

  set(index: number, value: Generic) {
    this.generics[index] = value;
  }

  reset() {
    this._generics = [];
  }
}

export function generateRefWithPlaceholders(sourceType: string) {
  // eslint-disable-next-line prefer-const
  let { identifier, generics } = extractGenerics(sourceType);

  generics = generics.map((_, i) => `T${i}`);

  return generics.length ? `${identifier}<${generics}>` : identifier;
}

export function extractSimpleGenerics(sourceType: string) {
  const { identifier, generics } = extractGenerics(sourceType);

  return {
    identifier: getLastSegment(identifier),
    generics: generics.map(getLastSegment),
  };
}

export function extractGenerics(sourceType: string) {
  const regex = /(?<identifier>[^<]+)(<(?<generics>.+)>)?/g;
  const { identifier = '', generics = '' } = regex.exec(sourceType)?.groups ?? {};

  return {
    identifier,
    generics: generics.split(/,\s*/).filter(Boolean),
  };
}

function getLastSegment(str: string) {
  return str.split('.').pop()!;
}

export function replacePlaceholdersWithGenerics(
  type: string,
  generics: string[],
  genericsCollector: GenericsCollector,
) {
  return generics
    .map(genericsCollector.apply)
    .reduce((acc, v, i) => acc.replace(new RegExp(`([<, ])T${i}([,>])`, 'g'), `$1${v}$2`), type);
}
