export function removeDiacritics(string: string): string {
  return string.normalize("NFD").replace(/\p{Diacritic}/gu, "");
}

export function removeDiaLowerCase(string: string): string {
  return string
    .normalize("NFD")
    .replace(/\p{Diacritic}/gu, "")
    .toLowerCase();
}
