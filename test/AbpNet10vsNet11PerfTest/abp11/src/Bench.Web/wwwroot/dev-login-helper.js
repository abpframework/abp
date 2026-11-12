/* Dev Login Helper */
(() => {
  const defaultAdminUsername = "admin";
  const defaultAdminPassword = "1q2w3E*";

  const run = () => {
    if (!/\/Account\/Login\/?$/i.test(window.location.pathname)) {
      return;
    }

    const userInput = document.querySelector(
      'input[name$="UserNameOrEmailAddress"], input[id$="UserNameOrEmailAddress"], input[name$="UserName"], input[id$="UserName"]'
    );
    const passwordInput = document.querySelector(
      'input[type="password"][name$="Password"], input[type="password"][id$="Password"]'
    );

    const addHint = (input, text) => {
      if (!input || input.dataset.defaultHintAdded === "true") {
        return;
      }

      const hint = document.createElement("span");
      hint.className = "text-muted small";
      hint.textContent = text;

      const floatingContainer = input.closest(".form-floating.mb-2");
      if (floatingContainer) {
        floatingContainer.appendChild(hint);
        input.dataset.defaultHintAdded = "true";
        return;
      }

      const container =
        input.closest(".input-group") ||
        input.closest(".form-group") ||
        input.closest(".mb-3") ||
        input.closest(".form-floating") ||
        input;

      if (container === input || container.classList.contains("input-group")) {
        container.insertAdjacentElement("afterend", hint);
      } else {
        container.appendChild(hint);
      }
      input.dataset.defaultHintAdded = "true";
    };

    const autoFillDefaults = () => {
      if (userInput && !userInput.value) {
        userInput.value = defaultAdminUsername;
      }
      if (passwordInput && !passwordInput.value) {
        passwordInput.value = defaultAdminPassword;
      }
    };

    addHint(userInput, `Default username: ${defaultAdminUsername}`);
    addHint(passwordInput, `Default password: ${defaultAdminPassword}`);

    setTimeout(autoFillDefaults, 150);
    if (userInput) {
      userInput.addEventListener("focus", autoFillDefaults, { once: true });
    }
    if (passwordInput) {
      passwordInput.addEventListener("focus", autoFillDefaults, { once: true });
    }
  };

  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", run);
  } else {
    run();
  }
})();