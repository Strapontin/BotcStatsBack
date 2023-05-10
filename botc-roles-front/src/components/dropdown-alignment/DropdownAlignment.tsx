import { alignmentList } from "@/entities/enums/alignment";
import { Dropdown } from "@nextui-org/react";
import { Fragment, useState } from "react";

export default function DropdownAlignment() {
  const [alignmentSelected, setAlignmentSelected] =
    useState("Alignement");

  function selectAlignment(key: number) {
    setAlignmentSelected(alignmentList()[key].value);
  }

  return (
    <Fragment>
      <Dropdown type="menu">
        <Dropdown.Button
          id="selection-stat"
          ghost
          iconRight
          css={{ display: "flex", justifyContent: "left" }}
        >
          {alignmentSelected}
        </Dropdown.Button>
        <Dropdown.Menu
          aria-label="Static Actions"
          onAction={(key) => selectAlignment(+key)}
        >
          {alignmentList().map((item) => (
            <Dropdown.Item key={item.key}>{item.value}</Dropdown.Item>
          ))}
        </Dropdown.Menu>
      </Dropdown>
    </Fragment>
  );
}
