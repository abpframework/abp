/*!
* sweetalert2 v11.26.3
* Released under the MIT License.
*/
function _assertClassBrand(e, t, n) {
  if ("function" == typeof e ? e === t : e.has(t)) return arguments.length < 3 ? t : n;
  throw new TypeError("Private element is not present on this object");
}
function _checkPrivateRedeclaration(e, t) {
  if (t.has(e)) throw new TypeError("Cannot initialize the same private elements twice on an object");
}
function _classPrivateFieldGet2(s, a) {
  return s.get(_assertClassBrand(s, a));
}
function _classPrivateFieldInitSpec(e, t, a) {
  _checkPrivateRedeclaration(e, t), t.set(e, a);
}
function _classPrivateFieldSet2(s, a, r) {
  return s.set(_assertClassBrand(s, a), r), r;
}

const RESTORE_FOCUS_TIMEOUT = 100;

/** @type {GlobalState} */
const globalState = {};
const focusPreviousActiveElement = () => {
  if (globalState.previousActiveElement instanceof HTMLElement) {
    globalState.previousActiveElement.focus();
    globalState.previousActiveElement = null;
  } else if (document.body) {
    document.body.focus();
  }
};

/**
 * Restore previous active (focused) element
 *
 * @param {boolean} returnFocus
 * @returns {Promise<void>}
 */
const restoreActiveElement = returnFocus => {
  return new Promise(resolve => {
    if (!returnFocus) {
      return resolve();
    }
    const x = window.scrollX;
    const y = window.scrollY;
    globalState.restoreFocusTimeout = setTimeout(() => {
      focusPreviousActiveElement();
      resolve();
    }, RESTORE_FOCUS_TIMEOUT); // issues/900

    window.scrollTo(x, y);
  });
};

const swalPrefix = 'swal2-';

/**
 * @typedef {Record<SwalClass, string>} SwalClasses
 */

/**
 * @typedef {'success' | 'warning' | 'info' | 'question' | 'error'} SwalIcon
 * @typedef {Record<SwalIcon, string>} SwalIcons
 */

/** @type {SwalClass[]} */
const classNames = ['container', 'shown', 'height-auto', 'iosfix', 'popup', 'modal', 'no-backdrop', 'no-transition', 'toast', 'toast-shown', 'show', 'hide', 'close', 'title', 'html-container', 'actions', 'confirm', 'deny', 'cancel', 'footer', 'icon', 'icon-content', 'image', 'input', 'file', 'range', 'select', 'radio', 'checkbox', 'label', 'textarea', 'inputerror', 'input-label', 'validation-message', 'progress-steps', 'active-progress-step', 'progress-step', 'progress-step-line', 'loader', 'loading', 'styled', 'top', 'top-start', 'top-end', 'top-left', 'top-right', 'center', 'center-start', 'center-end', 'center-left', 'center-right', 'bottom', 'bottom-start', 'bottom-end', 'bottom-left', 'bottom-right', 'grow-row', 'grow-column', 'grow-fullscreen', 'rtl', 'timer-progress-bar', 'timer-progress-bar-container', 'scrollbar-measure', 'icon-success', 'icon-warning', 'icon-info', 'icon-question', 'icon-error', 'draggable', 'dragging'];
const swalClasses = classNames.reduce((acc, className) => {
  acc[className] = swalPrefix + className;
  return acc;
}, /** @type {SwalClasses} */{});

/** @type {SwalIcon[]} */
const icons = ['success', 'warning', 'info', 'question', 'error'];
const iconTypes = icons.reduce((acc, icon) => {
  acc[icon] = swalPrefix + icon;
  return acc;
}, /** @type {SwalIcons} */{});

const consolePrefix = 'SweetAlert2:';

/**
 * Capitalize the first letter of a string
 *
 * @param {string} str
 * @returns {string}
 */
const capitalizeFirstLetter = str => str.charAt(0).toUpperCase() + str.slice(1);

/**
 * Standardize console warnings
 *
 * @param {string | string[]} message
 */
const warn = message => {
  console.warn(`${consolePrefix} ${typeof message === 'object' ? message.join(' ') : message}`);
};

/**
 * Standardize console errors
 *
 * @param {string} message
 */
const error = message => {
  console.error(`${consolePrefix} ${message}`);
};

/**
 * Private global state for `warnOnce`
 *
 * @type {string[]}
 * @private
 */
const previousWarnOnceMessages = [];

/**
 * Show a console warning, but only if it hasn't already been shown
 *
 * @param {string} message
 */
const warnOnce = message => {
  if (!previousWarnOnceMessages.includes(message)) {
    previousWarnOnceMessages.push(message);
    warn(message);
  }
};

/**
 * Show a one-time console warning about deprecated params/methods
 *
 * @param {string} deprecatedParam
 * @param {string?} useInstead
 */
const warnAboutDeprecation = (deprecatedParam, useInstead = null) => {
  warnOnce(`"${deprecatedParam}" is deprecated and will be removed in the next major release.${useInstead ? ` Use "${useInstead}" instead.` : ''}`);
};

/**
 * If `arg` is a function, call it (with no arguments or context) and return the result.
 * Otherwise, just pass the value through
 *
 * @param {(() => *) | *} arg
 * @returns {*}
 */
const callIfFunction = arg => typeof arg === 'function' ? arg() : arg;

/**
 * @param {*} arg
 * @returns {boolean}
 */
const hasToPromiseFn = arg => arg && typeof arg.toPromise === 'function';

/**
 * @param {*} arg
 * @returns {Promise<*>}
 */
const asPromise = arg => hasToPromiseFn(arg) ? arg.toPromise() : Promise.resolve(arg);

/**
 * @param {*} arg
 * @returns {boolean}
 */
const isPromise = arg => arg && Promise.resolve(arg) === arg;

/**
 * Gets the popup container which contains the backdrop and the popup itself.
 *
 * @returns {HTMLElement | null}
 */
const getContainer = () => document.body.querySelector(`.${swalClasses.container}`);

/**
 * @param {string} selectorString
 * @returns {HTMLElement | null}
 */
const elementBySelector = selectorString => {
  const container = getContainer();
  return container ? container.querySelector(selectorString) : null;
};

/**
 * @param {string} className
 * @returns {HTMLElement | null}
 */
const elementByClass = className => {
  return elementBySelector(`.${className}`);
};

/**
 * @returns {HTMLElement | null}
 */
const getPopup = () => elementByClass(swalClasses.popup);

/**
 * @returns {HTMLElement | null}
 */
const getIcon = () => elementByClass(swalClasses.icon);

/**
 * @returns {HTMLElement | null}
 */
const getIconContent = () => elementByClass(swalClasses['icon-content']);

/**
 * @returns {HTMLElement | null}
 */
const getTitle = () => elementByClass(swalClasses.title);

/**
 * @returns {HTMLElement | null}
 */
const getHtmlContainer = () => elementByClass(swalClasses['html-container']);

/**
 * @returns {HTMLElement | null}
 */
const getImage = () => elementByClass(swalClasses.image);

/**
 * @returns {HTMLElement | null}
 */
const getProgressSteps = () => elementByClass(swalClasses['progress-steps']);

/**
 * @returns {HTMLElement | null}
 */
const getValidationMessage = () => elementByClass(swalClasses['validation-message']);

/**
 * @returns {HTMLButtonElement | null}
 */
const getConfirmButton = () => (/** @type {HTMLButtonElement} */elementBySelector(`.${swalClasses.actions} .${swalClasses.confirm}`));

/**
 * @returns {HTMLButtonElement | null}
 */
const getCancelButton = () => (/** @type {HTMLButtonElement} */elementBySelector(`.${swalClasses.actions} .${swalClasses.cancel}`));

/**
 * @returns {HTMLButtonElement | null}
 */
const getDenyButton = () => (/** @type {HTMLButtonElement} */elementBySelector(`.${swalClasses.actions} .${swalClasses.deny}`));

/**
 * @returns {HTMLElement | null}
 */
const getInputLabel = () => elementByClass(swalClasses['input-label']);

/**
 * @returns {HTMLElement | null}
 */
const getLoader = () => elementBySelector(`.${swalClasses.loader}`);

/**
 * @returns {HTMLElement | null}
 */
const getActions = () => elementByClass(swalClasses.actions);

/**
 * @returns {HTMLElement | null}
 */
const getFooter = () => elementByClass(swalClasses.footer);

/**
 * @returns {HTMLElement | null}
 */
const getTimerProgressBar = () => elementByClass(swalClasses['timer-progress-bar']);

/**
 * @returns {HTMLElement | null}
 */
const getCloseButton = () => elementByClass(swalClasses.close);

// https://github.com/jkup/focusable/blob/master/index.js
const focusable = `
  a[href],
  area[href],
  input:not([disabled]),
  select:not([disabled]),
  textarea:not([disabled]),
  button:not([disabled]),
  iframe,
  object,
  embed,
  [tabindex="0"],
  [contenteditable],
  audio[controls],
  video[controls],
  summary
`;
/**
 * @returns {HTMLElement[]}
 */
const getFocusableElements = () => {
  const popup = getPopup();
  if (!popup) {
    return [];
  }
  /** @type {NodeListOf<HTMLElement>} */
  const focusableElementsWithTabindex = popup.querySelectorAll('[tabindex]:not([tabindex="-1"]):not([tabindex="0"])');
  const focusableElementsWithTabindexSorted = Array.from(focusableElementsWithTabindex)
  // sort according to tabindex
  .sort((a, b) => {
    const tabindexA = parseInt(a.getAttribute('tabindex') || '0');
    const tabindexB = parseInt(b.getAttribute('tabindex') || '0');
    if (tabindexA > tabindexB) {
      return 1;
    } else if (tabindexA < tabindexB) {
      return -1;
    }
    return 0;
  });

  /** @type {NodeListOf<HTMLElement>} */
  const otherFocusableElements = popup.querySelectorAll(focusable);
  const otherFocusableElementsFiltered = Array.from(otherFocusableElements).filter(el => el.getAttribute('tabindex') !== '-1');
  return [...new Set(focusableElementsWithTabindexSorted.concat(otherFocusableElementsFiltered))].filter(el => isVisible$1(el));
};

/**
 * @returns {boolean}
 */
const isModal = () => {
  return hasClass(document.body, swalClasses.shown) && !hasClass(document.body, swalClasses['toast-shown']) && !hasClass(document.body, swalClasses['no-backdrop']);
};

/**
 * @returns {boolean}
 */
const isToast = () => {
  const popup = getPopup();
  if (!popup) {
    return false;
  }
  return hasClass(popup, swalClasses.toast);
};

/**
 * @returns {boolean}
 */
const isLoading = () => {
  const popup = getPopup();
  if (!popup) {
    return false;
  }
  return popup.hasAttribute('data-loading');
};

/**
 * Securely set innerHTML of an element
 * https://github.com/sweetalert2/sweetalert2/issues/1926
 *
 * @param {HTMLElement} elem
 * @param {string} html
 */
const setInnerHtml = (elem, html) => {
  elem.textContent = '';
  if (html) {
    const parser = new DOMParser();
    const parsed = parser.parseFromString(html, `text/html`);
    const head = parsed.querySelector('head');
    if (head) {
      Array.from(head.childNodes).forEach(child => {
        elem.appendChild(child);
      });
    }
    const body = parsed.querySelector('body');
    if (body) {
      Array.from(body.childNodes).forEach(child => {
        if (child instanceof HTMLVideoElement || child instanceof HTMLAudioElement) {
          elem.appendChild(child.cloneNode(true)); // https://github.com/sweetalert2/sweetalert2/issues/2507
        } else {
          elem.appendChild(child);
        }
      });
    }
  }
};

/**
 * @param {HTMLElement} elem
 * @param {string} className
 * @returns {boolean}
 */
const hasClass = (elem, className) => {
  if (!className) {
    return false;
  }
  const classList = className.split(/\s+/);
  for (let i = 0; i < classList.length; i++) {
    if (!elem.classList.contains(classList[i])) {
      return false;
    }
  }
  return true;
};

/**
 * @param {HTMLElement} elem
 * @param {SweetAlertOptions} params
 */
const removeCustomClasses = (elem, params) => {
  Array.from(elem.classList).forEach(className => {
    if (!Object.values(swalClasses).includes(className) && !Object.values(iconTypes).includes(className) && !Object.values(params.showClass || {}).includes(className)) {
      elem.classList.remove(className);
    }
  });
};

/**
 * @param {HTMLElement} elem
 * @param {SweetAlertOptions} params
 * @param {string} className
 */
const applyCustomClass = (elem, params, className) => {
  removeCustomClasses(elem, params);
  if (!params.customClass) {
    return;
  }
  const customClass = params.customClass[(/** @type {keyof SweetAlertCustomClass} */className)];
  if (!customClass) {
    return;
  }
  if (typeof customClass !== 'string' && !customClass.forEach) {
    warn(`Invalid type of customClass.${className}! Expected string or iterable object, got "${typeof customClass}"`);
    return;
  }
  addClass(elem, customClass);
};

/**
 * @param {HTMLElement} popup
 * @param {import('./renderers/renderInput').InputClass | SweetAlertInput} inputClass
 * @returns {HTMLInputElement | null}
 */
const getInput$1 = (popup, inputClass) => {
  if (!inputClass) {
    return null;
  }
  switch (inputClass) {
    case 'select':
    case 'textarea':
    case 'file':
      return popup.querySelector(`.${swalClasses.popup} > .${swalClasses[inputClass]}`);
    case 'checkbox':
      return popup.querySelector(`.${swalClasses.popup} > .${swalClasses.checkbox} input`);
    case 'radio':
      return popup.querySelector(`.${swalClasses.popup} > .${swalClasses.radio} input:checked`) || popup.querySelector(`.${swalClasses.popup} > .${swalClasses.radio} input:first-child`);
    case 'range':
      return popup.querySelector(`.${swalClasses.popup} > .${swalClasses.range} input`);
    default:
      return popup.querySelector(`.${swalClasses.popup} > .${swalClasses.input}`);
  }
};

/**
 * @param {HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement} input
 */
const focusInput = input => {
  input.focus();

  // place cursor at end of text in text input
  if (input.type !== 'file') {
    // http://stackoverflow.com/a/2345915
    const val = input.value;
    input.value = '';
    input.value = val;
  }
};

/**
 * @param {HTMLElement | HTMLElement[] | null} target
 * @param {string | string[] | readonly string[] | undefined} classList
 * @param {boolean} condition
 */
const toggleClass = (target, classList, condition) => {
  if (!target || !classList) {
    return;
  }
  if (typeof classList === 'string') {
    classList = classList.split(/\s+/).filter(Boolean);
  }
  classList.forEach(className => {
    if (Array.isArray(target)) {
      target.forEach(elem => {
        if (condition) {
          elem.classList.add(className);
        } else {
          elem.classList.remove(className);
        }
      });
    } else {
      if (condition) {
        target.classList.add(className);
      } else {
        target.classList.remove(className);
      }
    }
  });
};

/**
 * @param {HTMLElement | HTMLElement[] | null} target
 * @param {string | string[] | readonly string[] | undefined} classList
 */
const addClass = (target, classList) => {
  toggleClass(target, classList, true);
};

/**
 * @param {HTMLElement | HTMLElement[] | null} target
 * @param {string | string[] | readonly string[] | undefined} classList
 */
const removeClass = (target, classList) => {
  toggleClass(target, classList, false);
};

/**
 * Get direct child of an element by class name
 *
 * @param {HTMLElement} elem
 * @param {string} className
 * @returns {HTMLElement | undefined}
 */
const getDirectChildByClass = (elem, className) => {
  const children = Array.from(elem.children);
  for (let i = 0; i < children.length; i++) {
    const child = children[i];
    if (child instanceof HTMLElement && hasClass(child, className)) {
      return child;
    }
  }
};

/**
 * @param {HTMLElement} elem
 * @param {string} property
 * @param {string | number | null | undefined} value
 */
const applyNumericalStyle = (elem, property, value) => {
  if (value === `${parseInt(`${value}`)}`) {
    value = parseInt(value);
  }
  if (value || parseInt(`${value}`) === 0) {
    elem.style.setProperty(property, typeof value === 'number' ? `${value}px` : value);
  } else {
    elem.style.removeProperty(property);
  }
};

/**
 * @param {HTMLElement | null} elem
 * @param {string} display
 */
const show = (elem, display = 'flex') => {
  if (!elem) {
    return;
  }
  elem.style.display = display;
};

/**
 * @param {HTMLElement | null} elem
 */
const hide = elem => {
  if (!elem) {
    return;
  }
  elem.style.display = 'none';
};

/**
 * @param {HTMLElement | null} elem
 * @param {string} display
 */
const showWhenInnerHtmlPresent = (elem, display = 'block') => {
  if (!elem) {
    return;
  }
  new MutationObserver(() => {
    toggle(elem, elem.innerHTML, display);
  }).observe(elem, {
    childList: true,
    subtree: true
  });
};

/**
 * @param {HTMLElement} parent
 * @param {string} selector
 * @param {string} property
 * @param {string} value
 */
const setStyle = (parent, selector, property, value) => {
  /** @type {HTMLElement | null} */
  const el = parent.querySelector(selector);
  if (el) {
    el.style.setProperty(property, value);
  }
};

/**
 * @param {HTMLElement} elem
 * @param {boolean | string | null | undefined} condition
 * @param {string} display
 */
const toggle = (elem, condition, display = 'flex') => {
  if (condition) {
    show(elem, display);
  } else {
    hide(elem);
  }
};

/**
 * borrowed from jquery $(elem).is(':visible') implementation
 *
 * @param {HTMLElement | null} elem
 * @returns {boolean}
 */
const isVisible$1 = elem => !!(elem && (elem.offsetWidth || elem.offsetHeight || elem.getClientRects().length));

/**
 * @returns {boolean}
 */
const allButtonsAreHidden = () => !isVisible$1(getConfirmButton()) && !isVisible$1(getDenyButton()) && !isVisible$1(getCancelButton());

/**
 * @param {HTMLElement} elem
 * @returns {boolean}
 */
const isScrollable = elem => !!(elem.scrollHeight > elem.clientHeight);

/**
 * @param {HTMLElement} element
 * @param {HTMLElement} stopElement
 * @returns {boolean}
 */
const selfOrParentIsScrollable = (element, stopElement) => {
  let parent = element;
  while (parent && parent !== stopElement) {
    if (isScrollable(parent)) {
      return true;
    }
    parent = parent.parentElement;
  }
  return false;
};

/**
 * borrowed from https://stackoverflow.com/a/46352119
 *
 * @param {HTMLElement} elem
 * @returns {boolean}
 */
const hasCssAnimation = elem => {
  const style = window.getComputedStyle(elem);
  const animDuration = parseFloat(style.getPropertyValue('animation-duration') || '0');
  const transDuration = parseFloat(style.getPropertyValue('transition-duration') || '0');
  return animDuration > 0 || transDuration > 0;
};

/**
 * @param {number} timer
 * @param {boolean} reset
 */
const animateTimerProgressBar = (timer, reset = false) => {
  const timerProgressBar = getTimerProgressBar();
  if (!timerProgressBar) {
    return;
  }
  if (isVisible$1(timerProgressBar)) {
    if (reset) {
      timerProgressBar.style.transition = 'none';
      timerProgressBar.style.width = '100%';
    }
    setTimeout(() => {
      timerProgressBar.style.transition = `width ${timer / 1000}s linear`;
      timerProgressBar.style.width = '0%';
    }, 10);
  }
};
const stopTimerProgressBar = () => {
  const timerProgressBar = getTimerProgressBar();
  if (!timerProgressBar) {
    return;
  }
  const timerProgressBarWidth = parseInt(window.getComputedStyle(timerProgressBar).width);
  timerProgressBar.style.removeProperty('transition');
  timerProgressBar.style.width = '100%';
  const timerProgressBarFullWidth = parseInt(window.getComputedStyle(timerProgressBar).width);
  const timerProgressBarPercent = timerProgressBarWidth / timerProgressBarFullWidth * 100;
  timerProgressBar.style.width = `${timerProgressBarPercent}%`;
};

/**
 * Detect Node env
 *
 * @returns {boolean}
 */
const isNodeEnv = () => typeof window === 'undefined' || typeof document === 'undefined';

const sweetHTML = `
 <div aria-labelledby="${swalClasses.title}" aria-describedby="${swalClasses['html-container']}" class="${swalClasses.popup}" tabindex="-1">
   <button type="button" class="${swalClasses.close}"></button>
   <ul class="${swalClasses['progress-steps']}"></ul>
   <div class="${swalClasses.icon}"></div>
   <img class="${swalClasses.image}" />
   <h2 class="${swalClasses.title}" id="${swalClasses.title}"></h2>
   <div class="${swalClasses['html-container']}" id="${swalClasses['html-container']}"></div>
   <input class="${swalClasses.input}" id="${swalClasses.input}" />
   <input type="file" class="${swalClasses.file}" />
   <div class="${swalClasses.range}">
     <input type="range" />
     <output></output>
   </div>
   <select class="${swalClasses.select}" id="${swalClasses.select}"></select>
   <div class="${swalClasses.radio}"></div>
   <label class="${swalClasses.checkbox}">
     <input type="checkbox" id="${swalClasses.checkbox}" />
     <span class="${swalClasses.label}"></span>
   </label>
   <textarea class="${swalClasses.textarea}" id="${swalClasses.textarea}"></textarea>
   <div class="${swalClasses['validation-message']}" id="${swalClasses['validation-message']}"></div>
   <div class="${swalClasses.actions}">
     <div class="${swalClasses.loader}"></div>
     <button type="button" class="${swalClasses.confirm}"></button>
     <button type="button" class="${swalClasses.deny}"></button>
     <button type="button" class="${swalClasses.cancel}"></button>
   </div>
   <div class="${swalClasses.footer}"></div>
   <div class="${swalClasses['timer-progress-bar-container']}">
     <div class="${swalClasses['timer-progress-bar']}"></div>
   </div>
 </div>
`.replace(/(^|\n)\s*/g, '');

/**
 * @returns {boolean}
 */
const resetOldContainer = () => {
  const oldContainer = getContainer();
  if (!oldContainer) {
    return false;
  }
  oldContainer.remove();
  removeClass([document.documentElement, document.body], [swalClasses['no-backdrop'], swalClasses['toast-shown'], swalClasses['has-column']]);
  return true;
};
const resetValidationMessage$1 = () => {
  globalState.currentInstance.resetValidationMessage();
};
const addInputChangeListeners = () => {
  const popup = getPopup();
  const input = getDirectChildByClass(popup, swalClasses.input);
  const file = getDirectChildByClass(popup, swalClasses.file);
  /** @type {HTMLInputElement} */
  const range = popup.querySelector(`.${swalClasses.range} input`);
  /** @type {HTMLOutputElement} */
  const rangeOutput = popup.querySelector(`.${swalClasses.range} output`);
  const select = getDirectChildByClass(popup, swalClasses.select);
  /** @type {HTMLInputElement} */
  const checkbox = popup.querySelector(`.${swalClasses.checkbox} input`);
  const textarea = getDirectChildByClass(popup, swalClasses.textarea);
  input.oninput = resetValidationMessage$1;
  file.onchange = resetValidationMessage$1;
  select.onchange = resetValidationMessage$1;
  checkbox.onchange = resetValidationMessage$1;
  textarea.oninput = resetValidationMessage$1;
  range.oninput = () => {
    resetValidationMessage$1();
    rangeOutput.value = range.value;
  };
  range.onchange = () => {
    resetValidationMessage$1();
    rangeOutput.value = range.value;
  };
};

/**
 * @param {string | HTMLElement} target
 * @returns {HTMLElement}
 */
const getTarget = target => typeof target === 'string' ? document.querySelector(target) : target;

/**
 * @param {SweetAlertOptions} params
 */
const setupAccessibility = params => {
  const popup = getPopup();
  popup.setAttribute('role', params.toast ? 'alert' : 'dialog');
  popup.setAttribute('aria-live', params.toast ? 'polite' : 'assertive');
  if (!params.toast) {
    popup.setAttribute('aria-modal', 'true');
  }
};

/**
 * @param {HTMLElement} targetElement
 */
const setupRTL = targetElement => {
  if (window.getComputedStyle(targetElement).direction === 'rtl') {
    addClass(getContainer(), swalClasses.rtl);
  }
};

/**
 * Add modal + backdrop to DOM
 *
 * @param {SweetAlertOptions} params
 */
const init = params => {
  // Clean up the old popup container if it exists
  const oldContainerExisted = resetOldContainer();
  if (isNodeEnv()) {
    error('SweetAlert2 requires document to initialize');
    return;
  }
  const container = document.createElement('div');
  container.className = swalClasses.container;
  if (oldContainerExisted) {
    addClass(container, swalClasses['no-transition']);
  }
  setInnerHtml(container, sweetHTML);
  container.dataset['swal2Theme'] = params.theme;
  const targetElement = getTarget(params.target);
  targetElement.appendChild(container);
  if (params.topLayer) {
    container.setAttribute('popover', '');
    container.showPopover();
  }
  setupAccessibility(params);
  setupRTL(targetElement);
  addInputChangeListeners();
};

/**
 * @param {HTMLElement | object | string} param
 * @param {HTMLElement} target
 */
const parseHtmlToContainer = (param, target) => {
  // DOM element
  if (param instanceof HTMLElement) {
    target.appendChild(param);
  }

  // Object
  else if (typeof param === 'object') {
    handleObject(param, target);
  }

  // Plain string
  else if (param) {
    setInnerHtml(target, param);
  }
};

/**
 * @param {object} param
 * @param {HTMLElement} target
 */
const handleObject = (param, target) => {
  // JQuery element(s)
  if (param.jquery) {
    handleJqueryElem(target, param);
  }

  // For other objects use their string representation
  else {
    setInnerHtml(target, param.toString());
  }
};

/**
 * @param {HTMLElement} target
 * @param {object} elem
 */
const handleJqueryElem = (target, elem) => {
  target.textContent = '';
  if (0 in elem) {
    for (let i = 0; i in elem; i++) {
      target.appendChild(elem[i].cloneNode(true));
    }
  } else {
    target.appendChild(elem.cloneNode(true));
  }
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderActions = (instance, params) => {
  const actions = getActions();
  const loader = getLoader();
  if (!actions || !loader) {
    return;
  }

  // Actions (buttons) wrapper
  if (!params.showConfirmButton && !params.showDenyButton && !params.showCancelButton) {
    hide(actions);
  } else {
    show(actions);
  }

  // Custom class
  applyCustomClass(actions, params, 'actions');

  // Render all the buttons
  renderButtons(actions, loader, params);

  // Loader
  setInnerHtml(loader, params.loaderHtml || '');
  applyCustomClass(loader, params, 'loader');
};

/**
 * @param {HTMLElement} actions
 * @param {HTMLElement} loader
 * @param {SweetAlertOptions} params
 */
function renderButtons(actions, loader, params) {
  const confirmButton = getConfirmButton();
  const denyButton = getDenyButton();
  const cancelButton = getCancelButton();
  if (!confirmButton || !denyButton || !cancelButton) {
    return;
  }

  // Render buttons
  renderButton(confirmButton, 'confirm', params);
  renderButton(denyButton, 'deny', params);
  renderButton(cancelButton, 'cancel', params);
  handleButtonsStyling(confirmButton, denyButton, cancelButton, params);
  if (params.reverseButtons) {
    if (params.toast) {
      actions.insertBefore(cancelButton, confirmButton);
      actions.insertBefore(denyButton, confirmButton);
    } else {
      actions.insertBefore(cancelButton, loader);
      actions.insertBefore(denyButton, loader);
      actions.insertBefore(confirmButton, loader);
    }
  }
}

/**
 * @param {HTMLElement} confirmButton
 * @param {HTMLElement} denyButton
 * @param {HTMLElement} cancelButton
 * @param {SweetAlertOptions} params
 */
function handleButtonsStyling(confirmButton, denyButton, cancelButton, params) {
  if (!params.buttonsStyling) {
    removeClass([confirmButton, denyButton, cancelButton], swalClasses.styled);
    return;
  }
  addClass([confirmButton, denyButton, cancelButton], swalClasses.styled);

  // Apply custom background colors to action buttons
  if (params.confirmButtonColor) {
    confirmButton.style.setProperty('--swal2-confirm-button-background-color', params.confirmButtonColor);
  }
  if (params.denyButtonColor) {
    denyButton.style.setProperty('--swal2-deny-button-background-color', params.denyButtonColor);
  }
  if (params.cancelButtonColor) {
    cancelButton.style.setProperty('--swal2-cancel-button-background-color', params.cancelButtonColor);
  }

  // Apply the outline color to action buttons
  applyOutlineColor(confirmButton);
  applyOutlineColor(denyButton);
  applyOutlineColor(cancelButton);
}

/**
 * @param {HTMLElement} button
 */
function applyOutlineColor(button) {
  const buttonStyle = window.getComputedStyle(button);
  if (buttonStyle.getPropertyValue('--swal2-action-button-focus-box-shadow')) {
    // If the button already has a custom outline color, no need to change it
    return;
  }
  const outlineColor = buttonStyle.backgroundColor.replace(/rgba?\((\d+), (\d+), (\d+).*/, 'rgba($1, $2, $3, 0.5)');
  button.style.setProperty('--swal2-action-button-focus-box-shadow', buttonStyle.getPropertyValue('--swal2-outline').replace(/ rgba\(.*/, ` ${outlineColor}`));
}

/**
 * @param {HTMLElement} button
 * @param {'confirm' | 'deny' | 'cancel'} buttonType
 * @param {SweetAlertOptions} params
 */
function renderButton(button, buttonType, params) {
  const buttonName = /** @type {'Confirm' | 'Deny' | 'Cancel'} */capitalizeFirstLetter(buttonType);
  toggle(button, params[`show${buttonName}Button`], 'inline-block');
  setInnerHtml(button, params[`${buttonType}ButtonText`] || ''); // Set caption text
  button.setAttribute('aria-label', params[`${buttonType}ButtonAriaLabel`] || ''); // ARIA label

  // Add buttons custom classes
  button.className = swalClasses[buttonType];
  applyCustomClass(button, params, `${buttonType}Button`);
}

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderCloseButton = (instance, params) => {
  const closeButton = getCloseButton();
  if (!closeButton) {
    return;
  }
  setInnerHtml(closeButton, params.closeButtonHtml || '');

  // Custom class
  applyCustomClass(closeButton, params, 'closeButton');
  toggle(closeButton, params.showCloseButton);
  closeButton.setAttribute('aria-label', params.closeButtonAriaLabel || '');
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderContainer = (instance, params) => {
  const container = getContainer();
  if (!container) {
    return;
  }
  handleBackdropParam(container, params.backdrop);
  handlePositionParam(container, params.position);
  handleGrowParam(container, params.grow);

  // Custom class
  applyCustomClass(container, params, 'container');
};

/**
 * @param {HTMLElement} container
 * @param {SweetAlertOptions['backdrop']} backdrop
 */
function handleBackdropParam(container, backdrop) {
  if (typeof backdrop === 'string') {
    container.style.background = backdrop;
  } else if (!backdrop) {
    addClass([document.documentElement, document.body], swalClasses['no-backdrop']);
  }
}

/**
 * @param {HTMLElement} container
 * @param {SweetAlertOptions['position']} position
 */
function handlePositionParam(container, position) {
  if (!position) {
    return;
  }
  if (position in swalClasses) {
    addClass(container, swalClasses[position]);
  } else {
    warn('The "position" parameter is not valid, defaulting to "center"');
    addClass(container, swalClasses.center);
  }
}

/**
 * @param {HTMLElement} container
 * @param {SweetAlertOptions['grow']} grow
 */
function handleGrowParam(container, grow) {
  if (!grow) {
    return;
  }
  addClass(container, swalClasses[`grow-${grow}`]);
}

/**
 * This module contains `WeakMap`s for each effectively-"private  property" that a `Swal` has.
 * For example, to set the private property "foo" of `this` to "bar", you can `privateProps.foo.set(this, 'bar')`
 * This is the approach that Babel will probably take to implement private methods/fields
 *   https://github.com/tc39/proposal-private-methods
 *   https://github.com/babel/babel/pull/7555
 * Once we have the changes from that PR in Babel, and our core class fits reasonable in *one module*
 *   then we can use that language feature.
 */

var privateProps = {
  innerParams: new WeakMap(),
  domCache: new WeakMap()
};

/// <reference path="../../../../sweetalert2.d.ts"/>


/** @type {InputClass[]} */
const inputClasses = ['input', 'file', 'range', 'select', 'radio', 'checkbox', 'textarea'];

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderInput = (instance, params) => {
  const popup = getPopup();
  if (!popup) {
    return;
  }
  const innerParams = privateProps.innerParams.get(instance);
  const rerender = !innerParams || params.input !== innerParams.input;
  inputClasses.forEach(inputClass => {
    const inputContainer = getDirectChildByClass(popup, swalClasses[inputClass]);
    if (!inputContainer) {
      return;
    }

    // set attributes
    setAttributes(inputClass, params.inputAttributes);

    // set class
    inputContainer.className = swalClasses[inputClass];
    if (rerender) {
      hide(inputContainer);
    }
  });
  if (params.input) {
    if (rerender) {
      showInput(params);
    }
    // set custom class
    setCustomClass(params);
  }
};

/**
 * @param {SweetAlertOptions} params
 */
const showInput = params => {
  if (!params.input) {
    return;
  }
  if (!renderInputType[params.input]) {
    error(`Unexpected type of input! Expected ${Object.keys(renderInputType).join(' | ')}, got "${params.input}"`);
    return;
  }
  const inputContainer = getInputContainer(params.input);
  if (!inputContainer) {
    return;
  }
  const input = renderInputType[params.input](inputContainer, params);
  show(inputContainer);

  // input autofocus
  if (params.inputAutoFocus) {
    setTimeout(() => {
      focusInput(input);
    });
  }
};

/**
 * @param {HTMLInputElement} input
 */
const removeAttributes = input => {
  for (let i = 0; i < input.attributes.length; i++) {
    const attrName = input.attributes[i].name;
    if (!['id', 'type', 'value', 'style'].includes(attrName)) {
      input.removeAttribute(attrName);
    }
  }
};

/**
 * @param {InputClass} inputClass
 * @param {SweetAlertOptions['inputAttributes']} inputAttributes
 */
const setAttributes = (inputClass, inputAttributes) => {
  const popup = getPopup();
  if (!popup) {
    return;
  }
  const input = getInput$1(popup, inputClass);
  if (!input) {
    return;
  }
  removeAttributes(input);
  for (const attr in inputAttributes) {
    input.setAttribute(attr, inputAttributes[attr]);
  }
};

/**
 * @param {SweetAlertOptions} params
 */
const setCustomClass = params => {
  if (!params.input) {
    return;
  }
  const inputContainer = getInputContainer(params.input);
  if (inputContainer) {
    applyCustomClass(inputContainer, params, 'input');
  }
};

/**
 * @param {HTMLInputElement | HTMLTextAreaElement} input
 * @param {SweetAlertOptions} params
 */
const setInputPlaceholder = (input, params) => {
  if (!input.placeholder && params.inputPlaceholder) {
    input.placeholder = params.inputPlaceholder;
  }
};

/**
 * @param {Input} input
 * @param {Input} prependTo
 * @param {SweetAlertOptions} params
 */
const setInputLabel = (input, prependTo, params) => {
  if (params.inputLabel) {
    const label = document.createElement('label');
    const labelClass = swalClasses['input-label'];
    label.setAttribute('for', input.id);
    label.className = labelClass;
    if (typeof params.customClass === 'object') {
      addClass(label, params.customClass.inputLabel);
    }
    label.innerText = params.inputLabel;
    prependTo.insertAdjacentElement('beforebegin', label);
  }
};

/**
 * @param {SweetAlertInput} inputType
 * @returns {HTMLElement | undefined}
 */
const getInputContainer = inputType => {
  const popup = getPopup();
  if (!popup) {
    return;
  }
  return getDirectChildByClass(popup, swalClasses[(/** @type {SwalClass} */inputType)] || swalClasses.input);
};

/**
 * @param {HTMLInputElement | HTMLOutputElement | HTMLTextAreaElement} input
 * @param {SweetAlertOptions['inputValue']} inputValue
 */
const checkAndSetInputValue = (input, inputValue) => {
  if (['string', 'number'].includes(typeof inputValue)) {
    input.value = `${inputValue}`;
  } else if (!isPromise(inputValue)) {
    warn(`Unexpected type of inputValue! Expected "string", "number" or "Promise", got "${typeof inputValue}"`);
  }
};

/** @type {Record<SweetAlertInput, (input: Input | HTMLElement, params: SweetAlertOptions) => Input>} */
const renderInputType = {};

/**
 * @param {HTMLInputElement} input
 * @param {SweetAlertOptions} params
 * @returns {HTMLInputElement}
 */
renderInputType.text = renderInputType.email = renderInputType.password = renderInputType.number = renderInputType.tel = renderInputType.url = renderInputType.search = renderInputType.date = renderInputType['datetime-local'] = renderInputType.time = renderInputType.week = renderInputType.month = /** @type {(input: Input | HTMLElement, params: SweetAlertOptions) => Input} */
(input, params) => {
  checkAndSetInputValue(input, params.inputValue);
  setInputLabel(input, input, params);
  setInputPlaceholder(input, params);
  input.type = params.input;
  return input;
};

/**
 * @param {HTMLInputElement} input
 * @param {SweetAlertOptions} params
 * @returns {HTMLInputElement}
 */
renderInputType.file = (input, params) => {
  setInputLabel(input, input, params);
  setInputPlaceholder(input, params);
  return input;
};

/**
 * @param {HTMLInputElement} range
 * @param {SweetAlertOptions} params
 * @returns {HTMLInputElement}
 */
renderInputType.range = (range, params) => {
  const rangeInput = range.querySelector('input');
  const rangeOutput = range.querySelector('output');
  checkAndSetInputValue(rangeInput, params.inputValue);
  rangeInput.type = params.input;
  checkAndSetInputValue(rangeOutput, params.inputValue);
  setInputLabel(rangeInput, range, params);
  return range;
};

/**
 * @param {HTMLSelectElement} select
 * @param {SweetAlertOptions} params
 * @returns {HTMLSelectElement}
 */
renderInputType.select = (select, params) => {
  select.textContent = '';
  if (params.inputPlaceholder) {
    const placeholder = document.createElement('option');
    setInnerHtml(placeholder, params.inputPlaceholder);
    placeholder.value = '';
    placeholder.disabled = true;
    placeholder.selected = true;
    select.appendChild(placeholder);
  }
  setInputLabel(select, select, params);
  return select;
};

/**
 * @param {HTMLInputElement} radio
 * @returns {HTMLInputElement}
 */
renderInputType.radio = radio => {
  radio.textContent = '';
  return radio;
};

/**
 * @param {HTMLLabelElement} checkboxContainer
 * @param {SweetAlertOptions} params
 * @returns {HTMLInputElement}
 */
renderInputType.checkbox = (checkboxContainer, params) => {
  const checkbox = getInput$1(getPopup(), 'checkbox');
  checkbox.value = '1';
  checkbox.checked = Boolean(params.inputValue);
  const label = checkboxContainer.querySelector('span');
  setInnerHtml(label, params.inputPlaceholder || params.inputLabel);
  return checkbox;
};

/**
 * @param {HTMLTextAreaElement} textarea
 * @param {SweetAlertOptions} params
 * @returns {HTMLTextAreaElement}
 */
renderInputType.textarea = (textarea, params) => {
  checkAndSetInputValue(textarea, params.inputValue);
  setInputPlaceholder(textarea, params);
  setInputLabel(textarea, textarea, params);

  /**
   * @param {HTMLElement} el
   * @returns {number}
   */
  const getMargin = el => parseInt(window.getComputedStyle(el).marginLeft) + parseInt(window.getComputedStyle(el).marginRight);

  // https://github.com/sweetalert2/sweetalert2/issues/2291
  setTimeout(() => {
    // https://github.com/sweetalert2/sweetalert2/issues/1699
    if ('MutationObserver' in window) {
      const initialPopupWidth = parseInt(window.getComputedStyle(getPopup()).width);
      const textareaResizeHandler = () => {
        // check if texarea is still in document (i.e. popup wasn't closed in the meantime)
        if (!document.body.contains(textarea)) {
          return;
        }
        const textareaWidth = textarea.offsetWidth + getMargin(textarea);
        if (textareaWidth > initialPopupWidth) {
          getPopup().style.width = `${textareaWidth}px`;
        } else {
          applyNumericalStyle(getPopup(), 'width', params.width);
        }
      };
      new MutationObserver(textareaResizeHandler).observe(textarea, {
        attributes: true,
        attributeFilter: ['style']
      });
    }
  });
  return textarea;
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderContent = (instance, params) => {
  const htmlContainer = getHtmlContainer();
  if (!htmlContainer) {
    return;
  }
  showWhenInnerHtmlPresent(htmlContainer);
  applyCustomClass(htmlContainer, params, 'htmlContainer');

  // Content as HTML
  if (params.html) {
    parseHtmlToContainer(params.html, htmlContainer);
    show(htmlContainer, 'block');
  }

  // Content as plain text
  else if (params.text) {
    htmlContainer.textContent = params.text;
    show(htmlContainer, 'block');
  }

  // No content
  else {
    hide(htmlContainer);
  }
  renderInput(instance, params);
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderFooter = (instance, params) => {
  const footer = getFooter();
  if (!footer) {
    return;
  }
  showWhenInnerHtmlPresent(footer);
  toggle(footer, Boolean(params.footer), 'block');
  if (params.footer) {
    parseHtmlToContainer(params.footer, footer);
  }

  // Custom class
  applyCustomClass(footer, params, 'footer');
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderIcon = (instance, params) => {
  const innerParams = privateProps.innerParams.get(instance);
  const icon = getIcon();
  if (!icon) {
    return;
  }

  // if the given icon already rendered, apply the styling without re-rendering the icon
  if (innerParams && params.icon === innerParams.icon) {
    // Custom or default content
    setContent(icon, params);
    applyStyles(icon, params);
    return;
  }
  if (!params.icon && !params.iconHtml) {
    hide(icon);
    return;
  }
  if (params.icon && Object.keys(iconTypes).indexOf(params.icon) === -1) {
    error(`Unknown icon! Expected "success", "error", "warning", "info" or "question", got "${params.icon}"`);
    hide(icon);
    return;
  }
  show(icon);

  // Custom or default content
  setContent(icon, params);
  applyStyles(icon, params);

  // Animate icon
  addClass(icon, params.showClass && params.showClass.icon);

  // Re-adjust the success icon on system theme change
  const colorSchemeQueryList = window.matchMedia('(prefers-color-scheme: dark)');
  colorSchemeQueryList.addEventListener('change', adjustSuccessIconBackgroundColor);
};

/**
 * @param {HTMLElement} icon
 * @param {SweetAlertOptions} params
 */
const applyStyles = (icon, params) => {
  for (const [iconType, iconClassName] of Object.entries(iconTypes)) {
    if (params.icon !== iconType) {
      removeClass(icon, iconClassName);
    }
  }
  addClass(icon, params.icon && iconTypes[params.icon]);

  // Icon color
  setColor(icon, params);

  // Success icon background color
  adjustSuccessIconBackgroundColor();

  // Custom class
  applyCustomClass(icon, params, 'icon');
};

// Adjust success icon background color to match the popup background color
const adjustSuccessIconBackgroundColor = () => {
  const popup = getPopup();
  if (!popup) {
    return;
  }
  const popupBackgroundColor = window.getComputedStyle(popup).getPropertyValue('background-color');
  /** @type {NodeListOf<HTMLElement>} */
  const successIconParts = popup.querySelectorAll('[class^=swal2-success-circular-line], .swal2-success-fix');
  for (let i = 0; i < successIconParts.length; i++) {
    successIconParts[i].style.backgroundColor = popupBackgroundColor;
  }
};

/**
 *
 * @param {SweetAlertOptions} params
 * @returns {string}
 */
const successIconHtml = params => `
  ${params.animation ? '<div class="swal2-success-circular-line-left"></div>' : ''}
  <span class="swal2-success-line-tip"></span> <span class="swal2-success-line-long"></span>
  <div class="swal2-success-ring"></div>
  ${params.animation ? '<div class="swal2-success-fix"></div>' : ''}
  ${params.animation ? '<div class="swal2-success-circular-line-right"></div>' : ''}
`;
const errorIconHtml = `
  <span class="swal2-x-mark">
    <span class="swal2-x-mark-line-left"></span>
    <span class="swal2-x-mark-line-right"></span>
  </span>
`;

/**
 * @param {HTMLElement} icon
 * @param {SweetAlertOptions} params
 */
const setContent = (icon, params) => {
  if (!params.icon && !params.iconHtml) {
    return;
  }
  let oldContent = icon.innerHTML;
  let newContent = '';
  if (params.iconHtml) {
    newContent = iconContent(params.iconHtml);
  } else if (params.icon === 'success') {
    newContent = successIconHtml(params);
    oldContent = oldContent.replace(/ style=".*?"/g, ''); // undo adjustSuccessIconBackgroundColor()
  } else if (params.icon === 'error') {
    newContent = errorIconHtml;
  } else if (params.icon) {
    const defaultIconHtml = {
      question: '?',
      warning: '!',
      info: 'i'
    };
    newContent = iconContent(defaultIconHtml[params.icon]);
  }
  if (oldContent.trim() !== newContent.trim()) {
    setInnerHtml(icon, newContent);
  }
};

/**
 * @param {HTMLElement} icon
 * @param {SweetAlertOptions} params
 */
const setColor = (icon, params) => {
  if (!params.iconColor) {
    return;
  }
  icon.style.color = params.iconColor;
  icon.style.borderColor = params.iconColor;
  for (const sel of ['.swal2-success-line-tip', '.swal2-success-line-long', '.swal2-x-mark-line-left', '.swal2-x-mark-line-right']) {
    setStyle(icon, sel, 'background-color', params.iconColor);
  }
  setStyle(icon, '.swal2-success-ring', 'border-color', params.iconColor);
};

/**
 * @param {string} content
 * @returns {string}
 */
const iconContent = content => `<div class="${swalClasses['icon-content']}">${content}</div>`;

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderImage = (instance, params) => {
  const image = getImage();
  if (!image) {
    return;
  }
  if (!params.imageUrl) {
    hide(image);
    return;
  }
  show(image, '');

  // Src, alt
  image.setAttribute('src', params.imageUrl);
  image.setAttribute('alt', params.imageAlt || '');

  // Width, height
  applyNumericalStyle(image, 'width', params.imageWidth);
  applyNumericalStyle(image, 'height', params.imageHeight);

  // Class
  image.className = swalClasses.image;
  applyCustomClass(image, params, 'image');
};

let dragging = false;
let mousedownX = 0;
let mousedownY = 0;
let initialX = 0;
let initialY = 0;

/**
 * @param {HTMLElement} popup
 */
const addDraggableListeners = popup => {
  popup.addEventListener('mousedown', down);
  document.body.addEventListener('mousemove', move);
  popup.addEventListener('mouseup', up);
  popup.addEventListener('touchstart', down);
  document.body.addEventListener('touchmove', move);
  popup.addEventListener('touchend', up);
};

/**
 * @param {HTMLElement} popup
 */
const removeDraggableListeners = popup => {
  popup.removeEventListener('mousedown', down);
  document.body.removeEventListener('mousemove', move);
  popup.removeEventListener('mouseup', up);
  popup.removeEventListener('touchstart', down);
  document.body.removeEventListener('touchmove', move);
  popup.removeEventListener('touchend', up);
};

/**
 * @param {MouseEvent | TouchEvent} event
 */
const down = event => {
  const popup = getPopup();
  if (event.target === popup || getIcon().contains(/** @type {HTMLElement} */event.target)) {
    dragging = true;
    const clientXY = getClientXY(event);
    mousedownX = clientXY.clientX;
    mousedownY = clientXY.clientY;
    initialX = parseInt(popup.style.insetInlineStart) || 0;
    initialY = parseInt(popup.style.insetBlockStart) || 0;
    addClass(popup, 'swal2-dragging');
  }
};

/**
 * @param {MouseEvent | TouchEvent} event
 */
const move = event => {
  const popup = getPopup();
  if (dragging) {
    let {
      clientX,
      clientY
    } = getClientXY(event);
    popup.style.insetInlineStart = `${initialX + (clientX - mousedownX)}px`;
    popup.style.insetBlockStart = `${initialY + (clientY - mousedownY)}px`;
  }
};
const up = () => {
  const popup = getPopup();
  dragging = false;
  removeClass(popup, 'swal2-dragging');
};

/**
 * @param {MouseEvent | TouchEvent} event
 * @returns {{ clientX: number, clientY: number }}
 */
const getClientXY = event => {
  let clientX = 0,
    clientY = 0;
  if (event.type.startsWith('mouse')) {
    clientX = /** @type {MouseEvent} */event.clientX;
    clientY = /** @type {MouseEvent} */event.clientY;
  } else if (event.type.startsWith('touch')) {
    clientX = /** @type {TouchEvent} */event.touches[0].clientX;
    clientY = /** @type {TouchEvent} */event.touches[0].clientY;
  }
  return {
    clientX,
    clientY
  };
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderPopup = (instance, params) => {
  const container = getContainer();
  const popup = getPopup();
  if (!container || !popup) {
    return;
  }

  // Width
  // https://github.com/sweetalert2/sweetalert2/issues/2170
  if (params.toast) {
    applyNumericalStyle(container, 'width', params.width);
    popup.style.width = '100%';
    const loader = getLoader();
    if (loader) {
      popup.insertBefore(loader, getIcon());
    }
  } else {
    applyNumericalStyle(popup, 'width', params.width);
  }

  // Padding
  applyNumericalStyle(popup, 'padding', params.padding);

  // Color
  if (params.color) {
    popup.style.color = params.color;
  }

  // Background
  if (params.background) {
    popup.style.background = params.background;
  }
  hide(getValidationMessage());

  // Classes
  addClasses$1(popup, params);
  if (params.draggable && !params.toast) {
    addClass(popup, swalClasses.draggable);
    addDraggableListeners(popup);
  } else {
    removeClass(popup, swalClasses.draggable);
    removeDraggableListeners(popup);
  }
};

/**
 * @param {HTMLElement} popup
 * @param {SweetAlertOptions} params
 */
const addClasses$1 = (popup, params) => {
  const showClass = params.showClass || {};
  // Default Class + showClass when updating Swal.update({})
  popup.className = `${swalClasses.popup} ${isVisible$1(popup) ? showClass.popup : ''}`;
  if (params.toast) {
    addClass([document.documentElement, document.body], swalClasses['toast-shown']);
    addClass(popup, swalClasses.toast);
  } else {
    addClass(popup, swalClasses.modal);
  }

  // Custom class
  applyCustomClass(popup, params, 'popup');
  // TODO: remove in the next major
  if (typeof params.customClass === 'string') {
    addClass(popup, params.customClass);
  }

  // Icon class (#1842)
  if (params.icon) {
    addClass(popup, swalClasses[`icon-${params.icon}`]);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderProgressSteps = (instance, params) => {
  const progressStepsContainer = getProgressSteps();
  if (!progressStepsContainer) {
    return;
  }
  const {
    progressSteps,
    currentProgressStep
  } = params;
  if (!progressSteps || progressSteps.length === 0 || currentProgressStep === undefined) {
    hide(progressStepsContainer);
    return;
  }
  show(progressStepsContainer);
  progressStepsContainer.textContent = '';
  if (currentProgressStep >= progressSteps.length) {
    warn('Invalid currentProgressStep parameter, it should be less than progressSteps.length ' + '(currentProgressStep like JS arrays starts from 0)');
  }
  progressSteps.forEach((step, index) => {
    const stepEl = createStepElement(step);
    progressStepsContainer.appendChild(stepEl);
    if (index === currentProgressStep) {
      addClass(stepEl, swalClasses['active-progress-step']);
    }
    if (index !== progressSteps.length - 1) {
      const lineEl = createLineElement(params);
      progressStepsContainer.appendChild(lineEl);
    }
  });
};

/**
 * @param {string} step
 * @returns {HTMLLIElement}
 */
const createStepElement = step => {
  const stepEl = document.createElement('li');
  addClass(stepEl, swalClasses['progress-step']);
  setInnerHtml(stepEl, step);
  return stepEl;
};

/**
 * @param {SweetAlertOptions} params
 * @returns {HTMLLIElement}
 */
const createLineElement = params => {
  const lineEl = document.createElement('li');
  addClass(lineEl, swalClasses['progress-step-line']);
  if (params.progressStepsDistance) {
    applyNumericalStyle(lineEl, 'width', params.progressStepsDistance);
  }
  return lineEl;
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderTitle = (instance, params) => {
  const title = getTitle();
  if (!title) {
    return;
  }
  showWhenInnerHtmlPresent(title);
  toggle(title, Boolean(params.title || params.titleText), 'block');
  if (params.title) {
    parseHtmlToContainer(params.title, title);
  }
  if (params.titleText) {
    title.innerText = params.titleText;
  }

  // Custom class
  applyCustomClass(title, params, 'title');
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const render = (instance, params) => {
  renderPopup(instance, params);
  renderContainer(instance, params);
  renderProgressSteps(instance, params);
  renderIcon(instance, params);
  renderImage(instance, params);
  renderTitle(instance, params);
  renderCloseButton(instance, params);
  renderContent(instance, params);
  renderActions(instance, params);
  renderFooter(instance, params);
  const popup = getPopup();
  if (typeof params.didRender === 'function' && popup) {
    params.didRender(popup);
  }
  globalState.eventEmitter.emit('didRender', popup);
};

/*
 * Global function to determine if SweetAlert2 popup is shown
 */
const isVisible = () => {
  return isVisible$1(getPopup());
};

/*
 * Global function to click 'Confirm' button
 */
const clickConfirm = () => {
  var _dom$getConfirmButton;
  return (_dom$getConfirmButton = getConfirmButton()) === null || _dom$getConfirmButton === void 0 ? void 0 : _dom$getConfirmButton.click();
};

/*
 * Global function to click 'Deny' button
 */
const clickDeny = () => {
  var _dom$getDenyButton;
  return (_dom$getDenyButton = getDenyButton()) === null || _dom$getDenyButton === void 0 ? void 0 : _dom$getDenyButton.click();
};

/*
 * Global function to click 'Cancel' button
 */
const clickCancel = () => {
  var _dom$getCancelButton;
  return (_dom$getCancelButton = getCancelButton()) === null || _dom$getCancelButton === void 0 ? void 0 : _dom$getCancelButton.click();
};

/** @type {Record<DismissReason, DismissReason>} */
const DismissReason = Object.freeze({
  cancel: 'cancel',
  backdrop: 'backdrop',
  close: 'close',
  esc: 'esc',
  timer: 'timer'
});

/**
 * @param {GlobalState} globalState
 */
const removeKeydownHandler = globalState => {
  if (globalState.keydownTarget && globalState.keydownHandlerAdded) {
    globalState.keydownTarget.removeEventListener('keydown', globalState.keydownHandler, {
      capture: globalState.keydownListenerCapture
    });
    globalState.keydownHandlerAdded = false;
  }
};

/**
 * @param {GlobalState} globalState
 * @param {SweetAlertOptions} innerParams
 * @param {(dismiss: DismissReason) => void} dismissWith
 */
const addKeydownHandler = (globalState, innerParams, dismissWith) => {
  removeKeydownHandler(globalState);
  if (!innerParams.toast) {
    globalState.keydownHandler = e => keydownHandler(innerParams, e, dismissWith);
    globalState.keydownTarget = innerParams.keydownListenerCapture ? window : getPopup();
    globalState.keydownListenerCapture = innerParams.keydownListenerCapture;
    globalState.keydownTarget.addEventListener('keydown', globalState.keydownHandler, {
      capture: globalState.keydownListenerCapture
    });
    globalState.keydownHandlerAdded = true;
  }
};

/**
 * @param {number} index
 * @param {number} increment
 */
const setFocus = (index, increment) => {
  var _dom$getPopup;
  const focusableElements = getFocusableElements();
  // search for visible elements and select the next possible match
  if (focusableElements.length) {
    index = index + increment;

    // shift + tab when .swal2-popup is focused
    if (index === -2) {
      index = focusableElements.length - 1;
    }

    // rollover to first item
    if (index === focusableElements.length) {
      index = 0;

      // go to last item
    } else if (index === -1) {
      index = focusableElements.length - 1;
    }
    focusableElements[index].focus();
    return;
  }
  // no visible focusable elements, focus the popup
  (_dom$getPopup = getPopup()) === null || _dom$getPopup === void 0 || _dom$getPopup.focus();
};
const arrowKeysNextButton = ['ArrowRight', 'ArrowDown'];
const arrowKeysPreviousButton = ['ArrowLeft', 'ArrowUp'];

/**
 * @param {SweetAlertOptions} innerParams
 * @param {KeyboardEvent} event
 * @param {(dismiss: DismissReason) => void} dismissWith
 */
const keydownHandler = (innerParams, event, dismissWith) => {
  if (!innerParams) {
    return; // This instance has already been destroyed
  }

  // Ignore keydown during IME composition
  // https://developer.mozilla.org/en-US/docs/Web/API/Document/keydown_event#ignoring_keydown_during_ime_composition
  // https://github.com/sweetalert2/sweetalert2/issues/720
  // https://github.com/sweetalert2/sweetalert2/issues/2406
  if (event.isComposing || event.keyCode === 229) {
    return;
  }
  if (innerParams.stopKeydownPropagation) {
    event.stopPropagation();
  }

  // ENTER
  if (event.key === 'Enter') {
    handleEnter(event, innerParams);
  }

  // TAB
  else if (event.key === 'Tab') {
    handleTab(event);
  }

  // ARROWS - switch focus between buttons
  else if ([...arrowKeysNextButton, ...arrowKeysPreviousButton].includes(event.key)) {
    handleArrows(event.key);
  }

  // ESC
  else if (event.key === 'Escape') {
    handleEsc(event, innerParams, dismissWith);
  }
};

/**
 * @param {KeyboardEvent} event
 * @param {SweetAlertOptions} innerParams
 */
const handleEnter = (event, innerParams) => {
  // https://github.com/sweetalert2/sweetalert2/issues/2386
  if (!callIfFunction(innerParams.allowEnterKey)) {
    return;
  }
  const input = getInput$1(getPopup(), innerParams.input);
  if (event.target && input && event.target instanceof HTMLElement && event.target.outerHTML === input.outerHTML) {
    if (['textarea', 'file'].includes(innerParams.input)) {
      return; // do not submit
    }
    clickConfirm();
    event.preventDefault();
  }
};

/**
 * @param {KeyboardEvent} event
 */
const handleTab = event => {
  const targetElement = event.target;
  const focusableElements = getFocusableElements();
  let btnIndex = -1;
  for (let i = 0; i < focusableElements.length; i++) {
    if (targetElement === focusableElements[i]) {
      btnIndex = i;
      break;
    }
  }

  // Cycle to the next button
  if (!event.shiftKey) {
    setFocus(btnIndex, 1);
  }

  // Cycle to the prev button
  else {
    setFocus(btnIndex, -1);
  }
  event.stopPropagation();
  event.preventDefault();
};

/**
 * @param {string} key
 */
const handleArrows = key => {
  const actions = getActions();
  const confirmButton = getConfirmButton();
  const denyButton = getDenyButton();
  const cancelButton = getCancelButton();
  if (!actions || !confirmButton || !denyButton || !cancelButton) {
    return;
  }
  /** @type HTMLElement[] */
  const buttons = [confirmButton, denyButton, cancelButton];
  if (document.activeElement instanceof HTMLElement && !buttons.includes(document.activeElement)) {
    return;
  }
  const sibling = arrowKeysNextButton.includes(key) ? 'nextElementSibling' : 'previousElementSibling';
  let buttonToFocus = document.activeElement;
  if (!buttonToFocus) {
    return;
  }
  for (let i = 0; i < actions.children.length; i++) {
    buttonToFocus = buttonToFocus[sibling];
    if (!buttonToFocus) {
      return;
    }
    if (buttonToFocus instanceof HTMLButtonElement && isVisible$1(buttonToFocus)) {
      break;
    }
  }
  if (buttonToFocus instanceof HTMLButtonElement) {
    buttonToFocus.focus();
  }
};

/**
 * @param {KeyboardEvent} event
 * @param {SweetAlertOptions} innerParams
 * @param {(dismiss: DismissReason) => void} dismissWith
 */
const handleEsc = (event, innerParams, dismissWith) => {
  event.preventDefault();
  if (callIfFunction(innerParams.allowEscapeKey)) {
    dismissWith(DismissReason.esc);
  }
};

/**
 * This module contains `WeakMap`s for each effectively-"private  property" that a `Swal` has.
 * For example, to set the private property "foo" of `this` to "bar", you can `privateProps.foo.set(this, 'bar')`
 * This is the approach that Babel will probably take to implement private methods/fields
 *   https://github.com/tc39/proposal-private-methods
 *   https://github.com/babel/babel/pull/7555
 * Once we have the changes from that PR in Babel, and our core class fits reasonable in *one module*
 *   then we can use that language feature.
 */

var privateMethods = {
  swalPromiseResolve: new WeakMap(),
  swalPromiseReject: new WeakMap()
};

// From https://developer.paciellogroup.com/blog/2018/06/the-current-state-of-modal-dialog-accessibility/
// Adding aria-hidden="true" to elements outside of the active modal dialog ensures that
// elements not within the active modal dialog will not be surfaced if a user opens a screen
// reader’s list of elements (headings, form controls, landmarks, etc.) in the document.

const setAriaHidden = () => {
  const container = getContainer();
  const bodyChildren = Array.from(document.body.children);
  bodyChildren.forEach(el => {
    if (el.contains(container)) {
      return;
    }
    if (el.hasAttribute('aria-hidden')) {
      el.setAttribute('data-previous-aria-hidden', el.getAttribute('aria-hidden') || '');
    }
    el.setAttribute('aria-hidden', 'true');
  });
};
const unsetAriaHidden = () => {
  const bodyChildren = Array.from(document.body.children);
  bodyChildren.forEach(el => {
    if (el.hasAttribute('data-previous-aria-hidden')) {
      el.setAttribute('aria-hidden', el.getAttribute('data-previous-aria-hidden') || '');
      el.removeAttribute('data-previous-aria-hidden');
    } else {
      el.removeAttribute('aria-hidden');
    }
  });
};

// @ts-ignore
const isSafariOrIOS = typeof window !== 'undefined' && !!window.GestureEvent; // true for Safari desktop + all iOS browsers https://stackoverflow.com/a/70585394

/**
 * Fix iOS scrolling
 * http://stackoverflow.com/q/39626302
 */
const iOSfix = () => {
  if (isSafariOrIOS && !hasClass(document.body, swalClasses.iosfix)) {
    const offset = document.body.scrollTop;
    document.body.style.top = `${offset * -1}px`;
    addClass(document.body, swalClasses.iosfix);
    lockBodyScroll();
  }
};

/**
 * https://github.com/sweetalert2/sweetalert2/issues/1246
 */
const lockBodyScroll = () => {
  const container = getContainer();
  if (!container) {
    return;
  }
  /** @type {boolean} */
  let preventTouchMove;
  /**
   * @param {TouchEvent} event
   */
  container.ontouchstart = event => {
    preventTouchMove = shouldPreventTouchMove(event);
  };
  /**
   * @param {TouchEvent} event
   */
  container.ontouchmove = event => {
    if (preventTouchMove) {
      event.preventDefault();
      event.stopPropagation();
    }
  };
};

/**
 * @param {TouchEvent} event
 * @returns {boolean}
 */
const shouldPreventTouchMove = event => {
  const target = event.target;
  const container = getContainer();
  const htmlContainer = getHtmlContainer();
  if (!container || !htmlContainer) {
    return false;
  }
  if (isStylus(event) || isZoom(event)) {
    return false;
  }
  if (target === container) {
    return true;
  }
  if (!isScrollable(container) && target instanceof HTMLElement && !selfOrParentIsScrollable(target, htmlContainer) &&
  // #2823
  target.tagName !== 'INPUT' &&
  // #1603
  target.tagName !== 'TEXTAREA' &&
  // #2266
  !(isScrollable(htmlContainer) &&
  // #1944
  htmlContainer.contains(target))) {
    return true;
  }
  return false;
};

/**
 * https://github.com/sweetalert2/sweetalert2/issues/1786
 *
 * @param {object} event
 * @returns {boolean}
 */
const isStylus = event => {
  return event.touches && event.touches.length && event.touches[0].touchType === 'stylus';
};

/**
 * https://github.com/sweetalert2/sweetalert2/issues/1891
 *
 * @param {TouchEvent} event
 * @returns {boolean}
 */
const isZoom = event => {
  return event.touches && event.touches.length > 1;
};
const undoIOSfix = () => {
  if (hasClass(document.body, swalClasses.iosfix)) {
    const offset = parseInt(document.body.style.top, 10);
    removeClass(document.body, swalClasses.iosfix);
    document.body.style.top = '';
    document.body.scrollTop = offset * -1;
  }
};

/**
 * Measure scrollbar width for padding body during modal show/hide
 * https://github.com/twbs/bootstrap/blob/master/js/src/modal.js
 *
 * @returns {number}
 */
const measureScrollbar = () => {
  const scrollDiv = document.createElement('div');
  scrollDiv.className = swalClasses['scrollbar-measure'];
  document.body.appendChild(scrollDiv);
  const scrollbarWidth = scrollDiv.getBoundingClientRect().width - scrollDiv.clientWidth;
  document.body.removeChild(scrollDiv);
  return scrollbarWidth;
};

/**
 * Remember state in cases where opening and handling a modal will fiddle with it.
 * @type {number | null}
 */
let previousBodyPadding = null;

/**
 * @param {string} initialBodyOverflow
 */
const replaceScrollbarWithPadding = initialBodyOverflow => {
  // for queues, do not do this more than once
  if (previousBodyPadding !== null) {
    return;
  }
  // if the body has overflow
  if (document.body.scrollHeight > window.innerHeight || initialBodyOverflow === 'scroll' // https://github.com/sweetalert2/sweetalert2/issues/2663
  ) {
    // add padding so the content doesn't shift after removal of scrollbar
    previousBodyPadding = parseInt(window.getComputedStyle(document.body).getPropertyValue('padding-right'));
    document.body.style.paddingRight = `${previousBodyPadding + measureScrollbar()}px`;
  }
};
const undoReplaceScrollbarWithPadding = () => {
  if (previousBodyPadding !== null) {
    document.body.style.paddingRight = `${previousBodyPadding}px`;
    previousBodyPadding = null;
  }
};

/**
 * @param {SweetAlert} instance
 * @param {HTMLElement} container
 * @param {boolean} returnFocus
 * @param {() => void} didClose
 */
function removePopupAndResetState(instance, container, returnFocus, didClose) {
  if (isToast()) {
    triggerDidCloseAndDispose(instance, didClose);
  } else {
    restoreActiveElement(returnFocus).then(() => triggerDidCloseAndDispose(instance, didClose));
    removeKeydownHandler(globalState);
  }

  // workaround for https://github.com/sweetalert2/sweetalert2/issues/2088
  // for some reason removing the container in Safari will scroll the document to bottom
  if (isSafariOrIOS) {
    container.setAttribute('style', 'display:none !important');
    container.removeAttribute('class');
    container.innerHTML = '';
  } else {
    container.remove();
  }
  if (isModal()) {
    undoReplaceScrollbarWithPadding();
    undoIOSfix();
    unsetAriaHidden();
  }
  removeBodyClasses();
}

/**
 * Remove SweetAlert2 classes from body
 */
function removeBodyClasses() {
  removeClass([document.documentElement, document.body], [swalClasses.shown, swalClasses['height-auto'], swalClasses['no-backdrop'], swalClasses['toast-shown']]);
}

/**
 * Instance method to close sweetAlert
 *
 * @param {SweetAlertResult | undefined} resolveValue
 */
function close(resolveValue) {
  resolveValue = prepareResolveValue(resolveValue);
  const swalPromiseResolve = privateMethods.swalPromiseResolve.get(this);
  const didClose = triggerClosePopup(this);
  if (this.isAwaitingPromise) {
    // A swal awaiting for a promise (after a click on Confirm or Deny) cannot be dismissed anymore #2335
    if (!resolveValue.isDismissed) {
      handleAwaitingPromise(this);
      swalPromiseResolve(resolveValue);
    }
  } else if (didClose) {
    // Resolve Swal promise
    swalPromiseResolve(resolveValue);
  }
}
const triggerClosePopup = instance => {
  const popup = getPopup();
  if (!popup) {
    return false;
  }
  const innerParams = privateProps.innerParams.get(instance);
  if (!innerParams || hasClass(popup, innerParams.hideClass.popup)) {
    return false;
  }
  removeClass(popup, innerParams.showClass.popup);
  addClass(popup, innerParams.hideClass.popup);
  const backdrop = getContainer();
  removeClass(backdrop, innerParams.showClass.backdrop);
  addClass(backdrop, innerParams.hideClass.backdrop);
  handlePopupAnimation(instance, popup, innerParams);
  return true;
};

/**
 * @param {Error | string} error
 */
function rejectPromise(error) {
  const rejectPromise = privateMethods.swalPromiseReject.get(this);
  handleAwaitingPromise(this);
  if (rejectPromise) {
    // Reject Swal promise
    rejectPromise(error);
  }
}

/**
 * @param {SweetAlert} instance
 */
const handleAwaitingPromise = instance => {
  if (instance.isAwaitingPromise) {
    delete instance.isAwaitingPromise;
    // The instance might have been previously partly destroyed, we must resume the destroy process in this case #2335
    if (!privateProps.innerParams.get(instance)) {
      instance._destroy();
    }
  }
};

/**
 * @param {SweetAlertResult | undefined} resolveValue
 * @returns {SweetAlertResult}
 */
const prepareResolveValue = resolveValue => {
  // When user calls Swal.close()
  if (typeof resolveValue === 'undefined') {
    return {
      isConfirmed: false,
      isDenied: false,
      isDismissed: true
    };
  }
  return Object.assign({
    isConfirmed: false,
    isDenied: false,
    isDismissed: false
  }, resolveValue);
};

/**
 * @param {SweetAlert} instance
 * @param {HTMLElement} popup
 * @param {SweetAlertOptions} innerParams
 */
const handlePopupAnimation = (instance, popup, innerParams) => {
  var _globalState$eventEmi;
  const container = getContainer();
  // If animation is supported, animate
  const animationIsSupported = hasCssAnimation(popup);
  if (typeof innerParams.willClose === 'function') {
    innerParams.willClose(popup);
  }
  (_globalState$eventEmi = globalState.eventEmitter) === null || _globalState$eventEmi === void 0 || _globalState$eventEmi.emit('willClose', popup);
  if (animationIsSupported) {
    animatePopup(instance, popup, container, innerParams.returnFocus, innerParams.didClose);
  } else {
    // Otherwise, remove immediately
    removePopupAndResetState(instance, container, innerParams.returnFocus, innerParams.didClose);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {HTMLElement} popup
 * @param {HTMLElement} container
 * @param {boolean} returnFocus
 * @param {() => void} didClose
 */
const animatePopup = (instance, popup, container, returnFocus, didClose) => {
  globalState.swalCloseEventFinishedCallback = removePopupAndResetState.bind(null, instance, container, returnFocus, didClose);
  /**
   * @param {AnimationEvent | TransitionEvent} e
   */
  const swalCloseAnimationFinished = function (e) {
    if (e.target === popup) {
      var _globalState$swalClos;
      (_globalState$swalClos = globalState.swalCloseEventFinishedCallback) === null || _globalState$swalClos === void 0 || _globalState$swalClos.call(globalState);
      delete globalState.swalCloseEventFinishedCallback;
      popup.removeEventListener('animationend', swalCloseAnimationFinished);
      popup.removeEventListener('transitionend', swalCloseAnimationFinished);
    }
  };
  popup.addEventListener('animationend', swalCloseAnimationFinished);
  popup.addEventListener('transitionend', swalCloseAnimationFinished);
};

/**
 * @param {SweetAlert} instance
 * @param {() => void} didClose
 */
const triggerDidCloseAndDispose = (instance, didClose) => {
  setTimeout(() => {
    var _globalState$eventEmi2;
    if (typeof didClose === 'function') {
      didClose.bind(instance.params)();
    }
    (_globalState$eventEmi2 = globalState.eventEmitter) === null || _globalState$eventEmi2 === void 0 || _globalState$eventEmi2.emit('didClose');
    // instance might have been destroyed already
    if (instance._destroy) {
      instance._destroy();
    }
  });
};

/**
 * Shows loader (spinner), this is useful with AJAX requests.
 * By default the loader be shown instead of the "Confirm" button.
 *
 * @param {HTMLButtonElement | null} [buttonToReplace]
 */
const showLoading = buttonToReplace => {
  let popup = getPopup();
  if (!popup) {
    new Swal();
  }
  popup = getPopup();
  if (!popup) {
    return;
  }
  const loader = getLoader();
  if (isToast()) {
    hide(getIcon());
  } else {
    replaceButton(popup, buttonToReplace);
  }
  show(loader);
  popup.setAttribute('data-loading', 'true');
  popup.setAttribute('aria-busy', 'true');
  popup.focus();
};

/**
 * @param {HTMLElement} popup
 * @param {HTMLButtonElement | null} [buttonToReplace]
 */
const replaceButton = (popup, buttonToReplace) => {
  const actions = getActions();
  const loader = getLoader();
  if (!actions || !loader) {
    return;
  }
  if (!buttonToReplace && isVisible$1(getConfirmButton())) {
    buttonToReplace = getConfirmButton();
  }
  show(actions);
  if (buttonToReplace) {
    hide(buttonToReplace);
    loader.setAttribute('data-button-to-replace', buttonToReplace.className);
    actions.insertBefore(loader, buttonToReplace);
  }
  addClass([popup, actions], swalClasses.loading);
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const handleInputOptionsAndValue = (instance, params) => {
  if (params.input === 'select' || params.input === 'radio') {
    handleInputOptions(instance, params);
  } else if (['text', 'email', 'number', 'tel', 'textarea'].some(i => i === params.input) && (hasToPromiseFn(params.inputValue) || isPromise(params.inputValue))) {
    showLoading(getConfirmButton());
    handleInputValue(instance, params);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} innerParams
 * @returns {SweetAlertInputValue}
 */
const getInputValue = (instance, innerParams) => {
  const input = instance.getInput();
  if (!input) {
    return null;
  }
  switch (innerParams.input) {
    case 'checkbox':
      return getCheckboxValue(input);
    case 'radio':
      return getRadioValue(input);
    case 'file':
      return getFileValue(input);
    default:
      return innerParams.inputAutoTrim ? input.value.trim() : input.value;
  }
};

/**
 * @param {HTMLInputElement} input
 * @returns {number}
 */
const getCheckboxValue = input => input.checked ? 1 : 0;

/**
 * @param {HTMLInputElement} input
 * @returns {string | null}
 */
const getRadioValue = input => input.checked ? input.value : null;

/**
 * @param {HTMLInputElement} input
 * @returns {FileList | File | null}
 */
const getFileValue = input => input.files && input.files.length ? input.getAttribute('multiple') !== null ? input.files : input.files[0] : null;

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const handleInputOptions = (instance, params) => {
  const popup = getPopup();
  if (!popup) {
    return;
  }
  /**
   * @param {*} inputOptions
   */
  const processInputOptions = inputOptions => {
    if (params.input === 'select') {
      populateSelectOptions(popup, formatInputOptions(inputOptions), params);
    } else if (params.input === 'radio') {
      populateRadioOptions(popup, formatInputOptions(inputOptions), params);
    }
  };
  if (hasToPromiseFn(params.inputOptions) || isPromise(params.inputOptions)) {
    showLoading(getConfirmButton());
    asPromise(params.inputOptions).then(inputOptions => {
      instance.hideLoading();
      processInputOptions(inputOptions);
    });
  } else if (typeof params.inputOptions === 'object') {
    processInputOptions(params.inputOptions);
  } else {
    error(`Unexpected type of inputOptions! Expected object, Map or Promise, got ${typeof params.inputOptions}`);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const handleInputValue = (instance, params) => {
  const input = instance.getInput();
  if (!input) {
    return;
  }
  hide(input);
  asPromise(params.inputValue).then(inputValue => {
    input.value = params.input === 'number' ? `${parseFloat(inputValue) || 0}` : `${inputValue}`;
    show(input);
    input.focus();
    instance.hideLoading();
  }).catch(err => {
    error(`Error in inputValue promise: ${err}`);
    input.value = '';
    show(input);
    input.focus();
    instance.hideLoading();
  });
};

/**
 * @param {HTMLElement} popup
 * @param {InputOptionFlattened[]} inputOptions
 * @param {SweetAlertOptions} params
 */
function populateSelectOptions(popup, inputOptions, params) {
  const select = getDirectChildByClass(popup, swalClasses.select);
  if (!select) {
    return;
  }
  /**
   * @param {HTMLElement} parent
   * @param {string} optionLabel
   * @param {string} optionValue
   */
  const renderOption = (parent, optionLabel, optionValue) => {
    const option = document.createElement('option');
    option.value = optionValue;
    setInnerHtml(option, optionLabel);
    option.selected = isSelected(optionValue, params.inputValue);
    parent.appendChild(option);
  };
  inputOptions.forEach(inputOption => {
    const optionValue = inputOption[0];
    const optionLabel = inputOption[1];
    // <optgroup> spec:
    // https://www.w3.org/TR/html401/interact/forms.html#h-17.6
    // "...all OPTGROUP elements must be specified directly within a SELECT element (i.e., groups may not be nested)..."
    // check whether this is a <optgroup>
    if (Array.isArray(optionLabel)) {
      // if it is an array, then it is an <optgroup>
      const optgroup = document.createElement('optgroup');
      optgroup.label = optionValue;
      optgroup.disabled = false; // not configurable for now
      select.appendChild(optgroup);
      optionLabel.forEach(o => renderOption(optgroup, o[1], o[0]));
    } else {
      // case of <option>
      renderOption(select, optionLabel, optionValue);
    }
  });
  select.focus();
}

/**
 * @param {HTMLElement} popup
 * @param {InputOptionFlattened[]} inputOptions
 * @param {SweetAlertOptions} params
 */
function populateRadioOptions(popup, inputOptions, params) {
  const radio = getDirectChildByClass(popup, swalClasses.radio);
  if (!radio) {
    return;
  }
  inputOptions.forEach(inputOption => {
    const radioValue = inputOption[0];
    const radioLabel = inputOption[1];
    const radioInput = document.createElement('input');
    const radioLabelElement = document.createElement('label');
    radioInput.type = 'radio';
    radioInput.name = swalClasses.radio;
    radioInput.value = radioValue;
    if (isSelected(radioValue, params.inputValue)) {
      radioInput.checked = true;
    }
    const label = document.createElement('span');
    setInnerHtml(label, radioLabel);
    label.className = swalClasses.label;
    radioLabelElement.appendChild(radioInput);
    radioLabelElement.appendChild(label);
    radio.appendChild(radioLabelElement);
  });
  const radios = radio.querySelectorAll('input');
  if (radios.length) {
    radios[0].focus();
  }
}

/**
 * Converts `inputOptions` into an array of `[value, label]`s
 *
 * @param {*} inputOptions
 * @typedef {string[]} InputOptionFlattened
 * @returns {InputOptionFlattened[]}
 */
const formatInputOptions = inputOptions => {
  /** @type {InputOptionFlattened[]} */
  const result = [];
  if (inputOptions instanceof Map) {
    inputOptions.forEach((value, key) => {
      let valueFormatted = value;
      if (typeof valueFormatted === 'object') {
        // case of <optgroup>
        valueFormatted = formatInputOptions(valueFormatted);
      }
      result.push([key, valueFormatted]);
    });
  } else {
    Object.keys(inputOptions).forEach(key => {
      let valueFormatted = inputOptions[key];
      if (typeof valueFormatted === 'object') {
        // case of <optgroup>
        valueFormatted = formatInputOptions(valueFormatted);
      }
      result.push([key, valueFormatted]);
    });
  }
  return result;
};

/**
 * @param {string} optionValue
 * @param {SweetAlertInputValue} inputValue
 * @returns {boolean}
 */
const isSelected = (optionValue, inputValue) => {
  return !!inputValue && inputValue.toString() === optionValue.toString();
};

/**
 * @param {SweetAlert} instance
 */
const handleConfirmButtonClick = instance => {
  const innerParams = privateProps.innerParams.get(instance);
  instance.disableButtons();
  if (innerParams.input) {
    handleConfirmOrDenyWithInput(instance, 'confirm');
  } else {
    confirm(instance, true);
  }
};

/**
 * @param {SweetAlert} instance
 */
const handleDenyButtonClick = instance => {
  const innerParams = privateProps.innerParams.get(instance);
  instance.disableButtons();
  if (innerParams.returnInputValueOnDeny) {
    handleConfirmOrDenyWithInput(instance, 'deny');
  } else {
    deny(instance, false);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {(dismiss: DismissReason) => void} dismissWith
 */
const handleCancelButtonClick = (instance, dismissWith) => {
  instance.disableButtons();
  dismissWith(DismissReason.cancel);
};

/**
 * @param {SweetAlert} instance
 * @param {'confirm' | 'deny'} type
 */
const handleConfirmOrDenyWithInput = (instance, type) => {
  const innerParams = privateProps.innerParams.get(instance);
  if (!innerParams.input) {
    error(`The "input" parameter is needed to be set when using returnInputValueOn${capitalizeFirstLetter(type)}`);
    return;
  }
  const input = instance.getInput();
  const inputValue = getInputValue(instance, innerParams);
  if (innerParams.inputValidator) {
    handleInputValidator(instance, inputValue, type);
  } else if (input && !input.checkValidity()) {
    instance.enableButtons();
    instance.showValidationMessage(innerParams.validationMessage || input.validationMessage);
  } else if (type === 'deny') {
    deny(instance, inputValue);
  } else {
    confirm(instance, inputValue);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertInputValue} inputValue
 * @param {'confirm' | 'deny'} type
 */
const handleInputValidator = (instance, inputValue, type) => {
  const innerParams = privateProps.innerParams.get(instance);
  instance.disableInput();
  const validationPromise = Promise.resolve().then(() => asPromise(innerParams.inputValidator(inputValue, innerParams.validationMessage)));
  validationPromise.then(validationMessage => {
    instance.enableButtons();
    instance.enableInput();
    if (validationMessage) {
      instance.showValidationMessage(validationMessage);
    } else if (type === 'deny') {
      deny(instance, inputValue);
    } else {
      confirm(instance, inputValue);
    }
  });
};

/**
 * @param {SweetAlert} instance
 * @param {*} value
 */
const deny = (instance, value) => {
  const innerParams = privateProps.innerParams.get(instance || undefined);
  if (innerParams.showLoaderOnDeny) {
    showLoading(getDenyButton());
  }
  if (innerParams.preDeny) {
    instance.isAwaitingPromise = true; // Flagging the instance as awaiting a promise so it's own promise's reject/resolve methods doesn't get destroyed until the result from this preDeny's promise is received
    const preDenyPromise = Promise.resolve().then(() => asPromise(innerParams.preDeny(value, innerParams.validationMessage)));
    preDenyPromise.then(preDenyValue => {
      if (preDenyValue === false) {
        instance.hideLoading();
        handleAwaitingPromise(instance);
      } else {
        instance.close(/** @type SweetAlertResult */{
          isDenied: true,
          value: typeof preDenyValue === 'undefined' ? value : preDenyValue
        });
      }
    }).catch(error => rejectWith(instance || undefined, error));
  } else {
    instance.close(/** @type SweetAlertResult */{
      isDenied: true,
      value
    });
  }
};

/**
 * @param {SweetAlert} instance
 * @param {*} value
 */
const succeedWith = (instance, value) => {
  instance.close(/** @type SweetAlertResult */{
    isConfirmed: true,
    value
  });
};

/**
 *
 * @param {SweetAlert} instance
 * @param {string} error
 */
const rejectWith = (instance, error) => {
  instance.rejectPromise(error);
};

/**
 *
 * @param {SweetAlert} instance
 * @param {*} value
 */
const confirm = (instance, value) => {
  const innerParams = privateProps.innerParams.get(instance || undefined);
  if (innerParams.showLoaderOnConfirm) {
    showLoading();
  }
  if (innerParams.preConfirm) {
    instance.resetValidationMessage();
    instance.isAwaitingPromise = true; // Flagging the instance as awaiting a promise so it's own promise's reject/resolve methods doesn't get destroyed until the result from this preConfirm's promise is received
    const preConfirmPromise = Promise.resolve().then(() => asPromise(innerParams.preConfirm(value, innerParams.validationMessage)));
    preConfirmPromise.then(preConfirmValue => {
      if (isVisible$1(getValidationMessage()) || preConfirmValue === false) {
        instance.hideLoading();
        handleAwaitingPromise(instance);
      } else {
        succeedWith(instance, typeof preConfirmValue === 'undefined' ? value : preConfirmValue);
      }
    }).catch(error => rejectWith(instance || undefined, error));
  } else {
    succeedWith(instance, value);
  }
};

/**
 * Hides loader and shows back the button which was hidden by .showLoading()
 */
function hideLoading() {
  // do nothing if popup is closed
  const innerParams = privateProps.innerParams.get(this);
  if (!innerParams) {
    return;
  }
  const domCache = privateProps.domCache.get(this);
  hide(domCache.loader);
  if (isToast()) {
    if (innerParams.icon) {
      show(getIcon());
    }
  } else {
    showRelatedButton(domCache);
  }
  removeClass([domCache.popup, domCache.actions], swalClasses.loading);
  domCache.popup.removeAttribute('aria-busy');
  domCache.popup.removeAttribute('data-loading');
  domCache.confirmButton.disabled = false;
  domCache.denyButton.disabled = false;
  domCache.cancelButton.disabled = false;
}
const showRelatedButton = domCache => {
  const buttonToReplace = domCache.popup.getElementsByClassName(domCache.loader.getAttribute('data-button-to-replace'));
  if (buttonToReplace.length) {
    show(buttonToReplace[0], 'inline-block');
  } else if (allButtonsAreHidden()) {
    hide(domCache.actions);
  }
};

/**
 * Gets the input DOM node, this method works with input parameter.
 *
 * @returns {HTMLInputElement | null}
 */
function getInput() {
  const innerParams = privateProps.innerParams.get(this);
  const domCache = privateProps.domCache.get(this);
  if (!domCache) {
    return null;
  }
  return getInput$1(domCache.popup, innerParams.input);
}

/**
 * @param {SweetAlert} instance
 * @param {string[]} buttons
 * @param {boolean} disabled
 */
function setButtonsDisabled(instance, buttons, disabled) {
  const domCache = privateProps.domCache.get(instance);
  buttons.forEach(button => {
    domCache[button].disabled = disabled;
  });
}

/**
 * @param {HTMLInputElement | null} input
 * @param {boolean} disabled
 */
function setInputDisabled(input, disabled) {
  const popup = getPopup();
  if (!popup || !input) {
    return;
  }
  if (input.type === 'radio') {
    /** @type {NodeListOf<HTMLInputElement>} */
    const radios = popup.querySelectorAll(`[name="${swalClasses.radio}"]`);
    for (let i = 0; i < radios.length; i++) {
      radios[i].disabled = disabled;
    }
  } else {
    input.disabled = disabled;
  }
}

/**
 * Enable all the buttons
 * @this {SweetAlert}
 */
function enableButtons() {
  setButtonsDisabled(this, ['confirmButton', 'denyButton', 'cancelButton'], false);
}

/**
 * Disable all the buttons
 * @this {SweetAlert}
 */
function disableButtons() {
  setButtonsDisabled(this, ['confirmButton', 'denyButton', 'cancelButton'], true);
}

/**
 * Enable the input field
 * @this {SweetAlert}
 */
function enableInput() {
  setInputDisabled(this.getInput(), false);
}

/**
 * Disable the input field
 * @this {SweetAlert}
 */
function disableInput() {
  setInputDisabled(this.getInput(), true);
}

/**
 * Show block with validation message
 *
 * @param {string} error
 * @this {SweetAlert}
 */
function showValidationMessage(error) {
  const domCache = privateProps.domCache.get(this);
  const params = privateProps.innerParams.get(this);
  setInnerHtml(domCache.validationMessage, error);
  domCache.validationMessage.className = swalClasses['validation-message'];
  if (params.customClass && params.customClass.validationMessage) {
    addClass(domCache.validationMessage, params.customClass.validationMessage);
  }
  show(domCache.validationMessage);
  const input = this.getInput();
  if (input) {
    input.setAttribute('aria-invalid', 'true');
    input.setAttribute('aria-describedby', swalClasses['validation-message']);
    focusInput(input);
    addClass(input, swalClasses.inputerror);
  }
}

/**
 * Hide block with validation message
 *
 * @this {SweetAlert}
 */
function resetValidationMessage() {
  const domCache = privateProps.domCache.get(this);
  if (domCache.validationMessage) {
    hide(domCache.validationMessage);
  }
  const input = this.getInput();
  if (input) {
    input.removeAttribute('aria-invalid');
    input.removeAttribute('aria-describedby');
    removeClass(input, swalClasses.inputerror);
  }
}

const defaultParams = {
  title: '',
  titleText: '',
  text: '',
  html: '',
  footer: '',
  icon: undefined,
  iconColor: undefined,
  iconHtml: undefined,
  template: undefined,
  toast: false,
  draggable: false,
  animation: true,
  theme: 'light',
  showClass: {
    popup: 'swal2-show',
    backdrop: 'swal2-backdrop-show',
    icon: 'swal2-icon-show'
  },
  hideClass: {
    popup: 'swal2-hide',
    backdrop: 'swal2-backdrop-hide',
    icon: 'swal2-icon-hide'
  },
  customClass: {},
  target: 'body',
  color: undefined,
  backdrop: true,
  heightAuto: true,
  allowOutsideClick: true,
  allowEscapeKey: true,
  allowEnterKey: true,
  stopKeydownPropagation: true,
  keydownListenerCapture: false,
  showConfirmButton: true,
  showDenyButton: false,
  showCancelButton: false,
  preConfirm: undefined,
  preDeny: undefined,
  confirmButtonText: 'OK',
  confirmButtonAriaLabel: '',
  confirmButtonColor: undefined,
  denyButtonText: 'No',
  denyButtonAriaLabel: '',
  denyButtonColor: undefined,
  cancelButtonText: 'Cancel',
  cancelButtonAriaLabel: '',
  cancelButtonColor: undefined,
  buttonsStyling: true,
  reverseButtons: false,
  focusConfirm: true,
  focusDeny: false,
  focusCancel: false,
  returnFocus: true,
  showCloseButton: false,
  closeButtonHtml: '&times;',
  closeButtonAriaLabel: 'Close this dialog',
  loaderHtml: '',
  showLoaderOnConfirm: false,
  showLoaderOnDeny: false,
  imageUrl: undefined,
  imageWidth: undefined,
  imageHeight: undefined,
  imageAlt: '',
  timer: undefined,
  timerProgressBar: false,
  width: undefined,
  padding: undefined,
  background: undefined,
  input: undefined,
  inputPlaceholder: '',
  inputLabel: '',
  inputValue: '',
  inputOptions: {},
  inputAutoFocus: true,
  inputAutoTrim: true,
  inputAttributes: {},
  inputValidator: undefined,
  returnInputValueOnDeny: false,
  validationMessage: undefined,
  grow: false,
  position: 'center',
  progressSteps: [],
  currentProgressStep: undefined,
  progressStepsDistance: undefined,
  willOpen: undefined,
  didOpen: undefined,
  didRender: undefined,
  willClose: undefined,
  didClose: undefined,
  didDestroy: undefined,
  scrollbarPadding: true,
  topLayer: false
};
const updatableParams = ['allowEscapeKey', 'allowOutsideClick', 'background', 'buttonsStyling', 'cancelButtonAriaLabel', 'cancelButtonColor', 'cancelButtonText', 'closeButtonAriaLabel', 'closeButtonHtml', 'color', 'confirmButtonAriaLabel', 'confirmButtonColor', 'confirmButtonText', 'currentProgressStep', 'customClass', 'denyButtonAriaLabel', 'denyButtonColor', 'denyButtonText', 'didClose', 'didDestroy', 'draggable', 'footer', 'hideClass', 'html', 'icon', 'iconColor', 'iconHtml', 'imageAlt', 'imageHeight', 'imageUrl', 'imageWidth', 'preConfirm', 'preDeny', 'progressSteps', 'returnFocus', 'reverseButtons', 'showCancelButton', 'showCloseButton', 'showConfirmButton', 'showDenyButton', 'text', 'title', 'titleText', 'theme', 'willClose'];

/** @type {Record<string, string | undefined>} */
const deprecatedParams = {
  allowEnterKey: undefined
};
const toastIncompatibleParams = ['allowOutsideClick', 'allowEnterKey', 'backdrop', 'draggable', 'focusConfirm', 'focusDeny', 'focusCancel', 'returnFocus', 'heightAuto', 'keydownListenerCapture'];

/**
 * Is valid parameter
 *
 * @param {string} paramName
 * @returns {boolean}
 */
const isValidParameter = paramName => {
  return Object.prototype.hasOwnProperty.call(defaultParams, paramName);
};

/**
 * Is valid parameter for Swal.update() method
 *
 * @param {string} paramName
 * @returns {boolean}
 */
const isUpdatableParameter = paramName => {
  return updatableParams.indexOf(paramName) !== -1;
};

/**
 * Is deprecated parameter
 *
 * @param {string} paramName
 * @returns {string | undefined}
 */
const isDeprecatedParameter = paramName => {
  return deprecatedParams[paramName];
};

/**
 * @param {string} param
 */
const checkIfParamIsValid = param => {
  if (!isValidParameter(param)) {
    warn(`Unknown parameter "${param}"`);
  }
};

/**
 * @param {string} param
 */
const checkIfToastParamIsValid = param => {
  if (toastIncompatibleParams.includes(param)) {
    warn(`The parameter "${param}" is incompatible with toasts`);
  }
};

/**
 * @param {string} param
 */
const checkIfParamIsDeprecated = param => {
  const isDeprecated = isDeprecatedParameter(param);
  if (isDeprecated) {
    warnAboutDeprecation(param, isDeprecated);
  }
};

/**
 * Show relevant warnings for given params
 *
 * @param {SweetAlertOptions} params
 */
const showWarningsForParams = params => {
  if (params.backdrop === false && params.allowOutsideClick) {
    warn('"allowOutsideClick" parameter requires `backdrop` parameter to be set to `true`');
  }
  if (params.theme && !['light', 'dark', 'auto', 'minimal', 'borderless', 'bootstrap-4', 'bootstrap-4-light', 'bootstrap-4-dark', 'bootstrap-5', 'bootstrap-5-light', 'bootstrap-5-dark', 'material-ui', 'material-ui-light', 'material-ui-dark', 'embed-iframe', 'bulma', 'bulma-light', 'bulma-dark'].includes(params.theme)) {
    warn(`Invalid theme "${params.theme}"`);
  }
  for (const param in params) {
    checkIfParamIsValid(param);
    if (params.toast) {
      checkIfToastParamIsValid(param);
    }
    checkIfParamIsDeprecated(param);
  }
};

/**
 * Updates popup parameters.
 *
 * @param {SweetAlertOptions} params
 */
function update(params) {
  const container = getContainer();
  const popup = getPopup();
  const innerParams = privateProps.innerParams.get(this);
  if (!popup || hasClass(popup, innerParams.hideClass.popup)) {
    warn(`You're trying to update the closed or closing popup, that won't work. Use the update() method in preConfirm parameter or show a new popup.`);
    return;
  }
  const validUpdatableParams = filterValidParams(params);
  const updatedParams = Object.assign({}, innerParams, validUpdatableParams);
  showWarningsForParams(updatedParams);
  container.dataset['swal2Theme'] = updatedParams.theme;
  render(this, updatedParams);
  privateProps.innerParams.set(this, updatedParams);
  Object.defineProperties(this, {
    params: {
      value: Object.assign({}, this.params, params),
      writable: false,
      enumerable: true
    }
  });
}

/**
 * @param {SweetAlertOptions} params
 * @returns {SweetAlertOptions}
 */
const filterValidParams = params => {
  const validUpdatableParams = {};
  Object.keys(params).forEach(param => {
    if (isUpdatableParameter(param)) {
      validUpdatableParams[param] = params[param];
    } else {
      warn(`Invalid parameter to update: ${param}`);
    }
  });
  return validUpdatableParams;
};

/**
 * Dispose the current SweetAlert2 instance
 */
function _destroy() {
  const domCache = privateProps.domCache.get(this);
  const innerParams = privateProps.innerParams.get(this);
  if (!innerParams) {
    disposeWeakMaps(this); // The WeakMaps might have been partly destroyed, we must recall it to dispose any remaining WeakMaps #2335
    return; // This instance has already been destroyed
  }

  // Check if there is another Swal closing
  if (domCache.popup && globalState.swalCloseEventFinishedCallback) {
    globalState.swalCloseEventFinishedCallback();
    delete globalState.swalCloseEventFinishedCallback;
  }
  if (typeof innerParams.didDestroy === 'function') {
    innerParams.didDestroy();
  }
  globalState.eventEmitter.emit('didDestroy');
  disposeSwal(this);
}

/**
 * @param {SweetAlert} instance
 */
const disposeSwal = instance => {
  disposeWeakMaps(instance);
  // Unset this.params so GC will dispose it (#1569)
  delete instance.params;
  // Unset globalState props so GC will dispose globalState (#1569)
  delete globalState.keydownHandler;
  delete globalState.keydownTarget;
  // Unset currentInstance
  delete globalState.currentInstance;
};

/**
 * @param {SweetAlert} instance
 */
const disposeWeakMaps = instance => {
  // If the current instance is awaiting a promise result, we keep the privateMethods to call them once the promise result is retrieved #2335
  if (instance.isAwaitingPromise) {
    unsetWeakMaps(privateProps, instance);
    instance.isAwaitingPromise = true;
  } else {
    unsetWeakMaps(privateMethods, instance);
    unsetWeakMaps(privateProps, instance);
    delete instance.isAwaitingPromise;
    // Unset instance methods
    delete instance.disableButtons;
    delete instance.enableButtons;
    delete instance.getInput;
    delete instance.disableInput;
    delete instance.enableInput;
    delete instance.hideLoading;
    delete instance.disableLoading;
    delete instance.showValidationMessage;
    delete instance.resetValidationMessage;
    delete instance.close;
    delete instance.closePopup;
    delete instance.closeModal;
    delete instance.closeToast;
    delete instance.rejectPromise;
    delete instance.update;
    delete instance._destroy;
  }
};

/**
 * @param {object} obj
 * @param {SweetAlert} instance
 */
const unsetWeakMaps = (obj, instance) => {
  for (const i in obj) {
    obj[i].delete(instance);
  }
};

var instanceMethods = /*#__PURE__*/Object.freeze({
  __proto__: null,
  _destroy: _destroy,
  close: close,
  closeModal: close,
  closePopup: close,
  closeToast: close,
  disableButtons: disableButtons,
  disableInput: disableInput,
  disableLoading: hideLoading,
  enableButtons: enableButtons,
  enableInput: enableInput,
  getInput: getInput,
  handleAwaitingPromise: handleAwaitingPromise,
  hideLoading: hideLoading,
  rejectPromise: rejectPromise,
  resetValidationMessage: resetValidationMessage,
  showValidationMessage: showValidationMessage,
  update: update
});

/**
 * @param {SweetAlertOptions} innerParams
 * @param {DomCache} domCache
 * @param {(dismiss: DismissReason) => void} dismissWith
 */
const handlePopupClick = (innerParams, domCache, dismissWith) => {
  if (innerParams.toast) {
    handleToastClick(innerParams, domCache, dismissWith);
  } else {
    // Ignore click events that had mousedown on the popup but mouseup on the container
    // This can happen when the user drags a slider
    handleModalMousedown(domCache);

    // Ignore click events that had mousedown on the container but mouseup on the popup
    handleContainerMousedown(domCache);
    handleModalClick(innerParams, domCache, dismissWith);
  }
};

/**
 * @param {SweetAlertOptions} innerParams
 * @param {DomCache} domCache
 * @param {(dismiss: DismissReason) => void} dismissWith
 */
const handleToastClick = (innerParams, domCache, dismissWith) => {
  // Closing toast by internal click
  domCache.popup.onclick = () => {
    if (innerParams && (isAnyButtonShown(innerParams) || innerParams.timer || innerParams.input)) {
      return;
    }
    dismissWith(DismissReason.close);
  };
};

/**
 * @param {SweetAlertOptions} innerParams
 * @returns {boolean}
 */
const isAnyButtonShown = innerParams => {
  return !!(innerParams.showConfirmButton || innerParams.showDenyButton || innerParams.showCancelButton || innerParams.showCloseButton);
};
let ignoreOutsideClick = false;

/**
 * @param {DomCache} domCache
 */
const handleModalMousedown = domCache => {
  domCache.popup.onmousedown = () => {
    domCache.container.onmouseup = function (e) {
      domCache.container.onmouseup = () => {};
      // We only check if the mouseup target is the container because usually it doesn't
      // have any other direct children aside of the popup
      if (e.target === domCache.container) {
        ignoreOutsideClick = true;
      }
    };
  };
};

/**
 * @param {DomCache} domCache
 */
const handleContainerMousedown = domCache => {
  domCache.container.onmousedown = e => {
    // prevent the modal text from being selected on double click on the container (allowOutsideClick: false)
    if (e.target === domCache.container) {
      e.preventDefault();
    }
    domCache.popup.onmouseup = function (e) {
      domCache.popup.onmouseup = () => {};
      // We also need to check if the mouseup target is a child of the popup
      if (e.target === domCache.popup || e.target instanceof HTMLElement && domCache.popup.contains(e.target)) {
        ignoreOutsideClick = true;
      }
    };
  };
};

/**
 * @param {SweetAlertOptions} innerParams
 * @param {DomCache} domCache
 * @param {(dismiss: DismissReason) => void} dismissWith
 */
const handleModalClick = (innerParams, domCache, dismissWith) => {
  domCache.container.onclick = e => {
    if (ignoreOutsideClick) {
      ignoreOutsideClick = false;
      return;
    }
    if (e.target === domCache.container && callIfFunction(innerParams.allowOutsideClick)) {
      dismissWith(DismissReason.backdrop);
    }
  };
};

const isJqueryElement = elem => typeof elem === 'object' && elem.jquery;
const isElement = elem => elem instanceof Element || isJqueryElement(elem);
const argsToParams = args => {
  const params = {};
  if (typeof args[0] === 'object' && !isElement(args[0])) {
    Object.assign(params, args[0]);
  } else {
    ['title', 'html', 'icon'].forEach((name, index) => {
      const arg = args[index];
      if (typeof arg === 'string' || isElement(arg)) {
        params[name] = arg;
      } else if (arg !== undefined) {
        error(`Unexpected type of ${name}! Expected "string" or "Element", got ${typeof arg}`);
      }
    });
  }
  return params;
};

/**
 * Main method to create a new SweetAlert2 popup
 *
 * @param  {...SweetAlertOptions} args
 * @returns {Promise<SweetAlertResult>}
 */
function fire(...args) {
  return new this(...args);
}

/**
 * Returns an extended version of `Swal` containing `params` as defaults.
 * Useful for reusing Swal configuration.
 *
 * For example:
 *
 * Before:
 * const textPromptOptions = { input: 'text', showCancelButton: true }
 * const {value: firstName} = await Swal.fire({ ...textPromptOptions, title: 'What is your first name?' })
 * const {value: lastName} = await Swal.fire({ ...textPromptOptions, title: 'What is your last name?' })
 *
 * After:
 * const TextPrompt = Swal.mixin({ input: 'text', showCancelButton: true })
 * const {value: firstName} = await TextPrompt('What is your first name?')
 * const {value: lastName} = await TextPrompt('What is your last name?')
 *
 * @param {SweetAlertOptions} mixinParams
 * @returns {SweetAlert}
 */
function mixin(mixinParams) {
  class MixinSwal extends this {
    _main(params, priorityMixinParams) {
      return super._main(params, Object.assign({}, mixinParams, priorityMixinParams));
    }
  }
  // @ts-ignore
  return MixinSwal;
}

/**
 * If `timer` parameter is set, returns number of milliseconds of timer remained.
 * Otherwise, returns undefined.
 *
 * @returns {number | undefined}
 */
const getTimerLeft = () => {
  return globalState.timeout && globalState.timeout.getTimerLeft();
};

/**
 * Stop timer. Returns number of milliseconds of timer remained.
 * If `timer` parameter isn't set, returns undefined.
 *
 * @returns {number | undefined}
 */
const stopTimer = () => {
  if (globalState.timeout) {
    stopTimerProgressBar();
    return globalState.timeout.stop();
  }
};

/**
 * Resume timer. Returns number of milliseconds of timer remained.
 * If `timer` parameter isn't set, returns undefined.
 *
 * @returns {number | undefined}
 */
const resumeTimer = () => {
  if (globalState.timeout) {
    const remaining = globalState.timeout.start();
    animateTimerProgressBar(remaining);
    return remaining;
  }
};

/**
 * Resume timer. Returns number of milliseconds of timer remained.
 * If `timer` parameter isn't set, returns undefined.
 *
 * @returns {number | undefined}
 */
const toggleTimer = () => {
  const timer = globalState.timeout;
  return timer && (timer.running ? stopTimer() : resumeTimer());
};

/**
 * Increase timer. Returns number of milliseconds of an updated timer.
 * If `timer` parameter isn't set, returns undefined.
 *
 * @param {number} ms
 * @returns {number | undefined}
 */
const increaseTimer = ms => {
  if (globalState.timeout) {
    const remaining = globalState.timeout.increase(ms);
    animateTimerProgressBar(remaining, true);
    return remaining;
  }
};

/**
 * Check if timer is running. Returns true if timer is running
 * or false if timer is paused or stopped.
 * If `timer` parameter isn't set, returns undefined
 *
 * @returns {boolean}
 */
const isTimerRunning = () => {
  return !!(globalState.timeout && globalState.timeout.isRunning());
};

let bodyClickListenerAdded = false;
const clickHandlers = {};

/**
 * @param {string} attr
 */
function bindClickHandler(attr = 'data-swal-template') {
  clickHandlers[attr] = this;
  if (!bodyClickListenerAdded) {
    document.body.addEventListener('click', bodyClickListener);
    bodyClickListenerAdded = true;
  }
}
const bodyClickListener = event => {
  for (let el = event.target; el && el !== document; el = el.parentNode) {
    for (const attr in clickHandlers) {
      const template = el.getAttribute(attr);
      if (template) {
        clickHandlers[attr].fire({
          template
        });
        return;
      }
    }
  }
};

// Source: https://gist.github.com/mudge/5830382?permalink_comment_id=2691957#gistcomment-2691957

class EventEmitter {
  constructor() {
    /** @type {Events} */
    this.events = {};
  }

  /**
   * @param {string} eventName
   * @returns {EventHandlers}
   */
  _getHandlersByEventName(eventName) {
    if (typeof this.events[eventName] === 'undefined') {
      // not Set because we need to keep the FIFO order
      // https://github.com/sweetalert2/sweetalert2/pull/2763#discussion_r1748990334
      this.events[eventName] = [];
    }
    return this.events[eventName];
  }

  /**
   * @param {string} eventName
   * @param {EventHandler} eventHandler
   */
  on(eventName, eventHandler) {
    const currentHandlers = this._getHandlersByEventName(eventName);
    if (!currentHandlers.includes(eventHandler)) {
      currentHandlers.push(eventHandler);
    }
  }

  /**
   * @param {string} eventName
   * @param {EventHandler} eventHandler
   */
  once(eventName, eventHandler) {
    /**
     * @param {Array} args
     */
    const onceFn = (...args) => {
      this.removeListener(eventName, onceFn);
      eventHandler.apply(this, args);
    };
    this.on(eventName, onceFn);
  }

  /**
   * @param {string} eventName
   * @param {Array} args
   */
  emit(eventName, ...args) {
    this._getHandlersByEventName(eventName).forEach(
    /**
     * @param {EventHandler} eventHandler
     */
    eventHandler => {
      try {
        eventHandler.apply(this, args);
      } catch (error) {
        console.error(error);
      }
    });
  }

  /**
   * @param {string} eventName
   * @param {EventHandler} eventHandler
   */
  removeListener(eventName, eventHandler) {
    const currentHandlers = this._getHandlersByEventName(eventName);
    const index = currentHandlers.indexOf(eventHandler);
    if (index > -1) {
      currentHandlers.splice(index, 1);
    }
  }

  /**
   * @param {string} eventName
   */
  removeAllListeners(eventName) {
    if (this.events[eventName] !== undefined) {
      // https://github.com/sweetalert2/sweetalert2/pull/2763#discussion_r1749239222
      this.events[eventName].length = 0;
    }
  }
  reset() {
    this.events = {};
  }
}

globalState.eventEmitter = new EventEmitter();

/**
 * @param {string} eventName
 * @param {EventHandler} eventHandler
 */
const on = (eventName, eventHandler) => {
  globalState.eventEmitter.on(eventName, eventHandler);
};

/**
 * @param {string} eventName
 * @param {EventHandler} eventHandler
 */
const once = (eventName, eventHandler) => {
  globalState.eventEmitter.once(eventName, eventHandler);
};

/**
 * @param {string} [eventName]
 * @param {EventHandler} [eventHandler]
 */
const off = (eventName, eventHandler) => {
  // Remove all handlers for all events
  if (!eventName) {
    globalState.eventEmitter.reset();
    return;
  }
  if (eventHandler) {
    // Remove a specific handler
    globalState.eventEmitter.removeListener(eventName, eventHandler);
  } else {
    // Remove all handlers for a specific event
    globalState.eventEmitter.removeAllListeners(eventName);
  }
};

var staticMethods = /*#__PURE__*/Object.freeze({
  __proto__: null,
  argsToParams: argsToParams,
  bindClickHandler: bindClickHandler,
  clickCancel: clickCancel,
  clickConfirm: clickConfirm,
  clickDeny: clickDeny,
  enableLoading: showLoading,
  fire: fire,
  getActions: getActions,
  getCancelButton: getCancelButton,
  getCloseButton: getCloseButton,
  getConfirmButton: getConfirmButton,
  getContainer: getContainer,
  getDenyButton: getDenyButton,
  getFocusableElements: getFocusableElements,
  getFooter: getFooter,
  getHtmlContainer: getHtmlContainer,
  getIcon: getIcon,
  getIconContent: getIconContent,
  getImage: getImage,
  getInputLabel: getInputLabel,
  getLoader: getLoader,
  getPopup: getPopup,
  getProgressSteps: getProgressSteps,
  getTimerLeft: getTimerLeft,
  getTimerProgressBar: getTimerProgressBar,
  getTitle: getTitle,
  getValidationMessage: getValidationMessage,
  increaseTimer: increaseTimer,
  isDeprecatedParameter: isDeprecatedParameter,
  isLoading: isLoading,
  isTimerRunning: isTimerRunning,
  isUpdatableParameter: isUpdatableParameter,
  isValidParameter: isValidParameter,
  isVisible: isVisible,
  mixin: mixin,
  off: off,
  on: on,
  once: once,
  resumeTimer: resumeTimer,
  showLoading: showLoading,
  stopTimer: stopTimer,
  toggleTimer: toggleTimer
});

class Timer {
  /**
   * @param {() => void} callback
   * @param {number} delay
   */
  constructor(callback, delay) {
    this.callback = callback;
    this.remaining = delay;
    this.running = false;
    this.start();
  }

  /**
   * @returns {number}
   */
  start() {
    if (!this.running) {
      this.running = true;
      this.started = new Date();
      this.id = setTimeout(this.callback, this.remaining);
    }
    return this.remaining;
  }

  /**
   * @returns {number}
   */
  stop() {
    if (this.started && this.running) {
      this.running = false;
      clearTimeout(this.id);
      this.remaining -= new Date().getTime() - this.started.getTime();
    }
    return this.remaining;
  }

  /**
   * @param {number} n
   * @returns {number}
   */
  increase(n) {
    const running = this.running;
    if (running) {
      this.stop();
    }
    this.remaining += n;
    if (running) {
      this.start();
    }
    return this.remaining;
  }

  /**
   * @returns {number}
   */
  getTimerLeft() {
    if (this.running) {
      this.stop();
      this.start();
    }
    return this.remaining;
  }

  /**
   * @returns {boolean}
   */
  isRunning() {
    return this.running;
  }
}

const swalStringParams = ['swal-title', 'swal-html', 'swal-footer'];

/**
 * @param {SweetAlertOptions} params
 * @returns {SweetAlertOptions}
 */
const getTemplateParams = params => {
  const template = typeof params.template === 'string' ? (/** @type {HTMLTemplateElement} */document.querySelector(params.template)) : params.template;
  if (!template) {
    return {};
  }
  /** @type {DocumentFragment} */
  const templateContent = template.content;
  showWarningsForElements(templateContent);
  const result = Object.assign(getSwalParams(templateContent), getSwalFunctionParams(templateContent), getSwalButtons(templateContent), getSwalImage(templateContent), getSwalIcon(templateContent), getSwalInput(templateContent), getSwalStringParams(templateContent, swalStringParams));
  return result;
};

/**
 * @param {DocumentFragment} templateContent
 * @returns {Record<string, string | boolean | number>}
 */
const getSwalParams = templateContent => {
  /** @type {Record<string, string | boolean | number>} */
  const result = {};
  /** @type {HTMLElement[]} */
  const swalParams = Array.from(templateContent.querySelectorAll('swal-param'));
  swalParams.forEach(param => {
    showWarningsForAttributes(param, ['name', 'value']);
    const paramName = /** @type {keyof SweetAlertOptions} */param.getAttribute('name');
    const value = param.getAttribute('value');
    if (!paramName || !value) {
      return;
    }
    if (typeof defaultParams[paramName] === 'boolean') {
      result[paramName] = value !== 'false';
    } else if (typeof defaultParams[paramName] === 'object') {
      result[paramName] = JSON.parse(value);
    } else {
      result[paramName] = value;
    }
  });
  return result;
};

/**
 * @param {DocumentFragment} templateContent
 * @returns {Record<string, () => void>}
 */
const getSwalFunctionParams = templateContent => {
  /** @type {Record<string, () => void>} */
  const result = {};
  /** @type {HTMLElement[]} */
  const swalFunctions = Array.from(templateContent.querySelectorAll('swal-function-param'));
  swalFunctions.forEach(param => {
    const paramName = /** @type {keyof SweetAlertOptions} */param.getAttribute('name');
    const value = param.getAttribute('value');
    if (!paramName || !value) {
      return;
    }
    result[paramName] = new Function(`return ${value}`)();
  });
  return result;
};

/**
 * @param {DocumentFragment} templateContent
 * @returns {Record<string, string | boolean>}
 */
const getSwalButtons = templateContent => {
  /** @type {Record<string, string | boolean>} */
  const result = {};
  /** @type {HTMLElement[]} */
  const swalButtons = Array.from(templateContent.querySelectorAll('swal-button'));
  swalButtons.forEach(button => {
    showWarningsForAttributes(button, ['type', 'color', 'aria-label']);
    const type = button.getAttribute('type');
    if (!type || !['confirm', 'cancel', 'deny'].includes(type)) {
      return;
    }
    result[`${type}ButtonText`] = button.innerHTML;
    result[`show${capitalizeFirstLetter(type)}Button`] = true;
    if (button.hasAttribute('color')) {
      result[`${type}ButtonColor`] = button.getAttribute('color');
    }
    if (button.hasAttribute('aria-label')) {
      result[`${type}ButtonAriaLabel`] = button.getAttribute('aria-label');
    }
  });
  return result;
};

/**
 * @param {DocumentFragment} templateContent
 * @returns {Pick<SweetAlertOptions, 'imageUrl' | 'imageWidth' | 'imageHeight' | 'imageAlt'>}
 */
const getSwalImage = templateContent => {
  const result = {};
  /** @type {HTMLElement | null} */
  const image = templateContent.querySelector('swal-image');
  if (image) {
    showWarningsForAttributes(image, ['src', 'width', 'height', 'alt']);
    if (image.hasAttribute('src')) {
      result.imageUrl = image.getAttribute('src') || undefined;
    }
    if (image.hasAttribute('width')) {
      result.imageWidth = image.getAttribute('width') || undefined;
    }
    if (image.hasAttribute('height')) {
      result.imageHeight = image.getAttribute('height') || undefined;
    }
    if (image.hasAttribute('alt')) {
      result.imageAlt = image.getAttribute('alt') || undefined;
    }
  }
  return result;
};

/**
 * @param {DocumentFragment} templateContent
 * @returns {object}
 */
const getSwalIcon = templateContent => {
  const result = {};
  /** @type {HTMLElement | null} */
  const icon = templateContent.querySelector('swal-icon');
  if (icon) {
    showWarningsForAttributes(icon, ['type', 'color']);
    if (icon.hasAttribute('type')) {
      result.icon = icon.getAttribute('type');
    }
    if (icon.hasAttribute('color')) {
      result.iconColor = icon.getAttribute('color');
    }
    result.iconHtml = icon.innerHTML;
  }
  return result;
};

/**
 * @param {DocumentFragment} templateContent
 * @returns {object}
 */
const getSwalInput = templateContent => {
  /** @type {object} */
  const result = {};
  /** @type {HTMLElement | null} */
  const input = templateContent.querySelector('swal-input');
  if (input) {
    showWarningsForAttributes(input, ['type', 'label', 'placeholder', 'value']);
    result.input = input.getAttribute('type') || 'text';
    if (input.hasAttribute('label')) {
      result.inputLabel = input.getAttribute('label');
    }
    if (input.hasAttribute('placeholder')) {
      result.inputPlaceholder = input.getAttribute('placeholder');
    }
    if (input.hasAttribute('value')) {
      result.inputValue = input.getAttribute('value');
    }
  }
  /** @type {HTMLElement[]} */
  const inputOptions = Array.from(templateContent.querySelectorAll('swal-input-option'));
  if (inputOptions.length) {
    result.inputOptions = {};
    inputOptions.forEach(option => {
      showWarningsForAttributes(option, ['value']);
      const optionValue = option.getAttribute('value');
      if (!optionValue) {
        return;
      }
      const optionName = option.innerHTML;
      result.inputOptions[optionValue] = optionName;
    });
  }
  return result;
};

/**
 * @param {DocumentFragment} templateContent
 * @param {string[]} paramNames
 * @returns {Record<string, string>}
 */
const getSwalStringParams = (templateContent, paramNames) => {
  /** @type {Record<string, string>} */
  const result = {};
  for (const i in paramNames) {
    const paramName = paramNames[i];
    /** @type {HTMLElement | null} */
    const tag = templateContent.querySelector(paramName);
    if (tag) {
      showWarningsForAttributes(tag, []);
      result[paramName.replace(/^swal-/, '')] = tag.innerHTML.trim();
    }
  }
  return result;
};

/**
 * @param {DocumentFragment} templateContent
 */
const showWarningsForElements = templateContent => {
  const allowedElements = swalStringParams.concat(['swal-param', 'swal-function-param', 'swal-button', 'swal-image', 'swal-icon', 'swal-input', 'swal-input-option']);
  Array.from(templateContent.children).forEach(el => {
    const tagName = el.tagName.toLowerCase();
    if (!allowedElements.includes(tagName)) {
      warn(`Unrecognized element <${tagName}>`);
    }
  });
};

/**
 * @param {HTMLElement} el
 * @param {string[]} allowedAttributes
 */
const showWarningsForAttributes = (el, allowedAttributes) => {
  Array.from(el.attributes).forEach(attribute => {
    if (allowedAttributes.indexOf(attribute.name) === -1) {
      warn([`Unrecognized attribute "${attribute.name}" on <${el.tagName.toLowerCase()}>.`, `${allowedAttributes.length ? `Allowed attributes are: ${allowedAttributes.join(', ')}` : 'To set the value, use HTML within the element.'}`]);
    }
  });
};

const SHOW_CLASS_TIMEOUT = 10;

/**
 * Open popup, add necessary classes and styles, fix scrollbar
 *
 * @param {SweetAlertOptions} params
 */
const openPopup = params => {
  const container = getContainer();
  const popup = getPopup();
  if (typeof params.willOpen === 'function') {
    params.willOpen(popup);
  }
  globalState.eventEmitter.emit('willOpen', popup);
  const bodyStyles = window.getComputedStyle(document.body);
  const initialBodyOverflow = bodyStyles.overflowY;
  addClasses(container, popup, params);

  // scrolling is 'hidden' until animation is done, after that 'auto'
  setTimeout(() => {
    setScrollingVisibility(container, popup);
  }, SHOW_CLASS_TIMEOUT);
  if (isModal()) {
    fixScrollContainer(container, params.scrollbarPadding, initialBodyOverflow);
    setAriaHidden();
  }
  if (!isToast() && !globalState.previousActiveElement) {
    globalState.previousActiveElement = document.activeElement;
  }
  if (typeof params.didOpen === 'function') {
    setTimeout(() => params.didOpen(popup));
  }
  globalState.eventEmitter.emit('didOpen', popup);
};

/**
 * @param {AnimationEvent} event
 */
const swalOpenAnimationFinished = event => {
  const popup = getPopup();
  if (event.target !== popup) {
    return;
  }
  const container = getContainer();
  popup.removeEventListener('animationend', swalOpenAnimationFinished);
  popup.removeEventListener('transitionend', swalOpenAnimationFinished);
  container.style.overflowY = 'auto';

  // no-transition is added in init() in case one swal is opened right after another
  removeClass(container, swalClasses['no-transition']);
};

/**
 * @param {HTMLElement} container
 * @param {HTMLElement} popup
 */
const setScrollingVisibility = (container, popup) => {
  if (hasCssAnimation(popup)) {
    container.style.overflowY = 'hidden';
    popup.addEventListener('animationend', swalOpenAnimationFinished);
    popup.addEventListener('transitionend', swalOpenAnimationFinished);
  } else {
    container.style.overflowY = 'auto';
  }
};

/**
 * @param {HTMLElement} container
 * @param {boolean} scrollbarPadding
 * @param {string} initialBodyOverflow
 */
const fixScrollContainer = (container, scrollbarPadding, initialBodyOverflow) => {
  iOSfix();
  if (scrollbarPadding && initialBodyOverflow !== 'hidden') {
    replaceScrollbarWithPadding(initialBodyOverflow);
  }

  // sweetalert2/issues/1247
  setTimeout(() => {
    container.scrollTop = 0;
  });
};

/**
 * @param {HTMLElement} container
 * @param {HTMLElement} popup
 * @param {SweetAlertOptions} params
 */
const addClasses = (container, popup, params) => {
  addClass(container, params.showClass.backdrop);
  if (params.animation) {
    // this workaround with opacity is needed for https://github.com/sweetalert2/sweetalert2/issues/2059
    popup.style.setProperty('opacity', '0', 'important');
    show(popup, 'grid');
    setTimeout(() => {
      // Animate popup right after showing it
      addClass(popup, params.showClass.popup);
      // and remove the opacity workaround
      popup.style.removeProperty('opacity');
    }, SHOW_CLASS_TIMEOUT); // 10ms in order to fix #2062
  } else {
    show(popup, 'grid');
  }
  addClass([document.documentElement, document.body], swalClasses.shown);
  if (params.heightAuto && params.backdrop && !params.toast) {
    addClass([document.documentElement, document.body], swalClasses['height-auto']);
  }
};

var defaultInputValidators = {
  /**
   * @param {string} string
   * @param {string} [validationMessage]
   * @returns {Promise<string | void>}
   */
  email: (string, validationMessage) => {
    return /^[a-zA-Z0-9.+_'-]+@[a-zA-Z0-9.-]+\.[a-zA-Z0-9-]+$/.test(string) ? Promise.resolve() : Promise.resolve(validationMessage || 'Invalid email address');
  },
  /**
   * @param {string} string
   * @param {string} [validationMessage]
   * @returns {Promise<string | void>}
   */
  url: (string, validationMessage) => {
    // taken from https://stackoverflow.com/a/3809435 with a small change from #1306 and #2013
    return /^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._+~#=]{1,256}\.[a-z]{2,63}\b([-a-zA-Z0-9@:%_+.~#?&/=]*)$/.test(string) ? Promise.resolve() : Promise.resolve(validationMessage || 'Invalid URL');
  }
};

/**
 * @param {SweetAlertOptions} params
 */
function setDefaultInputValidators(params) {
  // Use default `inputValidator` for supported input types if not provided
  if (params.inputValidator) {
    return;
  }
  if (params.input === 'email') {
    params.inputValidator = defaultInputValidators['email'];
  }
  if (params.input === 'url') {
    params.inputValidator = defaultInputValidators['url'];
  }
}

/**
 * @param {SweetAlertOptions} params
 */
function validateCustomTargetElement(params) {
  // Determine if the custom target element is valid
  if (!params.target || typeof params.target === 'string' && !document.querySelector(params.target) || typeof params.target !== 'string' && !params.target.appendChild) {
    warn('Target parameter is not valid, defaulting to "body"');
    params.target = 'body';
  }
}

/**
 * Set type, text and actions on popup
 *
 * @param {SweetAlertOptions} params
 */
function setParameters(params) {
  setDefaultInputValidators(params);

  // showLoaderOnConfirm && preConfirm
  if (params.showLoaderOnConfirm && !params.preConfirm) {
    warn('showLoaderOnConfirm is set to true, but preConfirm is not defined.\n' + 'showLoaderOnConfirm should be used together with preConfirm, see usage example:\n' + 'https://sweetalert2.github.io/#ajax-request');
  }
  validateCustomTargetElement(params);

  // Replace newlines with <br> in title
  if (typeof params.title === 'string') {
    params.title = params.title.split('\n').join('<br />');
  }
  init(params);
}

/** @type {SweetAlert} */
let currentInstance;
var _promise = /*#__PURE__*/new WeakMap();
class SweetAlert {
  /**
   * @param {...(SweetAlertOptions | string)} args
   * @this {SweetAlert}
   */
  constructor(...args) {
    /**
     * @type {Promise<SweetAlertResult>}
     */
    _classPrivateFieldInitSpec(this, _promise, void 0);
    // Prevent run in Node env
    if (typeof window === 'undefined') {
      return;
    }
    currentInstance = this;

    // @ts-ignore
    const outerParams = Object.freeze(this.constructor.argsToParams(args));

    /** @type {Readonly<SweetAlertOptions>} */
    this.params = outerParams;

    /** @type {boolean} */
    this.isAwaitingPromise = false;
    _classPrivateFieldSet2(_promise, this, this._main(currentInstance.params));
  }
  _main(userParams, mixinParams = {}) {
    showWarningsForParams(Object.assign({}, mixinParams, userParams));
    if (globalState.currentInstance) {
      const swalPromiseResolve = privateMethods.swalPromiseResolve.get(globalState.currentInstance);
      const {
        isAwaitingPromise
      } = globalState.currentInstance;
      globalState.currentInstance._destroy();
      if (!isAwaitingPromise) {
        swalPromiseResolve({
          isDismissed: true
        });
      }
      if (isModal()) {
        unsetAriaHidden();
      }
    }
    globalState.currentInstance = currentInstance;
    const innerParams = prepareParams(userParams, mixinParams);
    setParameters(innerParams);
    Object.freeze(innerParams);

    // clear the previous timer
    if (globalState.timeout) {
      globalState.timeout.stop();
      delete globalState.timeout;
    }

    // clear the restore focus timeout
    clearTimeout(globalState.restoreFocusTimeout);
    const domCache = populateDomCache(currentInstance);
    render(currentInstance, innerParams);
    privateProps.innerParams.set(currentInstance, innerParams);
    return swalPromise(currentInstance, domCache, innerParams);
  }

  // `catch` cannot be the name of a module export, so we define our thenable methods here instead
  then(onFulfilled) {
    return _classPrivateFieldGet2(_promise, this).then(onFulfilled);
  }
  finally(onFinally) {
    return _classPrivateFieldGet2(_promise, this).finally(onFinally);
  }
}

/**
 * @param {SweetAlert} instance
 * @param {DomCache} domCache
 * @param {SweetAlertOptions} innerParams
 * @returns {Promise}
 */
const swalPromise = (instance, domCache, innerParams) => {
  return new Promise((resolve, reject) => {
    // functions to handle all closings/dismissals
    /**
     * @param {DismissReason} dismiss
     */
    const dismissWith = dismiss => {
      instance.close({
        isDismissed: true,
        dismiss,
        isConfirmed: false,
        isDenied: false
      });
    };
    privateMethods.swalPromiseResolve.set(instance, resolve);
    privateMethods.swalPromiseReject.set(instance, reject);
    domCache.confirmButton.onclick = () => {
      handleConfirmButtonClick(instance);
    };
    domCache.denyButton.onclick = () => {
      handleDenyButtonClick(instance);
    };
    domCache.cancelButton.onclick = () => {
      handleCancelButtonClick(instance, dismissWith);
    };
    domCache.closeButton.onclick = () => {
      dismissWith(DismissReason.close);
    };
    handlePopupClick(innerParams, domCache, dismissWith);
    addKeydownHandler(globalState, innerParams, dismissWith);
    handleInputOptionsAndValue(instance, innerParams);
    openPopup(innerParams);
    setupTimer(globalState, innerParams, dismissWith);
    initFocus(domCache, innerParams);

    // Scroll container to top on open (#1247, #1946)
    setTimeout(() => {
      domCache.container.scrollTop = 0;
    });
  });
};

/**
 * @param {SweetAlertOptions} userParams
 * @param {SweetAlertOptions} mixinParams
 * @returns {SweetAlertOptions}
 */
const prepareParams = (userParams, mixinParams) => {
  const templateParams = getTemplateParams(userParams);
  const params = Object.assign({}, defaultParams, mixinParams, templateParams, userParams); // precedence is described in #2131
  params.showClass = Object.assign({}, defaultParams.showClass, params.showClass);
  params.hideClass = Object.assign({}, defaultParams.hideClass, params.hideClass);
  if (params.animation === false) {
    params.showClass = {
      backdrop: 'swal2-noanimation'
    };
    params.hideClass = {};
  }
  return params;
};

/**
 * @param {SweetAlert} instance
 * @returns {DomCache}
 */
const populateDomCache = instance => {
  const domCache = {
    popup: getPopup(),
    container: getContainer(),
    actions: getActions(),
    confirmButton: getConfirmButton(),
    denyButton: getDenyButton(),
    cancelButton: getCancelButton(),
    loader: getLoader(),
    closeButton: getCloseButton(),
    validationMessage: getValidationMessage(),
    progressSteps: getProgressSteps()
  };
  privateProps.domCache.set(instance, domCache);
  return domCache;
};

/**
 * @param {GlobalState} globalState
 * @param {SweetAlertOptions} innerParams
 * @param {(dismiss: DismissReason) => void} dismissWith
 */
const setupTimer = (globalState, innerParams, dismissWith) => {
  const timerProgressBar = getTimerProgressBar();
  hide(timerProgressBar);
  if (innerParams.timer) {
    globalState.timeout = new Timer(() => {
      dismissWith('timer');
      delete globalState.timeout;
    }, innerParams.timer);
    if (innerParams.timerProgressBar) {
      show(timerProgressBar);
      applyCustomClass(timerProgressBar, innerParams, 'timerProgressBar');
      setTimeout(() => {
        if (globalState.timeout && globalState.timeout.running) {
          // timer can be already stopped or unset at this point
          animateTimerProgressBar(innerParams.timer);
        }
      });
    }
  }
};

/**
 * Initialize focus in the popup:
 *
 * 1. If `toast` is `true`, don't steal focus from the document.
 * 2. Else if there is an [autofocus] element, focus it.
 * 3. Else if `focusConfirm` is `true` and confirm button is visible, focus it.
 * 4. Else if `focusDeny` is `true` and deny button is visible, focus it.
 * 5. Else if `focusCancel` is `true` and cancel button is visible, focus it.
 * 6. Else focus the first focusable element in a popup (if any).
 *
 * @param {DomCache} domCache
 * @param {SweetAlertOptions} innerParams
 */
const initFocus = (domCache, innerParams) => {
  if (innerParams.toast) {
    return;
  }
  // TODO: this is dumb, remove `allowEnterKey` param in the next major version
  if (!callIfFunction(innerParams.allowEnterKey)) {
    warnAboutDeprecation('allowEnterKey');
    blurActiveElement();
    return;
  }
  if (focusAutofocus(domCache)) {
    return;
  }
  if (focusButton(domCache, innerParams)) {
    return;
  }
  setFocus(-1, 1);
};

/**
 * @param {DomCache} domCache
 * @returns {boolean}
 */
const focusAutofocus = domCache => {
  const autofocusElements = Array.from(domCache.popup.querySelectorAll('[autofocus]'));
  for (const autofocusElement of autofocusElements) {
    if (autofocusElement instanceof HTMLElement && isVisible$1(autofocusElement)) {
      autofocusElement.focus();
      return true;
    }
  }
  return false;
};

/**
 * @param {DomCache} domCache
 * @param {SweetAlertOptions} innerParams
 * @returns {boolean}
 */
const focusButton = (domCache, innerParams) => {
  if (innerParams.focusDeny && isVisible$1(domCache.denyButton)) {
    domCache.denyButton.focus();
    return true;
  }
  if (innerParams.focusCancel && isVisible$1(domCache.cancelButton)) {
    domCache.cancelButton.focus();
    return true;
  }
  if (innerParams.focusConfirm && isVisible$1(domCache.confirmButton)) {
    domCache.confirmButton.focus();
    return true;
  }
  return false;
};
const blurActiveElement = () => {
  if (document.activeElement instanceof HTMLElement && typeof document.activeElement.blur === 'function') {
    document.activeElement.blur();
  }
};

// Assign instance methods from src/instanceMethods/*.js to prototype
SweetAlert.prototype.disableButtons = disableButtons;
SweetAlert.prototype.enableButtons = enableButtons;
SweetAlert.prototype.getInput = getInput;
SweetAlert.prototype.disableInput = disableInput;
SweetAlert.prototype.enableInput = enableInput;
SweetAlert.prototype.hideLoading = hideLoading;
SweetAlert.prototype.disableLoading = hideLoading;
SweetAlert.prototype.showValidationMessage = showValidationMessage;
SweetAlert.prototype.resetValidationMessage = resetValidationMessage;
SweetAlert.prototype.close = close;
SweetAlert.prototype.closePopup = close;
SweetAlert.prototype.closeModal = close;
SweetAlert.prototype.closeToast = close;
SweetAlert.prototype.rejectPromise = rejectPromise;
SweetAlert.prototype.update = update;
SweetAlert.prototype._destroy = _destroy;

// Assign static methods from src/staticMethods/*.js to constructor
Object.assign(SweetAlert, staticMethods);

// Proxy to instance methods to constructor, for now, for backwards compatibility
Object.keys(instanceMethods).forEach(key => {
  /**
   * @param {...(SweetAlertOptions | string | undefined)} args
   * @returns {SweetAlertResult | Promise<SweetAlertResult> | undefined}
   */
  SweetAlert[key] = function (...args) {
    if (currentInstance && currentInstance[key]) {
      return currentInstance[key](...args);
    }
    return null;
  };
});
SweetAlert.DismissReason = DismissReason;
SweetAlert.version = '11.26.3';

const Swal = SweetAlert;
// @ts-ignore
Swal.default = Swal;

export { Swal as default };
