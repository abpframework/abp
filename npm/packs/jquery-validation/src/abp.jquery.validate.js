$.validator.methods.range = function (value, element, param) {
  if (this.optional(element)) {
    return true;
  }

  // Remove thousands separators and convert decimal separators
  var cleanValue = value.replace(/[.,](?=.*[.,])/g, '').replace(/[.,](?!.*[.,])/g, '.');
  var numericValue = parseFloat(cleanValue);

  // Check if conversion was successful and value is within range
  return !isNaN(numericValue) && (numericValue >= param[0] && numericValue <= param[1]);
};

$.validator.methods.number = function (value, element) {
  if (this.optional(element)) {
    return true;
  }

  // 1. Period as decimal separator, comma as thousands separator (e.g., 1,234.56 or -1,234.56)
  // 2. Comma as decimal separator, period as thousands separator (e.g., 1.234,56 or -1.234,56)
  // 3. Pure numbers (integers)
  var pattern1 = /^(?:-?\d+|-?\d{1,3}(?:,\d{3})+)?(?:-?\.\d+)?$/;
  var pattern2 = /^(?:-?\d+|-?\d{1,3}(?:\.\d{3})+)?(?:-?,\d+)?$/;
  var pattern3 = /^-?\d+$/;

  return pattern1.test(value) || pattern2.test(value) || pattern3.test(value);
}
