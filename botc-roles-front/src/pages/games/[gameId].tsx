import { Fragment, useEffect, useState } from "react";
import { Game } from "@/entities/Game";
import { useRouter } from "next/router";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import PlayerName from "@/components/ui/playerName";
import DateUi from "@/components/ui/date-ui";
import ListItemLarge from "@/components/list-stats/ListItemLarge";
import { Link, Spacer } from "@nextui-org/react";
import { PlayerRole } from "@/entities/PlayerRole";
import { alignmentToString } from "@/entities/enums/alignment";
import { getGameById } from "../../../data/back-api";
import ListItemPlayerRole from "@/components/list-stats/ListItemPlayerRole";

export default function GamePage() {
  const gameId: number = Number(useRouter().query.gameId);
  const [game, setGame] = useState<Game>();

  useEffect(() => {
    async function initGame() {
      const p = await getGameById(gameId);
      setGame(p);
    }
    initGame();
  }, [gameId]);

  if (isNaN(gameId) || !game) {
    return <Title>Chargement {gameId}...</Title>;
  }

  const storyTellerPseudo =
    game.storyTeller.pseudo !== "" ? ` (${game.storyTeller.pseudo})` : "";

  const title = (
    <Title>
      Détails de la partie du <DateUi date={game.datePlayed} /> contée par{" "}
      <Link href={`/players/${game.storyTeller.name}`} color="text">
        <PlayerName name={`${game.storyTeller.name}${storyTellerPseudo}`} />
      </Link>
    </Title>
  );

  return (
    <Fragment>
      {title}
      <Container>
        <ListItem name="Module" value={game.edition.name} />
        <ListItem
          name="Conteur"
          value={<PlayerName name={`${game.storyTeller.name}${storyTellerPseudo}`} />}
        />
        <ListItem
          name="Date de la partie"
          value={<DateUi date={game.datePlayed} />}
        />
        <ListItem
          name="Alignement gagnant"
          value={alignmentToString(game.winningAlignment)}
        />
        <ListItemLarge name="Notes" value={game.notes} />
        <Spacer y={2} />

        {game.playerRoles.map((prg: PlayerRole) => (
          <Link
            key={prg.player.name}
            href={`/players/${prg.player.name}`}
            color="text"
          >
            <ListItemPlayerRole
              playerName={prg.player.name}
              pseudo={prg.player.pseudo}
              roleName={prg.role.name}
              characterType={prg.role.characterType}
            />
          </Link>
        ))}
      </Container>
    </Fragment>
  );
}
