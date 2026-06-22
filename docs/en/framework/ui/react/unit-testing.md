```json
//[doc-seo]
{
    "Description": "Learn how to run and write unit tests in ABP React UI applications with Vitest and React Testing Library."
}
```

# Unit Testing React UI

ABP React UI templates are preconfigured for unit testing. A solution created with ABP Studio v3.0+ or `abp new --modern --ui-framework react` includes Vitest, jsdom, React Testing Library, and jest-dom.

You can add a test file and run the test command without adding extra test infrastructure.

## Test Stack

The React template uses:

| Package | Purpose |
| --- | --- |
| `vitest` | Test runner and assertion library. |
| `jsdom` | Browser-like DOM environment for component tests. |
| `@testing-library/react` | Render React components and query the DOM like a user. |
| `@testing-library/jest-dom` | Extra DOM assertions such as `toBeInTheDocument`. |

The template also includes `src/test/setup.ts`, which imports `@testing-library/jest-dom/vitest` and initializes the React i18n setup.

## Configuration

The test configuration is in `vitest.config.ts`:

```ts
import { defineConfig } from 'vitest/config'
import react from '@vitejs/plugin-react'
import path from 'path'

export default defineConfig({
  plugins: [react()],
  test: {
    environment: 'jsdom',
    setupFiles: ['./src/test/setup.ts'],
    include: ['src/**/*.{test,spec}.{ts,tsx}'],
    globals: true,
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
})
```

Tests can import application files with the same `@/` alias used by the app.

## Running Tests

Install dependencies once:

```bash
npm install
```

Run tests in watch mode:

```bash
npm run test
```

Run tests once, which is useful for CI:

```bash
npm run test:run
```

The template's `package.json` maps these commands to `vitest` and `vitest run`.

## Example Test

The template includes example tests under `src/`. For example, `src/pages/home/HomePage.test.tsx` renders the home page and mocks the authentication hook:

```tsx
import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import { HomePage } from './HomePage'
import * as auth from '@/lib/auth/AuthContext'

vi.mock('@/lib/auth/AuthContext', () => ({
  useAuth: vi.fn(),
}))

describe('HomePage', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('renders login prompt when not authenticated', () => {
    vi.mocked(auth.useAuth).mockReturnValue({
      isAuthenticated: false,
      isLoading: false,
      user: null,
      login: vi.fn(),
      logout: vi.fn(),
      navigateToLogin: vi.fn(),
      getAccessToken: vi.fn(),
    } as unknown as ReturnType<typeof auth.useAuth>)

    render(<HomePage />)
    expect(screen.getByText('Welcome')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /login/i })).toBeInTheDocument()
  })
})
```

This style keeps the test focused on visible behavior. Dependencies that would require real authentication, network calls, or browser redirects are mocked.

## Writing a Component Test

Create a `*.test.tsx` file next to the component:

```tsx
import { render, screen } from '@testing-library/react'
import { describe, expect, it } from 'vitest'
import { Button } from '@/components/ui/button'

describe('Button', () => {
  it('renders its content', () => {
    render(<Button>Save</Button>)
    expect(screen.getByRole('button', { name: 'Save' })).toBeInTheDocument()
  })
})
```

Prefer queries such as `getByRole`, `getByLabelText`, and `getByText` because they describe what the user can see or do.

## Writing a Service or Hook Test

For non-component logic, use Vitest directly. The template includes tests for routing guards, permissions, authentication context, and Axios interceptors.

When testing API code, mock the shared Axios instance or the lower-level dependency instead of calling a real backend. When testing permission behavior, mock the application configuration client or use the exported permission helpers.

## Interpreting Output

Vitest reports each test file, failed assertions, stack traces, and a summary of passed/failed tests. In watch mode, it reruns affected tests when files change. In `test:run` mode, Vitest exits with a non-zero status code if any test fails, which makes it suitable for CI pipelines.

If a component test fails because an ABP service is not initialized, mock the hook or provider used by the component. For example, pages that call `useAuth()` or `usePermissions()` should provide a controlled mock for those hooks unless the test is specifically verifying the provider.

## See Also

- [Components](./components/index.md)
- [Authorization](./authorization.md)
- [Permission Management](./permission-management.md)
