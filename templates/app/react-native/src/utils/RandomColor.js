export function getRandomColors(count) {
  const colors = [];

  for (let i = 0; i < count; i++) {
    const r = ((i + 5) * (i + 5) * 474) % 255;
    const g = ((i + 5) * (i + 5) * 1600) % 255;
    const b = ((i + 5) * (i + 5) * 84065) % 255;
    colors.push(`rgba(${r}, ${g}, ${b})`);
  }

  return colors;
}
