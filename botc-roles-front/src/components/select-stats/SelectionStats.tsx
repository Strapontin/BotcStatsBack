import { Dropdown, Link } from "@nextui-org/react";
import classes from "./SelectionStats.module.css";
import { Key } from "react";
import { useRouter } from "next/router";

export default function SelectionStats() {
  const router = useRouter();

  return (
    <div className={classes.SelectionStats}>
      <Dropdown type="menu">
        <Dropdown.Button id="selection-stat" flat>
          Selection stat
        </Dropdown.Button>
        <Dropdown.Menu
          disabledKeys={[router.asPath]}
          onAction={(key) => {
            router.push(key.toString());
          }}
          aria-label="Static Actions"
        >
          <Dropdown.Item key="/parties-joueur">
            Nombre de parties par joueur
          </Dropdown.Item>
          <Dropdown.Item key="copy">Copy link</Dropdown.Item>
          <Dropdown.Item key="edit">Edit file</Dropdown.Item>
          <Dropdown.Item key="delete" color="error">
            Delete file
          </Dropdown.Item>
        </Dropdown.Menu>
      </Dropdown>
    </div>
  );
}
