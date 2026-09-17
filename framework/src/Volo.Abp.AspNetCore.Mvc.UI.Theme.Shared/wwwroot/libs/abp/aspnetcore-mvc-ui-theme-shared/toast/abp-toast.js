function AbpToastService(globalOptions) {
    // Static default configuration for all instances
    if (!AbpToastService.defaultOptions) {
        AbpToastService.defaultOptions = {
            closable: true,
            sticky: false,
            life: 5000,
            tapToDismiss: false,
            containerKey: undefined,
            iconClass: undefined,
            position: {
                top: 'auto',
                right: '30px',
                bottom: '30px',
                left: 'auto'
            }
        };
    }

    // Find existing container or create new one
    const containerId = globalOptions?.containerKey ? 
        `toast-container-${globalOptions.containerKey}` : 
        'toast-container';
    
    this.container = document.getElementById(containerId);
    if (!this.container) {
        this.container = document.createElement('div');
        this.container.id = containerId;
        this.container.className = 'abp-toast-container';
        document.body.appendChild(this.container);
    }
    
    this.toasts = [];
    this.lastId = 0;
    
    // Merge user provided global options with defaults
    this.globalOptions = this.extend({}, AbpToastService.defaultOptions, globalOptions);
    
    this.updateContainerPosition();
}

// Deep merge objects
AbpToastService.prototype.extend = function(target, ...sources) {
    sources.forEach(source => {
        for (const key in source) {
            if (source[key] && typeof source[key] === 'object') {
                target[key] = this.extend(target[key] || {}, source[key]);
            } else {
                target[key] = source[key];
            }
        }
    });
    return target;
};

// Update toast container position based on global options
AbpToastService.prototype.updateContainerPosition = function() {
    const { position } = this.globalOptions;
    Object.assign(this.container.style, position);
};

// Update global options
AbpToastService.prototype.setGlobalOptions = function(options) {
    this.globalOptions = this.extend({}, this.globalOptions, options);
    this.updateContainerPosition();
};

// Get icon class based on severity
AbpToastService.prototype.getIconClass = function(severity, options) {
    // Use custom icon class if provided
    if (options.iconClass) {
        return options.iconClass;
    }

    const icons = {
        success: 'fa fa-check',
        info: 'fa fa-info-circle',
        warning: 'fa fa-exclamation-triangle',
        error: 'fa fa-exclamation-circle'
    };
    return icons[severity] || 'fa fa-exclamation-triangle';
};

// Create toast DOM element
AbpToastService.prototype.createToastElement = function(message, title, severity, options) {
    const toast = document.createElement('div');
    toast.className = `abp-toast abp-toast-${severity}`;

    const closeButton = options.closable !== false ? 
        `<button class="abp-toast-close-button">
            <i class="fa fa-times" aria-hidden="true"></i>
        </button>` : '';

    const titleHtml = title ? `<div class="abp-toast-title">${title}</div>` : '';

    toast.innerHTML = `
        <div class="abp-toast-icon">
            <i class="${this.getIconClass(severity, options)} icon" aria-hidden="true"></i>
        </div>
        <div class="abp-toast-content">
            ${closeButton}
            ${titleHtml}
            <p class="abp-toast-message">${message}</p>
        </div>`;

    // Add event listeners
    const closeButtonElement = toast.querySelector('.abp-toast-close-button');
    if (closeButtonElement) {
        closeButtonElement.addEventListener('click', () => this.remove(toast));
    }

    if (options.tapToDismiss) {
        toast.addEventListener('click', () => this.remove(toast));
    }

    return toast;
};

// Show a toast with given options
AbpToastService.prototype.show = function(message, title, severity = 'neutral', options = {}) {
    const mergedOptions = this.extend({}, this.globalOptions, options);
    const id = ++this.lastId;
    const toast = this.createToastElement(message, title, severity, mergedOptions);
    
    // Set data attributes for non-object options
    Object.entries(mergedOptions)
        .filter(([_, value]) => typeof value !== 'object')
        .forEach(([key, value]) => toast.dataset[key] = value);
    
    toast.dataset.id = id;
    this.container.appendChild(toast);
    this.toasts.push(toast);

    // Auto remove if not sticky
    if (!mergedOptions.sticky) {
        setTimeout(() => this.remove(toast), mergedOptions.life);
    }

    return id;
};

// Remove a toast with animation
AbpToastService.prototype.remove = function(toastElement) {
    toastElement.classList.add('toast-removing');
    setTimeout(() => {
        if (toastElement.parentNode === this.container) {
            this.container.removeChild(toastElement);
        }
        this.toasts = this.toasts.filter(t => t !== toastElement);
    }, 300); // Match animation duration
};

// Convenience methods for different severities
AbpToastService.prototype.success = function(message, title, options) {
    return this.show(message, title, 'success', options);
};

AbpToastService.prototype.error = function(message, title, options) {
    return this.show(message, title, 'error', options);
};

AbpToastService.prototype.info = function(message, title, options) {
    return this.show(message, title, 'info', options);
};

AbpToastService.prototype.warning = function(message, title, options) {
    return this.show(message, title, 'warning', options);
};

// Clear all toasts
AbpToastService.prototype.clear = function(containerKey) {
    if (containerKey) {
        this.toasts = this.toasts.filter(toast => {
            const shouldRemove = toast.dataset.containerKey === containerKey;
            if (shouldRemove) {
                this.remove(toast);
            }
            return !shouldRemove;
        });
    } else {
        this.toasts.forEach(toast => this.remove(toast));
    }
};

// Static method to set default options for all instances
AbpToastService.setDefaultOptions = function(options) {
    AbpToastService.defaultOptions = this.prototype.extend({}, AbpToastService.defaultOptions, options);
};

var abp = abp || {};
(function () {    
    abp.notify.success = function (message, title, options) {
        new AbpToastService().success(message, title, options);
    };

    abp.notify.info = function (message, title, options) {
        new AbpToastService().info(message, title, options);
    };

    abp.notify.warn = function (message, title, options) {
        new AbpToastService().warning(message, title, options);
    };

    abp.notify.error = function (message, title, options) {
        new AbpToastService().error(message, title, options);
    };
})();