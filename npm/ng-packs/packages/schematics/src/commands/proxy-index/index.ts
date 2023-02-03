import { Tree } from '@angular-devkit/schematics';
import {
  buildTargetPath,
  createProxyIndexGenerator,
  removeDefaultPlaceholders,
  resolveProject,
} from '../../utils';

export default function (schema: { target?: string; entryPoint?: string }) {
  const params = removeDefaultPlaceholders(schema);

  return async (host: Tree) => {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    const target = await resolveProject(host, params.target!);
    const targetPath = buildTargetPath(target.definition, params.entryPoint);

    const generateIndex = createProxyIndexGenerator(targetPath);

    return generateIndex(host);
  };
}
