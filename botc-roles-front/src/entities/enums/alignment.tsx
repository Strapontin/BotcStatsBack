export enum Alignment {
  Good = 0,
  Evil = 1,
}

export function alignmentToString(alignment: Alignment) {
  return alignment === Alignment.Good ? "Gentil" : "Maléfique";
}

export function alignmentList(): { key: number; value: string }[] {
  const alignments = Object.values(Alignment)
    .filter((ct) => typeof ct !== "number")
    .map((key: any) => ({
      key: +Alignment[key],
      value: translate(key),
    }));

  return alignments;
}

function translate(name: string) {
  return name === "Good" ? "Gentil" : name === "Evil" ? "Maléfique" : "";
}
