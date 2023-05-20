import { Fragment, useEffect, useRef, useState } from "react";
import Classes from "./PlayerRolesSelector.module.css";
import { Button, Input, Spacer } from "@nextui-org/react";
import { X } from "react-feather";
import { PlayerRole } from "@/entities/PlayerRole";
import ListItemPlayerRole from "../list-stats/ListItemPlayerRole";
import ListItem from "../list-stats/ListItem";
import Container from "../list-stats/Container";
import { Player } from "@/entities/Player";
import { getAllPlayers } from "../../../data/back-api";
import { Role } from "@/entities/Role";
import { toLowerRemoveDiacritics } from "@/helper/string";

export default function PlayerRolesSelector(props: {
  selectedPlayerRoles: PlayerRole[];
  setSelectedPlayerRoles: any;
}) {
  const inputFilterPlayer = useRef<HTMLInputElement>(null);
  const [allPlayers, setAllPlayers] = useState<Player[]>([]);
  const [showPlayers, setShowPlayers] = useState(false);
  const [visiblePlayers, setVisiblePlayers] = useState<Player[]>([]);
  const [playerFilter, setPlayerFilter] = useState("");

  const inputFilterRole = useRef<HTMLInputElement>(null);
  const [showRoles, setShowRoles] = useState(false);
  const [visibleRoles, setVisibleRoles] = useState<Role[]>([]);
  const [roleFilter, setRoleFilter] = useState("");

  useEffect(() => {
    async function initPlayers() {
      const tempPlayers = await getAllPlayers();
      setAllPlayers(tempPlayers);
      setVisiblePlayers(tempPlayers);
    }
    initPlayers();
  }, []);

  function removeSelectedPlayerRole(playerId: number) {}

  function onFocusPlayerInput() {
    setShowPlayers(true);
    setShowRoles(false);
  }

  function onFocusRoleInput() {
    setShowPlayers(false);
    setShowRoles(true);
  }

  function playerFilterChanged(filter: string) {
    setPlayerFilter(filter);

    setShowPlayers(true);
    setVisiblePlayers(
      allPlayers.filter(
        (p) =>
          toLowerRemoveDiacritics(p.name).includes(
            toLowerRemoveDiacritics(filter)
          ) ||
          toLowerRemoveDiacritics(p.pseudo).includes(
            toLowerRemoveDiacritics(filter)
          )
      )
    );
  }

  function canBlur(event: any) {
    if (
      event === undefined ||
      event === null ||
      event.relatedTarget === undefined ||
      event.relatedTarget === null ||
      event.relatedTarget.classList === undefined ||
      event.relatedTarget.classList === null
    ) {
      console.log(0);
      return true;
    }

    if (
      (!event.relatedTarget.classList.contains(Classes["player-item"]) ||
        !event.relatedTarget.classList.contains(Classes["role-item"])) &&
      !event.relatedTarget.classList.contains("nextui-input-clear-button") &&
      !event.relatedTarget.classList.contains(
        Classes["container-players-values"]
      ) &&
      !event.relatedTarget.classList.contains(Classes["container-roles-values"])
    ) {
      console.log(1);
      return false;
    } else if (
      event.relatedTarget.classList.contains("nextui-input-clear-button")
    ) {
      // onChangeInput("");
      console.log(2);
      return false;
    } else if (
      event.relatedTarget.classList.contains(Classes["container-roles-values"])
    ) {
      // inputFilterRole.current?.focus();
      console.log(3);
      return false;
    }

    console.log(4);
    // return true;
  }

  function blurPlayerInput(event: any) {
    if (!canBlur(event)) {
      inputFilterPlayer.current?.focus();
    } else {
      setShowPlayers(false);
    }
  }

  function blurRoleInput(event: any) {
    setShowRoles(false);
  }

  function onSelectPlayer(playerId: number) {
    const playerSelected = allPlayers.find((p) => p.id === playerId);

    if (playerSelected !== undefined) {
      setPlayerFilter(playerSelected.name);
      setShowPlayers(false);
    }
  }

  return (
    <Fragment>
      <div className={Classes["players-roles-selected"]}>
        {props.selectedPlayerRoles.map((pr) => (
          <Fragment key={pr.player.id}>
            <div className={Classes["player-role-selected"]}>
              <ListItemPlayerRole
                playerName={pr.player.name}
                pseudo={pr.player.name}
                roleName={pr.role.name}
                characterType={pr.role.characterType}
              />
              <X
                className={Classes.delete}
                onClick={() => removeSelectedPlayerRole(pr.player.id)}
              />
            </div>
            <Spacer x={1.25} />
          </Fragment>
        ))}
      </div>
      <div className={Classes["inputs-container"]}>
        <Input
          css={{ flex: 1 }}
          labelPlaceholder="Joueur"
          aria-label="Joueur"
          clearable
          bordered
          value={playerFilter}
          onChange={(event) => playerFilterChanged(event.target.value)}
          onFocus={(event) => setTimeout(() => onFocusPlayerInput(), 0)}
          onBlur={(event) => blurPlayerInput(event)}
          ref={inputFilterPlayer}
        ></Input>
        <Input
          css={{ flex: 1 }}
          labelPlaceholder="Rôle"
          aria-label="Rôle"
          clearable
          bordered
          value={roleFilter}
          onChange={(event) => setRoleFilter(event.target.value)}
          onFocus={(event) => setTimeout(() => onFocusRoleInput(), 0)}
          onBlur={(event) => blurRoleInput(event)}
          ref={inputFilterRole}
        ></Input>
      </div>
      {(showPlayers || showRoles) && <Spacer y={0.75} />}
      {showPlayers && (
        // tabIndex are necessary to catch the class in the blur event of the inputs
        <div tabIndex={0} className={Classes["container-players-values"]}>
          {visiblePlayers.map((p) => (
            <div tabIndex={1} key={p.id} className={Classes["player-item"]}>
              <ListItem
                name={p.name}
                subName={p.pseudo}
                onPress={() => onSelectPlayer(p.id)}
              />
              <Spacer y={1} />
            </div>
          ))}
        </div>
      )}
    </Fragment>
  );
}
