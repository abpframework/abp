(function() {
    'use strict';

    // Extend abp namespace if it exists
    var abp = window.abp || {};
    if (!window.abp) {
        window.abp = abp;
    }

    abp.embedding = abp.embedding || {};

    // Static state to ensure only one instance manages history
    let historyManagerInstance = null;

    /**
     * ABP Embedding Web Component
     * Simple iframe with auto-height capability
     */
    class AbpEmbeddingElement extends HTMLElement {
        constructor() {
            super();
            this.iframe = null;
            this.isIframeLoaded = false;
            this.autoHeightConfig = null;
            this.isHistoryManager = false;
            this.initialSrc = null; // Original src attribute
            this.initialUrl = null; // Actual loaded URL
            
            // Bind methods
            this.handleIframeLoad = this.handleIframeLoad.bind(this);
            this.handleIframeMessage = this.handleIframeMessage.bind(this);
            this.handlePopState = this.handlePopState.bind(this);
        }

        static get observedAttributes() {
            return ['src', 'width', 'height', 'allow', 'sandbox', 'loading', 'auto-height'];
        }

        connectedCallback() {
            this.setupHistoryManager();
            this.render();
            this.setupEventListeners();
            this.handleInitialFragment();
        }

        disconnectedCallback() {
            this.cleanup();
        }

        attributeChangedCallback(name, oldValue, newValue) {
            if (oldValue !== newValue) {
                this.updateIframeAttribute(name, newValue);
            }
        }

        setupHistoryManager() {
            if (historyManagerInstance === null) {
                historyManagerInstance = this;
                this.isHistoryManager = true;
            } else {
                console.warn('ABP Embedding: Multiple instances detected. Only the first instance will manage browser history.');
            }
        }

        handleInitialFragment() {
            if (!this.isHistoryManager) return;

            // Store the original src attribute as initialSrc
            this.initialSrc = this.getAttribute('src');
            
            // Check if there's a fragment in the current URL
            const fragment = this.parseFragmentFromUrl();
            if (fragment && this.initialSrc) {
                // Build absolute URL from relative fragment
                const absoluteUrl = this.buildAbsoluteUrl(fragment);
                // Update iframe src to navigate to the fragment URL
                this.iframe.src = absoluteUrl;
            }
        }

        parseFragmentFromUrl() {
            const hash = window.location.hash;
            if (hash && hash.startsWith('#page=')) {
                return hash.substring(6); // Remove '#page=' prefix
            }
            return null;
        }

        buildAbsoluteUrl(relativePath) {
            if (!this.initialSrc) return relativePath;
            
            try {
                // If relativePath starts with '/', it's relative to the domain
                if (relativePath.startsWith('/')) {
                    const url = new URL(this.initialSrc);
                    return url.origin + relativePath;
                } else {
                    // Otherwise, it's relative to the current path
                    return new URL(relativePath, this.initialSrc).href;
                }
            } catch (e) {
                console.warn('ABP Embedding: Failed to build absolute URL', e);
                return relativePath;
            }
        }

        buildRelativePath(absoluteUrl) {
            if (!this.initialUrl || !absoluteUrl) return null;
            
            try {
                const initialUrlObj = new URL(this.initialUrl);
                const currentUrlObj = new URL(absoluteUrl);
                
                // Check if same origin
                if (initialUrlObj.origin !== currentUrlObj.origin) {
                    return null;
                }
                
                // Return pathname + search + hash relative to initial URL
                const relativePath = currentUrlObj.pathname + currentUrlObj.search + currentUrlObj.hash;
                const initialPath = initialUrlObj.pathname;
                
                // If it's the same as initial path, return relative
                if (relativePath === initialPath) {
                    return '/';
                }
                
                return relativePath;
            } catch (e) {
                console.warn('ABP Embedding: Failed to build relative path', e);
                return null;
            }
        }

        ensureInitialHistoryEntry() {
            if (!this.isHistoryManager) return;
            
            // Only push initial state if there's no fragment currently
            if (!window.location.hash || !window.location.hash.startsWith('#page=')) {
                // Replace current state to represent the initial iframe state
                history.replaceState({ isInitial: true }, '', window.location.href);
            }
        }

        updateUrlFragment(relativePath) {
            if (!this.isHistoryManager || !relativePath) return;
            
            const newHash = '#page=' + relativePath;
            if (window.location.hash !== newHash) {
                // Update URL without page refresh
                history.pushState({ relativePath: relativePath }, '', window.location.pathname + window.location.search + newHash);
            }
        }

        handlePopState(event) {
            if (!this.isHistoryManager || !this.iframe) return;
            
            const fragment = this.parseFragmentFromUrl();
            if (fragment && this.initialSrc) {
                const absoluteUrl = this.buildAbsoluteUrl(fragment);
                this.iframe.src = absoluteUrl;
            } else if (!fragment) {
                // No fragment, navigate back to initial URL
                if (this.initialUrl) {
                    this.iframe.src = this.initialUrl;
                } else if (this.initialSrc) {
                    this.iframe.src = this.initialSrc;
                }
            }
        }

        render() {
            // Clear existing content
            this.innerHTML = '';

            // Create iframe element
            this.iframe = document.createElement('iframe');
            
            // Set default attributes
            this.iframe.setAttribute('frameborder', '0');
            this.iframe.setAttribute('scrolling', 'auto');
            this.iframe.style.border = 'none';
            this.iframe.style.outline = 'none';
            this.iframe.style.display = 'block';
            this.iframe.style.width = '100%';

            // Apply custom attributes
            this.updateAllAttributes();

            // Add load event listener
            this.iframe.addEventListener('load', this.handleIframeLoad);

            this.appendChild(this.iframe);
        }

        updateAllAttributes() {
            const attributes = ['src', 'width', 'height', 'allow', 'sandbox', 'loading'];
            attributes.forEach(attr => {
                const value = this.getAttribute(attr);
                if (value) {
                    this.updateIframeAttribute(attr, value);
                }
            });
        }

        updateIframeAttribute(name, value) {
            if (!this.iframe) return;

            switch (name) {
                case 'src':
                    this.iframe.src = value;
                    this.isIframeLoaded = false;
                    break;
                case 'width':
                    this.iframe.style.width = value.includes('%') || value.includes('px') ? value : value + 'px';
                    break;
                case 'height':
                    if (!this.hasAttribute('auto-height')) {
                        this.iframe.style.height = value.includes('%') || value.includes('px') ? value : value + 'px';
                    }
                    break;
                case 'auto-height':
                    if (value === 'true' || value === '') {
                        this.enableAutoHeight();
                    } else {
                        this.disableAutoHeight();
                    }
                    break;
                default:
                    this.iframe.setAttribute(name, value);
            }
        }

        setupEventListeners() {
            // Listen for messages from iframe
            window.addEventListener('message', this.handleIframeMessage);
            
            // Listen for popstate events (back/forward buttons)
            if (this.isHistoryManager) {
                window.addEventListener('popstate', this.handlePopState);
            }
        }

        cleanup() {
            window.removeEventListener('message', this.handleIframeMessage);
            if (this.isHistoryManager) {
                window.removeEventListener('popstate', this.handlePopState);
                // Reset the static instance if this was the history manager
                if (historyManagerInstance === this) {
                    historyManagerInstance = null;
                }
            }
            if (this.iframe) {
                this.iframe.removeEventListener('load', this.handleIframeLoad);
            }
        }

        handleIframeLoad() {
            this.isIframeLoaded = true;

            // Dispatch custom event
            this.dispatchEvent(new CustomEvent('iframe-loaded', {
                detail: { iframe: this.iframe },
                bubbles: true
            }));
        }

        handleIframeMessage(event) {
            // Simple message filtering - only accept messages from our iframe
            if (this.iframe && event.source === this.iframe.contentWindow && event.data) {
                if (event.data.type === 'height-update') {
                    this.updateIframeHeight(event.data.height);
                } else if (event.data.type === 'url-change') {
                    this.handleUrlChange(event.data.url);
                }
            }
        }

        handleUrlChange(currentUrl) {
            try {
                if (!this.initialUrl) {
                    // First load - store the initial URL and ensure history entry exists
                    this.initialUrl = currentUrl;
                    this.ensureInitialHistoryEntry();
                } else if (this.isHistoryManager && currentUrl !== this.initialUrl) {
                    // Navigation detected - check if it's same domain
                    if (currentUrl.startsWith(this.initialUrl) || 
                        (this.initialSrc && currentUrl.startsWith(new URL(this.initialSrc).origin))) {
                        
                        // Generate relative path and update URL fragment
                        const relativePath = this.buildRelativePath(currentUrl);
                        if (relativePath) {
                            this.updateUrlFragment(relativePath);
                        }
                    }
                }
            } catch (e) {
                console.warn('ABP Embedding: Failed to handle URL change', e);
            }
        }

        /**
         * Update iframe height based on content
         */
        updateIframeHeight(height) {
            if (!this.iframe || !height || height <= 0) {
                return;
            }

            const config = this.autoHeightConfig || {};
            const minHeight = config.minHeight || 100;
            const maxHeight = config.maxHeight || window.innerHeight * 0.9;
            
            // Ensure height is within reasonable bounds
            const newHeight = Math.max(minHeight, Math.min(height, maxHeight));
            
            // Update iframe height
            this.iframe.style.height = newHeight + 'px';

            // Dispatch custom event
            this.dispatchEvent(new CustomEvent('height-updated', {
                detail: { 
                    height: newHeight,
                    originalHeight: height
                },
                bubbles: true
            }));
        }

        /**
         * Get the iframe element
         */
        getIframe() {
            return this.iframe;
        }

        /**
         * Check if iframe is loaded
         */
        isLoaded() {
            return this.isIframeLoaded;
        }

        /**
         * Enable auto-height functionality
         */
        enableAutoHeight(options = {}) {
            const config = {
                minHeight: options.minHeight || 100,
                maxHeight: options.maxHeight || window.innerHeight * 0.9,
                ...options
            };

            this.autoHeightConfig = config;
            this.setAttribute('auto-height', 'true');
        }

        /**
         * Disable auto-height functionality
         */
        disableAutoHeight() {
            this.removeAttribute('auto-height');
            this.autoHeightConfig = null;

            // Reset to original height if specified
            const originalHeight = this.getAttribute('height') || '400px';
            if (this.iframe) {
                this.iframe.style.height = originalHeight;
            }
        }
    }

    // Register the custom element
    if (!customElements.get('abp-embedding')) {
        customElements.define('abp-embedding', AbpEmbeddingElement);
    }

    // Add utility functions to abp.embedding namespace
    abp.embedding.create = function(options) {
        const element = document.createElement('abp-embedding');
        
        if (options.src) element.setAttribute('src', options.src);
        if (options.width) element.setAttribute('width', options.width);
        if (options.height) element.setAttribute('height', options.height);
        if (options.allow) element.setAttribute('allow', options.allow);
        if (options.sandbox) element.setAttribute('sandbox', options.sandbox);
        if (options.loading) element.setAttribute('loading', options.loading);
        if (options.autoHeight) element.setAttribute('auto-height', options.autoHeight);

        return element;
    };

    abp.embedding.enableAutoHeight = function(element, options) {
        if (element && typeof element.enableAutoHeight === 'function') {
            element.enableAutoHeight(options);
        }
    };

    abp.embedding.disableAutoHeight = function(element) {
        if (element && typeof element.disableAutoHeight === 'function') {
            element.disableAutoHeight();
        }
    };

})(); 