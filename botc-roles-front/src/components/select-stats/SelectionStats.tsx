import { Dropdown } from "@nextui-org/react";
import classes from "./SelectionStats.module.css";
import { useRouter } from "next/router";
import { useSession, signIn, signOut } from "next-auth/react";
import { Fragment } from "react";

export default function SelectionStats() {
  const { data: session } = useSession();
  const router = useRouter();

  let connexionBlock;

  console.log(session);
  if (session) {
    connexionBlock = (
      <Dropdown.Section>
        <Dropdown.Item key="/api/auth/signout">Se déconnecter</Dropdown.Item>
        <Dropdown.Item key="/create/game">
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
        <Dropdown.Item withDivider key="/update/games">
          Modifier une partie
        </Dropdown.Item>
        <Dropdown.Item key="/update/editions">Modifier un module</Dropdown.Item>
        <Dropdown.Item key="/update/roles">Modifier un rôle</Dropdown.Item>
        <Dropdown.Item key="/update/players">Modifier un joueur</Dropdown.Item>
      </Dropdown.Section>
    );
  } else {
    connexionBlock = (
      <Dropdown.Item key="/api/auth/signin">Se connecter</Dropdown.Item>
    );
  }

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
          {connexionBlock}
          <Dropdown.Item withDivider key="/games-player">
            Nombre de parties par joueur
          </Dropdown.Item>
          <Dropdown.Item key="/games-role">
            Nombre de parties par rôle
          </Dropdown.Item>
          <Dropdown.Item withDivider key="/games">
            Liste des parties
          </Dropdown.Item>
          <Dropdown.Item key="/editions">Liste des modules</Dropdown.Item>
          <Dropdown.Item key="/roles">Liste des rôles</Dropdown.Item>
        </Dropdown.Menu>
      </Dropdown>
    </div>
  );
}
