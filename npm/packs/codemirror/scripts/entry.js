import { EditorState } from '@codemirror/state';
import { EditorView } from '@codemirror/view';
import { basicSetup, minimalSetup } from 'codemirror';
import { css } from '@codemirror/lang-css';
import { javascript } from '@codemirror/lang-javascript';

const modeFactories = {
    css: css,
    javascript: javascript,
    js: javascript
};

function getModeExtension(mode) {
    if (!mode) {
        return null;
    }

    const modeName = typeof mode === 'string'
        ? mode
        : (mode.name || mode.mode || mode.helperType || '');
    const factory = modeFactories[modeName.toLowerCase()];

    return factory ? factory() : null;
}

function normalizeWrapperClasses(textarea) {
    return (textarea.className || '')
        .split(/\s+/)
        .filter(Boolean)
        .filter(name => name !== 'form-control' && name !== 'valid' && name !== 'input-validation-error')
        .join(' ');
}

function syncTextarea(textarea, view) {
    textarea.value = view.state.doc.toString();
}

function createWrapper(textarea) {
    const wrapper = document.createElement('div');
    const extraClasses = normalizeWrapperClasses(textarea);
    const style = window.getComputedStyle(textarea);

    wrapper.className = extraClasses
        ? 'abp-codemirror ' + extraClasses
        : 'abp-codemirror';
    if (textarea.id) {
        wrapper.id = textarea.id + '__codemirror';
    }

    wrapper.style.width = '100%';

    if (style.height && style.height !== 'auto' && style.height !== '0px') {
        wrapper.style.height = style.height;
    } else {
        wrapper.style.minHeight = '12rem';
    }

    return wrapper;
}

function getEditorSetup(options) {
    // CodeMirror 5 default: line numbers off unless lineNumbers === true
    return options.lineNumbers === true ? basicSetup : minimalSetup;
}

function createState(textarea, options) {
    const extensions = [
        getEditorSetup(options),
        EditorView.updateListener.of(update => {
            if (update.docChanged) {
                syncTextarea(textarea, update.view);
            }
        })
    ];

    const modeExtension = getModeExtension(options.mode);

    if (modeExtension) {
        extensions.push(modeExtension);
    }

    if (options.readOnly) {
        extensions.push(EditorState.readOnly.of(true));
    }

    return EditorState.create({
        doc: textarea.value || '',
        extensions
    });
}

function createCompatibilityEditor(textarea, options) {
    const wrapper = createWrapper(textarea);

    textarea.parentNode.insertBefore(wrapper, textarea.nextSibling);
    textarea.style.display = 'none';
    textarea.setAttribute('aria-hidden', 'true');

    const view = new EditorView({
        state: createState(textarea, options || {}),
        parent: wrapper
    });

    const save = function () {
        syncTextarea(textarea, view);
    };

    if (textarea.form) {
        textarea.form.addEventListener('submit', save);
    }

    return {
        getValue: function () {
            return view.state.doc.toString();
        },
        setValue: function (value) {
            view.dispatch({
                changes: {
                    from: 0,
                    to: view.state.doc.length,
                    insert: value || ''
                }
            });

            save();
        },
        refresh: function () {
            if (typeof view.requestMeasure === 'function') {
                view.requestMeasure();
            }
        },
        focus: function () {
            view.focus();
        },
        save: save,
        getWrapperElement: function () {
            return wrapper;
        }
    };
}

window.CodeMirror = window.CodeMirror || {};
window.CodeMirror.loadedModes = window.CodeMirror.loadedModes || {};
window.CodeMirror.fromTextArea = function (textarea, options) {
    return createCompatibilityEditor(textarea, options || {});
};