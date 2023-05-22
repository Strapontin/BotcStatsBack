import { Fragment, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import { editGame, getGameById } from "../../../../data/back-api";
import { Loading, Text } from "@nextui-org/react";
import classes from "../index.module.css";
import { Check, XOctagon } from "react-feather";
import { Alignment, alignmentList } from "@/entities/enums/alignment";
import GameCreateEdit from "@/components/create-edit/game-create-edit/GameCreateEdit";
import { Game, getNewEmptyGame } from "@/entities/Game";
import { dateToString } from "@/helper/date";
import { useRouter } from "next/router";

export default function CreateGame() {
  const gameId: number = Number(useRouter().query.gameId);

  const [gameCreateEditKey, setGameCreateEditKey] = useState(0);
  const [message, setMessage] = useState(<Fragment />);
  const [game, setGame] = useState<Game>(getNewEmptyGame());

  useEffect(() => {
    if (gameId === undefined || isNaN(gameId)) return;

    async function initGame() {
      const g = await getGameById(gameId);
      setGame(g);
    }
    initGame();
  }, [gameId]);

  if (game.id === -1) {
    return (
      <Fragment>
        <Loading />
      </Fragment>
    );
  }

  const title = <Title>Modification d{"'"}une partie existante</Title>;

  async function btnEditGame() {
    if (!canCreateGame()) return;

    if (
      await editGame(
        game.id,
        game.edition.id,
        game.storyTeller.id,
        dateToString(game.datePlayed),
        game.notes,
        game.winningAlignment,
        game.playerRoles
      )
    ) {
      const g = await getGameById(gameId);
      setGame(g);
      setGameCreateEditKey(gameCreateEditKey + 1);
      updateMessage(false, `La partie a été enregistrée correctement.`);
    } else {
      //Erreur
      updateMessage(
        true,
        "Une erreur est survenue lors de l'enregistrement du module."
      );
    }
  }

  function updateMessage(isError: boolean, message: string) {
    if (isError) {
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

  function canCreateGame() {
    if (game.edition.id === -1) {
      updateMessage(true, "Un module est obligatoire.");
      return false;
    }
    if (game.storyTeller.id === -1) {
      updateMessage(true, "Un conteur est obligatoire.");
      return false;
    }
    if (dateToString(game.datePlayed) === "") {
      updateMessage(
        true,
        "La date à laquelle la partie a été jouée est obligatoire."
      );
      return false;
    }
    if (game.winningAlignment === Alignment.None) {
      updateMessage(true, "L'alignement gagnant est obligatoire.");
      return false;
    }

    return true;
  }

  return (
    <GameCreateEdit
      key={gameCreateEditKey}
      title={title}
      game={game}
      setGame={setGame}
      message={message}
      btnPressed={btnEditGame}
      btnText="Modifier la partie"
    />
  );
}
