import { alignmentList } from "@/entities/enums/alignment";
import { Dropdown } from "@nextui-org/react";
import { Fragment, useState } from "react";

export default function DropdownAlignment(props: {
  setAlignment: any;
  text?: string;
}) {
  if (props.text === undefined) {
    props.text = "Alignement";
  }
  const [alignmentSelected, setAlignmentSelected] = useState(props.text);

  function selectAlignment(key: number) {
    props.setAlignment(key);
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
            <Dropdown.Item
              key={item.key}
            >
              {item.value}
            </Dropdown.Item>
          ))}
        </Dropdown.Menu>
      </Dropdown>
    </Fragment>
  );
}
