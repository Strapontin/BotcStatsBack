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
          <Dropdown.Item key="/create/partie">
            Ajouter une nouvelle partie
          </Dropdown.Item>
          <Dropdown.Item key="/create/edition">
            Ajouter un nouveau module
          </Dropdown.Item>
          <Dropdown.Item key="/create/role">
            Ajouter un nouveau rôle
          </Dropdown.Item>
          <Dropdown.Item key="/create/player">
            Ajouter un nouveau joueur
          </Dropdown.Item>
          <Dropdown.Item withDivider key="/games-player">
            Nombre de parties par joueur
          </Dropdown.Item>
          <Dropdown.Item key="/games">Liste parties</Dropdown.Item>
        </Dropdown.Menu>
      </Dropdown>
    </div>
  );
}
