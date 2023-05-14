export function removeDiacritics(string: string): string {
  return string.normalize("NFD").replace(/\p{Diacritic}/gu, "");
}

export function toLowerRemoveDiacritics(string: string): string {
  return string
    .normalize("NFD")
    .replace(/\p{Diacritic}/gu, "")
    .toLowerCase();
}
