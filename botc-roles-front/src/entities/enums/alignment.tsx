export enum Alignment {
  Good = 0,
  Evil = 1,
}

export function alignmentToString(alignment: Alignment) {
  return alignment === Alignment.Good ? "Gentil" : "Maléfique";
}
