# ABP Embedding Web Component

A framework-agnostic web component for embedding iframe content with data passing capabilities.

## Features

- ✅ **Framework Agnostic**: Works with plain HTML, Vue, Angular, React, and any other framework
- ✅ **Auto-Height**: Automatically resize iframe to match content height
- ✅ **Data Passing**: Send data to iframe content using postMessage API
- ✅ **Event Handling**: Listen for iframe load and message events
- ✅ **URL Synchronization**: Keep parent and iframe URLs in sync with browser history support
- ✅ **Clean Styling**: No borders, outlines, or visual artifacts by default
- ✅ **Responsive**: Built-in responsive behavior options
- ✅ **TypeScript Ready**: Includes type definitions
- ✅ **Accessible**: Proper ARIA attributes and semantic HTML

## Installation

```bash
npm install @abp/aspnetcore.mvc.ui.embedding
```

## Usage

### Basic HTML Usage

```html
<!DOCTYPE html>
<html>
<head>
    <link rel="stylesheet" href="node_modules/@abp/aspnetcore.mvc.ui.embedding/src/abp-embedding.css">
</head>
<body>
    <!-- Basic usage -->
    <abp-embedding src="https://example.com" width="800" height="600"></abp-embedding>

    <!-- With responsive behavior -->
    <abp-embedding src="https://example.com" class="responsive"></abp-embedding>

    <!-- With URL synchronization -->
    <abp-embedding src="https://example.com" width="100%" height="600px" url-sync="true"></abp-embedding>

    <!-- With auto-height -->
    <abp-embedding src="https://example.com" width="100%" auto-height="true"></abp-embedding>

    <script src="node_modules/@abp/aspnetcore.mvc.ui.embedding/src/abp-embedding.js"></script>
</body>
</html>
```

### JavaScript API

```javascript
// Create programmatically
const embedding = abp.embedding.create({
    src: 'https://example.com',
    width: '100%',
    height: '500px',
    urlSync: true // Enable URL synchronization
});

// Add to DOM
document.body.appendChild(embedding);

// Send data to iframe
embedding.sendData({ 
    type: 'user-data', 
    payload: { userId: 123, name: 'John Doe' } 
});

// Listen for iframe messages
embedding.addEventListener('iframe-message', (event) => {
    console.log('Received from iframe:', event.detail.data);
});

// Listen for iframe load
embedding.addEventListener('iframe-loaded', (event) => {
    console.log('Iframe loaded:', event.detail.iframe);
});

// Listen for URL synchronization
embedding.addEventListener('url-synced', (event) => {
    console.log('URL synced:', event.detail);
});

// Navigate programmatically (if URL sync is enabled)
embedding.syncUrl('/dashboard');

// Enable auto-height
abp.embedding.enableAutoHeight(embedding, {
    minHeight: 200,
    maxHeight: 1000,
    watchForChanges: true
});

// Listen for height updates
embedding.addEventListener('height-updated', (event) => {
    console.log('Height updated:', event.detail);
});
```

### Vue.js Usage

```vue
<template>
    <abp-embedding 
        :src="iframeUrl"
        width="100%"
        height="600px"
        @iframe-loaded="onIframeLoaded"
        @iframe-message="onIframeMessage"
        ref="embedding">
    </abp-embedding>
</template>

<script>
export default {
    data() {
        return {
            iframeUrl: 'https://example.com'
        };
    },
    methods: {
        onIframeLoaded() {
            // Send initial data
            this.$refs.embedding.sendData({
                type: 'init',
                config: { theme: 'dark' }
            });
        },
        onIframeMessage(event) {
            console.log('Message from iframe:', event.detail.data);
        }
    }
};
</script>
```

### Angular Usage

```typescript
// app.component.ts
import { Component, ViewChild, ElementRef } from '@angular/core';

@Component({
    selector: 'app-root',
    template: `
        <abp-embedding 
            src="https://example.com"
            width="100%"
            height="600px"
            (iframe-loaded)="onIframeLoaded()"
            (iframe-message)="onIframeMessage($event)"
            #embedding>
        </abp-embedding>
    `
})
export class AppComponent {
    @ViewChild('embedding') embedding!: ElementRef;

    onIframeLoaded() {
        this.embedding.nativeElement.sendData({
            type: 'angular-data',
            timestamp: Date.now()
        });
    }

    onIframeMessage(event: any) {
        console.log('Received:', event.detail.data);
    }
}
```

### React Usage

```jsx
import { useRef, useEffect } from 'react';

function EmbeddingComponent() {
    const embeddingRef = useRef(null);

    useEffect(() => {
        const element = embeddingRef.current;
        
        const handleLoad = () => {
            element.sendData({ type: 'react-init' });
        };

        const handleMessage = (event) => {
            console.log('Message:', event.detail.data);
        };

        element.addEventListener('iframe-loaded', handleLoad);
        element.addEventListener('iframe-message', handleMessage);

        return () => {
            element.removeEventListener('iframe-loaded', handleLoad);
            element.removeEventListener('iframe-message', handleMessage);
        };
    }, []);

    return (
        <abp-embedding
            ref={embeddingRef}
            src="https://example.com"
            width="100%"
            height="600px"
        />
    );
}
```

## Auto-Height Feature

The auto-height feature automatically adjusts the iframe height to match its content, preventing double scrollbars and providing a seamless user experience.

### Basic Usage

```html
<!-- Enable auto-height with attribute -->
<abp-embedding src="your-app.html" auto-height="true" width="100%"></abp-embedding>
```

### Programmatic Usage

```javascript
// Enable auto-height with options
const embedding = document.querySelector('abp-embedding');
abp.embedding.enableAutoHeight(embedding, {
    minHeight: 100,        // Minimum height in pixels
    maxHeight: 1000,       // Maximum height in pixels  
    watchForChanges: true  // Monitor content changes
});

// Disable auto-height
abp.embedding.disableAutoHeight(embedding);

// Manually request height update
embedding.requestHeightUpdate();
```

### Iframe Content Setup

Include this script in your iframe content to enable height communication:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Your Iframe Content</title>
</head>
<body>
    <!-- Your content here -->
    
    <script src="path/to/abp-embedding-iframe.js"></script>
</body>
</html>
```

The iframe script automatically:
- Waits for page to fully load (including images)
- Measures content height using multiple methods
- Sends height updates to parent via postMessage
- Monitors for content changes (if enabled)
- Handles configuration from parent

### Auto-Height Events

```javascript
// Listen for height updates
embedding.addEventListener('height-updated', (event) => {
    const { originalHeight, appliedHeight, wasConstrained } = event.detail;
    console.log(`Height: ${originalHeight}px → ${appliedHeight}px`);
    
    if (wasConstrained) {
        console.log('Height was constrained by min/max limits');
    }
});
```

### Configuration Options

```javascript
{
    minHeight: 100,        // Minimum iframe height (default: 100)
    maxHeight: 10000,      // Maximum iframe height (default: 90% of viewport)
    watchForChanges: true, // Monitor DOM changes (default: true)
    debounceDelay: 250     // Debounce delay for change detection (default: 250ms)
}
```

## URL Synchronization

The web component supports URL synchronization between the parent page and iframe content. When enabled, the component will:

- Keep parent and iframe URLs synchronized (paths only)
- Add `?embed=1` parameter to all iframe URLs
- Support browser back/forward navigation
- Work seamlessly with SPAs (Angular, React, Vue, etc.)

### Example URL Mapping

```
Parent URL:  https://parent.com/forms/2
Iframe URL:  https://iframe.com/forms/2?embed=1
```

### Enable URL Sync

```html
<abp-embedding 
    src="https://your-spa.com" 
    url-sync="true"
    width="100%" 
    height="600px">
</abp-embedding>
```

### Iframe Implementation for URL Sync

Your iframe application needs to implement URL change notifications:

```javascript
// Detect embedded mode
const isEmbedded = new URLSearchParams(window.location.search).has('embed');

// Your SPA router
function navigateToPath(path) {
    // Update your SPA content
    updateContent(path);
    
    // Notify parent if embedded
    if (isEmbedded) {
        window.parent.postMessage({
            type: 'url-change',
            path: path
        }, '*');
    }
}

// Listen for navigation commands from parent
window.addEventListener('message', function(event) {
    if (event.data && event.data.type === 'navigate') {
        // Update URL without notifying parent (to avoid loop)
        window.history.replaceState({}, '', event.data.path + '?embed=1');
        
        // Navigate in your SPA
        navigateToPath(event.data.path);
    }
});
```

### URL Sync Events

```javascript
embedding.addEventListener('url-synced', (event) => {
    const { type, oldPath, newPath, path, url } = event.detail;
    
    if (type === 'iframe-to-parent') {
        console.log(`URL synced from iframe: ${oldPath} → ${newPath}`);
    } else if (type === 'parent-to-iframe') {
        console.log(`URL synced to iframe: ${path}`);
    }
});
```

## Receiving Data in Your Iframe Application

Here's how to handle the data in your iframe content:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Iframe Content</title>
</head>
<body>
    <div id="content">
        <h1>Iframe Application</h1>
        <div id="received-data"></div>
        <button onclick="sendDataToParent()">Send Data to Parent</button>
    </div>

    <script>
        // Listen for messages from parent
        window.addEventListener('message', function(event) {
            console.log('Received data from parent:', event.data);
            
            // Handle different types of data
            if (event.data.type === 'user-data') {
                displayUserData(event.data.payload);
            } else if (event.data.type === 'init') {
                initializeApp(event.data.config);
            }
            
            // Update UI
            document.getElementById('received-data').innerHTML = 
                '<pre>' + JSON.stringify(event.data, null, 2) + '</pre>';
        });

        function displayUserData(userData) {
            console.log('User:', userData);
            // Handle user data...
        }

        function initializeApp(config) {
            console.log('Config:', config);
            // Initialize with config...
        }

        function sendDataToParent() {
            // Send data back to parent
            window.parent.postMessage({
                type: 'iframe-response',
                message: 'Hello from iframe!',
                timestamp: Date.now()
            }, '*');
        }

        // Notify parent that iframe is ready
        window.addEventListener('load', function() {
            window.parent.postMessage({
                type: 'iframe-ready'
            }, '*');
        });
    </script>
</body>
</html>
```

## Attributes

| Attribute | Type | Default | Description |
|-----------|------|---------|-------------|
| `src` | string | - | URL to load in the iframe |
| `width` | string | `100%` | Width of the iframe |
| `height` | string | `100%` | Height of the iframe |
| `allow` | string | - | Feature policy for the iframe |
| `sandbox` | string | - | Sandbox restrictions for the iframe |
| `loading` | string | - | Loading behavior (`lazy`, `eager`) |
| `url-sync` | boolean | `false` | Enable URL synchronization with browser history |
| `auto-height` | boolean | `false` | Enable automatic height adjustment to content |

## Events

| Event | Description | Detail |
|-------|-------------|--------|
| `iframe-loaded` | Fired when iframe finishes loading | `{ iframe: HTMLIFrameElement }` |
| `iframe-message` | Fired when iframe sends a message | `{ data: any, origin: string, source: Window }` |
| `url-synced` | Fired when URL synchronization occurs | `{ type: string, oldPath?: string, newPath?: string, path?: string, url?: string }` |
| `height-updated` | Fired when iframe height is updated | `{ originalHeight: number, appliedHeight: number, wasConstrained: boolean }` |

## Methods

| Method | Parameters | Description |
|--------|------------|-------------|
| `sendData(data, targetOrigin?)` | `data: any`, `targetOrigin: string = '*'` | Send data to iframe |
| `getIframe()` | - | Get the iframe element |
| `isLoaded()` | - | Check if iframe is loaded |
| `syncUrl(path)` | `path: string` | Navigate to path and sync URLs (requires `url-sync="true"`) |
| `enableAutoHeight(options?)` | `options: object` | Enable auto-height with configuration |
| `disableAutoHeight()` | - | Disable auto-height functionality |
| `requestHeightUpdate()` | - | Manually request height update from iframe |

## CSS Custom Properties

```css
abp-embedding {
    --border-radius: 8px;
    --box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    --background-color: #ffffff;
}
```

## Security Considerations

- Always validate data received from iframes
- Use specific `targetOrigin` when sending sensitive data
- Consider using `sandbox` attribute for untrusted content
- Implement proper CORS policies on your iframe content

## Browser Support

- Chrome 54+
- Firefox 63+
- Safari 10.1+
- Edge 79+

## License

LGPL-3.0 