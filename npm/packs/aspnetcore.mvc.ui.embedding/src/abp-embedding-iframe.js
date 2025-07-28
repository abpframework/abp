(function() {
    'use strict';

    // Check if we're in an iframe
    if (window.self === window.top) {
        return; // Not in iframe, exit
    }

    /**
     * ABP Embedding Iframe Handler
     * Simple one-way height reporting to parent
     */
    class AbpEmbeddingIframeHandler {
        constructor() {
            this.config = {
                minHeight: 100,
                maxHeight: 10000,
                debounceDelay: 100
            };
            this.isEnabled = false;
            this.observer = null;
            this.debounceTimer = null;
            this.lastReportedHeight = 0;
            this.lastReportedUrl = null;

            // Bind methods to ensure correct 'this' context
            this.measureHeight = this.measureHeight.bind(this);
            this.reportHeight = this.reportHeight.bind(this);
            this.debouncedReportHeight = this.debouncedReportHeight.bind(this);
            this.onContentChange = this.onContentChange.bind(this);
            this.reportUrlChange = this.reportUrlChange.bind(this);
            this.handlePopState = this.handlePopState.bind(this);
            this.handleHashChange = this.handleHashChange.bind(this);
        }

        init() {
            // Auto-enable for iframe context
            this.enable();

            // Report initial URL
            this.reportUrlChange();

            // Setup URL change monitoring
            this.setupUrlMonitoring();

            // Wait for DOM to be ready
            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', () => {
                    this.onDomReady();
                });
            } else {
                this.onDomReady();
            }
        }

        onDomReady() {
            // Wait for content to load
            if (document.readyState === 'complete') {
                this.onPageLoaded();
            } else {
                window.addEventListener('load', () => {
                    this.onPageLoaded();
                });
            }
        }

        onPageLoaded() {
            // Wait for layout completion then report height
            this.waitForLayout().then(() => {
                this.reportHeight();
            });
        }

        async waitForLayout() {
            // Wait for images to load
            const images = document.querySelectorAll('img');
            const imagePromises = Array.from(images).map(img => {
                if (img.complete) return Promise.resolve();
                
                return new Promise(resolve => {
                    const timeout = setTimeout(resolve, 2000); // Don't wait forever
                    img.addEventListener('load', () => { clearTimeout(timeout); resolve(); });
                    img.addEventListener('error', () => { clearTimeout(timeout); resolve(); });
                });
            });
            
            await Promise.all(imagePromises);
            
            // Wait for layout
            await new Promise(resolve => {
                requestAnimationFrame(() => {
                    requestAnimationFrame(resolve);
                });
            });
            
            // Small delay for CSS/fonts
            await new Promise(resolve => setTimeout(resolve, 100));
        }

        enable() {
            if (this.isEnabled) return;
            
            this.isEnabled = true;
            this.setupMonitoring();
        }

        disable() {
            this.isEnabled = false;
            
            if (this.observer) {
                this.observer.disconnect();
                this.observer = null;
            }
            
            if (this.debounceTimer) {
                clearTimeout(this.debounceTimer);
                this.debounceTimer = null;
            }
            
            window.removeEventListener('resize', this.debouncedReportHeight);
            
            // Remove URL monitoring listeners
            window.removeEventListener('popstate', this.handlePopState);
            window.removeEventListener('hashchange', this.handleHashChange);
        }

        setupMonitoring() {
            // Use MutationObserver for content changes
            this.observer = new MutationObserver(this.onContentChange);
            this.observer.observe(document.body, {
                childList: true,
                subtree: true,
                attributes: true,
                attributeFilter: ['style', 'class', 'height', 'width']
            });

            // Monitor window resize
            window.addEventListener('resize', this.debouncedReportHeight);
            
            // Monitor common dynamic content events
            this.setupDynamicContentMonitoring();
        }

        setupDynamicContentMonitoring() {
            // Monitor for images loading
            const handleImageLoad = this.debouncedReportHeight;
            document.addEventListener('load', handleImageLoad, true);
            document.addEventListener('error', handleImageLoad, true);
            
            // Monitor for AJAX/fetch requests
            const originalFetch = window.fetch;
            if (originalFetch) {
                window.fetch = (...args) => {
                    return originalFetch.apply(this, args).then(response => {
                        setTimeout(this.debouncedReportHeight, 100);
                        return response;
                    });
                };
            }
            
            // Monitor XMLHttpRequest
            const originalXHROpen = XMLHttpRequest.prototype.open;
            XMLHttpRequest.prototype.open = function(...args) {
                this.addEventListener('loadend', handleImageLoad);
                return originalXHROpen.apply(this, args);
            };
        }

        onContentChange(mutations) {
            let shouldUpdate = false;
            
            for (const mutation of mutations) {
                if (mutation.type === 'childList' && 
                    (mutation.addedNodes.length > 0 || mutation.removedNodes.length > 0)) {
                    shouldUpdate = true;
                    break;
                } else if (mutation.type === 'attributes') {
                    shouldUpdate = true;
                    break;
                }
            }
            
            if (shouldUpdate) {
                this.debouncedReportHeight();
            }
        }

        measureHeight() {
            // Force layout calculation
            document.body.offsetHeight;

            // Try different height measurement methods
            const heights = [
                Math.max(document.body.scrollHeight, document.body.offsetHeight),
                Math.max(document.documentElement.scrollHeight, document.documentElement.offsetHeight)
            ];

            // Get the maximum height
            let maxHeight = Math.max(...heights);
            
            if (maxHeight === 0) {
                maxHeight = window.innerHeight || 400;
            }

            // Add small padding and ensure within bounds
            maxHeight += 10;
            return Math.max(
                this.config.minHeight,
                Math.min(maxHeight, this.config.maxHeight)
            );
        }

        reportHeight() {
            if (!this.isEnabled) return;

            const height = this.measureHeight();
            const threshold = this.lastReportedHeight === 0 ? 1 : 3;
            
            if (Math.abs(height - this.lastReportedHeight) < threshold) {
                return;
            }

            this.lastReportedHeight = height;

            // Send height to parent
            if (window.parent && window.parent !== window) {
                window.parent.postMessage({
                    type: 'height-update',
                    height: height,
                    timestamp: Date.now()
                }, '*');
            }
        }

        debouncedReportHeight() {
            if (this.debounceTimer) {
                clearTimeout(this.debounceTimer);
            }

            this.debounceTimer = setTimeout(() => {
                this.reportHeight();
                this.debounceTimer = null;
            }, this.config.debounceDelay);
        }

        setupUrlMonitoring() {
            // Monitor popstate events (back/forward navigation)
            window.addEventListener('popstate', this.handlePopState);
            
            // Monitor hashchange events
            window.addEventListener('hashchange', this.handleHashChange);
            
            // Override history methods to catch programmatic navigation
            this.overrideHistoryMethods();
            
            // Monitor for navigation through other means
            this.setupNavigationMonitoring();
        }

        overrideHistoryMethods() {
            const originalPushState = history.pushState;
            const originalReplaceState = history.replaceState;
            
            history.pushState = (...args) => {
                const result = originalPushState.apply(history, args);
                setTimeout(() => this.reportUrlChange(), 0);
                return result;
            };
            
            history.replaceState = (...args) => {
                const result = originalReplaceState.apply(history, args);
                setTimeout(() => this.reportUrlChange(), 0);
                return result;
            };
        }

        setupNavigationMonitoring() {
            // Monitor for clicks on links
            document.addEventListener('click', (event) => {
                const link = event.target.closest('a');
                if (link && link.href) {
                    // Delay to allow navigation to complete
                    setTimeout(() => this.reportUrlChange(), 100);
                }
            });
            
            // Monitor for form submissions
            document.addEventListener('submit', () => {
                setTimeout(() => this.reportUrlChange(), 100);
            });
        }

        handlePopState(event) {
            this.reportUrlChange();
        }

        handleHashChange(event) {
            this.reportUrlChange();
        }

        reportUrlChange() {
            try {
                const currentUrl = window.location.href;
                
                // Only report if URL actually changed
                if (currentUrl !== this.lastReportedUrl) {
                    this.lastReportedUrl = currentUrl;
                    
                    // Send URL change to parent
                    if (window.parent && window.parent !== window) {
                        window.parent.postMessage({
                            type: 'url-change',
                            url: currentUrl,
                            timestamp: Date.now()
                        }, '*');
                    }
                }
            } catch (e) {
                console.warn('ABP Embedding Iframe: Failed to report URL change', e);
            }
        }
    }

    // Create and initialize the handler
    const handler = new AbpEmbeddingIframeHandler();
    handler.init();

    // Expose handler globally for manual control if needed
    window.abpEmbeddingIframeHandler = handler;

})(); 