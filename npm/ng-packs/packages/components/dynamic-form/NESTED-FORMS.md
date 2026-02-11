# Nested Forms Guide

## Overview

Dynamic Form now supports **nested forms** with two new field types:
- **`group`** - Group related fields together
- **`array`** - Dynamic lists with add/remove functionality

## Quick Start

### 1. Group Type (Nested Fields)

Group related fields together with visual hierarchy:

```typescript
{
  key: 'address',
  type: 'group',
  label: 'Address Information',
  gridSize: 12,
  children: [
    {
      key: 'street',
      type: 'text',
      label: 'Street',
      gridSize: 8
    },
    {
      key: 'city',
      type: 'text',
      label: 'City',
      gridSize: 4
    },
    {
      key: 'zipCode',
      type: 'text',
      label: 'ZIP Code',
      gridSize: 6
    }
  ]
}
```

**Output:**
```json
{
  "address": {
    "street": "123 Main St",
    "city": "New York",
    "zipCode": "10001"
  }
}
```

### 2. Array Type (Dynamic Lists)

Create dynamic lists with add/remove buttons:

```typescript
{
  key: 'phoneNumbers',
  type: 'array',
  label: 'Phone Numbers',
  minItems: 1,
  maxItems: 5,
  gridSize: 12,
  children: [
    {
      key: 'type',
      type: 'select',
      label: 'Type',
      gridSize: 4,
      options: {
        defaultValues: [
          { key: 'mobile', value: 'Mobile' },
          { key: 'home', value: 'Home' },
          { key: 'work', value: 'Work' }
        ]
      }
    },
    {
      key: 'number',
      type: 'tel',
      label: 'Number',
      gridSize: 8
    }
  ]
}
```

**Output:**
```json
{
  "phoneNumbers": [
    { "type": "mobile", "number": "555-1234" },
    { "type": "work", "number": "555-5678" }
  ]
}
```

## Features

### Array Features
- ✅ **Add Button** - Adds new item (respects maxItems)
- ✅ **Remove Button** - Removes item (respects minItems)
- ✅ **Item Counter** - Shows current count and limits
- ✅ **Item Labels** - "Phone Number #1", "Phone Number #2"
- ✅ **Min/Max Validation** - Buttons automatically disabled
- ✅ **Empty State** - Shows info message when no items

### Group Features
- ✅ **Visual Hierarchy** - Border and background styling
- ✅ **Legend Label** - Fieldset with legend for accessibility
- ✅ **Grid Support** - All children support gridSize
- ✅ **Nested Groups** - Groups inside groups supported

### Recursive Support
- ✅ **Array in Array** - Phone numbers can have sub-arrays
- ✅ **Group in Array** - Work experience can have grouped fields
- ✅ **Array in Group** - Address can have multiple phone numbers
- ✅ **Unlimited Nesting** - No depth limit

## Advanced Examples

### Complex Nested Structure

```typescript
{
  key: 'workExperience',
  type: 'array',
  label: 'Work Experience',
  minItems: 0,
  maxItems: 10,
  children: [
    {
      key: 'company',
      type: 'text',
      label: 'Company Name',
      gridSize: 6,
      required: true
    },
    {
      key: 'position',
      type: 'text',
      label: 'Position',
      gridSize: 6,
      required: true
    },
    {
      key: 'dates',
      type: 'group',  // Nested group inside array
      label: 'Employment Dates',
      gridSize: 12,
      children: [
        {
          key: 'startDate',
          type: 'date',
          label: 'Start Date',
          gridSize: 6
        },
        {
          key: 'endDate',
          type: 'date',
          label: 'End Date',
          gridSize: 6
        }
      ]
    },
    {
      key: 'description',
      type: 'textarea',
      label: 'Description',
      gridSize: 12
    }
  ]
}
```

## API Reference

### FormFieldConfig (Extended)

```typescript
interface FormFieldConfig {
  // ... existing properties
  
  // NEW: Nested form properties
  children?: FormFieldConfig[];  // Child fields for group/array types
  minItems?: number;              // Minimum items for array (default: 0)
  maxItems?: number;              // Maximum items for array (default: unlimited)
}
```

### New Components

#### DynamicFormGroupComponent
```typescript
@Input() groupConfig: FormFieldConfig;
@Input() formGroup: FormGroup;
@Input() visible: boolean = true;
```

#### DynamicFormArrayComponent
```typescript
@Input() arrayConfig: FormFieldConfig;
@Input() formGroup: FormGroup;
@Input() visible: boolean = true;

addItem(): void;        // Add new item to array
removeItem(index): void; // Remove item from array
```

## Styling

### Group Styling

```scss
.form-group-container {
  border-left: 3px solid var(--bs-primary);
  padding: 1rem;
  background-color: var(--bs-light);
}
```

### Array Styling

```scss
.array-item {
  border: 1px solid var(--bs-border-color);
  padding: 1rem;
  margin-bottom: 1rem;
  background: white;
  
  &:hover {
    box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  }
}
```

## Accessibility

All nested forms include:
- ✅ **ARIA roles** (`role="group"`, `role="list"`, `role="listitem"`)
- ✅ **ARIA labels** (`aria-label`, `aria-labelledby`)
- ✅ **Live regions** (`aria-live="polite"` for item count)
- ✅ **Semantic HTML** (`<fieldset>`, `<legend>`)
- ✅ **Keyboard navigation** (Tab, Enter, Space)
- ✅ **Screen reader announcements**

## Migration Guide

### From Simple to Nested

**Before:**
```typescript
{
  key: 'street',
  type: 'text',
  label: 'Street'
},
{
  key: 'city',
  type: 'text',
  label: 'City'
}
```

**After:**
```typescript
{
  key: 'address',
  type: 'group',
  label: 'Address',
  children: [
    { key: 'street', type: 'text', label: 'Street' },
    { key: 'city', type: 'text', label: 'City' }
  ]
}
```

### Data Structure Change

**Before:**
```json
{
  "street": "123 Main St",
  "city": "New York"
}
```

**After:**
```json
{
  "address": {
    "street": "123 Main St",
    "city": "New York"
  }
}
```

## Best Practices

1. **Use Groups** for logical field grouping (address, contact info)
2. **Use Arrays** for dynamic lists (phone numbers, work history)
3. **Set minItems/maxItems** to prevent empty or excessive arrays
4. **Use gridSize** for responsive layouts within nested forms
5. **Keep nesting shallow** (max 2-3 levels for UX)
6. **Add validation** to required nested fields
7. **Use meaningful labels** for array items

## Examples

See `apps/dev-app/src/app/dynamic-form-page` for complete examples:
- Phone Numbers (simple array)
- Work Experience (complex array)
- Address (group)

## Troubleshooting

### Array items not showing
- Check `minItems` - may need to be > 0
- Verify `children` array is not empty

### Can't add items
- Check `maxItems` limit
- Verify button is not disabled

### Form data not nested
- Confirm `type: 'group'` or `type: 'array'`
- Check FormGroup structure in component

## Performance

- ✅ **OnPush** change detection
- ✅ **TrackBy** functions for arrays
- ✅ **Lazy rendering** for conditional fields
- ✅ **Minimal re-renders** on add/remove

