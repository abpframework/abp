import { readFileSync } from 'node:fs';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

/**
 * Sets up component resource resolution for Angular component tests.
 * This is needed when components have external templates or stylesheets.
 *
 * @param componentDirPath - The path to the component directory relative to the test file.
 *                          For example: '../components/loader-bar' or './components/my-component'
 * @param testFileUrl - The import.meta.url from the test file. Defaults to the caller's location.
 *
 * @example
 * ```typescript
 * 
 * import { setupComponentResources } from './utils';
 *
 * beforeAll(() => setupComponentResources('../components/loader-bar', import.meta.url));
 * ```
 */
export async function setupComponentResources(
  componentDirPath: string,
  testFileUrl: string = import.meta.url,
): Promise<void> {
  try {
    if (typeof process !== 'undefined' && process.versions?.node) {
      const { ɵresolveComponentResources: resolveComponentResources } = await import('@angular/core');
      
      // Get the test file directory path
      const testFileDir = dirname(fileURLToPath(testFileUrl));
      const componentDir = resolve(testFileDir, componentDirPath);

      await resolveComponentResources((url: string) => {
        // For SCSS/SASS files, return empty CSS since jsdom can't parse SCSS
        if (url.endsWith('.scss') || url.endsWith('.sass')) {
          return Promise.resolve('');
        }
        
        // For other files (HTML, CSS, etc.), read the actual content
        try {
          // Resolve relative paths like './component.scss' or 'component.scss'
          const normalizedUrl = url.replace(/^\.\//, '');
          const filePath = resolve(componentDir, normalizedUrl);
          return Promise.resolve(readFileSync(filePath, 'utf-8'));
        } catch (error) {
          // If file not found, return empty string
          return Promise.resolve('');
        }
      });
    }
  } catch (error) {
    console.warn('Failed to set up component resource resolver:', error);
  }
}
