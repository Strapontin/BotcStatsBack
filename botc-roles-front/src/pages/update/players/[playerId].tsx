import { Fragment, useCallback, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import {
  updatePlayer,
  getPlayerById,
  getAllPlayers,
} from "../../../../data/back-api/back-api";
import { Loading, Text } from "@nextui-org/react";
import classes from "../index.module.css";
import { Check, XOctagon } from "react-feather";
import PlayerCreateEdit from "@/components/create-edit/player-create-edit/PlayerCreateEdit";
import { Player, getNewEmptyPlayer } from "@/entities/Player";
import { useRouter } from "next/router";
import { toLowerRemoveDiacritics } from "@/helper/string";

export default function UpdatePlayerPage() {
  const playerId: number = Number(useRouter().query.playerId);

  const [oldPlayer, setOldPlayer] = useState<Player>(getNewEmptyPlayer());

  const [playerCreateEditKey, setPlayerCreateEditKey] = useState(0);
  const [message, setMessage] = useState(<Fragment />);
  const [player, setPlayer] = useState<Player>(getNewEmptyPlayer());

  const [players, setPlayers] = useState<string[]>([]);

  const canUpdatePlayer = useCallback(() => {
    if (player.name === "") {
      updateMessage(true, "Un nom est obligatoire.");
      return false;
    } else if (
      players.filter(
        (p) =>
          toLowerRemoveDiacritics(p) !==
            toLowerRemoveDiacritics(oldPlayer.name) &&
          toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(player.name)
      ).length !== 0
    ) {
      updateMessage(true, "Un joueur avec ce nom existe déjà.");
      return false;
    } else {
      updateMessage(false, "");
      return true;
    }
  }, [player, players, oldPlayer]);

  useEffect(() => {
    if (playerId === undefined || isNaN(playerId)) return;

    async function initPlayer() {
      const e = await getPlayerById(playerId);
      setPlayer(e);
      setOldPlayer(e);
    }
    initPlayer();

    async function initPlayers() {
      const e = (await getAllPlayers()).map((e) => e.name);
      setPlayers(e);
    }
    initPlayers();
  }, [playerId]);

  // Updates message on component refreshes
  useEffect(() => {
    if (
      player.name === oldPlayer.name &&
      player.characterType === oldPlayer.characterType &&
      player.alignment === oldPlayer.alignment
    )
      return;

    if (toLowerRemoveDiacritics(player.name) === "") {
      updateMessage(true, "Un nom est obligatoire.");
    } else if (
      players.filter(
        (p) =>
          toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(player.name)
      ).length !== 0
    ) {
      updateMessage(true, "Un joueur avec ce nom existe déjà.");
    } else {
      setMessage(<Fragment />);
    }
  }, [player, players, oldPlayer]);

  if (player.id === -1) {
    return (
      <Fragment>
        <Loading />
      </Fragment>
    );
  }

  const title = <Title>Modification du joueur {`'${oldPlayer.name}'`}</Title>;

  async function btnUpdatePlayer() {
    if (!canUpdatePlayer()) return;

    if (await updatePlayer(player)) {
      const r = await getPlayerById(playerId);
      setPlayer(r);
      setPlayerCreateEditKey(playerCreateEditKey + 1);
      setTimeout(
        () =>
          updateMessage(
            false,
            `Le joueur '${player.name}' a été modifié correctement.`
          ),
        50
      );
    } else {
      //Erreur
      updateMessage(
        true,
        "Une erreur est survenue lors de la modification du joueur."
      );
    }
  }

  function updateMessage(isError: boolean, message: string) {
    if (message === "") {
      setMessage(<Fragment />);
    } else if (isError) {
      setMessage(
        <Text span className={classes.red}>
          <XOctagon className={classes.icon} />
          {message}
        </Text>
      );
    } else {
      setMessage(
        <Text span className={classes.green}>
          <Check className={classes.icon} />
          {message}
        </Text>
      );
    }
  }

  return (
    <PlayerCreateEdit
      key={playerCreateEditKey}
      title={title}
      player={player}
      setPlayer={setPlayer}
      message={message}
      btnPressed={btnUpdatePlayer}
      btnText="Modifier le joueur"
    />
  );
}
