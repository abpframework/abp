```json
//[doc-seo]
{
    "Description": "Learn about the component architecture and UI libraries used by ABP React UI applications."
}
```

# Components

ABP React UI templates use a source-owned component architecture. The generated app includes shadcn/ui-style primitives, layout components, feature components, route pages, and shared infrastructure under `src/lib/`.

The goal is to give you a working React application that you can customize without replacing framework-owned black boxes.

## Component Structure

The main React app is organized like this:

```text
src/
├── components/
│   ├── layout/
│   ├── ui/
│   └── identity/
├── lib/
│   ├── api/
│   ├── auth/
│   ├── i18n/
│   ├── routing/
│   └── theme/
├── locales/
├── pages/
└── routes/
```

The exact folders can vary by selected template options and modules.

## UI Stack

The React template uses:

| Library | Purpose |
| --- | --- |
| React | UI rendering. |
| Vite | Build tool and development server. |
| TanStack Router | Client-side routing. |
| TanStack Query | Server state, queries, mutations, and cache invalidation. |
| shadcn/ui-style components | Source-owned UI primitives built on Radix UI and Tailwind CSS. |
| Radix UI | Accessible low-level UI primitives. |
| Tailwind CSS | Utility-first styling and design tokens. |
| React Hook Form | Form state management. |
| Zod | Form and DTO validation schemas. |
| Axios | HTTP client. |
| i18next / react-i18next | Localization. |
| Zustand | Lightweight client state when needed. |
| Sonner | Toast notifications. |
| Lucide React | Icons. |

## `components/ui`

`src/components/ui/` contains reusable UI primitives. These components are copied into your project and can be edited directly.

Common components include:

- `Button`
- `Input`
- `Label`
- `Table`
- `Dialog`
- `DropdownMenu`
- `Select`
- `Card`
- `Tabs`
- `Badge`
- `DatePicker`
- `ConfirmDialog`

Use these primitives to build application pages and feature components.

```tsx
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'

export function ReportCard() {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Reports</CardTitle>
      </CardHeader>
      <CardContent>
        <Button>Refresh</Button>
      </CardContent>
    </Card>
  )
}
```

## Layout Components

Layout components are under `src/components/layout/`.

Important components include:

- `RootLayout`: root shell used by TanStack Router.
- `Header`: top bar, login button, theme toggle, and user menu.
- `Sidebar`: route-config-driven navigation menu.
- `UserMenu`: account-related dropdown menu.

The sidebar reads `src/lib/routing/route-config.ts`, checks authentication and permissions, and renders internal or external links.

## Feature Components

Feature-specific components should live near the feature that owns them. For example, Identity-specific layout components live under `src/components/identity/`, while Books-specific UI is implemented in `src/pages/books/BooksPage.tsx` in the sample template.

As a rule:

- Put generic, reusable primitives in `components/ui`.
- Put application layout in `components/layout`.
- Put feature-specific components under `components/<feature>` or next to the page when they are only used by one page.

## Pages

Route pages live under `src/pages/`. A page usually combines:

- UI primitives from `components/ui`.
- API functions from `src/lib/api`.
- Server state from TanStack Query.
- Form state from React Hook Form.
- Validation schemas from Zod.
- Permissions from `usePermissions()`.
- Localized strings from `useTranslation()`.

The Books page is the best full CRUD reference when the sample CRUD option is selected.

## Forms

Forms use React Hook Form and Zod:

```tsx
const productSchema = z.object({
  name: z.string().min(1, 'Required'),
  price: z.number().min(0),
})

type ProductFormData = z.infer<typeof productSchema>

const form = useForm<ProductFormData>({
  resolver: zodResolver(productSchema),
  defaultValues: {
    name: '',
    price: 0,
  },
})
```

This keeps runtime validation and TypeScript types close to each other.

## Routing Components

Routes are configured in `src/routes/router.tsx` with TanStack Router. Use:

- `authGuard` for authenticated pages.
- `createPermissionGuard('Permission.Name')` for permission-protected pages.
- `RootLayout` and nested layouts for shared page structure.

Menu entries are configured separately in `src/lib/routing/route-config.ts`, so route registration and navigation display can evolve independently.

## API Components and Hooks

API functions live under `src/lib/api/` and use the shared `api` Axios instance. Components normally consume these functions through TanStack Query:

```tsx
const usersQuery = useQuery({
  queryKey: ['app', 'users', queryParams],
  queryFn: () => getAppUsers(queryParams),
})
```

This keeps HTTP details out of rendering components and gives you caching, loading states, refetching, and mutation invalidation.

## See Also

- [Customization](../customization.md)
- [HTTP Requests](../http-requests.md)
- [Unit Testing](../unit-testing.md)
