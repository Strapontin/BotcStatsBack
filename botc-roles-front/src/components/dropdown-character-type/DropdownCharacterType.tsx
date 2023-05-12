import { Dropdown } from "@nextui-org/react";
import { Fragment, useState } from "react";
import { characterTypeList } from "@/entities/enums/characterType";

export default function DropdownCharacterType(props: {
  setCharacterType: any;
}) {
  const [characterTypeSelected, setCharacterTypeSelected] =
    useState("Type de personnage");

  function selectCharacterType(key: number) {
    props.setCharacterType(key);
    setCharacterTypeSelected(characterTypeList()[key].value);
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
          {characterTypeSelected}
        </Dropdown.Button>
        <Dropdown.Menu
          aria-label="Static Actions"
          onAction={(key) => selectCharacterType(+key)}
        >
          {characterTypeList().map((item) => (
            <Dropdown.Item key={item.key}>{item.value}</Dropdown.Item>
          ))}
        </Dropdown.Menu>
      </Dropdown>
    </Fragment>
  );
}